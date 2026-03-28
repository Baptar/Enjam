using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{

    public Transform player;
    private bool inAnim;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        inAnim = false;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!inAnim)
        {
            transform.rotation = player.rotation;
        }

        if(Input.GetKey(KeyCode.Space))
        {
            inAnim = true;
            animator.Play("glouglouMieux");
        }
    }

    void FreezeRotate()
    {
        inAnim = false;
    }


}
