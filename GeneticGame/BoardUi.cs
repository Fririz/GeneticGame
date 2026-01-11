using System.Text;
using GeneticGame.FieldEntities;

namespace GeneticGame;

public class BoardUi
{
    private readonly Engine _engine;
    private const int TableWidth = 80; 

    public BoardUi(Engine engine)
    {
        _engine = engine;
    }

    public void StartGame()
    {
        Console.CursorVisible = false;
        Console.OutputEncoding = Encoding.UTF8; 

        _engine.StartGame();
        
        int counter = 0;
        while (true)
        {
            if (counter % 200 == 0)
            {
                _engine.GenerateFood();
            }

            _engine.UpdateGame();
            
            RenderFrame();
            
            Thread.Sleep(100); 
            counter++;
        }
    }

    private void RenderFrame()
    {
        Console.SetCursorPosition(0, 0);
        
        var sb = new StringBuilder();
        var field = _engine.GetGameField();
        var units = _engine.GetAllUnits();

        sb.AppendLine(GetFieldString(field));

        sb.AppendLine(GetGlobalStatsString(units));

        sb.AppendLine(GetUnitTableString(units));

        Console.Write(sb.ToString());
    }

    private string GetFieldString(Field field)
    {
        var sb = new StringBuilder();
        sb.AppendLine("╔" + new string('═', field.Size * 2) + "╗"); 

        for (int i = 0; i < field.Size; i++)
        {
            sb.Append("║"); 
            for (int j = 0; j < field.Size; j++)
            {
                var cell = field.FieldCells[i, j];

                if (cell.FieldType == TypeOfFields.Food)
                    sb.Append("🍎");
                else if (cell.FieldType == TypeOfFields.Wall)
                    sb.Append("██");
                else if (cell.FieldType == TypeOfFields.Unit && cell.CurrentUnit != null)
                {
                    if (cell.CurrentUnit.IsDead) 
                        sb.Append("💀");
                    else
                        sb.Append(cell.CurrentUnit.Gender == 0 ? "🧑" : "👩");
                }
                else
                    sb.Append("  "); 
            }
            sb.AppendLine("║"); 
        }
        sb.AppendLine("╚" + new string('═', field.Size * 2) + "╝"); 
        return sb.ToString();
    }

    private string GetGlobalStatsString(List<Unit> units)
    {
        int aliveCount = units.Count(u => !u.IsDead);
        int maleCount = units.Count(u => !u.IsDead && u.Gender == 0);
        int femaleCount = units.Count(u => !u.IsDead && u.Gender == 1);
        
        return $"\n  POPULATION: {aliveCount} (♂ {maleCount} | ♀ {femaleCount})\n" +
               new string('-', TableWidth);
    }

    private string GetUnitTableString(List<Unit> units)
    {
        var sb = new StringBuilder();
        
        sb.AppendLine($"| {"Name",-6} | {"Sex",-3} | {"Health",-12} | {"Energy",-12} | {"Dmg",-5} | {"Arm",-4} | {"Genes (E/B/F)",-15} |");
        sb.AppendLine(new string('-', TableWidth));

        var topUnits = units
            .Where(u => !u.IsDead) 
            .OrderByDescending(u => u.CurrentHealth)
            .Take(10)
            .ToList();

        foreach (var unit in topUnits)
        {
            string sexIcon = unit.Gender == 0 ? "♂" : "♀";
            string hpBar = DrawProgressBar(unit.CurrentHealth, unit.Genes.MaxHealth, 10);
            string enBar = DrawProgressBar(unit.CurrentEnergy, unit.Genes.MaxEnergy, 10);
            
            string dmg = unit.Genes.BaseDamage.ToString("F1");
            string arm = (unit.Genes.ArmorPercent * 100).ToString("F0") + "%";
            
            string genes = $"{unit.Genes.EatModifier:F1}/{unit.Genes.BirthModifier:F1}/{unit.Genes.FightModifier:F1}";

            sb.AppendLine($"| {unit.Name,-6} | {sexIcon,-3} | {hpBar} | {enBar} | {dmg,-5} | {arm,-4} | {genes,-15} |");
        }
        
        for (int i = 0; i < 10 - topUnits.Count; i++)
        {
             sb.AppendLine($"| {"...",-6} | {" ",-3} | {" ",-12} | {" ",-12} | {" ",-5} | {" ",-4} | {" ",-15} |");
        }

        sb.AppendLine(new string('-', TableWidth));
        return sb.ToString();
    }

    private string DrawProgressBar(double current, double max, int width)
    {
        if (max <= 0) max = 1;
        double percent = current / max;
        if (percent < 0) percent = 0;
        if (percent > 1) percent = 1;

        int filledChars = (int)(percent * width); 
        int emptyChars = width - filledChars;    

        // Используем блоки разной плотности
        return "[" + new string('▓', filledChars) + new string('░', emptyChars) + "]";
    }
}