namespace GeneticGame;
using GeneticGame.FieldEntities;

public class BoardUi
{
    private readonly Engine _engine;
    public BoardUi(Engine engine)
    {
        _engine = engine;
    }

    public void StartGame()
    {
        _engine.CreateField();
        for (int i = 0; i <= 5; i++)
        {
            ShowField(_engine.GetGameField());
            _engine.GenerateFood(1);
            Thread.Sleep(1000);
            Console.Clear();
        }
    }
    public void ShowField(Field field)
    {
        for (int i = 0; i < field.Size; i++)
        {
            for (int j = 0; j < field.Size; j++)
            {
                if(field.FieldCells[i,j].FieldType == TypeOfFields.Food) Console.Write("F");
                if(field.FieldCells[i,j].FieldType == TypeOfFields.Wall) Console.Write("#");
                if(field.FieldCells[i,j].FieldType == TypeOfFields.Empty) Console.Write(" ");
            }
            Console.Write("\n");
        }
    }
}