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
    
    [Header("Refs")] 
    [SerializeField] public GameObject[] players;
    [SerializeField] private GameObject setCanvas;
    [SerializeField] private static GameObject canvas;
    [SerializeField] private GameObject player1;
    [SerializeField] private TMP_Text timer;
    [SerializeField] private TMP_Text scoreText;

    [Header("Other")] static bool canvasActive = false;
    
    // Start is called before the first frame update
    void Start()
    {
        canvas = setCanvas;
        if (testMode) AMOUNT_PLAYER = setAmountPlayer;
        else AMOUNT_PLAYER = Menu.Amount_Player;
        for (int i = 0; i < 4; i++) players[i].SetActive(false);
        for (int i = 0; i < AMOUNT_PLAYER; i++) players[i].SetActive(true);

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
            timer.SetText("Time left: " + (playTime-i).ToString());
            yield return new WaitForSeconds(1);
        }
        // yield return new WaitForSeconds(playTime);
        SceneManager.LoadScene("ScoreScene");
    }
    

    public static void RestartLevel()
    {
        SceneManager.LoadScene("FirstLevel");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
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
    
    
}
