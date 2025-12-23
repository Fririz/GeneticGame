    using GeneticGame.FieldEntities;
    namespace GeneticGame;

    public class Engine
    {
        Field _gameField  = new(GameSettings.GameFieldSize);
        Unit[] _gameUnits = new Unit[GameSettings.InitialAmountOfUnit];

        public void StartGame()
        {
            CreateField();
            InitializeUnits();
            GenerateFood();
        }
        public void UpdateGame()
        {
            for (int i = 0; i < _gameUnits.Length; i++)
            {
                var oldCoordinates = _gameUnits[i].Coordinates;
                var newCoordinates = _gameUnits[i].DecideNextMove(_gameField);
                if (oldCoordinates.X == newCoordinates.X && oldCoordinates.Y == newCoordinates.Y)
                    continue;
                if (_gameField.FieldCells[newCoordinates.X, newCoordinates.Y].FieldType == TypeOfFields.Empty)
                {
                    _gameField.FieldCells[oldCoordinates.X, oldCoordinates.Y].CurrentUnit = null;
                    _gameField.FieldCells[oldCoordinates.X, oldCoordinates.Y].FieldType = TypeOfFields.Empty;

                    _gameField.FieldCells[newCoordinates.X, newCoordinates.Y].CurrentUnit = _gameUnits[i];
                    _gameField.FieldCells[newCoordinates.X, newCoordinates.Y].FieldType = TypeOfFields.Unit;

                    _gameUnits[i].Coordinates = newCoordinates;
                }
            }
        }

        private FieldCell[] GetCellsThatCanInteractWithUnit(Unit unit)
        {
            var unitCoordinates = unit.Coordinates;
            List<Coordinates> possibleCoordinates = new List<Coordinates>();
            possibleCoordinates.Add(unitCoordinates with { X = unitCoordinates.X + 1 });
            possibleCoordinates.Add(unitCoordinates with { X = unitCoordinates.X - 1 });
            possibleCoordinates.Add(unitCoordinates with { Y = unitCoordinates.Y + 1 });
            possibleCoordinates.Add(unitCoordinates with { Y = unitCoordinates.Y - 1 });
            var possibleCells = possibleCoordinates.Select(coords => _gameField.FieldCells[coords.X, coords.Y]).ToArray();
            return possibleCells.Where(cell => cell.FieldType is TypeOfFields.Unit or TypeOfFields.Food).ToArray();
        }

        private void InitializeUnits()
        {
            var emptyCells = _gameField.GetAllCellsWithType(TypeOfFields.Empty).ToList();
            for (int i = 0; i < GameSettings.InitialAmountOfUnit; i++)
            {
                if (emptyCells.Count == 0) break; 

                UnitGenetics unitGenetics = GameSettings.StandartGenetic.GetInitializedRandomGenetics();
                int index = Random.Shared.Next(emptyCells.Count);
                FieldCell targetCell = emptyCells[index];


                var newUnit = new Unit(Convert.ToString(i), targetCell.Coordinates, unitGenetics);


                targetCell.CurrentUnit = newUnit;
                targetCell.FieldType = TypeOfFields.Unit;
                
                _gameUnits[i] = newUnit;
                
                emptyCells.RemoveAt(index);
            } 
        }
        
        public void CreateField()
        {
            int size = _gameField.Size;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    bool isBorder = i == 0 || i == size - 1 || j == 0 || j == size - 1;

                    var coords = new Coordinates(i, j);
                    
                    _gameField.FieldCells[i, j] = isBorder 
                        ? FieldCell.CreateWallCell(coords) 
                        : FieldCell.CreateEmptyCell(coords);
                }
            }
        }

        public Field GetGameField()
        {
            return _gameField;
        }


        public void GenerateFood()
        {
            int spawnedCount = 0;
            int safetyCounter = 0; // If field is full, we can't spawn food'
            var emptyFieldCells = _gameField.GetAllCellsWithType(TypeOfFields.Empty);
            
            while (spawnedCount < GameSettings.AmountOfFoodPlaces && safetyCounter < 100)
            {
                FieldCell randomEmptyCell = emptyFieldCells[Random.Shared.Next(0, emptyFieldCells.Length)];
                _gameField.FieldCells[randomEmptyCell.Coordinates.X, randomEmptyCell.Coordinates.Y] = 
                    FieldCell.CreateFoodCell(randomEmptyCell.Coordinates, GameSettings.AmountOfFood);
                spawnedCount++;
                safetyCounter++;

            }
        }
    }