using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static Vector3 ScreenToWorldPosition(Camera camera, Vector3 screenPosition)
    {
        screenPosition.z = camera.nearClipPlane;

        return camera.ScreenToWorldPoint(screenPosition);

        //???
        //camera.ScreenPointToRay();
    }


    public static Vector3[] GetRectWorldCorners(RectTransform rt)
    {
        var corners = new Vector3[4];

        rt.GetWorldCorners(corners);

        return corners;
    }


    public static Vector2Int ConvertWorldToBoardCoordinates(Vector3 worldPosition)
    {
        int boardX = Mathf.Abs(Mathf.RoundToInt(worldPosition.x));

        int boardY = Mathf.Abs(Mathf.RoundToInt(worldPosition.y));

        return new Vector2Int(boardX, boardY);
    }


    public static Vector3 ConvertBoardToWorldCoordinates(Vector2Int boardPosition)
    {
        return new Vector3(boardPosition.x, -boardPosition.y);
    }


    public static List<int> GetIndexes(int listCount)
    {
        List<int> list = new();

        for (int i = 0; i < listCount; i++) list.Add(i);

        return list;
    }
}