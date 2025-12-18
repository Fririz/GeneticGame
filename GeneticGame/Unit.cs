namespace GeneticGame;

public class Unit
{
    public Coordinates Coordinates { get; set; }
    
    public double BirthModifier { get; init; }
    public double EatModifier { get; init; }
    public double FightModifier { get; init; }
    public double Energy { get; private set; }
    
    public double Health { get; private set; }
    public double Damage { get; init; }
    public double Armor { get; init; } //in percent

    public void GetDamage(int damage)
    {
        Health -= (damage * Armor) / 100; // formula
    }
    
    
}