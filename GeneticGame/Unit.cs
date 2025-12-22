using GeneticGame.FieldEntities;

namespace GeneticGame;

public record struct UnitGenetics(
    double BirthModifier,
    double EatModifier,
    double FightModifier,
    double BaseDamage,
    double ArmorPercent,
    double MaxHealth,
    double MaxEnergy
)
{
    public UnitGenetics GetInitializedRandomGenetics()
    {
        double Rnd() => GameSettings.GetPercentOfInitGeneticsRandomization();

        return new UnitGenetics
        {
            BirthModifier = this.BirthModifier * Rnd(),
            EatModifier   = this.EatModifier * Rnd(),
            FightModifier = this.FightModifier * Rnd(),
            BaseDamage    = this.BaseDamage * Rnd(),
            ArmorPercent  = this.ArmorPercent * Rnd(),
            MaxHealth     = this.MaxHealth * Rnd(),
            MaxEnergy     = this.MaxEnergy * Rnd()
        };
    }
}

public class Unit
{
    public int Gender { get; init; } // 0 - male, 1 - female
    public string Name { get; init; }
    public Coordinates Coordinates { get; set; } 
    public UnitGenetics Genes { get; init; } 
    public double CurrentHealth { get; private set; }
    public double CurrentEnergy { get; private set; }


    public Unit(string name, Coordinates coords, UnitGenetics genes)
    {
        Name = name;
        Coordinates = coords;
        Genes = genes;
        CurrentHealth = genes.MaxHealth;
        CurrentEnergy = genes.MaxEnergy;
        Gender = Random.Shared.Next(0, 2);
    }
    public void GetDamage(int damage)
    {
        CurrentHealth -= damage * Genes.ArmorPercent; // formula
    }

    public enum TargetCategory
    {
        Food,
        Mate,   
        Enemy,
        Empty
    }

    public Coordinates DecideNextMove(Field field)
    {       
            
        var targets = PathFinder.FindNearest<TargetCategory>(
            this.Coordinates,
            field,
            cell => IdentifyTarget(cell)
        );

        FieldCell? bestTargetCell = null;
        double bestScore = double.MinValue;

        foreach (var pair in targets)
        {
            TargetCategory category = pair.Key;
            var (distance, cell) = pair.Value;

            double safeDist = distance <= 0 ? 0.1 : distance;

            double modifier = GetModifierForCategory(category);

            double currentScore = modifier / safeDist;

            if (currentScore > bestScore)
            {
                bestScore = currentScore;
                bestTargetCell = cell;
            }
        }

        if (bestTargetCell != null)
        {
            return GetNextStepTo(bestTargetCell.Coordinates);
        }

        return Coordinates;
    }

    private double GetModifierForCategory(TargetCategory category)
    {
        switch (category)
        {
            case TargetCategory.Food: return Genes.EatModifier;
            case TargetCategory.Mate: return Genes.BirthModifier;
            case TargetCategory.Enemy: return Genes.FightModifier;
            default: return 0;
        }
    }
    private TargetCategory IdentifyTarget(FieldCell cell)
    {
        switch (cell.FieldType)
        {
            case TypeOfFields.Food: 
                return TargetCategory.Food;
            
            case TypeOfFields.Unit:
                var otherUnit = cell.CurrentUnit;
                if (otherUnit == null) return TargetCategory.Empty; 

                return (this.Gender != otherUnit.Gender)
                    ? TargetCategory.Mate
                    : TargetCategory.Enemy;
            default: 
                return TargetCategory.Empty; 
        }
    }

    private Coordinates GetNextStepTo(Coordinates target)
    {
        int dx = Math.Sign(target.X - Coordinates.X);
        int dy = Math.Sign(target.Y - Coordinates.Y);
        
        if (Math.Abs(target.X - Coordinates.X) >= Math.Abs(target.Y - Coordinates.Y))
        {
            return new Coordinates(Coordinates.X + dx, Coordinates.Y);
        }
        else
        {
            return new Coordinates(Coordinates.X, Coordinates.Y + dy);
        }
    }
}