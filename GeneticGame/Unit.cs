namespace GeneticGame;

public record UnitGenetics(
    double BirthModifier, 
    double EatModifier, 
    double FightModifier, 
    double BaseDamage, 
    double ArmorPercent
);

public class Unit
{
    public string Name { get; init; }
    public Coordinates Coordinates { get; set; } 
    public UnitGenetics Genes { get; init; } 
    
    public double Energy { get; private set; }
    public double Health { get; private set; }

    // Конструктор стал чистым
    public Unit(string name, Coordinates coords, UnitGenetics genes, double startHealth, double startEnergy)
    {
        Name = name;
        Coordinates = coords;
        Genes = genes;
        Health = startHealth;
        Energy = startEnergy;
    }
    public void GetDamage(int damage)
    {
        Health -= (damage * Genes.ArmorPercent) / 100; // formula
    }
    
}