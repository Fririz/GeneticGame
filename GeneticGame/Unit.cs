using GeneticGame.FieldEntities;

namespace GeneticGame;

public class Unit
{
    public int Gender { get; init; } // 0 - male, 1 - female
    public string Name { get; init; }
    public Coordinates Coordinates { get; set; } 
    public UnitGenetics Genes { get; init; } 
    public double CurrentHealth { get; private set; }
    public double CurrentEnergy { get; set; }
    public bool IsDead => CurrentHealth <= 0;

    private int _birthCooldown = GameSettings.BaseBirthCooldown;
    
    public int BirthCooldown
    {
        get => _birthCooldown;
        set
        {
            if (value < 0) _birthCooldown = 0;
            else _birthCooldown = value;
        }
    }

    public Unit(string name, Coordinates coords, UnitGenetics genes)
    {
        Name = name;
        Coordinates = coords;
        Genes = genes;
        CurrentHealth = genes.MaxHealth;
        CurrentEnergy = genes.MaxEnergy;
        Gender = Random.Shared.Next(0, 2);
    }
    public void GetDamage(double damage)
    {
        double effectiveArmor = Math.Min(Genes.ArmorPercent, 0.9);
    
        CurrentHealth -= damage * (1.0 - effectiveArmor);
    
        if (CurrentHealth <= 0) CurrentHealth = 0;
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