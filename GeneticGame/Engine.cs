using GeneticGame.FieldEntities;
namespace GeneticGame;

public class Engine
{
    Field _gameField  = new(GameSettings.GameFieldSize);
    public void CreateField()
    {
        for (int i = 0; i < _gameField.Size; i++)
        {
            for (int j = 0; j < _gameField.Size; j++)
            {
                if(i == 0)
                    _gameField.FieldCells[i, j] = FieldCell.CreateWallCell(new Coordinates(i,j));
                else if(i == _gameField.Size - 1)
                    _gameField.FieldCells[i, j] = FieldCell.CreateWallCell(new Coordinates(i,j));
                
                else if (j == 0)
                    _gameField.FieldCells[i, j] = FieldCell.CreateWallCell(new Coordinates(i, j));
                else if (j == _gameField.Size - 1)
                    _gameField.FieldCells[i, j] = FieldCell.CreateWallCell(new Coordinates(i, j));


                else _gameField.FieldCells[i, j] = FieldCell.CreateEmptyCell(new Coordinates(i, j));
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

        while (spawnedCount < GameSettings.AmmountOfFoodPlaces && safetyCounter < 100)
        {
            int x = Random.Shared.Next(0, _gameField.Size);
            int y = Random.Shared.Next(0, _gameField.Size);

            if (_gameField.FieldCells[x, y].FieldType == TypeOfFields.Empty)
            {

                _gameField.FieldCells[x, y] = FieldCell.CreateFoodCell(new Coordinates(x, y), GameSettings.AmmountOfFood);
                spawnedCount++;
            }
        
            safetyCounter++;
        }
    }
}