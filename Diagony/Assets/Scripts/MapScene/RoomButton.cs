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
    [SerializeField] public int numContections;
    [SerializeField] public GameObject MapController_;
    [SerializeField] GameObject CanvasSingleton;
    [SerializeField] public GameObject LinePrefab;
    [SerializeField] public GameObject Content;

    public int Columna; // Columna que ocupa en el mapa


    public int RoomType; // 0: Combate | 1: Cofre | 2: Tienda | 3: Boss | 4: Tutorial

    void Start()
    {
        CanvasSingleton = GameObject.Find("CanvasSingleton");
        VariablesGlobales = GameObject.Find("VariablesGlobales");
        createLines();
    }


    void Update()
    {
       // ComprobarConexionesActivas();

        if(VariablesGlobales.GetComponent<VariablesGlobales>().EstaEnPausa)
            GetComponent<Button>().enabled = false;
        else
            GetComponent<Button>().enabled = true;

    }

    public void OnClick()
    {
        
        CanvasSingleton.GetComponent<CanvasSingleton>().VerMapa = false;
        gameObject.GetComponent<Button>().interactable = false;

        for (int i = 0; i < numContections; i++)
        {
            conections[i].GetComponent<Button>().interactable = true;
           //cambiar el color a las lineas acá
        }

        MapController_.GetComponent<MapController>().ComprobarInactivos(); //llama a la funcion de comprobar inactivos en map controller para desactivar las salas necesarias



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
                conectionsLines[i] = LineClon;
            }
        }
    }
}
