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
    }
    public void GetDamage(int damage)
    {
        CurrentHealth -= damage * Genes.ArmorPercent; // formula
    }

    // public Coordinates DecideNextMove(Field field)
    // {
    //     
    // }
    
}