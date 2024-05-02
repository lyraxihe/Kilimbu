using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PausaButtonMap : MonoBehaviour
{
    [SerializeField] GameObject PanelPausa;
    [SerializeField] Transform SettingsInterface;
    [SerializeField] GameObject VariablesGlobales;

    // Pause Interface
    [SerializeField] TMP_Text ResumeText;
    [SerializeField] TMP_Text SettingsText;
    [SerializeField] TMP_Text MainMenuText;

    // Music management
    public GameObject Music;
    public AudioSource MusicSource;
    public AudioMixerGroup defaultGroup;
    public AudioMixerGroup pausedGroup;



    void Start()
    {
        VariablesGlobales = GameObject.Find("VariablesGlobales");
        SettingsInterface = GameObject.Find("CanvasSettings").transform.GetChild(0);
        Music = GameObject.Find("Music");
        MusicSource = Music.GetComponent<AudioSource>();
        MusicSource.outputAudioMixerGroup = defaultGroup;

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


        }
        else                                                                   // Spanish
        {

            // Pause Interface
            ResumeText.text = "Reanudar";
            SettingsText.text = "Configuraci�n";
            MainMenuText.text = "Men� Principal";

        }

    }

    public void OnClick()
    {

        if (!VariablesGlobales.GetComponent<VariablesGlobales>().EstaEnPausa)
        {

            if (SceneManager.GetActiveScene().name != "MainMenu")
            {

                SettingsInterface.GetComponent<RectTransform>().offsetMin = new Vector2(393.82f, SettingsInterface.GetComponent<RectTransform>().offsetMin.y);
                SettingsInterface.GetComponent<RectTransform>().offsetMax = new Vector2(-393.82f, SettingsInterface.GetComponent<RectTransform>().offsetMax.y);

            }


            PanelPausa.SetActive(true);
            Time.timeScale = 0f;
            VariablesGlobales.GetComponent<VariablesGlobales>().EstaEnPausa = true;
            MusicSource.outputAudioMixerGroup = pausedGroup;

        }
        else
        {

            PanelPausa.SetActive(false);
            Time.timeScale = 1f;
            VariablesGlobales.GetComponent<VariablesGlobales>().EstaEnPausa = false;
            MusicSource.outputAudioMixerGroup = defaultGroup;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Cambiar la m sica cuando se cambia de escena.
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("ON SCENE LOADED");
        Music = GameObject.Find("Music");
        MusicSource = Music.GetComponent<AudioSource>();
        //MusicSource.outputAudioMixerGroup = defaultGroup;
    }
}
