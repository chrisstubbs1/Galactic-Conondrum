using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Text highscore;
    public static int shipselection = 1;
    public static int difficulty = 2;
    int mapselection;

    void Start()
    {

         highscore.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        
    }

    public void LoadMap1()
    {
        mapselection = 1;                  
    }

    public void LoadMap2()
    {
        mapselection = 2;
    }

    public void LoadShip1()
    {
        shipselection = 1;
    }

    public void LoadShip2()
    {
        shipselection = 2;
    }

    public void LoadShip3()
    {
        shipselection = 3;
    }

    public void LoadShip4()
    {
        shipselection = 4;
    }

    public void LoadDif1()
    {
        difficulty = 1;
    }

    public void LoadDif2()
    {
        difficulty = 2;
    }

    public void LoadDif3()
    {
        difficulty = 3;
    }

    public void LoadNextScene()
    {
        if(mapselection == 1)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            SceneManager.LoadScene(2);
        }
    }

    public void QuitGame()
    {
        //UnityEditor.EditorApplication.isPlaying = false;             //Only for testing 
        Application.Quit();                                 //Quits app
    }

    public void Backtomenu()
    {
        SceneManager.LoadScene(0);                        //Used to return back to the main menu
    }
}
