using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRecorder : MonoBehaviour
{

   public void OnTriggerEnter(Collider collision)
   {
      if (collision.gameObject.tag == "Player")
      {
         Debug.Log("RECORDING");
      }
   }
   
   public void OnTriggerExit(Collider collision)
   {
      if (collision.gameObject.tag == "Player")
      {
         Debug.Log("STOP RECORDING");
      }
   }
   
}
