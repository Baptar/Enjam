 using UnityEngine;

public class QuitButton : MonoBehaviour
{
        public void Quit()
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Click");
            
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.ExitPlaymode();
            #else
                    Application.Quit();
            #endif
        }
}
