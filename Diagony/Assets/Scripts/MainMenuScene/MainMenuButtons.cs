using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject Levels;

    public void Play()
    {

        //MainMenu.SetActive(false);
        //Levels.SetActive(true);

        SceneManager.LoadScene("MapScene");

    }

    public void Exit()
    {

        Application.Quit();

    }

    //public void Level1()
    //{

    //    SceneManager.LoadScene("MapScene");

    //}
}
