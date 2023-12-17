using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMech : MonoBehaviour
{
    [Header("Private")] 
    protected Sign sign;
    // Start is called before the first frame update
    protected void Start()
    {
        sign = GetComponentInChildren<Sign>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
