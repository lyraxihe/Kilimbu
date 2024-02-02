using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausaButtonMap : MonoBehaviour
{
    [SerializeField] GameObject PanelPausa;
    [SerializeField] GameObject SettingsInterface;
    [SerializeField] GameObject VariablesGlobales;

    // Pause Interface
    [SerializeField] TMP_Text ResumeText;
    [SerializeField] TMP_Text SettingsText;
    [SerializeField] TMP_Text MainMenuText;

    // Settings Interface
    [SerializeField] TMP_Text SettingsTitleText;
    [SerializeField] TMP_Text LanguageText;
    [SerializeField] TMP_Text AcceptText;

    TMP_Text textButton;

    void Start()
    {
        VariablesGlobales = GameObject.Find("VariablesGlobales");
        textButton = gameObject.GetComponentInChildren<TextMeshPro>();
    }


    void Update()
    {

        // Traductions
        if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
        {

            // Pause Interface
            ResumeText.text = "Resume";
            SettingsText.text = "Settings";
            MainMenuText.text = "Main Menu";

            // Settings Interface
            SettingsTitleText.text = "Settings";
            LanguageText.text = "Select Language:";
            AcceptText.text = "Accept";

        }
        else                                                                   // Spanish
        {

            // Pause Interface
            ResumeText.text = "Reanudar";
            SettingsText.text = "Configuración";
            MainMenuText.text = "Menú Principal";

            // Settings Interface
            SettingsTitleText.text = "Configuración";
            LanguageText.text = "Selecciona Idioma:";
            AcceptText.text = "Aceptar";

        }

    }

    public void OnClick()
    {

        if (!VariablesGlobales.GetComponent<VariablesGlobales>().EstaEnPausa)
        {

            PanelPausa.SetActive(true);
            Time.timeScale = 0f;
            VariablesGlobales.GetComponent<VariablesGlobales>().EstaEnPausa = true;

        }
        else
        {

            PanelPausa.SetActive(false);
            Time.timeScale = 1f;
            VariablesGlobales.GetComponent<VariablesGlobales>().EstaEnPausa = false;

        }

    }

    public void Settings()
    {

        PanelPausa.SetActive(!PanelPausa.activeSelf);
        SettingsInterface.SetActive(!SettingsInterface.activeSelf);

    }

    public void AcceptSettings()
    {

        SettingsInterface.SetActive(!SettingsInterface.activeSelf);
        PanelPausa.SetActive(!PanelPausa.activeSelf);

    }

    public void Escapar()
    {


        VariablesGlobales.GetComponent<VariablesGlobales>().EstaEnPausa = false;
        SceneManager.LoadScene("MainMenu");

    }

}
