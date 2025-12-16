namespace GeneticGame;

public class BoardUi
{
    public void StartGame()
    {
        char[,] field = CreateField();
        ShowField(field);
    }
    private const int _fieldSize = 25;
    public void ShowField(char[,] field)
    {
        for (int i = 0; i < _fieldSize; i++)
        {
            for (int j = 0; j < _fieldSize; j++)
            {
                Console.Write(field[i, j] + " ");
            }
            Console.Write("\n");
        }
    }

    public char[,] CreateField()
    {
        char[,] field = new char[_fieldSize,_fieldSize];
        for (int i = 0; i < _fieldSize; i++)
        {
            for (int j = 0; j < _fieldSize; j++)
            {
                if(i == 0)
                    field[i, j] = '#';
                else if(i == _fieldSize - 1)
                    field[i, j] = '#';
                
                else if(j == 0)
                    field[i, j] = '#';
                else if(j == _fieldSize-1)
                    field[i, j] = '#';

                
                else field[i, j] = ' ';
            }
        }

        return field;
    }
}