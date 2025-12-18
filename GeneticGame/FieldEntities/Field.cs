namespace GeneticGame.FieldEntities;

public class Field
{
    public Field(int size)
    {
        Size = size;
        FieldCells = new FieldCell[size, size];
    }
    public int Size {get; set;}
    public FieldCell[,] FieldCells {get; set;}


}