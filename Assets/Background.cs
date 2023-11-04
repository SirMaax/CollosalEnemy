using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private GameObject bg1;
    [SerializeField] private GameObject bg2;

    [SerializeField] private float xpos;
    [SerializeField] private float newXpos;

    [SerializeField] private float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = bg1.transform.position;
        if (bg1.transform.position.x < xpos)
        {
            pos.x = newXpos;
            bg1.transform.position = pos;
        }
        pos = bg2.transform.position;
        if (bg2.transform.position.x < xpos)
        {
            pos.x = newXpos;
            bg2.transform.position = pos;
        }
        
        bg1.transform.Translate(Vector3.left * speed);
        bg2.transform.Translate(Vector3.left * speed);
    }
}
