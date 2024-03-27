using UnityEngine;
using System.Collections.Generic;

public class TileIndexProvider
{
    public int GetIndex(Vector2Int position, MazeField mazeField)
    {
        var result = 0;
        var nwSlot = position;
        var wsSlot = position + Vector2Int.down;
        var seSlot = position + Vector2Int.down + Vector2Int.right;
        var enSlot = position + Vector2Int.right;

        AccountNeighbouringSlots(ref result, 1, nwSlot, wsSlot, mazeField);
        AccountNeighbouringSlots(ref result, 2, wsSlot, seSlot, mazeField);
        AccountNeighbouringSlots(ref result, 4, seSlot, enSlot, mazeField);
        AccountNeighbouringSlots(ref result, 8, enSlot, nwSlot, mazeField);

        return result;
    }

    private void AccountNeighbouringSlots(ref int index, int factor, Vector2Int basePosition, Vector2Int checkPosition, MazeField mazeField)
    {
        if (mazeField.SlotLinks.Contains(new SlotLink(basePosition, checkPosition)) || mazeField.SlotLinks.Contains(new SlotLink(checkPosition, basePosition)))
        {
            return;
        }

        index += factor;
    }
}
