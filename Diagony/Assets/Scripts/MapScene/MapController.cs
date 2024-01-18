
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Estructura de coordinadas
struct Coordinadas
{

    public int x; // Coordinada X
    public int y; // Coordinada Y

};

public class MapController : MonoBehaviour
{

    [SerializeField] GameObject Canvas;
    [SerializeField] GameObject Content;
    [SerializeField] GameObject RoomButton;
    [SerializeField] GameObject LinePrefab;

    static int x_coord = 5;
    static int y_coord = 12;
    int NextXCoord;

    Coordinadas StartCoord = new Coordinadas();        // Coordenadas del inicio de la mazmorra
    //Coordinadas RestCoord = new Coordinadas();         // Coordenadas de la sala de descanso
    //Coordinadas BossCoord = new Coordinadas();         // Coordenadas del boss de la mazmorra
    //Coordinadas[,] RoomsCoord = new Coordinadas[x_coord, y_coord]; // Coordenadas del resto de salas
    bool[,] Occupied_Rooms = new bool[x_coord, y_coord];           // Indicador de si la sala está ocupada
    public GameObject[,] RoomsGameobjects = new GameObject[x_coord, y_coord]; // Guarda los clones de las salas

    public int ContSalas;

    public static MapController instance;

    public GameObject VariablesGlobales;
    [SerializeField] TMP_Text Dinero_text;
    GameObject clonEntrada;
    GameObject clon;
    GameObject clonDescanso;
    GameObject clonBoss;

    private void Awake() //sigleton
    {
        VariablesGlobales = GameObject.Find("VariablesGlobales");

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


    void Start()
    {

        InicializarCoords(); // Inicializa las coordenadas de las salas
        CrearSalas();        // Crea las salas del mapa
        Conections();        // crea las conexiones de las salas
    }


    void Update()
    {
        //if (MapScene_active())
        //{
        //    // Canvas = GameObject.Find("Canvas");
        //    Canvas.SetActive(true);
        //}
        //else
        //{
        //    Canvas.SetActive(false);
        //}

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Destroy(Canvas);
            Destroy(VariablesGlobales);
            Destroy(gameObject);
        }
        Dinero_text.text = "" + VariablesGlobales.GetComponent<VariablesGlobales>().DineroAmount;

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
        StartCoord.x = -350;
        StartCoord.y = 0;
        NextXCoord = StartCoord.x;

    }

