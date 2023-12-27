using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UIElements;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    public static int Amount_Player;
    public static int amountLevel = 1;    
    public static bool hardMode = false;
    public static bool clearedLevel = true;
    public static int currentLevel = 1;

    [SerializeField] private TMP_Text levelStatusText;
    [SerializeField] private GameObject nextLevelButton;
    [SerializeField] private GameObject restartLevelButton;
    public void Awake()
    {
        String text = clearedLevel ? "Level Cleared!" : "Level Failed!";
        if(levelStatusText!=null)levelStatusText.SetText(text);
        if(nextLevelButton!=null)nextLevelButton.SetActive(clearedLevel);
        if(restartLevelButton!=null)restartLevelButton.SetActive(!clearedLevel);
    }

    public void StartGameWithPlayers(int player)
    {
        Amount_Player = player;
        LoadLevel(currentLevel);
    }

    public static void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public static void Restart()
    {
        LoadLevel(currentLevel);
    }

    public static void NextLevel()
    {
        currentLevel += 1;
        if (currentLevel > amountLevel) MainMenu();
        else LoadLevel(currentLevel);
    }

    private static void LoadLevel(int number)
    {
        if (currentLevel > amountLevel) MainMenu();
        String level = "Level" + number.ToString();
        SceneManager.LoadScene(level);
    }

    public static void LoadScoreScene()
    {
        SceneManager.LoadScene("ScoreScene"); 
    }
    
    public void Quit()
    {
        Application.Quit();
    }

    public void HardMode()
    {
        if (hardMode) hardMode = false;
        else hardMode = true;
    }
}
