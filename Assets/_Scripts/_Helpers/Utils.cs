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
}