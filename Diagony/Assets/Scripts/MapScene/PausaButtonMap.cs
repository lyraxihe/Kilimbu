using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausaButtonMap : MonoBehaviour
{
    [SerializeField] GameObject PanelPausa;
    [SerializeField] Transform SettingsInterface;
    [SerializeField] GameObject VariablesGlobales;

    // Pause Interface
    [SerializeField] TMP_Text ResumeText;
    [SerializeField] TMP_Text SettingsText;
    [SerializeField] TMP_Text MainMenuText;

    // Settings Interface
    [SerializeField] TMP_Text SettingsTitleText;
    [SerializeField] TMP_Text LanguageText;
    [SerializeField] TMP_Text AcceptText;
    [SerializeField] TMP_Text DescriptiveText;

    TMP_Text textButton;

    void Start()
    {
        VariablesGlobales = GameObject.Find("VariablesGlobales");
        SettingsInterface = GameObject.Find("CanvasSettings").transform.GetChild(0);
        SettingsTitleText = SettingsInterface.GetChild(0).GetComponent<TMP_Text>();
        LanguageText = SettingsInterface.GetChild(1).GetComponent<TMP_Text>();
        AcceptText = SettingsInterface.GetChild(3).GetChild(0).GetComponent<TMP_Text>();
        DescriptiveText = SettingsInterface.GetChild(4).GetComponent<TMP_Text>(); ;
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
            DescriptiveText.text = "Descriptive texts";

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
            DescriptiveText.text = "Textos descriptivos";

        }

    }

    public void OnClick()
    {

        if (!VariablesGlobales.GetComponent<VariablesGlobales>().EstaEnPausa)
        {

            if(SceneManager.GetActiveScene().name != "MainMenu")
            {

                SettingsInterface.GetComponent<RectTransform>().offsetMin = new Vector2(552.655f, SettingsInterface.GetComponent<RectTransform>().offsetMin.y);
                SettingsInterface.GetComponent<RectTransform>().offsetMax = new Vector2(-552.655f, SettingsInterface.GetComponent<RectTransform>().offsetMax.y);

            }


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
        SettingsInterface.gameObject.SetActive(!SettingsInterface.gameObject.activeSelf);

    }

    public void AcceptSettings()
    {

        SettingsInterface.gameObject.SetActive(!SettingsInterface.gameObject.activeSelf);
        PanelPausa.SetActive(!PanelPausa.activeSelf);

    }

    public void Escapar()
    {


        VariablesGlobales.GetComponent<VariablesGlobales>().EstaEnPausa = false;
        SceneManager.LoadScene("MainMenu");

    }

}
