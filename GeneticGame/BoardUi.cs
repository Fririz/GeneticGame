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
        _engine.StartGame();
        ShowField(_engine.GetGameField());
        Thread.Sleep(1000);
        int counter = 0;
        while (true)
        {
            if (counter % 50 == 0)
            {
                _engine.GenerateFood();
            }
            _engine.UpdateGame();
            ShowField(_engine.GetGameField());
            Thread.Sleep(100);
            Console.Clear();
            counter++;
        }
    }
    private void ShowField(Field field)
    {
        for (int i = 0; i < field.Size; i++)
        {
            for (int j = 0; j < field.Size; j++)
            {
                if(field.FieldCells[i,j].FieldType == TypeOfFields.Food) Console.Write("F");
                if(field.FieldCells[i,j].FieldType == TypeOfFields.Wall) Console.Write("#");
                if(field.FieldCells[i,j].FieldType == TypeOfFields.Empty) Console.Write(" ");
                if (field.FieldCells[i, j].FieldType == TypeOfFields.Unit)
                    Console.Write(field.FieldCells[i, j].CurrentUnit!.Name);
                //if(field.FieldCells[i,j].FieldType == TypeOfFields.Unit) Console.Write(Random.Shared.NextDouble() > 0.5 ? "\U0001F434" : "🦄");
                Console.Write(" ");
            }
            Console.Write("\n");
        }
    }
}