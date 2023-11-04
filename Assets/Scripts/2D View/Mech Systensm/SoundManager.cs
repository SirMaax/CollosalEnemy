using System;
using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private bool soundIsDisabled;
    private static bool disableSound;
    [SerializeField] AudioSource[] SerialeffectSource;
    static AudioSource[] effectSource;
    
    private void Start()
    {
        disableSound = soundIsDisabled;
        effectSource = new AudioSource[SerialeffectSource.Length];
        effectSource = SerialeffectSource;

        if (disableSound)
        {
            foreach (var ele in effectSource)
            {
                ele.Stop();
            }
        }
    }

    public static void Play(int index)
    {
        if (disableSound) return;
        effectSource[index].Play();
    }
    public static void Stop(int index){
        if (disableSound) return;
        effectSource[index].Stop();
    }
    

}

