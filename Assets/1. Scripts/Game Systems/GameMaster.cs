using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public static int AMOUNT_PLAYER;
    public static float SCORE;
    
    [Header("Setting")] 
    [SerializeField] private int setAmountPlayer;
    [SerializeField] private int playTime;
    [SerializeField] private bool testMode;
    [SerializeField] private bool noTime;
    [SerializeField] private String _timerText;
    
    [Header("Refs")] 
    [SerializeField] public GameObject[] players;
    [SerializeField] private GameObject setCanvas;
    [SerializeField] private static GameObject canvas;
    [SerializeField] private TMP_Text timer;
    [SerializeField] private TMP_Text scoreText;
    [Header("Other")] static bool canvasActive = false;
    [SerializeField] private EnemySpawner _enemySpawner;
    // Start is called before the first frame update
    void Start()
    {
        canvas = setCanvas;
        if (testMode) AMOUNT_PLAYER = setAmountPlayer;
        // else AMOUNT_PLAYER = Menu.Amount_Player;
        // for (int i = 0; i < 4; i++) players[i].SetActive(false);
        // for (int i = 0; i < AMOUNT_PLAYER; i++) players[i].SetActive(true);

        if (!noTime) StartCoroutine(LevelTime());
        // if(skipMainMenu)SceneManager.LoadScene("FirstLevel");
        // else SceneManager.LoadScene("MainMenu");
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.SetText("Score " + ((int)SCORE).ToString());        
    }

    IEnumerator LevelTime()
    {
        for (int i = 0; i < playTime; i++)
        {
            timer.SetText(_timerText + (playTime-i).ToString());
            if (playTime - i == 30)
            {
                _enemySpawner.amountSpawnEnemies = 3;
            }
            if (playTime-i == 10)
            {
                //Start Different end sound
            }
            yield return new WaitForSeconds(1);
        }
        Menu.LoadScoreScene();
    }
    

    public static void RestartLevel()
    {
       Menu.Restart();
    }

    public void MainMenu()
    {
        Menu.MainMenu();
    }

    public static void ToggleGui()
    {
        if (canvasActive)canvas.SetActive(false);
        else canvas.SetActive(true);
        canvasActive = !canvasActive;
    }

    public void HideGui()
    {
        canvas.SetActive(false);
        canvasActive = false;
    }

    public static void ChangeScoreBy(float score)
    {
        SCORE += score;
        
    }

    public void PlayerJoins()
    {
        AMOUNT_PLAYER += 1;
        Debug.Log("Player Joins");
    }
    
    
}
