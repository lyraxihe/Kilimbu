using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//public struct ListCards{

//    [SerializeField] public GameObject[] cards; //Array de cartas
//    [SerializeField] public int cont;           //Contador de cartas en la lista

//};

public class CombatController : MonoBehaviour
{
    public List<GameObject> CardList;
    public GameObject VariablesGlobales;

    public GameObject ArrowEmitter;
    public bool MovingArrow;

    public GameObject AuraEnemigoPrefab;

    //public List<TMP_Text> CardTitles;
    //public List<TMP_Text> CardDescriptions;

    [SerializeField] GameObject PrefabPlayer;         // Prefab del Player
    public GameObject Player;                         // Player
    [SerializeField] GameObject[] PrefabEnemyList;    // Array con enemigos
    public List<GameObject> EnemyList;
    [SerializeField] GameObject Card;           // Carta de combate
    //public ListCards CardList;        // Lista de cartas en el combate
    [SerializeField] GameObject DragZone;       // Zona en la que se eliminarán las cartas usadas
    [SerializeField] GameObject HealthBar;      // Prefab de la barra de vida
    [SerializeField] RectTransform canvas;      // Para tener referencia al canvas y ponerlo como padre (healthbar)
    [SerializeField] TMP_Text Mana;             // Texto que controla el Maná actual y máximo durante el combate
    public int ManaProtagonista;                // Controla el maná actual del jugador en este combate
    public bool TurnoJugador;                         // Indica si es el turno del jugador (true si lo es, false si es el del enemigo)
    public bool CartasCreadas;
    [SerializeField] public UnityEngine.UI.Button botonTurno;
    [SerializeField] GameObject VictoriaDerrotaPanel;
    [SerializeField] TMP_Text VictoriaDerrotaText;
    [SerializeField] TMP_Text RecompensaText;
    [SerializeField] GameObject GameObject_Character_text;
    [SerializeField] TMP_Text Character_text;
    [SerializeField] public bool RecompensaVictoria;
    [SerializeField] int victoria_etc;
    public int numTristeza;

    public int RecompensaDinero;

    [SerializeField] GameObject GameObject_Dmg_text;
    // [SerializeField] TMP_Text Dmg_text;

    [SerializeField] public GameObject[] PrefabSpell;

    float PorcentajeDevolverIra;

    [SerializeField] RectTransform CanvasCartas;

    // Cartas
    public List<Sprite> CardSprites;
    
    List<string> CardTitles = new List<string>() { "Respiro hondo", "Escribo lo que me pasa", "Hablo de lo que me pasa", "Puedo decir que no", "Reconozco lo que siento",
                                                   "Aprendo de lo que siento", "Me divierto con amigos", "Salgo a tomar el sol", "Un paseo por la naturaleza", "Cantar",
                                                   "Cuento hasta diez", "Me ordeno por dentro y por fuera", "Me cuido", "Ahora no, luego sí", "No me pasa nada", "Estoy así ahora",
                                                   "No pasa nada si sale mal", "Hablo de lo que me pasa", "Hablo todo el tiempo de lo que me pasa", "Estoy en ello",
                                                   "Nada es para siempre", "No sé que hacer", "Todo se transforma", "Soy consciente de como me afecta lo que hago"};

    List<string> CardDescriptions = new List<string>() { "Ataque 5", "Ataque 3x2", "Ataque 5 a todos", "Ataque 10", "Ataque 20", "Ataque 10x2", "Gana 2 de maná", "Cura 5",
                                                         "Cura 10", "Roba 5 de vida", "Roba 10 de vida", "Roba 5 de vida a todos", "Roba 10 de vida a todos", "<b>Bloqueado</b> a un enemigo",
                                                         "<b>Bloqueado</b> a un enemigo pero le cura 10", "<b>Débil</b> a un enemigo", "<b>Débil</b> a todos los enemigos",
                                                         "<b>Fuerte</b> al jugador", "<b>Fuerte</b> al jugador pero cura 5 a los enemigos", "<b>Esperanza</b> al jugador",
                                                         "<b>Envenenado</b> a un enemigo", "<b>Débil</b> a un enemigo pero le cura 15", "Los enemigos curan en vez de dañar",
                                                         "Se eliminan todos los efectos del jugador" };

    List<string> CardCost = new List<string>() { "1", "1", "2", "2", "3", "3", "0", "1", "2", "2", "3", "3", "5", "1", "0", "1", "2", "1", "0", "2", "1", "0", "4", "3" };

    List<string> CardDuration = new List<string>() { "", "", "", "", "", "", "", "", "", "", "", "", "", "1", "1", "3", "2", "4", "4", "4", "3", "3", "1", "" };

    [SerializeField] List<int> TotalCards = new List<int>(); // Lista con el número de cartas del Jugador para el combate (Se rellena con las cantidades especificadas en Variables Globales)
    [SerializeField] List<int> HandCardsAmount = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; // Cantidad de cartas de cada tipo en la mano durante el turno del Jugador

    private void Awake()
    {
        
        victoria_etc = 0;
        VariablesGlobales = GameObject.Find("VariablesGlobales");

    }
    void Start()
    {

        PorcentajeDevolverIra = 1f;

        RecompensaVictoria = false;
        
        //CardList.cards = new GameObject[5];
        //CardList.cont = 0; // Inicializa el contador de la lista

        if (VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista <= 0)
        {
            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista = VariablesGlobales.GetComponent<VariablesGlobales>().MaxHealthProtagonista;
        }

        // Inicializa la lista con la cantidad de cartas del Jugadro en el combate
        for(int i = 0; i < VariablesGlobales.GetComponent<VariablesGlobales>().AmountCards.Count; i++)
        {

            for(int j = 0; j < VariablesGlobales.GetComponent<VariablesGlobales>().AmountCards[i]; j++)
                TotalCards.Add(i);

        }

        Time.timeScale = 1f;
        CartasCreadas = false;

        TurnoJugador = true;

        ManaProtagonista = VariablesGlobales.GetComponent<VariablesGlobales>().MaxManaProtagonista;

        MovingArrow = false;

        CreatePlayer();  // Crea al jugador
        CreateEnemies(); // Crea los enemigos
        StartCoroutine(CreateCards());   // Crea las cartas
                                         //CreateCards();
       
    }


    void Update()
    {

        Mana.text = ManaProtagonista + " / " + VariablesGlobales.GetComponent<VariablesGlobales>().MaxManaProtagonista; // Actualiza el texto que indica el maná del jugador
        CardsPosition();

        //victoriaDerrota();
       

    }


    /*
   * Crea el texto con nombre del personaje
   */
    public GameObject CreateCharacterText(float x, float y, string nombre)
    {
        GameObject clonCharacterText = Instantiate(GameObject_Character_text);
        clonCharacterText.transform.SetParent(canvas, false);
        clonCharacterText.transform.position = new Vector2(x, y - 1.5f);
        TextMeshProUGUI textCharacter = clonCharacterText.GetComponent<TextMeshProUGUI>();
        textCharacter.text = nombre;
        return clonCharacterText;
    }

