using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Vector3 dir;
    [SerializeField] private GameObject vec1;
    [SerializeField] private GameObject vec2;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position,vec1.transform.position,Color.red);
        Debug.DrawRay(transform.position,vec2.transform.position,Color.green);

        float angle = Vector3.Dot(vec1.transform.position, transform.up); 
            
        Debug.Log(angle);
    }



}
