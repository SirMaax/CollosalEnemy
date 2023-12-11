using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private bool test;
    [SerializeField] private Vector2 dir;
    [SerializeField] private GameObject vec;
    [SerializeField] private GameObject vec2;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!test) return;
        transform.Translate(1 * Time.deltaTime * dir);
    }



}