    /*
    * Crea las barras de vida
    */
    public void CreateHealthBar(float x, float y, bool EsPlayer, GameObject personaje, GameObject TextCharacter)
    {
        GameObject clonHealthBar = Instantiate(HealthBar);          //crea el prefab de la barra de vida
        clonHealthBar.transform.SetParent(canvas, false);            //declara el canvas como padre para que sea visible
        clonHealthBar.transform.position = new Vector2(x, y);    //lo coloca arriba del personaje
        clonHealthBar.GetComponent<HealthBar>().EsPlayer = EsPlayer;
        clonHealthBar.GetComponent<HealthBar>().TextCharacter = TextCharacter;




        if (EsPlayer)
             {
                clonHealthBar.GetComponent<HealthBar>()._player = personaje;
             }
             else
             {
                clonHealthBar.GetComponent<HealthBar>()._enemy = personaje;
             }


    }



    /*
     * Crea al jugador
     */
    public void CreatePlayer()
    {
        
        GameObject clonPlayer = Instantiate(PrefabPlayer); // Crea el clon del Player
        clonPlayer.transform.position = new Vector2(-4, -0.8f);
        
        CreateHealthBar(clonPlayer.transform.position.x, clonPlayer.transform.position.y + 1.5f, true, clonPlayer, CreateCharacterText(clonPlayer.transform.position.x, clonPlayer.transform.position.y, "PLAYER")); //tipo 3 = player
        clonPlayer.GetComponent<PlayerController>().VariablesGlobales = VariablesGlobales;
        clonPlayer.GetComponent<PlayerController>().CombatScene = gameObject;
        
        Player = clonPlayer;
    }

    /*
     * Crea de 1 a 3 enemigos de manera aleatoria
     */
    public void CreateEnemies()
    {

        int rand = Random.Range(2, 4);           // Random de 2 a 3
        if (rand==2)
        {
            RecompensaDinero = Random.Range(20, 40);
        }
        else
        {
            RecompensaDinero = Random.Range(40, 81);
        }

        GameObject clonEnemy;                    // Declara el clon del prefab
        int tipo; //0 = ira | 1 = miedo | 2 = tristeza
        string EnemyName;

        for (int i = 0; i < rand; i++)           // Bucle dependiendo del número de enemigos que hay en la sala
        {

            if (i == 0)                          // Si es el primer enemigo
            {
                tipo = Random.Range(0, 3);
                if (tipo == 0)
                {
                    EnemyName = "IRA";
                }
                else if (tipo == 1)
                {
                    EnemyName = "MIEDO";
                }
                else
                {
                    EnemyName = "TRISTEZA";
                    numTristeza++;
                }
                clonEnemy = Instantiate(PrefabEnemyList[tipo]); // Crea el clon del prefab
                clonEnemy.transform.position = new Vector3(2.5f, 0.5f, 0);
                clonEnemy.GetComponent<EnemyController>().Tipo = tipo;
                clonEnemy.GetComponent<EnemyController>().VariablesGlobales = VariablesGlobales;
                clonEnemy.GetComponent<EnemyController>().CombatScene = gameObject;
                clonEnemy.GetComponent<EnemyController>().Id = i;
                clonEnemy.GetComponent<EnemyController>().ArrowEmitter = ArrowEmitter;
                clonEnemy.GetComponent<EnemyController>().Player = Player;
                clonEnemy.GetComponent<EnemyController>().AuraEnemigo = Instantiate(AuraEnemigoPrefab);
                clonEnemy.GetComponent<EnemyController>().AuraEnemigo.transform.position = new Vector3(2.5f, 0.5f, 1);

                EnemyList.Add(clonEnemy);
                CreateHealthBar(clonEnemy.transform.position.x, clonEnemy.transform.position.y + 1.5f, false, clonEnemy, CreateCharacterText(clonEnemy.transform.position.x, clonEnemy.transform.position.y, EnemyName));
                
            }
           else if (i == 1)                     // Si es el segundo enemigo
            {

                tipo = Random.Range(0, 3);
                if (tipo == 0)
                {
                    EnemyName = "IRA";
                }
                else if (tipo == 1)
                {
                    EnemyName = "MIEDO";
                }
                else
                {
                    EnemyName = "TRISTEZA";
                    numTristeza++;
                }
                clonEnemy = Instantiate(PrefabEnemyList[tipo]); // Crea el clon del prefab
                clonEnemy.transform.position = new Vector3(4.5f, -0.5f, 0);
                clonEnemy.GetComponent<EnemyController>().Tipo = tipo;
                clonEnemy.GetComponent<EnemyController>().VariablesGlobales = VariablesGlobales;
                clonEnemy.GetComponent<EnemyController>().CombatScene = gameObject;
                clonEnemy.GetComponent<EnemyController>().Id = i;
                clonEnemy.GetComponent<EnemyController>().ArrowEmitter = ArrowEmitter;
                clonEnemy.GetComponent<EnemyController>().Player = Player;
                clonEnemy.GetComponent<EnemyController>().AuraEnemigo = Instantiate(AuraEnemigoPrefab);
                clonEnemy.GetComponent<EnemyController>().AuraEnemigo.transform.position = new Vector3(4.5f, -0.5f, 1);

                EnemyList.Add(clonEnemy);
                CreateHealthBar(clonEnemy.transform.position.x, clonEnemy.transform.position.y + 1.5f, false, clonEnemy, CreateCharacterText(clonEnemy.transform.position.x, clonEnemy.transform.position.y, EnemyName));
                
            }
            else                                 // Si es el tercer enemigo
            {
                tipo = Random.Range(0, 3);
                if (tipo == 0)
                {
                    EnemyName = "IRA";
                }
                else if (tipo == 1)
                {
                    EnemyName = "MIEDO";
                }
                else
                {
                    EnemyName = "TRISTEZA";
                    numTristeza++;
                }
                clonEnemy = Instantiate(PrefabEnemyList[tipo]); // Crea el clon del prefab
                clonEnemy.transform.position = new Vector3(6.5f, -1.5f, 0);
                clonEnemy.GetComponent<EnemyController>().Tipo = tipo;
                clonEnemy.GetComponent<EnemyController>().VariablesGlobales = VariablesGlobales;
                clonEnemy.GetComponent<EnemyController>().CombatScene = gameObject;
                clonEnemy.GetComponent<EnemyController>().Id = i;
                clonEnemy.GetComponent<EnemyController>().ArrowEmitter = ArrowEmitter;
                clonEnemy.GetComponent<EnemyController>().Player = Player;
                clonEnemy.GetComponent<EnemyController>().AuraEnemigo = Instantiate(AuraEnemigoPrefab);
                clonEnemy.GetComponent<EnemyController>().AuraEnemigo.transform.position = new Vector3(6.5f, -1.5f, 1);

                EnemyList.Add(clonEnemy);
                CreateHealthBar(clonEnemy.transform.position.x, clonEnemy.transform.position.y + 1.5f, false, clonEnemy, CreateCharacterText(clonEnemy.transform.position.x, clonEnemy.transform.position.y, EnemyName));
               
            }
           

            ArrowEmitter.GetComponent<ArrowEmitter>().Enemies.Add(clonEnemy);

        }
        

    }


