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
                return North;
            case 2:
                return West;
            case 3:
                return South;
            case 4:
                return East;
            case 5:
                return NW;
            case 6:
                return WS;
            case 7:
                return SE;
            case 8:
                return EN;
            case 9:
                return NWS;
            case 10:
                return WSE;
            case 11:
                return SEN;
            case 12:
                return ENW;

            default:
                return None;
        }
    }
}
