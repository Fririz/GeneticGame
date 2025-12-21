namespace GeneticGame.FieldEntities;

public class Field
{
    public Field(int size)
    {
        Size = size;
        FieldCells = new FieldCell[size, size];
    }
    public FieldCell[] GetAllCellsWithType(TypeOfFields type)
    {
        return FieldCells.Cast<FieldCell>().Where(cell => cell.FieldType == type).ToArray();
    }

    public FieldCell[] GetAllCellsWithType(TypeOfFields[] types)
    {
        return FieldCells.Cast<FieldCell>().Where(cell => types.Contains(cell.FieldType)).ToArray();
    }
    
    public int Size {get; init;}
    public FieldCell[,] FieldCells {get; set;}


}