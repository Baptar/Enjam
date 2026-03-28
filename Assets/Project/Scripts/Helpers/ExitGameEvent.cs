using UnityEditor;
using UnityEngine;

public class ExitGameEvent : MonoBehaviour
{
    public void ExitGame()
    {
    #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
    #else
        Application.Quit(); 
    #endif
    }
}
