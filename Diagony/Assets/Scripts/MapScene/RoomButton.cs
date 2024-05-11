using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour
{

    [SerializeField] GameObject VariablesGlobales;

    [SerializeField] public float x; // Coordinada X
    [SerializeField] public float y; // Coordinada Y


    [SerializeField] public float id; // id
    public GameObject[] conections = new GameObject[4];
    GameObject[] conectionsLines = new GameObject[4];
    public List<GameObject> conectedLines;

    [SerializeField] public int numContections;
    [SerializeField] public GameObject MapController_;
    [SerializeField] GameObject CanvasSingleton;
    [SerializeField] public GameObject LinePrefab;
    [SerializeField] public GameObject Content;
    private RectTransform ContentTransform;

    public int Columna; // Columna que ocupa en el mapa


    public int RoomType; // 0: Combate | 1: Cofre | 2: Tienda | 3: Boss | 4: Tutorial

    public bool Visitado;

    [SerializeField] float minTam = 0.6f, maxTam = 1; // Tamaños sala
    private bool lineasComprobadas;

    void Start()
    {
        lineasComprobadas = false;
        CanvasSingleton = GameObject.Find("CanvasSingleton");
        VariablesGlobales = GameObject.Find("VariablesGlobales");
        Visitado = false;
        createLines();

    }


    void Update()
    {

        if(VariablesGlobales.GetComponent<VariablesGlobales>().EstaEnPausa)
            GetComponent<Button>().enabled = false;
        else
            GetComponent<Button>().enabled = true;

        ControlButtonSize();

        if (!lineasComprobadas) //sirve para comprobar que las lineas cambien de color cuando deban
        {
            for (int i = 0; i < numContections; i++)
            {
                if(conections[i].GetComponent<RoomButton>().Visitado == true)
                {
                    desactivarLineasNoVisitadas();
                    lineasComprobadas = true;
                    break;
                }
            }
        }

    }

    public void OnClick()
    {
        
        CanvasSingleton.GetComponent<CanvasSingleton>().VerMapa = false;
        gameObject.GetComponent<Button>().interactable = false;
        Visitado = true;

        for (int i = 0; i < numContections; i++)
        {
            conections[i].GetComponent<Button>().interactable = true;
            conectionsLines[i].GetComponent<Line>().ChangeColor(1); //cambia el color a por visitar
            conectionsLines[i].GetComponent<Line>().porVisitar = true;
        }
        for (int i = 0; i < conectedLines.Count; i++)
        {
            if (conectedLines[i].GetComponent<Line>().porVisitar)
            {
                conectedLines[i].GetComponent<Line>().porVisitar=false;
                conectedLines[i].GetComponent<Line>().ChangeColor(2); //cambia el color a visitado
                conectedLines[i].GetComponent<Line>().visitado = true;
            }
        }

        MapController_.GetComponent<MapController>().ComprobarInactivos(); //llama a la funcion de comprobar inactivos en map controller para desactivar las salas necesarias

        // Recoloca el mapa para apuntar a la sala en la que estará
        if (Columna < 15) // Si no es la sala del boss
        {
            Debug.Log("Mapa Columna");
            ContentTransform = Content.GetComponent<RectTransform>();
            ContentTransform.anchoredPosition = new Vector2(MapController_.GetComponent<MapController>().ColumnasPos[Columna], ContentTransform.anchoredPosition.y);

        }

        if (RoomType == 1)      // Sala Cofre
            SceneManager.LoadScene("ChestScene");
        else if (RoomType == 2) // Sala Tienda
            SceneManager.LoadScene("ShopScene");
        else if (RoomType == 3) // Sala Boss
        {

            VariablesGlobales.GetComponent<VariablesGlobales>().Boss = true;
            SceneManager.LoadScene("CombatScene");

        }
        else if (RoomType == 4)
        {

            VariablesGlobales.GetComponent<VariablesGlobales>().Tutorial = true;
            SceneManager.LoadScene("CombatScene");

        }
        else                    // Sala Combate normal
            SceneManager.LoadScene("CombatScene");

    }


    public void createLines()
    {
        if (numContections > 0)
        {
            for (int i = 0; i < numContections; i++)
            {
                GameObject LineClon = Instantiate(LinePrefab);
                LineClon.GetComponent<Line>().AsignPositions(new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z), new Vector3(conections[i].transform.position.x - 0.5f, conections[i].transform.position.y, conections[i].transform.position.z));
                LineClon.transform.SetParent(Content.transform, true);
                conections[i].GetComponent<RoomButton>().conectedLines.Add(LineClon);
                conectionsLines[i] = LineClon;
            }
        }
    }

    public void ControlButtonSize()
    {

        if (!gameObject.GetComponent<Button>().interactable && !Visitado && RoomType != 3)
            gameObject.transform.localScale = new Vector3(minTam, minTam, minTam);
        else if (Visitado)
        {

            gameObject.transform.localScale = new Vector3(maxTam, maxTam, maxTam);
            gameObject.GetComponent<Image>().color = new Color(0, 0, 0);

        }
        else
            gameObject.transform.localScale = new Vector3(maxTam, maxTam, maxTam);

    }

    public void desactivarLineasNoVisitadas()
    {
        for (int i = 0; i < numContections; i++)
        {
            if (!conectionsLines[i].GetComponent<Line>().visitado && conectionsLines[i].GetComponent<Line>().porVisitar)
            {
                Debug.Log("cambiar color a lineas no visitADAS");
                conectionsLines[i].GetComponent<Line>().ChangeColor(0); //cambia el color a no visitada
                conectionsLines[i].GetComponent<Line>().porVisitar = false;

            }
        }
    }
}
