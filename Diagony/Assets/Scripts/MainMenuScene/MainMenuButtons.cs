using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject Levels;
    [SerializeField] GameObject OptionsPanel;

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

    public void Options()
    {

        MainMenu.SetActive(!MainMenu.activeSelf);
        OptionsPanel.SetActive(!OptionsPanel.activeSelf);

    }

    public void OptionsAceptar()
    {

        OptionsPanel.SetActive(!OptionsPanel.activeSelf);
        MainMenu.SetActive(!MainMenu.activeSelf);

    }

    //public void Level1()
    //{

    //    SceneManager.LoadScene("MapScene");

    //}
}
