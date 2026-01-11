namespace GeneticGame;
using GeneticGame.FieldEntities;
using System.Text; 
public class BoardUi
{
    private readonly Engine _engine;
    public BoardUi(Engine engine)
    {
        _engine = engine;
    }

    public void StartGame()
    {
        int counter = 0;
        Console.CursorVisible = false;
        _engine.StartGame();
        ShowField(_engine.GetGameField());
        Thread.Sleep(1000);
        Console.Clear();

        while (true)
        {
            if (counter % 50 == 0)
            {
                _engine.GenerateFood();
            }
            _engine.UpdateGame();
            ShowField(_engine.GetGameField());
            Thread.Sleep(100);
            Console.SetCursorPosition(0, 0);
            counter++;
        }
    }
    private void ShowField(Field field)
    {
        Console.SetCursorPosition(0, 0);
        var sb = new StringBuilder();

        for (int i = 0; i < field.Size; i++)
        {
            for (int j = 0; j < field.Size; j++)
            {
                var cell = field.FieldCells[i, j];
            
                if (cell.FieldType == TypeOfFields.Food)
                {
                    sb.Append("🍎"); 
                }
                else if (cell.FieldType == TypeOfFields.Wall)
                {
                    sb.Append("██"); 
                }
                else if (cell.FieldType == TypeOfFields.Unit && cell.CurrentUnit != null)
                {
                    if (cell.CurrentUnit.Gender == 0)
                        sb.Append("🧑"); 
                    else
                        sb.Append("👩"); 
                }
                else 
                {
                    sb.Append("  "); 
                }
            }
            sb.AppendLine();
        }
        Console.Write(sb.ToString());
    }
}