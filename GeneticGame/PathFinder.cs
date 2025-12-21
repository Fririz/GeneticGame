using GeneticGame.FieldEntities;
namespace GeneticGame;

public static class PathFinder
{
    public static Dictionary<T, (int Distance, FieldCell Cell)> FindNearest<T>(
        Coordinates startPos, 
        Field field, 
        Func<FieldCell, T?> selector) where T : notnull
    {
        var result = new Dictionary<T, (int Distance, FieldCell Cell)>();

        foreach (var cell in field.FieldCells)
        {

            if (cell.Coordinates.X == startPos.X && cell.Coordinates.Y == startPos.Y)
                continue;

            T? typeKey = selector(cell);

            if (typeKey == null) continue;

            int distance = Math.Abs(startPos.X - cell.Coordinates.X) + Math.Abs(startPos.Y - cell.Coordinates.Y);

            if (!result.ContainsKey(typeKey) || distance < result[typeKey].Distance)
            {
                result[typeKey] = (distance, cell);
            }
        }

        return result;
    }
}