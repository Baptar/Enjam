using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pilepos : MonoBehaviour
{
    public Transform bancPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(bancPos.position.x, bancPos.position.y + 1f, bancPos.position.z);
    }
}
