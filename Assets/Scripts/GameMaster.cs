using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static int AMOUNT_PLAYER;

    [Header("Setting")] [SerializeField] private int setAmountPlayer;

    [Header("Refs")] 
    [SerializeField] public GameObject[] players;
    // Start is called before the first frame update
    void Start()
    {
        if(setAmountPlayer != -1) AMOUNT_PLAYER = setAmountPlayer;
        for (int i = 0; i < 4; i++) players[i].SetActive(false);
        for (int i = 0; i < AMOUNT_PLAYER; i++) players[i].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