    /*
     * Instancia las salas del mapa asignándolas unas coordenadas
     */
    public void CrearSalas()
    {
        int rand;
        //asigna falsa las salas para indicar que están libres (luego se pone en true las que SI están ocupadas)
        for (int i = 0; i < Occupied_Rooms.GetLength(0); i++)
        {
            for (int k = 0; k < Occupied_Rooms.GetLength(1); k++)
            {
                Occupied_Rooms[i, k] = false;
            }
        }


        int nSalasEnColumna, posicionSala;

        bool correcto;

        // Crea la Entrada
        clonEntrada = Instantiate(RoomButton);
        clonEntrada.transform.position = new Vector3(StartCoord.x, StartCoord.y, 0);
        clonEntrada.transform.SetParent(Content.transform, false);
        clonEntrada.GetComponent<Button>().image.color = Color.cyan;
        clonEntrada.GetComponent<Button>().interactable = true;
        clonEntrada.GetComponent<RoomButton>().MapController_ = gameObject;
        clonEntrada.GetComponent<RoomButton>().RoomType = 0;
        clonEntrada.GetComponent<RoomButton>().LinePrefab = LinePrefab;
        clonEntrada.GetComponent<RoomButton>().Content = Content;



        // Crea el resto de salas
        for (int j = 0; j < Occupied_Rooms.GetLength(1); j++) // Crea un bucle que recorre el array con su respectiva medida
        {
            rand = Random.Range(1, 11); // Establece aleatoriamente el número de salas en esa columna desde 1 a medida con el array
            if (rand <= 4)
            {
                nSalasEnColumna = 2;
            }
            else if (rand <= 8)
            {
                nSalasEnColumna = 3;
            }
            else
            {
                nSalasEnColumna = 4;
            }

            NextXCoord += 300;
            for (int i = 0; i < nSalasEnColumna; i++)
            {

                do
                {
                    correcto = false;
                    posicionSala = Random.Range(0, x_coord); // Establece aleatoriamente la posición de la sala

                    if (!Occupied_Rooms[posicionSala, j]) // Comprueba si la sala seleccionada está ocupada (si es falsa está libre)
                    {
                        correcto = true;
                        Occupied_Rooms[posicionSala, j] = true; // Establece que la sala está ocupada
                    }


                } while (!correcto);

                clon = Instantiate(RoomButton);
                clon.GetComponent<RoomButton>().x = posicionSala;
                clon.GetComponent<RoomButton>().y = j;
                clon.GetComponent<Button>().interactable = false;
                clon.GetComponent<RoomButton>().MapController_ = gameObject;
                clon.GetComponent<RoomButton>().LinePrefab = LinePrefab;
                clon.GetComponent<RoomButton>().Content = Content;

                if (j == 0)
                {
                    clon.GetComponent<RoomButton>().RoomType = 2;
                }
                else if (j%2 == 0)
                {
                    clon.GetComponent<RoomButton>().RoomType = 0;
                }
                else
                {
                    clon.GetComponent<RoomButton>().RoomType = 2;
                }
               

                RoomsGameobjects[posicionSala, j] = clon;

                switch (posicionSala)
                {
                    case 0:
                        {
                            clon.transform.position = new Vector3(NextXCoord, StartCoord.y + 360, 0); // Coloca la sala en sus coordenadas

                            break;
                        }


                    case 1:
                        {
                            clon.transform.position = new Vector3(NextXCoord, StartCoord.y + 180, 0); // Coloca la sala en sus coordenadas

                            break;
                        }


                    case 2:
                        {
                            clon.transform.position = new Vector3(NextXCoord, StartCoord.y, 0); // Coloca la sala en sus coordenadas

                            break;
                        }


                    case 3:
                        {
                            clon.transform.position = new Vector3(NextXCoord, StartCoord.y - 180, 0); // Coloca la sala en sus coordenadas

                            break;
                        }


                    case 4:
                        {
                            clon.transform.position = new Vector3(NextXCoord, StartCoord.y - 360, 0); // Coloca la sala en sus coordenadas

                            break;
                        }



                }


                clon.transform.SetParent(Content.transform, false);
                // clon.transform.position = new Vector2(RoomsCoord[posicionSala, j].x, RoomsCoord[posicionSala, j].y); // Coloca la sala en sus coordenadas adecuadas


                // rand = Random.Range(1, 11);

                //Probabilidad de que salga combate, tienda, etc
                //if (rand <=)
                //{

                //}
                //clon.GetComponent<Button>().image.color = Color.blue;
            }
        }
        NextXCoord += 300;
        // Crea la Sala de Descanso
        clonDescanso = Instantiate(RoomButton);
        clonDescanso.transform.position = new Vector3(NextXCoord, StartCoord.y, 0);
        clonDescanso.transform.SetParent(Content.transform, false);
        clonDescanso.GetComponent<Button>().image.color = Color.green;
        clonDescanso.GetComponent<Button>().interactable = false;
        clonDescanso.GetComponent<RoomButton>().MapController_ = gameObject;
        clonDescanso.GetComponent<RoomButton>().LinePrefab = LinePrefab;
        clonDescanso.GetComponent<RoomButton>().Content = Content;


        NextXCoord += 300;
        // Crea el Boss
        clonBoss = Instantiate(RoomButton);
        clonBoss.transform.position = new Vector3(NextXCoord + Canvas.transform.position.x, StartCoord.y + Canvas.transform.position.y, 0);
        clonBoss.transform.SetParent(Content.transform, false);
        clonBoss.GetComponent<Button>().image.color = Color.blue;
        clonBoss.GetComponent<Button>().interactable = false;
        clonBoss.GetComponent<RoomButton>().MapController_ = gameObject;
        clonBoss.GetComponent<RoomButton>().LinePrefab = LinePrefab;
        clonBoss.GetComponent<RoomButton>().Content = Content;


    }



