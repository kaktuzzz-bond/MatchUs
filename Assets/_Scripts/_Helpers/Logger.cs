using System.Diagnostics;
using UnityEngine;

public static class Logger
{
    [Conditional("ENABLE_LOGS")]
    public static void Debug(string logMsg)
    {
        UnityEngine.Debug.Log(logMsg);
    }

    [Conditional("ENABLE_LOGS")]
    public static void DebugWarning(string logMsg)
    {
        UnityEngine.Debug.LogWarning(logMsg);
    }

    [Conditional("ENABLE_LOGS")]
    public static void DebugError(string logMsg)
    {
        UnityEngine.Debug.LogError(logMsg);
    }


    [Conditional("ENABLE_LOGS")]
    public static void DebugDrawRay(Vector3 start, Vector3 direction, Color color, float duration)
    {
        UnityEngine.Debug.DrawRay(start, direction, color, duration);
    }
}