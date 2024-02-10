using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    // Variables Globales
    public GameObject Traduction;

    // Main Interface
    [SerializeField] TMP_Text NewGameText;
    [SerializeField] TMP_Text SettingsText;
    [SerializeField] TMP_Text CreditsText;
    [SerializeField] TMP_Text ExitText;

    // Settings Interface
    [SerializeField] TMP_Text SettingsTitleText;
    [SerializeField] TMP_Text LanguageText;
    [SerializeField] TMP_Text AcceptText;
    [SerializeField] TMP_Text DescriptiveText;

    private void Awake()
    {
        Traduction = GameObject.Find("Traduction");
    }

    // Start is called before the first frame update
    void Start()
    {
        
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

            // Settings Interface
            SettingsTitleText.text = "Settings";
            LanguageText.text = "Select Language:";
            AcceptText.text = "Accept";
            DescriptiveText.text = "Descriptive texts";

        }
        else                                                                   // Spanish
        {

            // Main Interface
            NewGameText.text = "Nueva Partida";
            SettingsText.text = "Configuración";
            CreditsText.text = "Créditos";
            ExitText.text = "Salir del Juego";

            // Settings Interface
            SettingsTitleText.text = "Configuración";
            LanguageText.text = "Selecciona Idioma:";
            AcceptText.text = "Aceptar";
            DescriptiveText.text = "Textos descriptivos";

        }

    }
}
