using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Traduction : MonoBehaviour
{
    public static Traduction instance;
    public GameObject Settings;

    // Idiomas
    public int Language; // 0 - Inglés || 1 - Español

    // Textos descriptivos
    public bool DescriptiveTexts;

    // Texto con turnos
    public bool ShowTurns;

    // Settings Interface
    [SerializeField] TMP_Text SettingsTitleText;
    [SerializeField] TMP_Text LanguageText;
    [SerializeField] TMP_Text AcceptText;
    [SerializeField] TMP_Text DescriptiveText;
    [SerializeField] TMP_Text TurnsText;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(Settings);
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(Settings);
                Destroy(gameObject);
            }
        }
        
        Language = 0; // Inglés por defecto
        DescriptiveTexts = true; // Textos descriptivos activados
        ShowTurns = false; // Textos con turnos desactivados

    }

    void Start()
    {
      
    }

   
    void Update()
    {
        if (Language == 0) //ingles
        {
            // Settings Interface
            SettingsTitleText.text = "Settings";
            LanguageText.text = "Select Language:";
            AcceptText.text = "Accept";
            DescriptiveText.text = "Descriptive texts";
            TurnsText.text = "Show number of turns";
        }
        else if (Language == 1) //español
        {
            // Settings Interface
            SettingsTitleText.text = "Configuración";
            LanguageText.text = "Selecciona Idioma:";
            AcceptText.text = "Aceptar";
            DescriptiveText.text = "Textos descriptivos";
            TurnsText.text = "Mostrar cantidad de turnos";
        }
    }

    public void ChangeDescriptiveTexts()
    {

        DescriptiveTexts = !DescriptiveTexts;

    }

    public void changeTurnsText()
    {
        ShowTurns = !ShowTurns;
    }

}
