using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2 : MonoBehaviour
{
    [SerializeField]private MechSystem[] doors;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ToggleDoors());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ToggleDoors()
    {
        while (true)
        {
            yield return new WaitForSeconds(30);
            foreach (var door in doors)
            {
                door.Trigger();
            }
        }
    }
}
