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
        for (int i = 0; i < GameSettings.InitialAmountOfUnit; i++)
        {
            UnitGenetics unitGenetics = GetRandomizeUnitStartGenetics();
            FieldCell[] emptyFieldCells = GetEmptyFieldCells();
            FieldCell randomEmptyCell = emptyFieldCells[Random.Shared.Next(0, emptyFieldCells.Length)];
            var unit = new Unit(Convert.ToString(i), randomEmptyCell.Coordinates, unitGenetics);
            _gameUnits[i] = unit;
            randomEmptyCell.CurrentUnit = unit;
            randomEmptyCell.FieldType = TypeOfFields.Unit;
            
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

    public FieldCell[] GetEmptyFieldCells()
    {
        return _gameField.FieldCells.Cast<FieldCell>().Where(cell => cell.FieldType == TypeOfFields.Empty).ToArray();
    }
    public void GenerateFood()
    {
        int spawnedCount = 0;
        int safetyCounter = 0; // If field is full, we can't spawn food'
        var emptyFieldCells = GetEmptyFieldCells();
        
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