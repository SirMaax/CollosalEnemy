using System;
using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private bool soundIsDisabled;
    [SerializeField] private bool isBackgroundMusicEnabled;
    private static bool disableSound;
    [SerializeField] AudioSource[] SerialeffectSource;
    static AudioSource[] effectSource;

    public enum Sounds
    {
        IsAttackedAlarm,
        EnemyHit,
        ButtonPressed,
        NotEnoughEnergy,
        EjectShell,
        MechGotHit,
        MechDeflectedShot,
        NoEnergyLeft,
        Interact,
        BackgroundMusic,
        TimerSound,
        PickUp, 
        Drop,
        Jump,
        NewEnemyAppeared,
        EnemyLeft
    }
    
    private void Start()
    {
        disableSound = soundIsDisabled;
        effectSource = new AudioSource[SerialeffectSource.Length];
        effectSource = SerialeffectSource;

        if (!isBackgroundMusicEnabled)
        {
            effectSource[(int)Sounds.BackgroundMusic].Stop();
            effectSource[(int)Sounds.TimerSound].Stop();
        }
        
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
    public static void Play(Sounds sound)
    {
        if (disableSound) return;
        effectSource[(int)sound].Play();
    }
    public static void Stop(int index){
        if (disableSound) return;
        effectSource[index].Stop();
    }
    public static void Stop(Sounds sound){
        if (disableSound) return;
        effectSource[(int)sound].Stop();
    }

}

