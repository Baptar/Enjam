using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AppearParc : MonoBehaviour
{
    [SerializeField] private GameObject parc;
    [SerializeField] private float highUntilWhere = 10f;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float timeBetweenParc = 2f;

    private bool move = false;
    private Vector3 newPosition;
    
    public void ParcAppear()
    {
        move = true;
        StartCoroutine(StopMovement());
    }

    private void Start()
    {
        newPosition = parc.transform.position + Vector3.up * highUntilWhere;
    }
    private void Update()
    {
        if (move)
        {
            parc.gameObject.transform.position = Vector3 .MoveTowards(parc.gameObject.transform.position, newPosition, speed * Time.deltaTime);
        }
    }

    IEnumerator StopMovement()
    {
        yield return new WaitForSeconds(timeBetweenParc); 
        move = false;
    }
}