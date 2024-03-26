using UnityEngine;
using System;

public class SlotLink : IEquatable<SlotLink>
{
    public readonly Vector2Int Start;
    public readonly Vector2Int End;

    public Vector2Int Direction => End - Start;

    public SlotLink(Vector2Int start, Vector2Int end)
    {
        Start = start;
        End = end;
    }

    public override string ToString()
    {
        return $"Start:{Start} End:{End} Direction:{Direction}";
    }

    public override bool Equals(object obj)
    {
        if (obj is SlotLink other)
        {
            return Equals(other);
        }

        return false;
    }

    public bool Equals(SlotLink other)
    {
        if (other == null)
        {
            return false;
        }

        return Start == other.Start && End == other.End;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Start, End);
    }
}
