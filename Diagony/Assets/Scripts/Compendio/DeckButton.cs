using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class DeckButton : MonoBehaviour
{

    [SerializeField] GameObject ScrollAreaCompendio;
    [SerializeField] Sprite VolverIcon;
    [SerializeField] Sprite VolverIconHighlight;
    [SerializeField] Sprite VolverIconPressed;
    [SerializeField] Sprite CartasIcon;
    [SerializeField] Sprite CartasIconHighlight;
    [SerializeField] Sprite CartasIconPressed;
    [SerializeField] Sprite CartasIconDisabled;
    [SerializeField] GameObject Map;
    [SerializeField] GameObject PauseButton;
    public GameObject MapButton;

    private SpriteState deckButtonState;
    private Button button;
    //[SerializeField] TMP_Text ButtonText;

    // Music management
    public GameObject Music;
    public AudioSource MusicSource;
    public AudioMixerGroup defaultGroup;
    public AudioMixerGroup pausedGroup;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        deckButtonState = button.spriteState;

        // Encontrar la música sonando para poder editarla
        Music = GameObject.Find("Music");
        MusicSource = Music.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {

        if (!ScrollAreaCompendio.activeSelf)
        {
            //ButtonText.text = "Volver";
            gameObject.GetComponent<Image>().sprite = VolverIcon;
            deckButtonState.pressedSprite = VolverIconPressed;
            deckButtonState.highlightedSprite = VolverIconHighlight;
            button.spriteState = deckButtonState;

            Map.SetActive(false);
            PauseButton.SetActive(false);
            if (MapButton != null)
            {
                MapButton.SetActive(false);
            }

            MusicSource.outputAudioMixerGroup = pausedGroup;
        }
        else
        {
            //ButtonText.text = "Mis Cartas";
            gameObject.GetComponent<Image>().sprite = CartasIcon;
            deckButtonState.disabledSprite = CartasIconDisabled;
            deckButtonState.pressedSprite = CartasIconPressed;
            deckButtonState.highlightedSprite = CartasIconHighlight;
            button.spriteState = deckButtonState;

            Map.SetActive(true);
            PauseButton.SetActive(true);
            if (MapButton != null)
            {
              MapButton.SetActive(true);
            }

            MusicSource.outputAudioMixerGroup = defaultGroup;
        }

        ScrollAreaCompendio.SetActive(!ScrollAreaCompendio.activeSelf);


    }

    /// <summary>
    /// Cambiar la musica cuando se cambia de escena.
    /// </summary>
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// Cambiar la musica cuando se cambia de escena.
    /// </summary>
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Cambiar la musica cuando se cambia de escena.
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "ShopScene")
        {
            Debug.Log("escena shop");
            MapButton = GameObject.Find("VerMapa");
        }
        Music = GameObject.Find("Music");
        MusicSource = Music.GetComponent<AudioSource>();
    }

}
