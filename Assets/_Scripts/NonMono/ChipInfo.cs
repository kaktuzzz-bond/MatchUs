using System;
using UnityEngine;

[Serializable]
public struct ChipInfo
{
    public int shapeIndex;

    public int colorIndex;

    public Vector3 position;

    public Chip.States state;


    public ChipInfo(int shapeIndex, int colorIndex, Vector3 position, Chip.States state)
    {
        this.shapeIndex = shapeIndex;
        this.colorIndex = colorIndex;
        this.position = position;
        this.state = state;
    }
}