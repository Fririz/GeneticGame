using GeneticGame.FieldEntities;
namespace GeneticGame;


public static class PathFinder
{
    public static (Coordinates? Target, double Distance) FindClosestFood(Coordinates unitPos, Field field)
    {
        Coordinates? bestTarget = null;
        double minDistance = double.MaxValue;
        for (int x = 0; x < field.Size; x++)
        {
            for (int y = 0; y < field.Size; y++)
            {
                var cell = field.FieldCells[x, y];
                if (cell.FieldType == FieldEntities.TypeOfFields.Food)
                {
                    // Manhettan distacne: |x1 - x2| + |y1 - y2|
                    double distance = Math.Abs(unitPos.X - x) + Math.Abs(unitPos.Y - y);

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        bestTarget = new Coordinates(x, y);
                    }
                }
            }
        }

        return (bestTarget, minDistance);
    }
}