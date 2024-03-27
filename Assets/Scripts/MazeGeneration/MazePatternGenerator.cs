using UnityEngine;
using System.Collections.Generic;

public class MazePatternGenerator
{
    public List<SlotLink> GeneratePatternLinks(int size)
    {
        var slotsCount = size * size;
        var addedSlots = new List<Vector2Int>(slotsCount);
        var result = new List<SlotLink>(slotsCount);
        result.Add(new SlotLink(new Vector2Int(0, 0), new Vector2Int(0, 1)));
        addedSlots.Add(result[result.Count - 1].Start);
        addedSlots.Add(result[result.Count - 1].End);
        Vector2Int direction;
        Vector2Int turnPosition;

        while (result.Count < slotsCount - 1)
        {
            direction = result[result.Count - 1].Direction;
            turnPosition = Rotate(direction) + result[result.Count - 1].End;

            if (addedSlots.Contains(turnPosition))
            {
                addedSlots.Add(direction + result[result.Count - 1].End);
                result.Add(new SlotLink(result[result.Count - 1].End, direction + result[result.Count - 1].End));
            }
            else
            {
                addedSlots.Add(turnPosition);
                result.Add(new SlotLink(result[result.Count - 1].End, turnPosition));
            }
        }

        return result;
    }

    public static Vector2Int Rotate(Vector2Int vector)
    {
        return new Vector2Int(-vector.y, vector.x);
    }
}
