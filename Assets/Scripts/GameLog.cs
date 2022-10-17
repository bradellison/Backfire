using UnityEngine;

public static class GameLog
{
    public static void LogMessage(string message)
    {
        #if UNITY_EDITOR
        Debug.Log(message);
        #endif
    }
    public static void LogWarning(string message)
    {
        #if UNITY_EDITOR
        Debug.LogWarning(message);
        #endif
    }
    public static void LogError(string message)
    {
        #if UNITY_EDITOR
        Debug.LogWarning(message);
        #endif
    }
}