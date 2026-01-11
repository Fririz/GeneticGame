namespace GeneticGame;

public static class GameSettings
{
    //field size
    public static readonly int GameFieldSize = 10;
    //food
    public static readonly int AmountOfFood = 20;
    public static readonly int AmountOfFoodPlaces = 10;

    public static readonly double MaxFoodPerBite = 5;
    //units
    public static readonly int InitialAmountOfUnit = 10;
    public static readonly double PercentOfInitGeneticsRandomization = 0.1;
    //Birth
    public static readonly int BaseBirthCooldown = 15;
    public static readonly int BirthCooldownAfterBirth = 20;
    
    public static double GetPercentOfInitGeneticsRandomization()
    {
        return Random.Shared.NextDouble() > 0.5 ? 1.3 : 0.6;
    } 

    public static UnitGenetics StandartGenetic = 
        new UnitGenetics(1,1,1,5,0.15, 20, 50);
    
}