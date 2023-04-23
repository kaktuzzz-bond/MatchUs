using System.Collections.Generic;
using UnityEngine;

public class ChipLogger
{
    private readonly Stack<ChipRecord> _log;


    public ChipLogger()
    {
        _log = new Stack<ChipRecord>();
    }


    public void Push(Vector3 worldPosition, ChipStateEnum stateEnum)
    {
        Vector2Int boardPosition = Utils.ConvertWorldToBoardCoordinates(worldPosition);
        
        _log.Push(new ChipRecord(boardPosition, stateEnum));
    }
    
    public void Push(Vector2Int boardPosition, ChipStateEnum stateEnum)
    {
        _log.Push(new ChipRecord(boardPosition, stateEnum));
    }


    public void Push(ChipRecord chipRecord)
    {
        _log.Push(chipRecord);
    }


    public ChipRecord Pop()
    {
        return _log.Pop();
    }


    public ChipRecord Peek()
    {
        return _log.Peek();
    }
}

public struct ChipRecord
{
    public Vector2Int boardPosition;

    public ChipStateEnum stateEnum;


    public ChipRecord(Vector2Int boardPosition, ChipStateEnum stateEnum)
    {
        this.boardPosition = boardPosition;
        this.stateEnum = stateEnum;
    }
}