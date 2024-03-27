using UnityEngine;

public class GenericTile<T> : ScriptableObject
{
    public T None;
    public T North;
    public T West;
    public T South;
    public T East;
    public T NW;
    public T WS;
    public T SE;
    public T EN;
    public T NWS;
    public T WSE;
    public T SEN;
    public T ENW;
    public T NS;
    public T WE;
    public T Cross;

    public T GetTile(int index)
    {
        switch (index)
        {
            case 0:
                return None;
            case 1:
                return West;
            case 2:
                return South;
            case 3:
                return WS;
            case 4:
                return East;
            case 5:
                return WE;
            case 6:
                return SE;
            case 7:
                return WSE;
            case 8:
                return North;
            case 9:
                return NW;
            case 10:
                return NS;
            case 11:
                return NWS;
            case 12:
                return EN;
            case 13:
                return ENW;
            case 14:
                return SEN;
            case 15:
                return Cross;

            default:
                return None;
        }
    }
}
