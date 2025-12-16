namespace GeneticGame;

public abstract class Unit
{
    public Coordinates Coordinates { get; set; }
    
    public double BirthModifier { get; init; }
    public int EatModifier { get; init; }
    
    public int Speed { get; init; } // tiles per second
    public double Health { get; private set; }
    public double Damage { get; init; }
    public double Armor { get; init; } //in percent

    public void GetDamage(int damage)
    {
        Health -= (damage * Armor) / 100; // formula
    }
    public abstract void Move();
    
}