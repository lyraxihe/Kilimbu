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
    public bool VerMapa;

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
        VerMapa = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "ShopScene")
        {
            gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
        }
        // Control l�mites del mapa (Un poco meh de momento la verdad pero funciona)
        if (Content.GetComponent<RectTransform>().anchoredPosition.x > 30)
        {

            Content.GetComponent<RectTransform>().anchoredPosition = new Vector2(30, Content.GetComponent<RectTransform>().anchoredPosition.y);
            ScrollArea.GetComponent<ScrollRect>().enabled = false;

        }
        else if (Content.GetComponent<RectTransform>().anchoredPosition.x < -3500)
        {

            Content.GetComponent<RectTransform>().anchoredPosition = new Vector2(-3500, Content.GetComponent<RectTransform>().anchoredPosition.y);
            ScrollArea.GetComponent<ScrollRect>().enabled = false;

        }
        else
            ScrollArea.GetComponent<ScrollRect>().enabled = true;

        // Control l�mites del compendio de cartas (Un poco meh de momento la verdad pero funciona)
        if (Compendio.GetComponent<RectTransform>().anchoredPosition.x > 5)
        {

            Compendio.GetComponent<RectTransform>().anchoredPosition = new Vector2(5, 0);
            ScrollAreaCompendio.GetComponent<ScrollRect>().enabled = false;

        }
        else if (Compendio.GetComponent<RectTransform>().anchoredPosition.x < -2186)
        {

            Compendio.GetComponent<RectTransform>().anchoredPosition = new Vector2(-2186, 0);
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
