using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasSingleton : MonoBehaviour
{
    public static CanvasSingleton instance;
    [SerializeField] GameObject ScrollArea;
    [SerializeField] GameObject Content;
    [SerializeField] GameObject ScrollAreaCompendio;
    [SerializeField] GameObject Compendio;
    [SerializeField] GameObject Map;
    [SerializeField] GameObject HealthBarPersonaje;
    public bool VerMapa;
    public bool barridoTerminado;

    // SoundFX Management
    public AudioSource LlegarTopeMapaSound;




    private void Awake() //sigleton
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        barridoTerminado = false;
        VerMapa = false;
        Content.GetComponent<RectTransform>().anchoredPosition = new Vector2(-3850, Content.GetComponent<RectTransform>().anchoredPosition.y);

        // Find SoundFx
        LlegarTopeMapaSound = GameObject.Find("LlegarTopeMapa_SoundFX").GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        //if (SceneManager.GetActiveScene().name == "ShopScene")
        //{
            gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
       // }

        if (SceneManager.GetActiveScene().name == "CombatScene")
        {
            HealthBarPersonaje.SetActive(false);
        }
        else
        {
            HealthBarPersonaje.SetActive(true);
        }

        // Control límites del mapa (Un poco meh de momento la verdad pero funciona)
        if (Content.GetComponent<RectTransform>().anchoredPosition.x > 355)
        {

            Content.GetComponent<RectTransform>().anchoredPosition = new Vector2(350, Content.GetComponent<RectTransform>().anchoredPosition.y);
            ScrollArea.GetComponent<ScrollRect>().enabled = false;
            if (barridoTerminado)
            {
                LlegarTopeMapaSound.Play();
            }
               

        }
        else if (Content.GetComponent<RectTransform>().anchoredPosition.x < -5005)
        {

            Content.GetComponent<RectTransform>().anchoredPosition = new Vector2(-5000, Content.GetComponent<RectTransform>().anchoredPosition.y);
            ScrollArea.GetComponent<ScrollRect>().enabled = false;

            if (barridoTerminado)
            {
                LlegarTopeMapaSound.Play();
            }
        }
        else
            ScrollArea.GetComponent<ScrollRect>().enabled = true;

        // Control límites del compendio de cartas (Un poco meh de momento la verdad pero funciona)
        if (Compendio.GetComponent<RectTransform>().anchoredPosition.x > 10)
        {

            Compendio.GetComponent<RectTransform>().anchoredPosition = new Vector2(5, Compendio.GetComponent<RectTransform>().anchoredPosition.y);
            ScrollAreaCompendio.GetComponent<ScrollRect>().enabled = false;

        }
        else if (Compendio.GetComponent<RectTransform>().anchoredPosition.x < -2191)
        {

            Compendio.GetComponent<RectTransform>().anchoredPosition = new Vector2(-2186, Compendio.GetComponent<RectTransform>().anchoredPosition.y);
            ScrollAreaCompendio.GetComponent<ScrollRect>().enabled = false;

        }
        else
            ScrollAreaCompendio.GetComponent<ScrollRect>().enabled = true;


        if (MapScene_active() || VerMapa)
        {
            Map.SetActive(true);
        }
        else if (!MapScene_active()/* && !VerMapa*/)
        {
            Map.SetActive(false);
        }
    }

    public bool MapScene_active()
    {
        if (SceneManager.GetActiveScene().name == "MapScene")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void MapaVisible(bool visibilidad)
    {
        Map.SetActive(visibilidad);
        VerMapa = visibilidad;
    }
}
