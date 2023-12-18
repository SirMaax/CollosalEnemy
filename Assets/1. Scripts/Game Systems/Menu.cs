using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    public static int Amount_Player;
    public int type;
    public static bool hardMode = false;
    [SerializeField] private TMP_Text score;
    void Start()
    {
        if (type == 1)
        {
            score.SetText(GameMaster.SCORE.ToString());
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGameWithPlayers(int player)
    {
        Amount_Player = player;
        SceneManager.LoadScene("FirstLevelV2");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Restart()
    {
        SceneManager.LoadScene("FirstLevelV2");
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
