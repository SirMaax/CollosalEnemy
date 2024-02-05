using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Image[] sliders;

    public void SetPercent(float percent, bool inverted = false)
    {
        foreach (Image bar in sliders)
        {
            if (inverted) bar.fillAmount = 1 - percent;
            else bar.fillAmount = percent;
        }
    }
}