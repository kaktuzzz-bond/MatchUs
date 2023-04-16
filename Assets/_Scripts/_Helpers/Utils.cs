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
    
}