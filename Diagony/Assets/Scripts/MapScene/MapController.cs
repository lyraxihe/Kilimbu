
using System.Collections.Generic;
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

    [SerializeField] Sprite[] RoomIcons; // 0 - Combate | 1 - Cofre | 2 - Cofre abierto | 3 - Tienda | 4 - Boss

    static int x_coord = 5;
    static int y_coord = 12;
    int NextXCoord;

    Coordinadas StartCoord = new Coordinadas();        // Coordenadas del inicio de la mazmorra
    //Coordinadas RestCoord = new Coordinadas();         // Coordenadas de la sala de descanso
    //Coordinadas BossCoord = new Coordinadas();         // Coordenadas del boss de la mazmorra
    //Coordinadas[,] RoomsCoord = new Coordinadas[x_coord, y_coord]; // Coordenadas del resto de salas
    bool[,] Occupied_Rooms = new bool[x_coord, y_coord];           // Indicador de si la sala está ocupada
    public GameObject[,] RoomsGameobjects = new GameObject[x_coord, y_coord]; // Guarda los clones de las salas
    public List<GameObject> ListRooms = new List<GameObject>(); // Guarda los clones de las salas en una sóla dimensión

    public int ContSalas;

    public static MapController instance;

    public GameObject VariablesGlobales;
    [SerializeField] TMP_Text Dinero_text;
    GameObject clonEntrada;
    GameObject clon;
    GameObject clonDescanso;
    GameObject clonBoss;

    public int contCombates = 0, contTiendas = 0, contCofres = 0;

    // Posiciones salas (ruido, distancia, etc.)
    [SerializeField] int distanciaSalaX = 0;
    [SerializeField] int distanciaSalaY = 0;
    [SerializeField] int minRuidoX = 0, maxRuidoX = 0, minRuidoY = 0, maxRuidoY = 0;

    // Array de posiciones de las columnas
    public List<int> ColumnasPos;

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
        Time.timeScale = 1f;
        InicializarCoords(); // Inicializa las coordenadas de las salas
        CrearSalas();        // Crea las salas del mapa
        Conections();        // crea las conexiones de las salas

        do
        {

            CheckMinRooms();     // Comprueba los mínimos de salas de Tienda y Cofres
            CheckShops();        // Comprueba que dos tiendas no estén conectadas

        } while (contTiendas < 4 || contCofres < 4); // Vuelve ha hacer cambios hasta que se cumplan los mínimos

        SetSprites();

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
        StartCoord.y = 50;
        NextXCoord = StartCoord.x;

    }

    /*
     * Instancia las salas del mapa asignándolas unas coordenadas
     */
    public void CrearSalas()
    {
        int rand, randRoom, randTienda;
        //int randTienda1 = 0, randTienda2 = 0;
        //int randCofre1 = 0, randCofre2 = 0, randCofre3 = 0, randCofre4 = 0;

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
        int randRuidoX = RandRuido(minRuidoX, maxRuidoX);
        int randRuidoY = RandRuido(minRuidoY, maxRuidoY);
        clonEntrada.transform.position = new Vector3(StartCoord.x + randRuidoX, StartCoord.y + randRuidoY, 0);
        clonEntrada.transform.SetParent(Content.transform, false);
        //clonEntrada.GetComponent<Button>().image.color = Color.cyan;
        //clonEntrada.GetComponent<Image>().sprite = RoomIcons[0];
        clonEntrada.GetComponent<Button>().interactable = false;
        clonEntrada.GetComponent<RoomButton>().MapController_ = gameObject;
        clonEntrada.GetComponent<RoomButton>().Columna = 1;
        clonEntrada.GetComponent<RoomButton>().RoomType = 4; // 4 - Tutorial
        clonEntrada.GetComponent<RoomButton>().LinePrefab = LinePrefab;
        clonEntrada.GetComponent<RoomButton>().Content = Content;
        Content.GetComponent<FelipeBarridoMap>().FirstRoom = clonEntrada;
        contCombates++;
        ListRooms.Add(clonEntrada);


        // Crea el resto de salas
        for (int j = 0; j < Occupied_Rooms.GetLength(1); j++) // Crea un bucle que recorre el array con su respectiva medida
        {

            bool hayTienda = false; // Indica si en la columna hay una tienda o no para que no se repitan
            randTienda = -1; // -1 para indicar que no hay sala en esta columna
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

            NextXCoord += distanciaSalaX;

            if (j == 2) // Si es la columna 3 debe haber una tienda si o si
                randTienda = Random.Range(0, nSalasEnColumna);

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
                clon.GetComponent<RoomButton>().Columna = j + 2;

                //if (j == 0)
                //{
                //    clon.GetComponent<RoomButton>().RoomType = 2;
                //}
                //else if (j%2 == 0)
                //{
                //    clon.GetComponent<RoomButton>().RoomType = 0;
                //}
                //else
                //{
                //    clon.GetComponent<RoomButton>().RoomType = 2;
                //}

                //// Establece las posiciones para un mínimo de 4 Tiendas (2 + Tienda en columna 3 + Tienda en penúltima columna)
                //randTienda1 = Random.Range(3, Occupied_Rooms.GetLength(1) - 1); // 3 porque la tienda no puede estar en j == 0 ni j == 2 y en j == 3 ya hay una tienda si o si
                //do
                //{

                //    randTienda2 = Random.Range(3, Occupied_Rooms.GetLength(1) - 1);

                //} while (randTienda2 == randTienda1);

                //// Establece las posiciones para un mínimo de 4 cofres
                //randCofre1 = Random.Range(0, Occupied_Rooms.GetLength(1) - 1);
                //do
                //{

                //    randCofre2 = Random.Range(0, Occupied_Rooms.GetLength(1) - 1);

                //} while (randCofre2 == randCofre1);
                //do
                //{

                //    randCofre3 = Random.Range(0, Occupied_Rooms.GetLength(1) - 1);

                //} while (randCofre3 == randCofre1 || randCofre3 == randCofre2);
                //do
                //{

                //    randCofre4 = Random.Range(0, Occupied_Rooms.GetLength(1) - 1);

                //} while (randCofre4 == randCofre1 || randCofre4 == randCofre2 || randCofre4 == randCofre3);

                if (i == randTienda)
                {

                    clon.GetComponent<RoomButton>().RoomType = 2;
                    //clon.GetComponent<Image>().sprite = RoomIcons[3]; // Sala de tienda
                    hayTienda = true;
                    contTiendas++;

                }
                else
                {

                    randRoom = Random.Range(0, 11);

                    if (randRoom <= 6 /*&& contCombates < 24*/)
                    {

                        clon.GetComponent<RoomButton>().RoomType = 0;
                        //clon.GetComponent<Image>().sprite = RoomIcons[0]; // Sala de combate
                        contCombates++;

                    }
                    else if (randRoom <= 8 && contCofres < 8)
                    {

                        clon.GetComponent<RoomButton>().RoomType = 1;
                        //clon.GetComponent<Image>().sprite = RoomIcons[1]; // Sala de cofre
                        contCofres++;

                    }
                    else
                    {

                        if (j > 2 && j < Occupied_Rooms.GetLength(1) - 1 && j != 3 && randTienda == -1 && !hayTienda && contTiendas < 4) // En las primeras dos columnas no hay combates
                        {

                            clon.GetComponent<RoomButton>().RoomType = 2;
                            //clon.GetComponent<Image>().sprite = RoomIcons[3]; // Sala de tienda
                            hayTienda = true;
                            contTiendas++;

                        }
                        else
                        {

                            randRoom = Random.Range(0, 9);

                            if (randRoom <= 6 /*&& contCombates < 24*/)
                            {

                                clon.GetComponent<RoomButton>().RoomType = 0;
                                //clon.GetComponent<Image>().sprite = RoomIcons[0]; // Sala de combate
                                contCombates++;

                            }
                            else if (randRoom <= 8 && contCofres < 8)
                            {

                                clon.GetComponent<RoomButton>().RoomType = 1;
                                //clon.GetComponent<Image>().sprite = RoomIcons[1]; // Sala de cofre
                                contCofres++;

                            }

                        }

                    }

                }

                RoomsGameobjects[posicionSala, j] = clon;
                ListRooms.Add(clon);

                randRuidoX = RandRuido(minRuidoX, maxRuidoX);
                randRuidoY = RandRuido(minRuidoY, maxRuidoY);

                switch (posicionSala)
                {
                    case 0:
                        {
                            clon.transform.position = new Vector3(NextXCoord + randRuidoX, StartCoord.y + (distanciaSalaY * 2) + randRuidoY, 0); // Coloca la sala en sus coordenadas

                            break;
                        }


                    case 1:
                        {
                            clon.transform.position = new Vector3(NextXCoord + randRuidoX, StartCoord.y + distanciaSalaY + randRuidoY, 0); // Coloca la sala en sus coordenadas

                            break;
                        }


                    case 2:
                        {
                            clon.transform.position = new Vector3(NextXCoord + randRuidoX, StartCoord.y + randRuidoY, 0); // Coloca la sala en sus coordenadas

                            break;
                        }


                    case 3:
                        {
                            clon.transform.position = new Vector3(NextXCoord + randRuidoX, StartCoord.y - distanciaSalaY + randRuidoY, 0); // Coloca la sala en sus coordenadas

                            break;
                        }


                    case 4:
                        {
                            clon.transform.position = new Vector3(NextXCoord + randRuidoX, StartCoord.y - (distanciaSalaY * 2) + randRuidoY, 0); // Coloca la sala en sus coordenadas

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
        NextXCoord += distanciaSalaX;
        // Crea la Sala de Descanso
        clonDescanso = Instantiate(RoomButton);
        randRuidoX = RandRuido(minRuidoX, maxRuidoX);
        randRuidoY = RandRuido(minRuidoY, maxRuidoY);
        clonDescanso.transform.position = new Vector3(NextXCoord + randRuidoX, StartCoord.y + randRuidoY, 0);
        clonDescanso.transform.SetParent(Content.transform, false);
        //clonDescanso.GetComponent<Button>().image.color = Color.green;
        clonDescanso.GetComponent<RoomButton>().RoomType = 2;
        //clonDescanso.GetComponent<Image>().sprite = RoomIcons[3];
        clonDescanso.GetComponent<Button>().interactable = false;
        clonDescanso.GetComponent<RoomButton>().MapController_ = gameObject;
        clonDescanso.GetComponent<RoomButton>().Columna = Occupied_Rooms.GetLength(1) + 2;
        clonDescanso.GetComponent<RoomButton>().LinePrefab = LinePrefab;
        clonDescanso.GetComponent<RoomButton>().Content = Content;
        contTiendas++;
        ListRooms.Add(clonDescanso);


        NextXCoord += distanciaSalaX;
        // Crea el Boss
        clonBoss = Instantiate(RoomButton);
        randRuidoX = RandRuido(minRuidoX, maxRuidoX);
        randRuidoY = RandRuido(minRuidoY, maxRuidoY);
        clonBoss.transform.position = new Vector3(NextXCoord + Canvas.transform.position.x + randRuidoX, StartCoord.y + Canvas.transform.position.y + randRuidoY, 0);
        clonBoss.transform.SetParent(Content.transform, false);
        clonBoss.GetComponent<RoomButton>().RoomType = 3;
        //clonBoss.GetComponent<Image>().sprite = RoomIcons[4];
        clonBoss.GetComponent<Button>().interactable = false;
        clonBoss.GetComponent<RoomButton>().MapController_ = gameObject;
        clonBoss.GetComponent<RoomButton>().Columna = Occupied_Rooms.GetLength(1) + 3;
        clonBoss.GetComponent<RoomButton>().LinePrefab = LinePrefab;
        clonBoss.GetComponent<RoomButton>().Content = Content;
        ListRooms.Add(clonBoss);

    }

    // Comprueba que no haya tiendas conectadas a otras tiendas
    private void CheckShops()
    {

        for(int i = 0; i < ListRooms.Count; i++) // Recorre la lista de salas del mapa
        {

            if (ListRooms[i].GetComponent<RoomButton>().RoomType == 2) // Si la sala es una tienda
            {

                for (int j = 0; j < ListRooms[i].GetComponent<RoomButton>().conections.Length; j++) // Recorre el array de conexiones de la tienda
                {

                    if (ListRooms[i].GetComponent<RoomButton>().conections[j] != null && ListRooms[i].GetComponent<RoomButton>().conections[j].GetComponent<RoomButton>().RoomType == 2) // Si una de sus conexiones es otra tienda
                    {
                        int rand = Random.Range(0, 9);

                        if(rand <= 6 /*&& contCombates < 24*/)
                        {

                            ListRooms[i].GetComponent<RoomButton>().conections[j].GetComponent<RoomButton>().RoomType = 0; // Cambia la tienda a un combate
                            //ListRooms[i].GetComponent<RoomButton>().conections[j].GetComponent<Image>().sprite = RoomIcons[0];
                            contCombates++;

                        }
                        else if (rand <= 8 && contCofres < 8)
                        {

                            ListRooms[i].GetComponent<RoomButton>().conections[j].GetComponent<RoomButton>().RoomType = 1; // Cambia la tienda a un cofre
                            //ListRooms[i].GetComponent<RoomButton>().conections[j].GetComponent<Image>().sprite = RoomIcons[1];
                            contCofres++;

                        }

                        contTiendas--;

                    }

                }

            }

        }

    }

    private void CheckMinRooms()
    {

        List<int> salasCogidas = new List<int>();
        int rand;

        if(contTiendas < 4)
        {

            for (int i = 0; i < ListRooms.Count && contTiendas < 4; i++)
            {

                if (ListRooms[i].GetComponent<RoomButton>().Columna > 3 && ListRooms[i].GetComponent<RoomButton>().Columna < 14)
                {

                    do
                    {

                        rand = Random.Range(i, ListRooms.Count - 2); // Aleatorio menos la primera, penúltima y última posición que son predeterminadas

                    } while (salasCogidas.Contains(rand) || ListRooms[rand].GetComponent<RoomButton>().RoomType == 1 || ListRooms[rand].GetComponent<RoomButton>().RoomType == 2); // Repite hasta que el rand no haya sido cogido previamente o no sea una tienda

                    // Cambia la sala escogida por una Tienda
                    if (ListRooms[rand].GetComponent<RoomButton>().RoomType == 0)
                        contCombates--;
                    //else if (ListRooms[rand].GetComponent<RoomButton>().RoomType == 1)
                    //    contCofres--;

                    ListRooms[rand].GetComponent<RoomButton>().RoomType = 2;
                    //ListRooms[rand].GetComponent<Image>().sprite = RoomIcons[3];
                    salasCogidas.Add(rand);
                    contTiendas++;

                }

            }

        }

        if (contCofres < 4)
        {

            for (int i = 0; i < ListRooms.Count && contCofres < 4; i++)
            {

                if (ListRooms[i].GetComponent<RoomButton>().Columna > 1 && ListRooms[i].GetComponent<RoomButton>().Columna < 14)
                {

                    do
                    {

                        rand = Random.Range(i, ListRooms.Count - 2); // Aleatorio menos la primera, penúltima y última posición que son predeterminadas

                    } while (salasCogidas.Contains(rand) || ListRooms[rand].GetComponent<RoomButton>().RoomType == 1 || ListRooms[rand].GetComponent<RoomButton>().RoomType == 2); // Repite hasta que el rand no haya sifo cogido previamente

                    // Cambia la sala escogida por un Cofre
                    if (ListRooms[rand].GetComponent<RoomButton>().RoomType == 0)
                        contCombates--;
                    //else if (ListRooms[rand].GetComponent<RoomButton>().RoomType == 2)
                    //    contTiendas--;

                    ListRooms[rand].GetComponent<RoomButton>().RoomType = 1;
                    //ListRooms[rand].GetComponent<Image>().sprite = RoomIcons[1];
                    salasCogidas.Add(rand);
                    contCofres++;

                }

            }

        }

    }

    private void SetSprites()
    {

        int RoomType;

        for (int i = 0; i < ListRooms.Count; i++)
        {

            RoomType = ListRooms[i].GetComponent<RoomButton>().RoomType;

            if (RoomType == 0 || RoomType == 4)
                ListRooms[i].GetComponent<Image>().sprite = RoomIcons[0];
            else if (RoomType == 1)
                ListRooms[i].GetComponent<Image>().sprite = RoomIcons[1];
            else if (RoomType == 2)
                ListRooms[i].GetComponent<Image>().sprite = RoomIcons[3];
            else
                ListRooms[i].GetComponent<Image>().sprite = RoomIcons[4];

        }

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

    public int RandRuido(int min, int max)
    {

        int randRuido = Random.Range(min, max + 1);

        if (randRuido < -70)
            return -80;
        else if (randRuido < -60)
            return -70;
        else if (randRuido < -50)
            return -60;
        else if (randRuido < -40)
            return -50;
        else if (randRuido < -30)
            return -40;
        else if (randRuido < -20)
            return -30;
        else if (randRuido < -10)
            return -20;
        else if (randRuido < 0)
            return -10;
        else if (randRuido == 0)
            return 0;
        else if (randRuido <= 10)
            return 10;
        else if (randRuido <= 20)
            return 20;
        else if (randRuido <= 30)
            return 30;
        else if (randRuido <= 40)
            return 40;
        else if (randRuido <= 50)
            return 50;
        else if (randRuido <= 60)
            return 60;
        else if (randRuido <= 70)
            return 70;
        else
            return 80;

    }
}
