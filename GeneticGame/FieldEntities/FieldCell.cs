namespace GeneticGame.FieldEntities;

public class FieldCell
{
    public TypeOfFields FieldType { get; set; }

    public Coordinates Coordinates { get; set; }

    public double FoodAmount { get; set; }
    public Unit? CurrentUnit { get; set; }

    public static FieldCell CreateWallCell(Coordinates coords)
    {
        return new FieldCell(coords, TypeOfFields.Wall);
    }

    public static FieldCell CreateEmptyCell(Coordinates coords)
    {
        return new FieldCell(coords, TypeOfFields.Empty);
    }

    public static FieldCell CreateFoodCell(Coordinates coords, double amount)
    {
        return new FieldCell(coords, TypeOfFields.Food, amount);
    }
    public static FieldCell CreateUnitCell(Coordinates coords, Unit unit)
    {
        return new FieldCell(coords, TypeOfFields.Unit, unit);
    }


private FieldCell(Coordinates coords, TypeOfFields type)
    {
        Coordinates = coords;
        FieldType = type;
        FoodAmount = 0;
        CurrentUnit = null;
    }
    private FieldCell(Coordinates coords, TypeOfFields type, double foodAmount)
    {
        Coordinates = coords;
        FieldType = type;
        FoodAmount = foodAmount;
        CurrentUnit = null;
    }
    private FieldCell(Coordinates coords, TypeOfFields type, Unit unit)
    {
        Coordinates = coords;
        FieldType = type;
        CurrentUnit = unit;
        CurrentUnit = null;
    }
}