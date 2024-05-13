using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    // Credits Interface
    [SerializeField] TMP_Text CreditsTitleText;
    [SerializeField] TMP_Text FelipeRolText;
    [SerializeField] TMP_Text XavierRolText;
    [SerializeField] TMP_Text LyraRolText;
    [SerializeField] TMP_Text NereaRolText;
    [SerializeField] TMP_Text AlejandraRolText;
    [SerializeField] TMP_Text FelipeRolText2;
    [SerializeField] TMP_Text ReturnCreditsText;

    // Créditos
    public bool ActivateCredits; // Si es true, CreditsPanel debe ser activado al volver al Menú Principal

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
        ActivateCredits = false;

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

            // Credits Interface
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                if (CreditsTitleText == null)
                    CreditsTitleText = GameObject.Find("CanvasCredits").transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<TMP_Text>();
                CreditsTitleText.text = "Credits";
                if (FelipeRolText == null)
                    FelipeRolText = GameObject.Find("CanvasCredits").transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>();
                FelipeRolText.text = "Lead Designer / Producer";
                if (XavierRolText == null)
                    XavierRolText = GameObject.Find("CanvasCredits").transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.GetChild(2).GetComponent<TMP_Text>();
                XavierRolText.text = "Programmer";
                if (LyraRolText == null)
                    LyraRolText = GameObject.Find("CanvasCredits").transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.GetChild(4).GetComponent<TMP_Text>();
                LyraRolText.text = "Programmer";
                if (NereaRolText == null)
                    NereaRolText = GameObject.Find("CanvasCredits").transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.GetChild(6).GetComponent<TMP_Text>();
                NereaRolText.text = "Lead Artist";
                if (AlejandraRolText == null)
                    AlejandraRolText = GameObject.Find("CanvasCredits").transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.GetChild(8).GetComponent<TMP_Text>();
                AlejandraRolText.text = "Artist";
                if (FelipeRolText2 == null)
                    FelipeRolText2 = GameObject.Find("CanvasCredits").transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.GetChild(10).GetComponent<TMP_Text>();
                FelipeRolText2.text = "Sound Designer & Music";
                if (ReturnCreditsText == null)
                    ReturnCreditsText = GameObject.Find("CanvasCredits").transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).GetComponent<TMP_Text>();
                ReturnCreditsText.text = "Return";
            }
        }
        else if (Language == 1) //español
        {
            // Settings Interface
            SettingsTitleText.text = "Configuración";
            LanguageText.text = "Selecciona Idioma:";
            AcceptText.text = "Aceptar";
            DescriptiveText.text = "Textos descriptivos";
            TurnsText.text = "Mostrar cantidad de turnos";

            // Credits Interface
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                if (CreditsTitleText == null)
                    CreditsTitleText = GameObject.Find("CanvasCredits").transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<TMP_Text>();
                CreditsTitleText.text = "Créditos";
                if (FelipeRolText == null)
                    FelipeRolText = GameObject.Find("CanvasCredits").transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>();
                FelipeRolText.text = "Diseñador Principal / Productor";
                if (XavierRolText == null)
                    XavierRolText = GameObject.Find("CanvasCredits").transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.GetChild(2).GetComponent<TMP_Text>();
                XavierRolText.text = "Programador";
                if (LyraRolText == null)
                    LyraRolText = GameObject.Find("CanvasCredits").transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.GetChild(4).GetComponent<TMP_Text>();
                LyraRolText.text = "Programadora";
                if (NereaRolText == null)
                    NereaRolText = GameObject.Find("CanvasCredits").transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.GetChild(6).GetComponent<TMP_Text>();
                NereaRolText.text = "Artista Principal";
                if (AlejandraRolText == null)
                    AlejandraRolText = GameObject.Find("CanvasCredits").transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.GetChild(8).GetComponent<TMP_Text>();
                AlejandraRolText.text = "Artista";
                if (FelipeRolText2 == null)
                    FelipeRolText2 = GameObject.Find("CanvasCredits").transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.GetChild(10).GetComponent<TMP_Text>();
                FelipeRolText2.text = "Diseñador de Sonido y Música";
                if (ReturnCreditsText == null)
                    ReturnCreditsText = GameObject.Find("CanvasCredits").transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).GetComponent<TMP_Text>();
                ReturnCreditsText.text = "Volver";
            }
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
