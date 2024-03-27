using UnityEngine;
using System.Collections.Generic;

public class MazePassageGenerator
{
    private const float PassagePercentage = 0.1f;

    public void GeneratePassages(List<SlotLink> slotLinks)
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

        var size = maxPosition - minPosition;
        var passagesCount = (int)(slotLinks.Count * PassagePercentage);
        var additionalPassages = new List<SlotLink>();

        while (passagesCount > 0)
        {
            var slotLink = slotLinks[Random.Range(0, slotLinks.Count)];
            var newPassageDirection = MazePatternGenerator.Rotate(slotLink.Direction);
            var newPassage = new SlotLink(slotLink.Start, slotLink.Start + newPassageDirection);

            if (additionalPassages.Contains(newPassage))
            {
                continue;
            }
            additionalPassages.Add(newPassage);
            passagesCount--;
        }
        
        slotLinks.AddRange(additionalPassages);
    }
}
