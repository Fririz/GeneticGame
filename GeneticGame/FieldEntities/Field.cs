namespace GeneticGame.FieldEntities;

public class Field
{
    public Field(int size)
    {
        Size = size;
        FieldCells = new FieldCell[size, size];
    }
    public FieldCell[] GetEmptyFieldCells()
    {
        return FieldCells.Cast<FieldCell>().Where(cell => cell.FieldType == TypeOfFields.Empty).ToArray();
    }
    public int Size {get; set;}
    public FieldCell[,] FieldCells {get; set;}


}