    /*
     * Coloca las cartas del combate
     */
    public IEnumerator CreateCards()
    {

        if(!CartasCreadas)
        {

            CartasCreadas = true;
            GameObject clon;
            TMP_Text[] newText;
            int cardType;
            bool canCreate;

            for (int i = 0; i < 5; i++)
            {

                do
                {
                    canCreate = true;
                    cardType = Random.Range(0, TotalCards.Count); // Aleatorio entre las cartas totales del Jugador para el combate
                    if (HandCardsAmount[TotalCards[cardType]] + 1 > VariablesGlobales.GetComponent<VariablesGlobales>().AmountCards[TotalCards[cardType]])
                        canCreate = false;

                } while (!canCreate);

                HandCardsAmount[TotalCards[cardType]]++;

                clon = Instantiate(Card);                                          // Crea una carta
                clon.GetComponent<CardController>().CombatScene = gameObject; // Almacena el controlador del combate en cada carta para acceder a sus variables
                clon.GetComponent<CardController>().DragZone = DragZone;         // Almacena la DragZone en cada carta para poder eliminarla una vez se acerque a ella
                clon.GetComponent<CardController>().Id = i;                        // Almacena el ID de cada carta (para saber su posicion al eliminarla de la lista)
                clon.GetComponent<CardController>().Tipo = TotalCards[cardType]; //hace que la carta sea de alguna de las del tipo
                clon.GetComponent<CardController>().VariablesGlobales = VariablesGlobales; // Almacena las variables globales en la carta
                clon.GetComponent<CardController>().ArrowEmitter = ArrowEmitter;
                clon.GetComponent<CardController>().CosteMana = CardManaCost(clon.GetComponent<CardController>().Tipo);
                clon.transform.SetParent(CanvasCartas, false);
                //clon.GetComponent<CardController>().TextTitle = CardTitles[i];
                //clon.GetComponent<CardController>().TextDescription = CardDescriptions[i];

                // Implementa los sprites
                if (TotalCards[cardType] <= 5)
                    clon.GetComponent<Image>().sprite = CardSprites[0];
                else if (TotalCards[cardType] <= 12)
                    clon.GetComponent<Image>().sprite = CardSprites[1];
                else
                    clon.GetComponent <Image>().sprite = CardSprites[2];

                // Actualiza los textos
                newText = clon.GetComponentsInChildren<TMP_Text>();
                newText[0].text = CardTitles[TotalCards[cardType]];
                newText[1].text = CardDescriptions[TotalCards[cardType]];
                newText[2].text = CardCost[TotalCards[cardType]];
                newText[3].text = CardDuration[TotalCards[cardType]];

                CardList.Add(clon);                                         // Almacena la carta en la lista
                yield return new WaitForSeconds(0.1f);
                //CardList.cards[CardList.cont] = clon;                              // Almacena la carta en la lista
                //CardList.cont++;                                                   // Aumenta el contador de la lista
            }

        }

    }

    //public void CreateCards()
    //{

    //    GameObject clon;

    //    for (int i = 0; i < 5; i++)
    //    {

    //        clon = Instantiate(Card);                                          // Crea una carta
    //        clon.GetComponent<CardController>().CombatScene = gameObject; // Almacena el controlador del combate en cada carta para acceder a sus variables
    //        clon.GetComponent<CardController>().DragZone = DragZone;         // Almacena la DragZone en cada carta para poder eliminarla una vez se acerque a ella
    //        clon.GetComponent<CardController>().Id = i;                        // Almacena el ID de cada carta (para saber su posicion al eliminarla de la lista)
    //        CardList.Add(clon);                                         // Almacena la carta en la lista
    //        //yield return new WaitForSeconds(0.05f);
    //        //CardList.cards[CardList.cont] = clon;                              // Almacena la carta en la lista
    //        //CardList.cont++;                                                   // Aumenta el contador de la lista
    //    }

    //}

