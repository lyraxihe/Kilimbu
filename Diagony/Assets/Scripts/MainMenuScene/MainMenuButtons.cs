using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] GameObject Traduction;
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject Levels;
    [SerializeField] GameObject OptionsPanel;
    [SerializeField] GameObject CreditsPanel;
    [SerializeField] GameObject CanvasPause;




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
        if (OptionsPanel == null)
            OptionsPanel = GameObject.Find("CanvasSettings").transform.GetChild(0).gameObject;
        OptionsPanel.gameObject.SetActive(!OptionsPanel.gameObject.activeSelf);

    }

    public void OptionsAceptar()
    {

        OptionsPanel.gameObject.SetActive(!OptionsPanel.gameObject.activeSelf);
        if(SceneManager.GetActiveScene().name == "MainMenu")
        {

            if (MainMenu == null)
                MainMenu = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
            MainMenu.SetActive(!MainMenu.activeSelf);

        }
        else
        {

            CanvasPause = GameObject.Find("CanvasPause");
            CanvasPause.transform.GetChild(0).gameObject.SetActive(true);

        }

    }

    public void Credits()
    {

        MainMenu.SetActive(!MainMenu.activeSelf);
        if (CreditsPanel == null)
            CreditsPanel = GameObject.Find("CanvasCredits").transform.GetChild(0).gameObject;
        CreditsPanel.gameObject.SetActive(!CreditsPanel.gameObject.activeSelf);

    }

    public void CreditsReturn()
    {

        CreditsPanel.gameObject.SetActive(!CreditsPanel.gameObject.activeSelf);
        if (Traduction == null)
            Traduction = GameObject.Find("Traduction");
        Traduction.GetComponent<Traduction>().ActivateCredits = false;
        if (MainMenu == null)
            MainMenu = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
        MainMenu.SetActive(true);

    }

}
