using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class S_RandomKnockInterval : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        var counter = Time.time; // here we set the initial time couter
        var RandTime = 1;
        if (Time.time >= counter + RandTime && animator.GetBool("IsKnocking")==true) 
        {
                animator.Play("Animation_Door");
            counter = Time.time;
            RandTime = Random.Range(1, 3); // each random interval time, the function will be excuted
            

        }
    }

  
    
    
       

    
}