    /*
     * Controla la posicion de las cartas en funcion del numero de ellas que haya en pantalla
     */
    public void CardsPosition()
    {

        if(TurnoJugador)
        {

            if (CardList.Count == 1)
            {

                if (!CardList[0].GetComponent<CardController>().MouseDrag && !CardList[0].GetComponent<CardController>().MouseOver)
                {

                    //CardList[0].transform.position = new Vector3(0, -4, 0);
                    //CardList[0].transform.eulerAngles = new Vector3(0, 0, 0);
                    CardList[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -200, 0);
                    CardList[0].GetComponent<RectTransform>().eulerAngles = new Vector3 (0, 0, 0);
                    CardList[0].GetComponent<CardController>().SetPosition();
                    CardList[0].transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

                }

            }
            else if (CardList.Count == 2)
            {

                if (!CardList[0].GetComponent<CardController>().MouseDrag && !CardList[0].GetComponent<CardController>().MouseOver)
                {

                    //CardList[0].transform.position = new Vector3(1, -4, 0);
                    //CardList[0].transform.eulerAngles = new Vector3(0, 0, -5);
                    CardList[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(40, -206, 0);
                    CardList[0].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -5);
                    CardList[0].transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    CardList[0].GetComponent<CardController>().SetPosition();

                }
                if (!CardList[1].GetComponent<CardController>().MouseDrag && !CardList[1].GetComponent<CardController>().MouseOver)
                {

                    //CardList[1].transform.position = new Vector3(-1, -4, 1);
                    //CardList[1].transform.eulerAngles = new Vector3(0, 0, 5);
                    CardList[1].GetComponent<RectTransform>().anchoredPosition = new Vector3(-40, -206, 0);
                    CardList[1].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 5);
                    CardList[1].transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    CardList[1].GetComponent<CardController>().SetPosition();

                }

            }
            else if (CardList.Count == 3)
            {

                if (!CardList[0].GetComponent<CardController>().MouseDrag && !CardList[0].GetComponent<CardController>().MouseOver)
                {

                    //CardList[0].transform.position = new Vector3(2, -4.15f, 0);
                    //CardList[0].transform.eulerAngles = new Vector3(0, 0, -10);
                    CardList[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(75, -206, 0);
                    CardList[0].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -10);
                    CardList[0].transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    CardList[0].GetComponent<CardController>().SetPosition();

                }
                if (!CardList[1].GetComponent<CardController>().MouseDrag && !CardList[1].GetComponent<CardController>().MouseOver)
                {

                    //CardList[1].transform.position = new Vector3(0, -4, 1);
                    //CardList[1].transform.eulerAngles = new Vector3(0, 0, 0);
                    CardList[1].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -200, 0);
                    CardList[1].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 0);
                    CardList[1].transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    CardList[1].GetComponent<CardController>().SetPosition();

                }
                if (!CardList[2].GetComponent<CardController>().MouseDrag && !CardList[2].GetComponent<CardController>().MouseOver)
                {

                    //CardList[2].transform.position = new Vector3(-2, -4.15f, 2);
                    //CardList[2].transform.eulerAngles = new Vector3(0, 0, 10);
                    CardList[2].GetComponent<RectTransform>().anchoredPosition = new Vector3(-75, -206, 0);
                    CardList[2].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 10);
                    CardList[2].transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    CardList[2].GetComponent<CardController>().SetPosition();

                }

            }
            else if (CardList.Count == 4)
            {

                if (!CardList[0].GetComponent<CardController>().MouseDrag && !CardList[0].GetComponent<CardController>().MouseOver)
                {

                    //CardList[0].transform.position = new Vector3(3, -4.35f, 0);
                    //CardList[0].transform.eulerAngles = new Vector3(0, 0, -15);
                    CardList[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(116, -220, 0);
                    CardList[0].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -15);
                    CardList[0].transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    CardList[0].GetComponent<CardController>().SetPosition();

                }
                if (!CardList[1].GetComponent<CardController>().MouseDrag && !CardList[1].GetComponent<CardController>().MouseOver)
                {

                    //CardList[1].transform.position = new Vector3(1, -4, 1);
                    //CardList[1].transform.eulerAngles = new Vector3(0, 0, -5);
                    CardList[1].GetComponent<RectTransform>().anchoredPosition = new Vector3(40, -206, 0);
                    CardList[1].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -5);
                    CardList[1].transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    CardList[1].GetComponent<CardController>().SetPosition();

                }
                if (!CardList[2].GetComponent<CardController>().MouseDrag && !CardList[2].GetComponent<CardController>().MouseOver)
                {

                    //CardList[2].transform.position = new Vector3(-1, -4, 2);
                    //CardList[2].transform.eulerAngles = new Vector3(0, 0, 5);
                    CardList[2].GetComponent<RectTransform>().anchoredPosition = new Vector3(-40, -206, 0);
                    CardList[2].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 5);
                    CardList[2].transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    CardList[2].GetComponent<CardController>().SetPosition();

                }
                if (!CardList[3].GetComponent<CardController>().MouseDrag && !CardList[3].GetComponent<CardController>().MouseOver)
                {

                    //CardList[3].transform.position = new Vector3(-3, -4.35f, 3);
                    //CardList[3].transform.eulerAngles = new Vector3(0, 0, 15);
                    CardList[3].GetComponent<RectTransform>().anchoredPosition = new Vector3(-116, -220, 0);
                    CardList[3].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 15);
                    CardList[3].transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    CardList[3].GetComponent<CardController>().SetPosition();

                }

            }
            else if (CardList.Count == 5)
            {

                if (!CardList[0].GetComponent<CardController>().MouseDrag && !CardList[0].GetComponent<CardController>().MouseOver)
                {

                    //CardList[0].transform.position = new Vector3(3.95f, -4.7f, 0);
                    //CardList[0].transform.eulerAngles = new Vector3(0, 0, -20);
                    CardList[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(150, -226, 0);
                    CardList[0].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -20);
                    CardList[0].transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    CardList[0].GetComponent<CardController>().SetPosition();

                }
                if (!CardList[1].GetComponent<CardController>().MouseDrag && !CardList[1].GetComponent<CardController>().MouseOver)
                {

                    //CardList[1].transform.position = new Vector3(2, -4.15f, 1);
                    //CardList[1].transform.eulerAngles = new Vector3(0, 0, -10);
                    CardList[1].GetComponent<RectTransform>().anchoredPosition = new Vector3(75, -206, 0);
                    CardList[1].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -10);
                    CardList[1].transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    CardList[1].GetComponent<CardController>().SetPosition();

                }
                if (!CardList[2].GetComponent<CardController>().MouseDrag && !CardList[2].GetComponent<CardController>().MouseOver)
                {

                    //CardList[2].transform.position = new Vector3(0, -4, 2);
                    //CardList[2].transform.eulerAngles = new Vector3(0, 0, 0);
                    CardList[2].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -200, 0);
                    CardList[2].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 0);
                    CardList[2].transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    CardList[2].GetComponent<CardController>().SetPosition();

                }
                if (!CardList[3].GetComponent<CardController>().MouseDrag && !CardList[3].GetComponent<CardController>().MouseOver)
                {

                    //CardList[3].transform.position = new Vector3(-2, -4.15f, 3);
                    //CardList[3].transform.eulerAngles = new Vector3(0, 0, 10);
                    CardList[3].GetComponent<RectTransform>().anchoredPosition = new Vector3(-75, -206, 0);
                    CardList[3].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 10);
                    CardList[3].transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    CardList[3].GetComponent<CardController>().SetPosition();

                }
                if (!CardList[4].GetComponent<CardController>().MouseDrag && !CardList[4].GetComponent<CardController>().MouseOver)
                {

                    //CardList[4].transform.position = new Vector3(-3.95f, -4.7f, 4);
                    //CardList[4].transform.eulerAngles = new Vector3(0, 0, 20);
                    CardList[4].GetComponent<RectTransform>().anchoredPosition = new Vector3(-150, -226, 0);
                    CardList[4].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 20);
                    CardList[4].transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    CardList[4].GetComponent<CardController>().SetPosition();

                }

            }

        }

    }

    /*
     * Elimina una carta de la lista de cartas (La elimina de la lista, recoloca la lista y reduce el contador de la lista)
     */
    public void EliminarCarta(int id)
    {

        // Reduce la cantidad de cartas en la mano del Jugador en el combate
        HandCardsAmount[CardList[id].GetComponent<CardController>().Tipo]--;

        // Elimina la carta
        CardList.RemoveAt(id);
        for (int i = 0; i < CardList.Count; i++)
            CardList[i].GetComponent<CardController>().Id = i;

    }

    /*
     * Elimina un enemigo de la lista de enemigos si este es derrotado
     */
    public void EliminarEnemig0(int id)
    {

        EnemyList.RemoveAt(id);
        for (int i = 0; i < EnemyList.Count; i++)
            EnemyList[i].GetComponent<EnemyController>().Id = i;

    }

    /*
     * Hace la jugada de los enemigos cuando es su turno
     */
    public IEnumerator TurnoEnemigo()
    {

        if (!TurnoJugador)
        {
        
            for (int i = 0; i < EnemyList.Count; i++)
            {

                yield return new WaitForSeconds(1);
                EnemyList[i].GetComponent<EnemyController>().Atacar();

            }
            
            yield return new WaitForSeconds(1);

            // Transformacion (Jugador)
            if (Player.GetComponent<PlayerController>().Transformacion)
            {

                Player.GetComponent<PlayerController>().ContadorDeTurnosTransformacion--;
                if (Player.GetComponent<PlayerController>().ContadorDeTurnosTransformacion == 0)
                {
                    Player.GetComponent<PlayerController>().Transformacion = false;
                    Player.GetComponent<PlayerController>().debilidad_icon = 0;
                    Player.GetComponent<PlayerController>().EliminarSpell(4);
                }
                   

            }

            ManaProtagonista = VariablesGlobales.GetComponent<VariablesGlobales>().MaxManaProtagonista; // Se resetea el maná del jugador

            if (Player.GetComponent<PlayerController>().ReducirMana)
            {

                ManaProtagonista -= 1;
                Player.GetComponent<PlayerController>().ReducirMana = false;

            }

            TurnoJugador = true;                                                                        // Cambia de turno
            StartCoroutine(CreateCards());

        }
        
        Player.GetComponent<PlayerController>().ContadorDeTurnos++;

    }

    //crea el popUp de texto con la cantidad de daño o de heal
    public void CreateDmgHealText(bool IsHeal, int Amount, GameObject Character)
    {
        if(Amount < 0)
            Amount = 0;
        
        GameObject DmgText = Instantiate(GameObject_Dmg_text);
        DmgText.transform.SetParent(canvas, false);
        DmgText.GetComponent<DmgText>().IsHeal = IsHeal;
        DmgText.GetComponent<DmgText>().Amount = Amount;
        DmgText.GetComponent<DmgText>().IsSpell = false;
        DmgText.transform.position = new Vector2(Character.transform.position.x + 2, Character.transform.position.y + 1);
    }

    public void CreateSpellText(string Spell, GameObject Character)
    {
        GameObject SpellText = Instantiate(GameObject_Dmg_text);
        SpellText.transform.SetParent(canvas, false);
        SpellText.GetComponent<DmgText>().Spell = Spell;
        SpellText.GetComponent<DmgText>().IsSpell = true;
        SpellText.transform.position = new Vector2(Character.transform.position.x + 2, Character.transform.position.y + 1);
    }

    public void UsarCarta(int tipo, int enemigo) // enemigo -> 0: EnemyList[0] | 1: EnemyList[1] | 2: EnemyList[2] | 3: Jugador | 4: TODOS los enemigos (casi no se usa)
    {

        if (tipo == 0)
        {

            //Ataque 5 de daño
            int danyo = 5 + Player.GetComponent<PlayerController>().Debilidad + Player.GetComponent<PlayerController>().Fuerza;
            EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo -= danyo;

            // en esta fución como ultimo parametro debería pasarse el gameobject del enemigo al que ataca la flecha
            //le puse "EnemyList[0]" porque es el que marca arriba para restarle la vida
            CreateDmgHealText(false, danyo, EnemyList[enemigo]);
            ControlEsperanzado();

            EnemyList[enemigo].GetComponent<EnemyController>().RecibirDanyo = true;
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

            // Ira devolver ataque
            if (EnemyList[enemigo].GetComponent<EnemyController>().Tipo == 0) // Si el enemigo seleccionado es Ira
            {
                float porcentajeIraDevolver = Random.Range(0f, 11f);

                if (porcentajeIraDevolver < PorcentajeDevolverIra)
                    StartCoroutine(IraDevolverAtaque(danyo, tipo, enemigo));

            }

        }
        else if (tipo == 1)
        {
            //Ataque 3 x 2 de daño después de 0,5 seg
            StartCoroutine(DoubleAttack(enemigo, 3, 0.5f, 1));

        }
        else if (tipo == 2)
        {
            //Ataque 5 de daño a todos
            int danyo = 5 + Player.GetComponent<PlayerController>().Debilidad + Player.GetComponent<PlayerController>().Fuerza;
            for (int i = 0; i < EnemyList.Count; i++)
            {

                EnemyList[i].GetComponent<EnemyController>().HealthEnemigo -= danyo;
                EnemyList[i].GetComponent<EnemyController>().RecibirDanyo = true;
                CreateDmgHealText(false, danyo, EnemyList[i]);

                // Ira devolver ataque
                if (EnemyList[i].GetComponent<EnemyController>().Tipo == 0) // Si el enemigo seleccionado es Ira
                {
                    float porcentajeIraDevolver = Random.Range(0f, 11f);

                    if (porcentajeIraDevolver < PorcentajeDevolverIra)
                        StartCoroutine(IraDevolverAtaque(danyo, tipo, i));

                }

            }

            ControlEsperanzado();
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

        }
        else if (tipo == 3)
        {

            //Ataque 10 de daño
            int danyo = 10 + Player.GetComponent<PlayerController>().Debilidad + Player.GetComponent<PlayerController>().Fuerza;
            EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo -= danyo;

            CreateDmgHealText(false, danyo, EnemyList[enemigo]);
            ControlEsperanzado();

            EnemyList[enemigo].GetComponent<EnemyController>().RecibirDanyo = true;
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

            // Ira devolver ataque
            if (EnemyList[enemigo].GetComponent<EnemyController>().Tipo == 0) // Si el enemigo seleccionado es Ira
            {
                float porcentajeIraDevolver = Random.Range(0f, 11f);

                if (porcentajeIraDevolver < PorcentajeDevolverIra)
                    StartCoroutine(IraDevolverAtaque(danyo, tipo, enemigo));

            }

        }
        else if (tipo == 4)
        {

            //Ataque 20 de daño
            int danyo = 20 + Player.GetComponent<PlayerController>().Debilidad + Player.GetComponent<PlayerController>().Fuerza;
            EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo -= danyo;

            CreateDmgHealText(false, danyo, EnemyList[enemigo]);
            ControlEsperanzado();

            EnemyList[enemigo].GetComponent<EnemyController>().RecibirDanyo = true;
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

            // Ira devolver ataque
            if (EnemyList[enemigo].GetComponent<EnemyController>().Tipo == 0) // Si el enemigo seleccionado es Ira
            {
                float porcentajeIraDevolver = Random.Range(0f, 11f);

                if (porcentajeIraDevolver < PorcentajeDevolverIra)
                    StartCoroutine(IraDevolverAtaque(danyo, tipo, enemigo));

            }

        }
        else if (tipo == 5)
        {

            //Ataque 10 x 2 de daño después de 0,5 seg
            StartCoroutine(DoubleAttack(enemigo, 10, 0.5f, 5));

        }
        else if (tipo == 6)
        {

            // El Jugador gana 2 de maná
            ManaProtagonista += 2;
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

        }
        else if (tipo == 7)
        {

            // El Jugador se cura 5
            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += 5;
            CreateDmgHealText(true, 5, Player);
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

        }
        else if (tipo == 8)
        {

            // El Jugador se cura 10
            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += 10;
            CreateDmgHealText(true, 10, Player);
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

        }
        else if (tipo == 9)
        {

            // El roba 5 de vida a un enemigo
            int danyo = 5 + Player.GetComponent<PlayerController>().Debilidad + Player.GetComponent<PlayerController>().Fuerza;
            EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo -= danyo;
            CreateDmgHealText(false, danyo, EnemyList[enemigo]);
            ControlEsperanzado();
            EnemyList[enemigo].GetComponent<EnemyController>().RecibirDanyo = true;

            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += danyo;
            CreateDmgHealText(true, danyo, Player);
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

            // Ira devolver ataque
            if (EnemyList[enemigo].GetComponent<EnemyController>().Tipo == 0) // Si el enemigo seleccionado es Ira
            {
                float porcentajeIraDevolver = Random.Range(0f, 11f);

                if (porcentajeIraDevolver < PorcentajeDevolverIra)
                    StartCoroutine(IraDevolverAtaque(danyo, tipo, enemigo));

            }

        }

        else if (tipo == 10)
        {

            // El Jugador roba 10 de vida a un enemigo
            int danyo = 10 + Player.GetComponent<PlayerController>().Debilidad + Player.GetComponent<PlayerController>().Fuerza;
            EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo -= danyo;
            CreateDmgHealText(false, danyo, EnemyList[enemigo]);
            ControlEsperanzado();
            EnemyList[enemigo].GetComponent<EnemyController>().RecibirDanyo = true;

            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += danyo;
            CreateDmgHealText(true, danyo, Player);
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

            // Ira devolver ataque
            if (EnemyList[enemigo].GetComponent<EnemyController>().Tipo == 0) // Si el enemigo seleccionado es Ira
            {
                float porcentajeIraDevolver = Random.Range(0f, 11f);

                if (porcentajeIraDevolver < PorcentajeDevolverIra)
                    StartCoroutine(IraDevolverAtaque(danyo, tipo, enemigo));

            }

        }
        else if (tipo == 11)
        {

            // El Jugador roba 5 de vida a todos los enemigos
            int i;
            int danyo = 5 + Player.GetComponent<PlayerController>().Debilidad + Player.GetComponent<PlayerController>().Fuerza;

            for ( i = 0; i < EnemyList.Count; i++)
            {

                EnemyList[i].GetComponent<EnemyController>().HealthEnemigo -= danyo;
                EnemyList[i].GetComponent<EnemyController>().RecibirDanyo = true;
                CreateDmgHealText(false, danyo, EnemyList[i]);

                // Ira devolver ataque
                if (EnemyList[i].GetComponent<EnemyController>().Tipo == 0) // Si el enemigo seleccionado es Ira
                {
                    float porcentajeIraDevolver = Random.Range(0f, 11f);

                    if (porcentajeIraDevolver < PorcentajeDevolverIra)
                        StartCoroutine(IraDevolverAtaque(danyo, tipo, i));

                }

            }

            ControlEsperanzado();
            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += (danyo * i);
            CreateDmgHealText(true, (danyo * i), Player);
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

        }
        else if (tipo == 12)
        {

            // El Jugador roba 10 de vida a todos los enemigos
            int i;
            int danyo = 10 + Player.GetComponent<PlayerController>().Debilidad + Player.GetComponent<PlayerController>().Fuerza;

            for (i = 0; i < EnemyList.Count; i++)
            {

                EnemyList[i].GetComponent<EnemyController>().HealthEnemigo -= danyo;
                EnemyList[i].GetComponent<EnemyController>().RecibirDanyo = true;
                CreateDmgHealText(false, danyo, EnemyList[i]);

                // Ira devolver ataque
                if (EnemyList[i].GetComponent<EnemyController>().Tipo == 0) // Si el enemigo seleccionado es Ira
                {
                    float porcentajeIraDevolver = Random.Range(0f, 11f);

                    if (porcentajeIraDevolver < PorcentajeDevolverIra)
                        StartCoroutine(IraDevolverAtaque(danyo, tipo, i));

                }

            }

            ControlEsperanzado();
            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += (danyo * i);
            CreateDmgHealText(true, (danyo * i), Player);
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

        }
        else if (tipo == 13)
        {

            // El Jugador bloquea a un enemigo
            EnemyList[enemigo].GetComponent<EnemyController>().Bloqueado = true;
            EnemyList[enemigo].GetComponent<EnemyController>().RecibirDanyo = true;
            CreateSpellText("Bloqueado", EnemyList[enemigo]);

            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

            // Ira devolver ataque
            if (EnemyList[enemigo].GetComponent<EnemyController>().Tipo == 0) // Si el enemigo seleccionado es Ira
            {
                float porcentajeIraDevolver = Random.Range(0f, 11f);

                if (porcentajeIraDevolver < PorcentajeDevolverIra)
                    StartCoroutine(IraDevolverAtaque(-1, tipo, enemigo));

            }

        }
        else if (tipo == 14)
        {

            // El Jugador bloquea a un enemigo pero le cura 10
            EnemyList[enemigo].GetComponent<EnemyController>().Bloqueado = true;
            EnemyList[enemigo].GetComponent<EnemyController>().RecibirDanyo = true;
            CreateSpellText("Bloqueado", EnemyList[enemigo]);
            EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo += 10;
            CreateDmgHealText(true, 10, EnemyList[enemigo]);
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

            // Ira devolver ataque
            if (EnemyList[enemigo].GetComponent<EnemyController>().Tipo == 0) // Si el enemigo seleccionado es Ira
            {
                float porcentajeIraDevolver = Random.Range(0f, 11f);

                if (porcentajeIraDevolver < PorcentajeDevolverIra)
                    StartCoroutine(IraDevolverAtaque(-1, tipo, enemigo));

            }

        }
        else if (tipo == 15)
        {

            // El Jugador aplica Débil a un enemigo durante 3 turnos (efecto acumulativo)
            EnemyList[enemigo].GetComponent<EnemyController>().Debilitado = true;
            EnemyList[enemigo].GetComponent<EnemyController>().Debilidad -= 3;
            EnemyList[enemigo].GetComponent<EnemyController>().ContadorDeTurnosDebilitado += 3;
            EnemyList[enemigo].GetComponent<EnemyController>().RecibirDanyo = true;
            CreateSpellText("Debilitado", EnemyList[enemigo]);
            Debug.Log("Enemigo Debilitado");
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

            // Ira devolver ataque
            if (EnemyList[enemigo].GetComponent<EnemyController>().Tipo == 0) // Si el enemigo seleccionado es Ira
            {
                float porcentajeIraDevolver = Random.Range(0f, 11f);

                if (porcentajeIraDevolver < PorcentajeDevolverIra)
                    StartCoroutine(IraDevolverAtaque(-1, tipo, enemigo));

            }

        }
        else if (tipo == 16)
        {

            // El Jugador aplica Débil a todos los enemigos durante 3 turnos (efecto acumulativo)
            for (int i = 0; i < EnemyList.Count; i++)
            {

                EnemyList[i].GetComponent<EnemyController>().Debilitado = true;
                EnemyList[i].GetComponent<EnemyController>().Debilidad -= 3;
                EnemyList[i].GetComponent<EnemyController>().ContadorDeTurnosDebilitado += 3;
                EnemyList[i].GetComponent<EnemyController>().RecibirDanyo = true;
                CreateSpellText("Debilitado", EnemyList[i]);

                // Ira devolver ataque
                if (EnemyList[i].GetComponent<EnemyController>().Tipo == 0) // Si el enemigo seleccionado es Ira
                {
                    float porcentajeIraDevolver = Random.Range(0f, 11f);

                    if (porcentajeIraDevolver < PorcentajeDevolverIra)
                        StartCoroutine(IraDevolverAtaque(-1, tipo, i));

                }

            }

            Debug.Log("Todos los Enemigos Debilitados");
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

        }
        else if (tipo == 17)
        {

            // El Jugador se aplica Fuerte 4 turnos (efecto acumulativo)
            Player.GetComponent<PlayerController>().Fuerte = true;
            Player.GetComponent<PlayerController>().Fuerza += 3;
            Player.GetComponent<PlayerController>().ContadorDeTurnosFuerte += 4;
            CreateSpellText("Fuerte", Player);

            Debug.Log("El Jugador obtiene Fuerte");
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

        }
        else if (tipo == 18)
        {

            // El Jugador se aplica Fuerte 4 turnos (efecto acumulativo) pero cura 5 a todos los enemigos
            Player.GetComponent<PlayerController>().Fuerte = true;
            Player.GetComponent<PlayerController>().Fuerza += 3;
            Player.GetComponent<PlayerController>().ContadorDeTurnosFuerte += 4;
            CreateSpellText("Fuerte", Player);

            for (int i = 0; i < EnemyList.Count; i++)
            {

                EnemyList[i].GetComponent<EnemyController>().HealthEnemigo += 5;
                CreateDmgHealText(true, 5, EnemyList[i]);

            }
            Debug.Log("El Jugador obtiene Fuerte");
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

        }
        else if (tipo == 19)
        {

            // El Jugador se aplica Esperanza durante 4 turnos
            Player.GetComponent<PlayerController>().Esperanzado = true;
            Player.GetComponent<PlayerController>().Esperanza += 3;
            Player.GetComponent<PlayerController>().ContadorDeTurnosEsperanzado += 4;
            CreateSpellText("Esperanzado", Player);
            Debug.Log("El Jugador obtiene Esperanza");
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

        }
        else if (tipo == 20)
        {

            // El Jugador aplica Envenenado a un Enemigo durante 3 turnos (efecto acumulativo)
            EnemyList[enemigo].GetComponent<EnemyController>().Envenenado = true;
            EnemyList[enemigo].GetComponent<EnemyController>().Veneno += 3;
            EnemyList[enemigo].GetComponent<EnemyController>().ContadorDeTurnosEnvenenado += 3;
            EnemyList[enemigo].GetComponent<EnemyController>().RecibirDanyo = true;
            CreateSpellText("Envenenado", EnemyList[enemigo]);

            Debug.Log("Enemigo Envenenado");
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

            // Ira devolver ataque
            if (EnemyList[enemigo].GetComponent<EnemyController>().Tipo == 0) // Si el enemigo seleccionado es Ira
            {
                float porcentajeIraDevolver = Random.Range(0f, 11f);

                if (porcentajeIraDevolver < PorcentajeDevolverIra)
                    StartCoroutine(IraDevolverAtaque(-1, tipo, enemigo));

            }

        }
        else if (tipo == 21)
        {

            // El Jugador aplica Débil a un enemigo pero le cura 15
            EnemyList[enemigo].GetComponent<EnemyController>().Debilitado = true;
            EnemyList[enemigo].GetComponent<EnemyController>().Debilidad -= 3;
            EnemyList[enemigo].GetComponent<EnemyController>().ContadorDeTurnosDebilitado += 3;
            EnemyList[enemigo].GetComponent<EnemyController>().RecibirDanyo = true;

            EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo += 15;
            CreateDmgHealText(true, 15, EnemyList[enemigo]);
            CreateSpellText("Debilitado", EnemyList[enemigo]);

            Debug.Log("Enemigo Debilitado");
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

            // Ira devolver ataque
            if (EnemyList[enemigo].GetComponent<EnemyController>().Tipo == 0) // Si el enemigo seleccionado es Ira
            {
                float porcentajeIraDevolver = Random.Range(0f, 11f);

                if (porcentajeIraDevolver < PorcentajeDevolverIra)
                    StartCoroutine(IraDevolverAtaque(-1, tipo, enemigo));

            }

        }
        else if (tipo == 22)
        {

            // Durante un turno, todos los ataques al Jugador le curan en vez de hacerle daño
            Player.GetComponent<PlayerController>().Transformacion = true;
            Player.GetComponent<PlayerController>().ContadorDeTurnosTransformacion += 1;
            CreateSpellText("Transformado", Player);
            Debug.Log("Jugador Transformado");
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

        }
        else if (tipo == 23)
        {

            // Se eliminan todos los efectos del Jugador (tanto buenos como malos) en este momento
            if (Player.GetComponent<PlayerController>().Debilitado)      // Débil
            {
                Player.GetComponent<PlayerController>().Debilitado = false;
                Player.GetComponent<PlayerController>().Debilidad = 0;
                Player.GetComponent<PlayerController>().ContadorDeTurnosDebilitado = 0;
                Player.GetComponent<PlayerController>().ContadorDebilitado = 0;
                Player.GetComponent<PlayerController>().debilidad_icon = 0;
                Player.GetComponent<PlayerController>().EliminarSpell(1);
            }
               

            if (Player.GetComponent<PlayerController>().Envenenado)      // Envenenado
            {
                Player.GetComponent<PlayerController>().Envenenado = false;
                Player.GetComponent<PlayerController>().Veneno = 0;
                Player.GetComponent<PlayerController>().ContadorDeTurnosEnvenenado = 0;
                Player.GetComponent<PlayerController>().ContadorEnvenenado = 0;
                Player.GetComponent<PlayerController>().veneno_icon = 0;
                Player.GetComponent<PlayerController>().EliminarSpell(0);
            }
               

            if (Player.GetComponent<PlayerController>().Fuerte)         // Fuerte
            {

                Player.GetComponent<PlayerController>().Fuerte = false;
                Player.GetComponent<PlayerController>().Fuerza = 0;
                Player.GetComponent<PlayerController>().ContadorDeTurnosFuerte = 0;
                Player.GetComponent<PlayerController>().ContadorFuertes = 0;
                Player.GetComponent<PlayerController>().fuerte_icon = 0;
                Player.GetComponent<PlayerController>().EliminarSpell(2);

            }

            if (Player.GetComponent<PlayerController>().Esperanzado)    // Esperanza
            {

                Player.GetComponent<PlayerController>().Esperanzado = false;
                Player.GetComponent<PlayerController>().Esperanza = 0;
                Player.GetComponent<PlayerController>().ContadorDeTurnosEsperanzado = 0;
                Player.GetComponent<PlayerController>().ContadorEsperanzas = 0;
                Player.GetComponent<PlayerController>().esperanza_icon = 0;
                Player.GetComponent<PlayerController>().EliminarSpell(3);

            }

            if (Player.GetComponent<PlayerController>().Transformacion) // Transformacion
            {

                Player.GetComponent<PlayerController>().Transformacion = false;
                Player.GetComponent<PlayerController>().ContadorDeTurnosTransformacion = 0;
                Player.GetComponent<PlayerController>().esperanza_icon = 0;
                Player.GetComponent<PlayerController>().EliminarSpell(4);

            }

            CreateSpellText("Efectos Eliminados", Player);
            Debug.Log("Efectos del Jugador eliminados");
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

        }

        if(tipo != 1 && tipo != 5)
        {

            if (Player.GetComponent<PlayerController>().Envenenado) // Creo que esto tiene que ser cada vez que ataca el Jugador, no cada vez que usa una carta (hablar con Flipper)
            {
                //  yield return new WaitForSeconds(1);
                VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista -= Player.GetComponent<PlayerController>().Veneno;
                CreateDmgHealText(false, Player.GetComponent<PlayerController>().Veneno, Player);
            }

        }

    }

    public void victoriaDerrota()
    {
        //int enemigosVivos = EnemyList.Count;
        //for (int i = 0; i < EnemyList.Count; i++)
        //{
        //    if (EnemyList[i].GetComponent<EnemyController>().HealthEnemigo == 0)
        //    {
        //        enemigosVivos--;
        //    }

        //}
        if (VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista <= 0)
        {
          
            VictoriaDerrotaPanel.SetActive(true);
            VictoriaDerrotaText.text = "DERROTA";
            RecompensaText.text = "¡Te han derrotado!";
            Time.timeScale = 0f;
           // VariablesGlobales.GetComponent<VariablesGlobales>().EstaEnPausa = true;
            botonTurno.enabled = false;
           

        }
        //else if (VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista > 0 && enemigosVivos == 0)
        else if(EnemyList.Count == 0)
        {
            RecompensaVictoria = true;
            Debug.Log("victoria lol");
            VictoriaDerrotaPanel.SetActive(true);
            VictoriaDerrotaText.text = "VICTORIA";
            RecompensaText.text = "Recompensa " + RecompensaDinero + " de oro";
           
            if (victoria_etc == 0 && RecompensaVictoria == true)
            {
                Debug.Log("ganas $" + RecompensaDinero + " de recompensa");
                VariablesGlobales.GetComponent<VariablesGlobales>().DineroAmount += RecompensaDinero;
                victoria_etc += 1;
            }

            Time.timeScale = 0f;
           // VariablesGlobales.GetComponent<VariablesGlobales>().EstaEnPausa = true;
            botonTurno.enabled = false;

        }
        else
        {

            VictoriaDerrotaPanel.SetActive(false);
            if (VariablesGlobales.GetComponent<VariablesGlobales>().EstaEnPausa == false)
            {
                Time.timeScale = 1f;
            }
        }
    }

    public int CardManaCost(int cardType)
    {

        if (cardType == 0)
            return 1;
        else if (cardType == 1)
            return 1;
        else if (cardType == 2)
            return 2;
        else if (cardType == 3)
            return 2;
        else if (cardType == 4)
            return 3;
        else if (cardType == 5)
            return 3;
        else if (cardType == 6)
            return 0;
        else if (cardType == 7)
            return 1;
        else if (cardType == 8)
            return 2;
        else if (cardType == 9)
            return 2;
        else if (cardType == 10)
            return 3;
        else if (cardType == 11)
            return 3;
        else if (cardType == 12)
            return 5;
        else if (cardType == 13)
            return 1;
        else if (cardType == 14)
            return 0;
        else if (cardType == 15)
            return 1;
        else if (cardType == 16)
            return 2;
        else if (cardType == 17)
            return 1;
        else if (cardType == 18)
            return 0;
        else if (cardType == 19)
            return 2;
        else if (cardType == 20)
            return 1;
        else if (cardType == 21)
            return 0;
        else if (cardType == 22)
            return 4;
        else if (cardType == 23)
            return 3;
        else
            return -1;

    }

    public IEnumerator DoubleAttack(int enemigo, int danyo, float tiempo, int tipo)
    {
        if (danyo <0)
            danyo = 0;

        int danyoTotal = danyo + Player.GetComponent<PlayerController>().Debilidad + Player.GetComponent<PlayerController>().Fuerza;

        EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo -= danyoTotal;
        CreateDmgHealText(false, danyoTotal, EnemyList[enemigo]);
        ControlEsperanzado();
        EnemyList[enemigo].GetComponent<EnemyController>().RecibirDanyo = true;
        Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

        if (Player.GetComponent<PlayerController>().Envenenado) // Creo que esto tiene que ser cada vez que ataca el Jugador, no cada vez que usa una carta (hablar con Flipper)
        {
            //  yield return new WaitForSeconds(1);
            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista -= Player.GetComponent<PlayerController>().Veneno;
            CreateDmgHealText(false, Player.GetComponent<PlayerController>().Veneno, Player);
        }

        yield return new WaitForSeconds(tiempo);

        EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo -= danyoTotal;
        CreateDmgHealText(false, danyoTotal, EnemyList[enemigo]);
        ControlEsperanzado();
        EnemyList[enemigo].GetComponent<EnemyController>().RecibirDanyo = true;
        Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

        if (Player.GetComponent<PlayerController>().Envenenado) // Creo que esto tiene que ser cada vez que ataca el Jugador, no cada vez que usa una carta (hablar con Flipper)
        {
            //  yield return new WaitForSeconds(1);
            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista -= Player.GetComponent<PlayerController>().Veneno;
            CreateDmgHealText(false, Player.GetComponent<PlayerController>().Veneno, Player);
        }

        // Ira devolver ataque
        if (EnemyList[enemigo].GetComponent<EnemyController>().Tipo == 0) // Si el enemigo seleccionado es Ira
        {
            float porcentajeIraDevolver = Random.Range(0f, 11f);

            if (porcentajeIraDevolver < PorcentajeDevolverIra)
                StartCoroutine(IraDevolverAtaque(danyo, tipo, enemigo));

        }

    }

    public void ControlEsperanzado()
    {

        if (Player.GetComponent<PlayerController>().Esperanzado)
        {

            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += Player.GetComponent<PlayerController>().Esperanza;
            CreateDmgHealText(true, Player.GetComponent<PlayerController>().Esperanza, Player);

        }

    }

    public IEnumerator IraDevolverAtaque(int danyo, int tipo, int enemigo)
    {

        yield return new WaitForSeconds(0.5f);

        Debug.Log("Ira devuelve el ataque");

        CreateSpellText("Devolver Ataque", EnemyList[enemigo]);

        if (tipo == 0 || tipo == 2 || tipo == 3 || tipo == 4)
        {

            if (Player.GetComponent<PlayerController>().Transformacion)
            {

                VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += danyo;
                CreateDmgHealText(true, danyo, Player);

            }
            else
            {

                VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista -= danyo;
                CreateDmgHealText(false, danyo, Player);

            }

            EnemyList[enemigo].GetComponent<EnemyController>().EnemyAnimator.SetBool("atacar", true);

        }
        else if (tipo == 1 || tipo == 5)
        {

            StartCoroutine(EnemyList[enemigo].GetComponent<EnemyController>().DoubleAttack(danyo, 0.5f));

        }
        else if (tipo == 9 || tipo == 10 || tipo == 11 || tipo == 12)
        {

            if(Player.GetComponent<PlayerController>().Transformacion)
            {

                VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += danyo;
                CreateDmgHealText(true, danyo, Player);

                EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo += 0;
                CreateDmgHealText(true, 0, EnemyList[enemigo]);

            }
            else
            {

                VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista -= danyo;
                CreateDmgHealText(false, danyo, Player);

                EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo += danyo;
                CreateDmgHealText(true, danyo, EnemyList[enemigo]);

            }

            EnemyList[enemigo].GetComponent<EnemyController>().EnemyAnimator.SetBool("atacar", true);

        }
        else if (tipo == 13)
        {

            Player.GetComponent<PlayerController>().Bloqueado = true;
            CreateSpellText("Bloqueado", Player);

            EnemyList[enemigo].GetComponent<EnemyController>().EnemyAnimator.SetBool("atacar", true);

        }
        else if (tipo == 14)
        {

            Player.GetComponent<PlayerController>().Bloqueado = true;
            CreateSpellText("Bloqueado", Player);

            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += 10;
            CreateDmgHealText(true, 10, Player);

            EnemyList[enemigo].GetComponent<EnemyController>().EnemyAnimator.SetBool("atacar", true);

        }
        else if (tipo == 15 || tipo == 16)
        {

            CreateSpellText("Debilitado", Player);
            Player.GetComponent<PlayerController>().Debilitado = true;
            Player.GetComponent<PlayerController>().Debilidad -= 3;
            Player.GetComponent<PlayerController>().ContadorDeTurnosDebilitadoDevolverIra += 3;

            EnemyList[enemigo].GetComponent<EnemyController>().EnemyAnimator.SetBool("atacar", true);

        }
        else if (tipo == 20)
        {

            CreateSpellText("Envenenado", Player);
            Player.GetComponent<PlayerController>().Envenenado = true;
            Player.GetComponent<PlayerController>().Veneno += 3;
            Player.GetComponent<PlayerController>().ContadorDeTurnosEnvenenadoDevolverIra += 3;

            EnemyList[enemigo].GetComponent<EnemyController>().EnemyAnimator.SetBool("atacar", true);

        }
        else if (tipo == 21)
        {

            CreateSpellText("Envenenado", Player);
            Player.GetComponent<PlayerController>().Envenenado = true;
            Player.GetComponent<PlayerController>().Veneno += 3;
            Player.GetComponent<PlayerController>().ContadorDeTurnosEnvenenadoDevolverIra += 3;

            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += 15;
            CreateDmgHealText(true, 15, Player);

            EnemyList[enemigo].GetComponent<EnemyController>().EnemyAnimator.SetBool("atacar", true);

        }


    }

}
