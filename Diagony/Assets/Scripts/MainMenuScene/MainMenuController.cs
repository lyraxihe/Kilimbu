using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    // Traducción
    public GameObject Traduction;

    // Main Interface
    [SerializeField] GameObject MainMenuPanel;
    [SerializeField] TMP_Text NewGameText;
    [SerializeField] TMP_Text SettingsText;
    [SerializeField] GameObject CreditsPanel;
    [SerializeField] TMP_Text CreditsText;
    [SerializeField] TMP_Text ExitText;
    [SerializeField] GameObject CanvasSettings;


    private void Awake()
    {
        Traduction = GameObject.Find("Traduction");
        Time.timeScale = 1f;
        CanvasSettings = GameObject.Find("CanvasSettings");
        CanvasSettings.GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>(); // Sets the new main camera as the CanvasSettings camera

        if(Traduction.GetComponent<Traduction>().ActivateCredits)
        {

            if (MainMenuPanel == null)
                MainMenuPanel = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
            MainMenuPanel.SetActive(false);
            if (CreditsPanel == null)
                CreditsPanel = GameObject.Find("CanvasCredits").transform.GetChild(0).gameObject;
            CreditsPanel.SetActive(true);

        }

    }

    // Update is called once per frame
    void Update()
    {
        
        // Traductions
        if (Traduction.GetComponent<Traduction>().Language == 0) // English
        {

            // Main Interface
            NewGameText.text = "New Game";
            SettingsText.text = "Settings";
            CreditsText.text = "Credits";
            ExitText.text = "Exit Game";


        }
        else                                                                   // Spanish
        {

            // Main Interface
            NewGameText.text = "Nueva Partida";
            SettingsText.text = "Configuración";
            CreditsText.text = "Créditos";
            ExitText.text = "Salir del Juego";

           

        }

    }
}
