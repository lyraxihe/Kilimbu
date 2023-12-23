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
       
    }

    // Update is called once per frame
    void Update()
    {

        // Control límites del mapa (Un poco meh de momento la verdad pero funciona)
        if (Content.GetComponent<RectTransform>().anchoredPosition.x > 300)
        {

            Content.GetComponent<RectTransform>().anchoredPosition = new Vector2(300, 0);
            ScrollArea.GetComponent<ScrollRect>().enabled = false;

        }
        else if (Content.GetComponent<RectTransform>().anchoredPosition.x < -1000)
        {

            Content.GetComponent<RectTransform>().anchoredPosition = new Vector2(-1000, 0);
            ScrollArea.GetComponent<ScrollRect>().enabled = false;

        }
        else
            ScrollArea.GetComponent<ScrollRect>().enabled = true;

        // Control límites del compendio de cartas (Un poco meh de momento la verdad pero funciona)
        if (Compendio.GetComponent<RectTransform>().anchoredPosition.x > 5)
        {

            Compendio.GetComponent<RectTransform>().anchoredPosition = new Vector2(5, 0);
            ScrollAreaCompendio.GetComponent<ScrollRect>().enabled = false;

        }
        else if (Compendio.GetComponent<RectTransform>().anchoredPosition.x < -1055)
        {

            Compendio.GetComponent<RectTransform>().anchoredPosition = new Vector2(-1055, 0);
            ScrollAreaCompendio.GetComponent<ScrollRect>().enabled = false;

        }
        else
            ScrollAreaCompendio.GetComponent<ScrollRect>().enabled = true;

    }
}
