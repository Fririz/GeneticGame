using GeneticGame.FieldEntities;
namespace GeneticGame;

public class Engine
{
    Field _gameField  = new(GameSettings.GameFieldSize);
    Unit[] _gameUnits = new Unit[GameSettings.InitialAmountOfUnit];

    public void StartGame()
    {
        
    }
    public void UpdateGame()
    {
        
    }

    public void InitializeUnits()
    {
        UnitGenetics geneticsOfUnit = GameSettings.StandartGenetic;
        var emptyCells = _gameField.GetEmptyFieldCells().ToList();
        for (int i = 0; i < GameSettings.InitialAmountOfUnit; i++)
        {
            if (emptyCells.Count == 0) break; 

            UnitGenetics unitGenetics = GetRandomizeUnitStartGenetics();


            int index = Random.Shared.Next(emptyCells.Count);
            FieldCell targetCell = emptyCells[index];


            var newUnit = new Unit(Convert.ToString(i), targetCell.Coordinates, unitGenetics);


            targetCell.CurrentUnit = newUnit;
            targetCell.FieldType = TypeOfFields.Unit;
            
            _gameUnits[i] = newUnit;
            
            emptyCells.RemoveAt(index);
        } 
    }

    private UnitGenetics GetRandomizeUnitStartGenetics()
    {
        return GameSettings.StandartGenetic.GetInitializedRandomGenetics();
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
        var emptyFieldCells = _gameField.GetEmptyFieldCells();
        
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