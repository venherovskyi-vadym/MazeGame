using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;

public class MazeExitGenerator
{
    public List<SlotLink> GenerateExits(List<SlotLink> slotLinks, int exitsCount)
    {
        var minPosition = Vector2Int.zero;
        var maxPosition = Vector2Int.zero;

        foreach (var item in slotLinks)
        {
            minPosition = Vector2Int.Min(minPosition, item.Start);
            minPosition = Vector2Int.Min(minPosition, item.End);
            maxPosition = Vector2Int.Max(maxPosition, item.Start);
            maxPosition = Vector2Int.Max(maxPosition, item.End);
        }

        var edgeSlots = new List<Vector2Int>();

        for (int x = minPosition.x; x < maxPosition.x; x++)
        {
            edgeSlots.Add(new Vector2Int(x, minPosition.y));
            edgeSlots.Add(new Vector2Int(x, maxPosition.y));
        }

        for (int y = minPosition.y; y < maxPosition.y; y++)
        {
            edgeSlots.Add(new Vector2Int(minPosition.x, y));
            edgeSlots.Add(new Vector2Int(maxPosition.x, y));
        }

        var exits = new List<SlotLink>();

        while (exitsCount > 0)
        {
            var index = Random.Range(0, edgeSlots.Count);
            var edgePos = edgeSlots[index];
            var exitPos = edgePos;

            if (Mathf.Abs(edgePos.x) > Mathf.Abs(edgePos.y))
            {
                exitPos.x += (int)Mathf.Sign(edgePos.x);
            }
            else
            {
                exitPos.y += (int)Mathf.Sign(edgePos.y);
            }

            var exit = new SlotLink(edgePos, exitPos);

            exits.Add(exit);
            edgeSlots.RemoveAt(index);
            exitsCount--;
        }

        return exits;
    }
}
