using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2 : MonoBehaviour
{
    [SerializeField] private float _closingAndOpenIntervall;
    [SerializeField]private MechSystem[] doors;
    [SerializeField] private Bar bar;
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
            int time = 0;
            for (float i = 0; i < _closingAndOpenIntervall; i+=0.2f)
            {
                bar.SetPercent(i/_closingAndOpenIntervall, true);
                yield return new WaitForSeconds(0.2f);
            }
            foreach (var door in doors)
            {
                door.Trigger();
            }
        }
    }
}
