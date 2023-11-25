using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Estructura de coordinadas
struct Coordinadas{

    public float x; // Coordinada X
    public float y; // Coordinada Y

};

public class MapController : MonoBehaviour
{
    
    [SerializeField] GameObject Canvas;
    [SerializeField] GameObject RoomButton;

    Coordinadas StartCoord = new Coordinadas();        // Coordenadas del inicio de la mazmorra
    Coordinadas RestCoord = new Coordinadas();         // Coordenadas de la sala de descanso
    Coordinadas BossCoord = new Coordinadas();         // Coordenadas del boss de la mazmorra
    Coordinadas[,] RoomsCoord = new Coordinadas[3, 4]; // Coordenadas del resto de salas
    bool[,] Occupied_Rooms = new bool[3, 4];           // Indicador de si la sala está ocupada
    [SerializeField] GameObject[,] RoomsGameobjects = new GameObject[3, 4]; // Guarda los clones de las salas

    int ContSalas;

    public static MapController instance;

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

        InicializarCoords(); // Inicializa las coordenadas de las salas
        CrearSalas();        // Crea las salas del mapa

    }

    // Update is called once per frame
    void Update()
    {
        if (MapScene_active())
        {
            Debug.Log("pasa");
           // Canvas = GameObject.Find("Canvas");
            Canvas.SetActive(true);
        }
        else
        {
            Canvas.SetActive(false);
        }
  
    }

    /*
     * Inicializa las coordenadas de las distintas salas, incluidas la Entrada y el Boss
     */

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

    public void InicializarCoords()
    {

        // Inicialización de las coordenadas de la Entrada
        StartCoord.x = -360 + Canvas.transform.position.x;
        StartCoord.y = 0 + Canvas.transform.position.y;

        // Inicialización de las coordenadas de la Sala de Descanso
        RestCoord.x = 230 + Canvas.transform.position.x;
        RestCoord.y = 0 + Canvas.transform.position.y; ;

        // Inicialización de las coordenadas del Boss
        BossCoord.x = 360 + Canvas.transform.position.x;
        BossCoord.y = 0 + Canvas.transform.position.y; ;

        // Inicialización del resto de Salas
        RoomsCoord[0, 0].x = -200 + Canvas.transform.position.x;
        RoomsCoord[0, 0].y = 140 + Canvas.transform.position.y;

        RoomsCoord[0, 1].x = -100 + Canvas.transform.position.x;
        RoomsCoord[0, 1].y = 140 + Canvas.transform.position.y;

        RoomsCoord[0, 2].x = 0 + Canvas.transform.position.x;
        RoomsCoord[0, 2].y = 140 + Canvas.transform.position.y; ;

        RoomsCoord[0, 3].x = 100 + Canvas.transform.position.x;
        RoomsCoord[0, 3].y = 140 + Canvas.transform.position.y; ;

        RoomsCoord[1, 0].x = -200 + Canvas.transform.position.x;
        RoomsCoord[1, 0].y = 0 + Canvas.transform.position.y; ;

        RoomsCoord[1, 1].x = -100 + Canvas.transform.position.x;
        RoomsCoord[1, 1].y = 0 + Canvas.transform.position.y; ;

        RoomsCoord[1, 2].x = 0 + Canvas.transform.position.x;
        RoomsCoord[1, 2].y = 0 + Canvas.transform.position.y; ;

        RoomsCoord[1, 3].x = 100 + Canvas.transform.position.x;
        RoomsCoord[1, 3].y = 0 + Canvas.transform.position.y; ;

        RoomsCoord[2, 0].x = -200 + Canvas.transform.position.x;
        RoomsCoord[2, 0].y = -140 + Canvas.transform.position.y; ;

        RoomsCoord[2, 1].x = -100 + Canvas.transform.position.x;
        RoomsCoord[2, 1].y = -140 + Canvas.transform.position.y; ;

        RoomsCoord[2, 2].x = 0 + Canvas.transform.position.x;
        RoomsCoord[2, 2].y = -140 + Canvas.transform.position.y; ;

        RoomsCoord[2, 3].x = 100 + Canvas.transform.position.x;
        RoomsCoord[2, 3].y = -140 + Canvas.transform.position.y; ;

    }

    /*
     * Instancia las salas del mapa asignándolas unas coordenadas
     */
    public void CrearSalas()
    {
        int rand;
        //asigna falsa las salas para indicar que están libres (luego se pone en true las que SI están ocupadas)
        for (int i = 0; i< Occupied_Rooms.GetLength(0); i++)
        {
            for (int k = 0; k < Occupied_Rooms.GetLength(1); k++)
            {
                Occupied_Rooms[i, k] = false;
            }
        }


        int nSalasEnColumna, posicionSala;
       
        bool correcto;

        // Crea la Entrada
        GameObject clon = Instantiate(RoomButton);
        clon.transform.SetParent(Canvas.transform);
        clon.transform.position = new Vector2(StartCoord.x, StartCoord.y);
        clon.GetComponent<Button>().image.color = Color.cyan;

        // Crea la Sala de Descanso
        clon = Instantiate(RoomButton);
        clon.transform.SetParent(Canvas.transform);
        clon.transform.position = new Vector2(RestCoord.x, RestCoord.y);
        clon.GetComponent<Button>().interactable = false;
        clon.GetComponent<Button>().image.color = Color.green;

        // Crea el Boss
        clon = Instantiate(RoomButton);
        clon.transform.SetParent(Canvas.transform);
        clon.transform.position = new Vector2(BossCoord.x, BossCoord.y);
        clon.GetComponent<Button>().interactable = false;
        clon.GetComponent<Button>().image.color = Color.blue;

        // Crea el resto de salas
        for (int j = 0; j <  Occupied_Rooms.GetLength(1); j++) // Crea un bucle que recorre el array con su respectiva medida
        {
            rand = Random.Range(1, 11);
           if (rand <= 4 )
            {
                nSalasEnColumna = 1;
            }
           else if (rand <= 8)
            {
                nSalasEnColumna = 2;
            }
            else
            {
                nSalasEnColumna = 3;
            }
           // nSalasEnColumna = Random.Range(1, Occupied_Rooms.GetLength(0)); // Establece aleatoriamente el número de salas en esa columna desde 1 a medida con el array

            for(int i = 0; i < nSalasEnColumna; i++)
            {

                do
                {
                    correcto = false;
                    posicionSala = Random.Range(0, 3); // Establece aleatoriamente la posición de la sala

                    if (!Occupied_Rooms[posicionSala,j]) // Comprueba si la sala seleccionada está ocupada (si es falsa está libre)
                    { 
                        correcto = true;
                        Occupied_Rooms[posicionSala, j] = true; // Establece que la sala está ocupada
                    }


                } while (!correcto);

                clon = Instantiate(RoomButton);
                clon.GetComponent<RoomButton>().Occupied_Rooms = Occupied_Rooms;
                clon.GetComponent<RoomButton>().x = posicionSala;
                clon.GetComponent<RoomButton>().y = j;
                clon.GetComponent<RoomButton>().id = ContSalas;
                ContSalas++;
                clon.transform.SetParent(Canvas.transform);
                RoomsGameobjects[posicionSala,j] = clon;
                clon.transform.position = new Vector2(RoomsCoord[posicionSala, j].x, RoomsCoord[posicionSala, j].y); // Coloca la sala en sus coordenadas adecuadas
                clon.GetComponent<Button>().interactable = false;

                rand = Random.Range(1, 11);

                //Probabilidad de que salga combate, tienda, etc
                //if (rand <=)
                //{
                    
                //}
                //clon.GetComponent<Button>().image.color = Color.blue;

            }

        }

    }
}