    void Conections()
    {

        for (int i = 0; i < x_coord; i++)
        {
            if (Occupied_Rooms[i, 0])
            {
                clonEntrada.GetComponent<RoomButton>().conections[clonEntrada.GetComponent<RoomButton>().numContections] = RoomsGameobjects[i, 0];
                clonEntrada.GetComponent<RoomButton>().numContections++;
            }
        }

        
     

        int siguiente_salas_por_fila = 0;
        int actual_salas_por_fila = 0;

        for (int i = 0; i < y_coord; i++)
        {
            int actual_sala_ocupada = 0;
            int ultima_sala_ocupada = 0; //sirve para retomar el bucle for desde la anterior sala conectada
            int CantidadConections = 0;

            for (int j = 0; j < x_coord; j++)
            {
               
                if (i != (y_coord - 1))
                {

                    if (j == 0)
                    {
                        siguiente_salas_por_fila = 0;
                        actual_salas_por_fila = 0;

                        for (int k = 0; k < x_coord; k++)
                        {
                            if (Occupied_Rooms[k, i])
                            {
                                actual_salas_por_fila += 1;
                            }
                            if (Occupied_Rooms[k, i + 1])
                            {
                                siguiente_salas_por_fila += 1;
                            }
                        }
                    }
                   
                    if (Occupied_Rooms[j, i])
                    {
                        actual_sala_ocupada++;
                        if (actual_salas_por_fila == 2 && siguiente_salas_por_fila == 2) // 2-2
                        {
                            if (actual_sala_ocupada == 2)
                            {
                                ultima_sala_ocupada++;
                            }

                            for (int k = ultima_sala_ocupada; k < x_coord; k++)
                            {
                                if (Occupied_Rooms[k, i + 1])
                                {

                                    // Debug.Log("22 con");
                                    RoomsGameobjects[j, i].GetComponent<RoomButton>().conections[RoomsGameobjects[j, i].GetComponent<RoomButton>().numContections] = RoomsGameobjects[k, i + 1];
                                    RoomsGameobjects[j, i].GetComponent<RoomButton>().numContections++;
                                    ultima_sala_ocupada = k;
                                    break;

                                }

                            }
                        }

                        else if (actual_salas_por_fila == 2 && siguiente_salas_por_fila == 3) // 2-3
                        {

                            for (int k = ultima_sala_ocupada; k < x_coord; k++)
                            {
                                if (Occupied_Rooms[k, i + 1])
                                {
                                    if (actual_sala_ocupada == 1 && CantidadConections < 2)
                                    {
                                        //  Debug.Log("23 primera con");
                                        RoomsGameobjects[j, i].GetComponent<RoomButton>().conections[RoomsGameobjects[j, i].GetComponent<RoomButton>().numContections] = RoomsGameobjects[k, i + 1];
                                        RoomsGameobjects[j, i].GetComponent<RoomButton>().numContections++;
                                        ultima_sala_ocupada = k;
                                        CantidadConections++;
                                    }
                                    else if (actual_sala_ocupada == 2)
                                    {
                                        //  Debug.Log("23 segunda con");
                                        RoomsGameobjects[j, i].GetComponent<RoomButton>().conections[RoomsGameobjects[j, i].GetComponent<RoomButton>().numContections] = RoomsGameobjects[k, i + 1];
                                        RoomsGameobjects[j, i].GetComponent<RoomButton>().numContections++;

                                    }
                                }

                            }
                        }

                        else if (actual_salas_por_fila == 2 && siguiente_salas_por_fila == 4) // 2-4
                        {
                            if (actual_sala_ocupada == 2)
                            {
                                ultima_sala_ocupada++;
                            }

                            for (int k = ultima_sala_ocupada; k < x_coord; k++)
                            {
                                if (Occupied_Rooms[k, i + 1])
                                {
                                    if (actual_sala_ocupada == 1 && CantidadConections < 2)
                                    {
                                        //  Debug.Log("24 primera con");
                                        RoomsGameobjects[j, i].GetComponent<RoomButton>().conections[RoomsGameobjects[j, i].GetComponent<RoomButton>().numContections] = RoomsGameobjects[k, i + 1];
                                        RoomsGameobjects[j, i].GetComponent<RoomButton>().numContections++;
                                        ultima_sala_ocupada = k;
                                        CantidadConections++;
                                    }
                                    else if (actual_sala_ocupada == 2)
                                    {
                                        //  Debug.Log("24 segunda con");
                                        RoomsGameobjects[j, i].GetComponent<RoomButton>().conections[RoomsGameobjects[j, i].GetComponent<RoomButton>().numContections] = RoomsGameobjects[k, i + 1];
                                        RoomsGameobjects[j, i].GetComponent<RoomButton>().numContections++;

                                    }
                                }

                            }
                        }


                        else if (actual_salas_por_fila == 3 && siguiente_salas_por_fila == 2) // 3-2
                        {


                            for (int k = ultima_sala_ocupada; k < x_coord; k++)
                            {
                                if (Occupied_Rooms[k, i + 1])
                                {
                                    if (actual_sala_ocupada == 1)
                                    {
                                        // Debug.Log("32 primera con");
                                        RoomsGameobjects[j, i].GetComponent<RoomButton>().conections[RoomsGameobjects[j, i].GetComponent<RoomButton>().numContections] = RoomsGameobjects[k, i + 1];
                                        RoomsGameobjects[j, i].GetComponent<RoomButton>().numContections++;
                                        ultima_sala_ocupada = k;
                                        break;
                                    }

                                    else if (CantidadConections < 2)
                                    {
                                        // Debug.Log("32 seg con");
                                        RoomsGameobjects[j, i].GetComponent<RoomButton>().conections[RoomsGameobjects[j, i].GetComponent<RoomButton>().numContections] = RoomsGameobjects[k, i + 1];
                                        RoomsGameobjects[j, i].GetComponent<RoomButton>().numContections++;
                                        ultima_sala_ocupada = k;
                                        CantidadConections++;

                                    }
                                    else
                                    {
                                        // Debug.Log("32 ter con");
                                        RoomsGameobjects[j, i].GetComponent<RoomButton>().conections[RoomsGameobjects[j, i].GetComponent<RoomButton>().numContections] = RoomsGameobjects[k, i + 1];
                                        RoomsGameobjects[j, i].GetComponent<RoomButton>().numContections++;
                                    }


                                }

                            }
                        }

                        else if (actual_salas_por_fila == 3 && siguiente_salas_por_fila == 3) // 3-3
                        {

                            for (int k = ultima_sala_ocupada; k < x_coord; k++)
                            {
                                if (Occupied_Rooms[k, i + 1])
                                {
                                    if (actual_sala_ocupada == 1 && CantidadConections < 2)
                                    {
                                        // Debug.Log("33 prim con");
                                        RoomsGameobjects[j, i].GetComponent<RoomButton>().conections[RoomsGameobjects[j, i].GetComponent<RoomButton>().numContections] = RoomsGameobjects[k, i + 1];
                                        RoomsGameobjects[j, i].GetComponent<RoomButton>().numContections++;
                                        ultima_sala_ocupada = k;
                                        CantidadConections++;

                                    }
                                    else if (actual_sala_ocupada == 2)
                                    {
                                        //  Debug.Log("33 seg con");
                                        RoomsGameobjects[j, i].GetComponent<RoomButton>().conections[RoomsGameobjects[j, i].GetComponent<RoomButton>().numContections] = RoomsGameobjects[k, i + 1];
                                        RoomsGameobjects[j, i].GetComponent<RoomButton>().numContections++;
                                        break;
                                    }
                                    else if (actual_sala_ocupada == 3 && CantidadConections < 4)
                                    {
                                        // Debug.Log("33 ter con");
                                        RoomsGameobjects[j, i].GetComponent<RoomButton>().conections[RoomsGameobjects[j, i].GetComponent<RoomButton>().numContections] = RoomsGameobjects[k, i + 1];
                                        RoomsGameobjects[j, i].GetComponent<RoomButton>().numContections++;
                                        CantidadConections++;
                                    }
                                }

                            }

                        }
                        else if (actual_salas_por_fila == 3 && siguiente_salas_por_fila == 4) // 3-4
                        {
                            if (actual_sala_ocupada != 1)
                            {
                                ultima_sala_ocupada++;
                            }

                            for (int k = ultima_sala_ocupada; k < x_coord; k++)
                            {
                                if (Occupied_Rooms[k, i + 1])
                                {

                                    if (actual_sala_ocupada == 1 || actual_sala_ocupada == 3)
                                    {
                                      //  Debug.Log("34 prim/ter con");
                                        RoomsGameobjects[j, i].GetComponent<RoomButton>().conections[RoomsGameobjects[j, i].GetComponent<RoomButton>().numContections] = RoomsGameobjects[k, i + 1];
                                        RoomsGameobjects[j, i].GetComponent<RoomButton>().numContections++;
                                        ultima_sala_ocupada = k;
                                        break;
                                    }
                                    else if (CantidadConections < 2)
                                    {
                                       // Debug.Log("34 seg con");
                                        RoomsGameobjects[j, i].GetComponent<RoomButton>().conections[RoomsGameobjects[j, i].GetComponent<RoomButton>().numContections] = RoomsGameobjects[k, i + 1];
                                        RoomsGameobjects[j, i].GetComponent<RoomButton>().numContections++;
                                        ultima_sala_ocupada = k;
                                        CantidadConections++;

                                    }


                                }
                            }
                        }

                        else if (actual_salas_por_fila == 4 && siguiente_salas_por_fila == 2) // 4-2
                        {
                            if (actual_sala_ocupada == 3)
                            {
                                ultima_sala_ocupada++;
                            }

                            for (int k = ultima_sala_ocupada; k < x_coord; k++)
                            {
                                if (Occupied_Rooms[k, i + 1])
                                {

                                   // Debug.Log("42 con");
                                    RoomsGameobjects[j, i].GetComponent<RoomButton>().conections[RoomsGameobjects[j, i].GetComponent<RoomButton>().numContections] = RoomsGameobjects[k, i + 1];
                                    RoomsGameobjects[j, i].GetComponent<RoomButton>().numContections++;
                                    ultima_sala_ocupada = k;
                                    break;

                                }
                            }
                        }

                        else if (actual_salas_por_fila == 4 && siguiente_salas_por_fila == 3) // 4-3
                        {
                            if (actual_sala_ocupada == 2 || actual_sala_ocupada == 4)
                            {
                                ultima_sala_ocupada++;
                            }

                            for (int k = ultima_sala_ocupada; k < x_coord; k++)
                            {
                                if (Occupied_Rooms[k, i + 1])
                                {
                                  // Debug.Log("43 con");
                                   RoomsGameobjects[j, i].GetComponent<RoomButton>().conections[RoomsGameobjects[j, i].GetComponent<RoomButton>().numContections] = RoomsGameobjects[k, i + 1];
                                   RoomsGameobjects[j, i].GetComponent<RoomButton>().numContections++;
                                   ultima_sala_ocupada = k;
                                   break;
                                }
                            }
                        }


                        else if (actual_salas_por_fila == 4 && siguiente_salas_por_fila == 4) // 4-4
                        {
                            if (actual_sala_ocupada != 1)
                            {
                                ultima_sala_ocupada++;
                            }

                            for (int k = ultima_sala_ocupada; k < x_coord; k++)
                            {
                                if (Occupied_Rooms[k, i + 1])
                                {
                                   // Debug.Log("44 con");
                                    RoomsGameobjects[j, i].GetComponent<RoomButton>().conections[RoomsGameobjects[j, i].GetComponent<RoomButton>().numContections] = RoomsGameobjects[k, i + 1];
                                    RoomsGameobjects[j, i].GetComponent<RoomButton>().numContections++;
                                    ultima_sala_ocupada = k;
                                    break;
                                }
                            }
                        }



                    }


                }
                    else if (Occupied_Rooms[j, i])
                    {
                        RoomsGameobjects[j, i].GetComponent<RoomButton>().conections[RoomsGameobjects[j, i].GetComponent<RoomButton>().numContections] = clonDescanso;
                        RoomsGameobjects[j, i].GetComponent<RoomButton>().numContections++;
                    }
                
            }
        }
        clonDescanso.GetComponent<RoomButton>().conections[clonDescanso.GetComponent<RoomButton>().numContections] = clonBoss;
        clonDescanso.GetComponent<RoomButton>().numContections++;

        //for (int i = 0; i < y_coord; i++)
        //{
        //    for (int j = 0; j < x_coord; j++)
        //    {
        //        if (Occupied_Rooms[j, i])
        //        {
        //            RoomsGameobjects[j, i].GetComponent<RoomButton>().createLines();
        //        }
        //    }
        //}
    }

    public void ComprobarInactivos()
    {
        bool desactivar = false;
        for (int i = 0; i < y_coord-1; i++) //recorre el array con todas las salas en el mapa
        {
            
            for (int j = 0; j < x_coord; j++)
            {
                if (j == 0) //si está e el 0 de la columna
                {
                    desactivar = false;
                    for (int k = 0; k < x_coord; k++) //recorre la columna entera
                    {
                        if (Occupied_Rooms[k, i+1]) //solo en las salas ocupadas de la siguiente columna
                        {
                            if ((RoomsGameobjects[k, i + 1].GetComponent<Button>().interactable)) //comprueba si está el botón interactuable 
                            {
                                desactivar = true; //pone en true el booleano así luego desactiva las salas de la columna actual
                                break;
                            }
                           
                        }
                    }
                }
                if (Occupied_Rooms[j, i] && desactivar) //en caso de que sea positivo y la sala esté ocupada
                {
                    Debug.Log("Desactivar salas en la siguiente columna.");
                    RoomsGameobjects[j, i].GetComponent<Button>().interactable = false; //pone para que no se pueda interactuar con esos botones
                }
            }
        }

    }
}
