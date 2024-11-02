using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Exit : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Should Exit Game");
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #else
        Application.Quit(); 
        #endif
    }
}
