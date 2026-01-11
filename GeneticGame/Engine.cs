using GeneticGame.FieldEntities;
namespace GeneticGame;

public class Engine
{
    Field _gameField  = new(GameSettings.GameFieldSize);
    List<Unit> _gameUnits = new(GameSettings.InitialAmountOfUnit);
    private int _unitIdCounter = 0;
    
    public void StartGame()
    {
        CreateField();
        InitializeUnits();
        GenerateFood();
    }

    public void UpdateGame()
    {
        for (int i = _gameUnits.Count - 1; i >= 0; i--)
        {
            var unit = _gameUnits[i];

            if (unit.IsDead) continue;

            if (unit.CurrentEnergy <= 0)
            {
                RemoveUnitFromField(unit);
                continue;
            }
            
            unit.BirthCooldown--;
            unit.CurrentEnergy -= 1;
            
            // Movement Logic
            var oldCoordinates = unit.Coordinates;
            var newCoordinates = unit.DecideNextMove(_gameField);
            
            if (_gameField.FieldCells[newCoordinates.X, newCoordinates.Y].FieldType == TypeOfFields.Empty)
            {
                _gameField.FieldCells[oldCoordinates.X, oldCoordinates.Y].CurrentUnit = null;
                _gameField.FieldCells[oldCoordinates.X, oldCoordinates.Y].FieldType = TypeOfFields.Empty;

                _gameField.FieldCells[newCoordinates.X, newCoordinates.Y].CurrentUnit = unit;
                _gameField.FieldCells[newCoordinates.X, newCoordinates.Y].FieldType = TypeOfFields.Unit;

                unit.Coordinates = newCoordinates;
            }

            // Interaction Logic
            var interactCellsNearUnit = GetCellsThatCanInteractWithUnit(unit);
            if (!interactCellsNearUnit.Any())
            {
                continue;
            }

            for (int j = 0; j < interactCellsNearUnit.Length; j++)
            {
                // Eat
                if (interactCellsNearUnit[j].FieldType == TypeOfFields.Food)
                {
                    EatFood(unit, interactCellsNearUnit[j]);
                }
                // Attack 
                else if (interactCellsNearUnit[j].FieldType == TypeOfFields.Unit 
                         && interactCellsNearUnit[j].CurrentUnit!.Gender == unit.Gender)
                {
                    HitTarget(unit, interactCellsNearUnit[j].CurrentUnit!);
                }
                // Breed 
                else if (interactCellsNearUnit[j].FieldType == TypeOfFields.Unit 
                         && interactCellsNearUnit[j].CurrentUnit!.Gender != unit.Gender)
                {
                    if(unit.BirthCooldown <= 0 && interactCellsNearUnit[j].CurrentUnit!.BirthCooldown <= 0)
                        Pairing(unit, interactCellsNearUnit[j].CurrentUnit!);
                }
            }

        }
        _gameUnits.RemoveAll(u => u.IsDead);
    }

    private void Pairing(Unit mother, Unit father)
    {
        var emptyCell = GetEmptyCellNearUnit(mother);
        if(emptyCell == null) return;
        
        UnitGenetics newGenetics = UnitGenetics.MergeGenetics(mother.Genes, father.Genes);
        
        Unit newUnit = new(_unitIdCounter.ToString(), emptyCell.Coordinates, newGenetics);
        
        _gameUnits.Add(newUnit);
        
        _gameField.FieldCells[emptyCell.Coordinates.X, emptyCell.Coordinates.Y].CurrentUnit = newUnit;
        _gameField.FieldCells[emptyCell.Coordinates.X, emptyCell.Coordinates.Y].FieldType = TypeOfFields.Unit;
        
        _unitIdCounter++;
        mother.BirthCooldown = 10;
        father.BirthCooldown = 10;
    }
    
    private void EatFood(Unit unit, FieldCell foodCell)
    {
        if (unit.CurrentEnergy >= unit.Genes.MaxEnergy) return;

        double spaceInStomach = unit.Genes.MaxEnergy - unit.CurrentEnergy;
        
        double amountToEat = Math.Min(foodCell.FoodAmount, GameSettings.MaxFoodPerBite);
        amountToEat = Math.Min(amountToEat, spaceInStomach);

        unit.CurrentEnergy += amountToEat; 
        foodCell.FoodAmount -= (int)amountToEat;

        if (foodCell.FoodAmount <= 0)
        {
            foodCell.FoodAmount = 0;
            foodCell.FieldType = TypeOfFields.Empty;
        }
    }
    
    private void RemoveUnitFromField(Unit unit)
    {
        var coords = unit.Coordinates;
        _gameField.FieldCells[coords.X, coords.Y] = FieldCell.CreateEmptyCell(coords);
    }
    
    private void HitTarget(Unit attackUnit, Unit defendUnit)
    {
        defendUnit.GetDamage(attackUnit.Genes.BaseDamage);
        
        if (defendUnit.IsDead)
        {
            RemoveUnitFromField(defendUnit);
        }
    }

    private FieldCell? GetEmptyCellNearUnit(Unit unit)
    {
        var unitCoordinates = unit.Coordinates;
        List<Coordinates> possibleCoordinates = new List<Coordinates>();
        possibleCoordinates.Add(unitCoordinates with { X = unitCoordinates.X + 1 });
        possibleCoordinates.Add(unitCoordinates with { X = unitCoordinates.X - 1 });
        possibleCoordinates.Add(unitCoordinates with { Y = unitCoordinates.Y + 1 });
        possibleCoordinates.Add(unitCoordinates with { Y = unitCoordinates.Y - 1 });
        
        return possibleCoordinates
            .Select(coords => _gameField.FieldCells[coords.X, coords.Y])
            .FirstOrDefault(cell => cell.FieldType == TypeOfFields.Empty);
    }

    private FieldCell[] GetCellsThatCanInteractWithUnit(Unit unit)
    {
        var unitCoordinates = unit.Coordinates;
        List<Coordinates> possibleCoordinates = new List<Coordinates>();
        possibleCoordinates.Add(unitCoordinates with { X = unitCoordinates.X + 1 });
        possibleCoordinates.Add(unitCoordinates with { X = unitCoordinates.X - 1 });
        possibleCoordinates.Add(unitCoordinates with { Y = unitCoordinates.Y + 1 });
        possibleCoordinates.Add(unitCoordinates with { Y = unitCoordinates.Y - 1 });
        
        var possibleCells = possibleCoordinates
            .Select(coords => _gameField.FieldCells[coords.X, coords.Y])
            .ToArray();
            
        return possibleCells
            .Where(cell => cell.FieldType is TypeOfFields.Unit or TypeOfFields.Food)
            .ToArray();
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
            
            _gameUnits.Add(newUnit);
            _unitIdCounter++;
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
        int safetyCounter = 0; 
        var emptyFieldCells = _gameField.GetAllCellsWithType(TypeOfFields.Empty);
        
        if (emptyFieldCells.Length == 0) return;

        while (spawnedCount < GameSettings.AmountOfFoodPlaces && safetyCounter < 100)
        {
            FieldCell randomEmptyCell = emptyFieldCells[Random.Shared.Next(0, emptyFieldCells.Length)];
            
            if(randomEmptyCell.FieldType == TypeOfFields.Empty) 
            {
                _gameField.FieldCells[randomEmptyCell.Coordinates.X, randomEmptyCell.Coordinates.Y] = 
                    FieldCell.CreateFoodCell(randomEmptyCell.Coordinates, GameSettings.AmountOfFood);
                spawnedCount++;
            }
            safetyCounter++;
        }
    }
}