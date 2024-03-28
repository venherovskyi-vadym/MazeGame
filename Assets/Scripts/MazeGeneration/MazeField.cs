using UnityEngine;
using System.Collections.Generic;


public class MazeField
{
    public List<SlotLink> SlotLinks { get; private set; }
    public List<SlotLink> ExitSlotLinks { get; private set; }
    public Dictionary<Vector2Int, List<SlotLink>> SlotLinksByStart { get; private set; } = new Dictionary<Vector2Int, List<SlotLink>>();
    public Dictionary<Vector2Int, List<SlotLink>> SlotLinksByEnd { get; private set; } = new Dictionary<Vector2Int, List<SlotLink>>();
    public Dictionary<Vector2Int, List<SlotLink>> SlotLinksByPosition { get; private set; } = new Dictionary<Vector2Int, List<SlotLink>>();
    public Vector2Int MinPosition { get; private set; }
    public Vector2Int MaxPosition { get; private set; }

    public MazeField(List<SlotLink> slotLinks, List<SlotLink> exitSlotLinks)
    {
        SlotLinks = slotLinks;
        ExitSlotLinks = exitSlotLinks;
        SlotLinks.AddRange(ExitSlotLinks);

        foreach (var item in SlotLinks)
        {
            AddSlotLinkByPosition(item.Start, item, SlotLinksByStart);
            AddSlotLinkByPosition(item.End, item, SlotLinksByEnd);
            AddSlotLinkByPosition(item.Start, item, SlotLinksByPosition);
            AddSlotLinkByPosition(item.End, item, SlotLinksByPosition);

            MinPosition = Vector2Int.Min(MinPosition, item.Start);
            MinPosition = Vector2Int.Min(MinPosition, item.End);
            MaxPosition = Vector2Int.Max(MaxPosition, item.Start);
            MaxPosition = Vector2Int.Max(MaxPosition, item.End);
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
