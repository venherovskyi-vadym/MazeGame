using UnityEngine;
using System.Collections.Generic;


public class MazeField
{
    public List<SlotLink> SlotLinks { get; private set; }
    public Dictionary<Vector2Int, List<SlotLink>> SlotLinksByStart { get; private set; } = new Dictionary<Vector2Int, List<SlotLink>>();
    public Dictionary<Vector2Int, List<SlotLink>> SlotLinksByEnd { get; private set; } = new Dictionary<Vector2Int, List<SlotLink>>();

    public MazeField(List<SlotLink> slotLinks)
    {
        SlotLinks = slotLinks;

        foreach (var item in slotLinks)
        {
            AddSlotLinkByPosition(item.Start, item, SlotLinksByStart);
            AddSlotLinkByPosition(item.End, item, SlotLinksByEnd);
        }
    }

    private void AddSlotLinkByPosition(Vector2Int position, SlotLink slotLink, Dictionary<Vector2Int, List<SlotLink>> dictionary)
    {
        if (!dictionary.ContainsKey(position))
        {
            dictionary.Add(position, new List<SlotLink>());
        }

        dictionary[position].Add(slotLink);
    }
}
