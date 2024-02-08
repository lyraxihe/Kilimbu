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
    [SerializeField] TMP_Text Mana;             // Texto que controla el Maná actual durante el combate
    [SerializeField] TMP_Text ManaMax;          // Texto que controla el Maná máximo durante el combate
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
    public int ContadorTurnos;

    [SerializeField] GameObject GameObject_Dmg_text;
    // [SerializeField] TMP_Text Dmg_text;

    [SerializeField] public GameObject[] PrefabSpell;

    float PorcentajeDevolverIra;

    [SerializeField] RectTransform CanvasCartas;

    // Cartas
    public List<Sprite> CardSprites;

    [SerializeField] List<int> TotalCards = new List<int>(); // Lista con el número de cartas del Jugador para el combate (Se rellena con las cantidades especificadas en Variables Globales)
    [SerializeField] List<int> HandCardsAmount = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; // Cantidad de cartas de cada tipo en la mano durante el turno del Jugador

    [SerializeField] TMP_Text turnosText;

    [SerializeField] TMP_Text EndTurnText;
    [SerializeField] TMP_Text LeaveCombatText;

    // Tutorial
    public bool Tutorial;

    private void Awake()
    {
       
        victoria_etc = 0;
        VariablesGlobales = GameObject.Find("VariablesGlobales");

    }
    void Start()
    {
        Time.timeScale = 1f;
        PorcentajeDevolverIra = 1f;

        RecompensaVictoria = false;
        RecompensaDinero = 0;
        ContadorTurnos = 0;

        // Tutorial
        Tutorial = VariablesGlobales.GetComponent<VariablesGlobales>().Tutorial;

        //CardList.cards = new GameObject[5];
        //CardList.cont = 0; // Inicializa el contador de la lista

        if (VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista <= 0)
        {
            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista = VariablesGlobales.GetComponent<VariablesGlobales>().MaxHealthProtagonista;
        }

        // Inicializa la lista con la cantidad de cartas del Jugadro en el combate
        for (int i = 0; i < VariablesGlobales.GetComponent<VariablesGlobales>().AmountCards.Count; i++)
        {

            for (int j = 0; j < VariablesGlobales.GetComponent<VariablesGlobales>().AmountCards[i]; j++)
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

        Mana.text = "" + ManaProtagonista; // Actualiza el texto que indica el maná del jugador
        ManaMax.text = "/ " + VariablesGlobales.GetComponent<VariablesGlobales>().MaxManaProtagonista;
        CardsPosition();

        if (VariablesGlobales.GetComponent<VariablesGlobales>().Language== 0) // English
            turnosText.text = "Turn: " + ContadorTurnos.ToString();
        else                                                                  // Spanish
            turnosText.text = "Turno: " + ContadorTurnos.ToString();
        UpdateLanguageTexts();
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

        if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
            CreateHealthBar(clonPlayer.transform.position.x, clonPlayer.transform.position.y + 1.5f, true, clonPlayer, CreateCharacterText(clonPlayer.transform.position.x, clonPlayer.transform.position.y, "YOU")); //tipo 3 = player
        else                                                                  // Spanish
            CreateHealthBar(clonPlayer.transform.position.x, clonPlayer.transform.position.y + 1.5f, true, clonPlayer, CreateCharacterText(clonPlayer.transform.position.x, clonPlayer.transform.position.y, "TÚ")); //tipo 3 = player
        clonPlayer.GetComponent<PlayerController>().VariablesGlobales = VariablesGlobales;
        clonPlayer.GetComponent<PlayerController>().CombatScene = gameObject;

        Player = clonPlayer;
    }

    /*
     * Crea de 1 a 3 enemigos de manera aleatoria
     */
    public void CreateEnemies()
    {

        GameObject clonEnemy;                    // Declara el clon del prefab
        int tipo; //0 = ira | 1 = miedo | 2 = tristeza | 3 = Boss
        string EnemyName;

        if (!VariablesGlobales.GetComponent<VariablesGlobales>().Boss)
        {
            int rand;

            if(Tutorial)
                rand = 1;                  // Un sólo enemigo porque es el tutorial
            else if (VariablesGlobales.GetComponent<VariablesGlobales>().MaxManaProtagonista <= 3 || VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista <= 50)
            {
                Debug.Log("Tiene 3 o menos de maná o 50 o menos de vida");
                rand = 2;                  // Si el Jugador tiene 3 o menos de maná o 50 o menos de vida, habrá 2 enemigos
            }
            else
                rand = Random.Range(2, 4); // Random de 2 a 3

            for (int i = 0; i < rand; i++)           // Bucle dependiendo del número de enemigos que hay en la sala
            {

                if (Tutorial)
                    tipo = 1; // Si es el tutorial, el enemigo será Miedo
                else
                    tipo = Random.Range(0, 3);

                if (tipo == 0)
                {
                    RecompensaDinero += 17;
                    if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                        EnemyName = "ANGER";
                    else                                                                  // Spanish
                        EnemyName = "IRA";
                }
                else if (tipo == 1)
                {
                    RecompensaDinero += 13;
                    if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                        EnemyName = "FEAR";
                    else                                                                  // Spanish
                        EnemyName = "MIEDO";
                }
                else
                {
                    RecompensaDinero += 9;
                    if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                        EnemyName = "SADNESS";
                    else                                                                  // Spanish
                        EnemyName = "TRISTEZA";
                    numTristeza++;
                }

                if (i == 0)                          // Si es el primer enemigo
                {

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
                    //CreateHealthBar(270, 166, false, clonEnemy, CreateCharacterText(270, -108, EnemyName));

                }
                else if (i == 1)                     // Si es el segundo enemigo
                {
                    
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
                    //CreateHealthBar(486, 58, false, clonEnemy, CreateCharacterText(486, -216, EnemyName));

                }
                else                                 // Si es el tercer enemigo
                {
                    
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
                    //CreateHealthBar(702, -50, false, clonEnemy, CreateCharacterText(702, -324, EnemyName));

                }


                ArrowEmitter.GetComponent<ArrowEmitter>().Enemies.Add(clonEnemy);

            }

        }
        else
        {

            tipo = 3; // Boss
            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                EnemyName = "BOSS";
            else                                                                  // Spanish
                EnemyName = "JEFE";
            clonEnemy = Instantiate(PrefabEnemyList[tipo]); // Crea el clon del prefab
            clonEnemy.transform.position = new Vector3(2.5f, 0.5f, 0);
            clonEnemy.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            clonEnemy.GetComponent<EnemyController>().Tipo = tipo;
            clonEnemy.GetComponent<EnemyController>().VariablesGlobales = VariablesGlobales;
            clonEnemy.GetComponent<EnemyController>().CombatScene = gameObject;
            clonEnemy.GetComponent<EnemyController>().Id = 0;
            clonEnemy.GetComponent<EnemyController>().ArrowEmitter = ArrowEmitter;
            clonEnemy.GetComponent<EnemyController>().Player = Player;
            clonEnemy.GetComponent<EnemyController>().AuraEnemigo = Instantiate(AuraEnemigoPrefab);
            clonEnemy.GetComponent<EnemyController>().AuraEnemigo.transform.position = new Vector3(2.5f, 0.5f, 1);

            EnemyList.Add(clonEnemy);
            CreateHealthBar(clonEnemy.transform.position.x, clonEnemy.transform.position.y + 3f, false, clonEnemy, CreateCharacterText(clonEnemy.transform.position.x, clonEnemy.transform.position.y - 0.5f, EnemyName));
            //CreateHealthBar(486, 58, false, clonEnemy, CreateCharacterText(486, -216, EnemyName));

            ArrowEmitter.GetComponent<ArrowEmitter>().Enemies.Add(clonEnemy);

        }


    }


    /*
     * Coloca las cartas del combate
     */
    public IEnumerator CreateCards()
    {

        if (!CartasCreadas)
        {

            CartasCreadas = true;
            GameObject clon;
            int cardType;
            bool canCreate;

            for (int i = 0; i < 5; i++)
            {

                do
                {
                    canCreate = true;
                    cardType = Random.Range(0, TotalCards.Count); // Aleatorio entre las cartas totales del Jugador para el combate
                    if (HandCardsAmount[TotalCards[cardType]] + 1 > VariablesGlobales.GetComponent<VariablesGlobales>().AmountCards[TotalCards[cardType]]/* && VariablesGlobales.GetComponent<VariablesGlobales>().AmountCards[cardType] == 0*/)
                    {
                        canCreate = false;
                    }


                } while (!canCreate);

                HandCardsAmount[TotalCards[cardType]]++;

                clon = Instantiate(Card);                                          // Crea una carta
                clon.GetComponent<CardController>().CombatScene = gameObject; // Almacena el controlador del combate en cada carta para acceder a sus variables
                clon.GetComponent<CardController>().DragZone = DragZone;         // Almacena la DragZone en cada carta para poder eliminarla una vez se acerque a ella
                clon.GetComponent<CardController>().Id = i;                        // Almacena el ID de cada carta (para saber su posicion al eliminarla de la lista)
                clon.GetComponent<CardController>().Tipo = TotalCards[cardType]; //hace que la carta sea de alguna de las del tipo
                clon.GetComponent<CardController>().VariablesGlobales = VariablesGlobales; // Almacena las variables globales en la carta
                clon.GetComponent<CardController>().ArrowEmitter = ArrowEmitter;
                clon.GetComponent<CardController>().Player = Player;
                clon.GetComponent<CardController>().CosteMana = CardManaCost(clon.GetComponent<CardController>().Tipo);
                clon.transform.SetParent(CanvasCartas, false);
                //clon.GetComponent<CardController>().TextTitle = CardTitles[i];
                //clon.GetComponent<CardController>().TextDescription = CardDescriptions[i];

                // Implementa los sprites
                clon.GetComponent<Image>().sprite = CardSprites[TotalCards[cardType]];

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

        if (TurnoJugador)
        {

            if (CardList.Count == 1)
            {

                if (!CardList[0].GetComponent<CardController>().MouseDrag && !CardList[0].GetComponent<CardController>().MouseOver)
                {

                    //CardList[0].transform.position = new Vector3(0, -4, 0);
                    //CardList[0].transform.eulerAngles = new Vector3(0, 0, 0);
                    CardList[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -480, 0);
                    CardList[0].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 0);
                    CardList[0].GetComponent<CardController>().SetPosition();
                    CardList[0].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

                }

            }
            else if (CardList.Count == 2)
            {

                if (!CardList[0].GetComponent<CardController>().MouseDrag && !CardList[0].GetComponent<CardController>().MouseOver)
                {

                    //CardList[0].transform.position = new Vector3(1, -4, 0);
                    //CardList[0].transform.eulerAngles = new Vector3(0, 0, -5);
                    CardList[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(75, -495, 0);
                    CardList[0].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -5);
                    CardList[0].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                    CardList[0].GetComponent<CardController>().SetPosition();

                }
                if (!CardList[1].GetComponent<CardController>().MouseDrag && !CardList[1].GetComponent<CardController>().MouseOver)
                {

                    //CardList[1].transform.position = new Vector3(-1, -4, 1);
                    //CardList[1].transform.eulerAngles = new Vector3(0, 0, 5);
                    CardList[1].GetComponent<RectTransform>().anchoredPosition = new Vector3(-75, -495, 0);
                    CardList[1].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 5);
                    CardList[1].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                    CardList[1].GetComponent<CardController>().SetPosition();

                }

            }
            else if (CardList.Count == 3)
            {

                if (!CardList[0].GetComponent<CardController>().MouseDrag && !CardList[0].GetComponent<CardController>().MouseOver)
                {

                    //CardList[0].transform.position = new Vector3(2, -4.15f, 0);
                    //CardList[0].transform.eulerAngles = new Vector3(0, 0, -10);
                    CardList[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(148, -495, 0);
                    CardList[0].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -10);
                    CardList[0].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                    CardList[0].GetComponent<CardController>().SetPosition();

                }
                if (!CardList[1].GetComponent<CardController>().MouseDrag && !CardList[1].GetComponent<CardController>().MouseOver)
                {

                    //CardList[1].transform.position = new Vector3(0, -4, 1);
                    //CardList[1].transform.eulerAngles = new Vector3(0, 0, 0);
                    CardList[1].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -480, 0);
                    CardList[1].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 0);
                    CardList[1].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                    CardList[1].GetComponent<CardController>().SetPosition();

                }
                if (!CardList[2].GetComponent<CardController>().MouseDrag && !CardList[2].GetComponent<CardController>().MouseOver)
                {

                    //CardList[2].transform.position = new Vector3(-2, -4.15f, 2);
                    //CardList[2].transform.eulerAngles = new Vector3(0, 0, 10);
                    CardList[2].GetComponent<RectTransform>().anchoredPosition = new Vector3(-148, -495, 0);
                    CardList[2].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 10);
                    CardList[2].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                    CardList[2].GetComponent<CardController>().SetPosition();

                }

            }
            else if (CardList.Count == 4)
            {

                if (!CardList[0].GetComponent<CardController>().MouseDrag && !CardList[0].GetComponent<CardController>().MouseOver)
                {

                    //CardList[0].transform.position = new Vector3(3, -4.35f, 0);
                    //CardList[0].transform.eulerAngles = new Vector3(0, 0, -15);
                    CardList[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(220, -522, 0);
                    CardList[0].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -15);
                    CardList[0].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                    CardList[0].GetComponent<CardController>().SetPosition();

                }
                if (!CardList[1].GetComponent<CardController>().MouseDrag && !CardList[1].GetComponent<CardController>().MouseOver)
                {

                    //CardList[1].transform.position = new Vector3(1, -4, 1);
                    //CardList[1].transform.eulerAngles = new Vector3(0, 0, -5);
                    CardList[1].GetComponent<RectTransform>().anchoredPosition = new Vector3(75, -495, 0);
                    CardList[1].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -5);
                    CardList[1].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                    CardList[1].GetComponent<CardController>().SetPosition();

                }
                if (!CardList[2].GetComponent<CardController>().MouseDrag && !CardList[2].GetComponent<CardController>().MouseOver)
                {

                    //CardList[2].transform.position = new Vector3(-1, -4, 2);
                    //CardList[2].transform.eulerAngles = new Vector3(0, 0, 5);
                    CardList[2].GetComponent<RectTransform>().anchoredPosition = new Vector3(-75, -495, 0);
                    CardList[2].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 5);
                    CardList[2].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                    CardList[2].GetComponent<CardController>().SetPosition();

                }
                if (!CardList[3].GetComponent<CardController>().MouseDrag && !CardList[3].GetComponent<CardController>().MouseOver)
                {

                    //CardList[3].transform.position = new Vector3(-3, -4.35f, 3);
                    //CardList[3].transform.eulerAngles = new Vector3(0, 0, 15);
                    CardList[3].GetComponent<RectTransform>().anchoredPosition = new Vector3(-220, -522, 0);
                    CardList[3].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 15);
                    CardList[3].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                    CardList[3].GetComponent<CardController>().SetPosition();

                }

            }
            else if (CardList.Count == 5)
            {

                if (!CardList[0].GetComponent<CardController>().MouseDrag && !CardList[0].GetComponent<CardController>().MouseOver)
                {

                    //CardList[0].transform.position = new Vector3(3.95f, -4.7f, 0);
                    //CardList[0].transform.eulerAngles = new Vector3(0, 0, -20);
                    CardList[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(289, -536, 0);
                    CardList[0].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -20);
                    CardList[0].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                    CardList[0].GetComponent<CardController>().SetPosition();

                }
                if (!CardList[1].GetComponent<CardController>().MouseDrag && !CardList[1].GetComponent<CardController>().MouseOver)
                {

                    //CardList[1].transform.position = new Vector3(2, -4.15f, 1);
                    //CardList[1].transform.eulerAngles = new Vector3(0, 0, -10);
                    CardList[1].GetComponent<RectTransform>().anchoredPosition = new Vector3(148, -495, 0);
                    CardList[1].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -10);
                    CardList[1].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                    CardList[1].GetComponent<CardController>().SetPosition();

                }
                if (!CardList[2].GetComponent<CardController>().MouseDrag && !CardList[2].GetComponent<CardController>().MouseOver)
                {

                    //CardList[2].transform.position = new Vector3(0, -4, 2);
                    //CardList[2].transform.eulerAngles = new Vector3(0, 0, 0);
                    CardList[2].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -480, 0);
                    CardList[2].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 0);
                    CardList[2].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                    CardList[2].GetComponent<CardController>().SetPosition();

                }
                if (!CardList[3].GetComponent<CardController>().MouseDrag && !CardList[3].GetComponent<CardController>().MouseOver)
                {

                    //CardList[3].transform.position = new Vector3(-2, -4.15f, 3);
                    //CardList[3].transform.eulerAngles = new Vector3(0, 0, 10);
                    CardList[3].GetComponent<RectTransform>().anchoredPosition = new Vector3(-148, -495, 0);
                    CardList[3].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 10);
                    CardList[3].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                    CardList[3].GetComponent<CardController>().SetPosition();

                }
                if (!CardList[4].GetComponent<CardController>().MouseDrag && !CardList[4].GetComponent<CardController>().MouseOver)
                {

                    //CardList[4].transform.position = new Vector3(-3.95f, -4.7f, 4);
                    //CardList[4].transform.eulerAngles = new Vector3(0, 0, 20);
                    CardList[4].GetComponent<RectTransform>().anchoredPosition = new Vector3(-289, -536, 0);
                    CardList[4].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 20);
                    CardList[4].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
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

            if (VariablesGlobales.GetComponent<VariablesGlobales>().Boss)
                EnemyList[0].GetComponent<EnemyController>().contAcumulacionDanyoBoss = 0;

            ManaProtagonista = VariablesGlobales.GetComponent<VariablesGlobales>().MaxManaProtagonista; // Se resetea el maná del jugador

            if (Player.GetComponent<PlayerController>().ReducirMana)
            {

                ManaProtagonista -= 1;
                Player.GetComponent<PlayerController>().ReducirMana = false;

            }

            TurnoJugador = true;                                                                        // Cambia de turno
            StartCoroutine(CreateCards());

        }

        if (VariablesGlobales.GetComponent<VariablesGlobales>().Boss)
            EnemyList[0].GetComponent<EnemyController>().BossDanyoComprobado = false;

        Player.GetComponent<PlayerController>().ContadorDeTurnos++;
        ContadorTurnos++;
    }

    //crea el popUp de texto con la cantidad de daño o de heal
    public IEnumerator CreateDmgHealText(bool IsHeal, int Amount, GameObject Character, bool esperarAntes)
    {
        if (esperarAntes)
        {
            yield return new WaitForSeconds(0.5f);
        }

        if (Amount < 0)
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
            VariablesGlobales.GetComponent<VariablesGlobales>().CardUses[0]++;
            int danyo = 5 + Player.GetComponent<PlayerController>().Debilidad + Player.GetComponent<PlayerController>().Fuerza; ;
            //Ataque 5 de daño
            if (EnemyList[enemigo].GetComponent<EnemyController>().Transformacion) // Si el enemigo está transformado el ataque le curará
            {

                EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo += danyo;
                StartCoroutine(CreateDmgHealText(true, danyo, EnemyList[enemigo], false));

            }
            else
            {

                EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo -= danyo;
                // en esta fución como ultimo parametro debería pasarse el gameobject del enemigo al que ataca la flecha
                //le puse "EnemyList[0]" porque es el que marca arriba para restarle la vida
                StartCoroutine(CreateDmgHealText(false, danyo, EnemyList[enemigo], false));

                if (VariablesGlobales.GetComponent<VariablesGlobales>().Boss)
                    EnemyList[enemigo].GetComponent<EnemyController>().contAcumulacionDanyoBoss += danyo;

            }

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
            VariablesGlobales.GetComponent<VariablesGlobales>().CardUses[1]++;
            //Ataque 3 x 2 de daño después de 0,5 seg
            StartCoroutine(DoubleAttack(enemigo, 3, 0.5f, 1));

        }
        else if (tipo == 2)
        {
            VariablesGlobales.GetComponent<VariablesGlobales>().CardUses[2]++;
            //Ataque 5 de daño a todos
            int danyo = 5 + Player.GetComponent<PlayerController>().Debilidad + Player.GetComponent<PlayerController>().Fuerza;
            for (int i = 0; i < EnemyList.Count; i++)
            {

                if (EnemyList[i].GetComponent<EnemyController>().Transformacion) // Si el enemigo está transformado el ataque le curará
                {

                    EnemyList[i].GetComponent<EnemyController>().HealthEnemigo += danyo;
                    StartCoroutine(CreateDmgHealText(true, danyo, EnemyList[i], false));

                }
                else
                {

                    EnemyList[i].GetComponent<EnemyController>().HealthEnemigo -= danyo;
                    StartCoroutine(CreateDmgHealText(false, danyo, EnemyList[i], false));

                    if (VariablesGlobales.GetComponent<VariablesGlobales>().Boss)
                        EnemyList[i].GetComponent<EnemyController>().contAcumulacionDanyoBoss += danyo;

                }

                EnemyList[i].GetComponent<EnemyController>().RecibirDanyo = true;

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
            VariablesGlobales.GetComponent<VariablesGlobales>().CardUses[3]++;

            //Ataque 10 de daño
            int danyo = 10 + Player.GetComponent<PlayerController>().Debilidad + Player.GetComponent<PlayerController>().Fuerza;

            if (EnemyList[enemigo].GetComponent<EnemyController>().Transformacion) // Si el enemigo está transformado el ataque le curará
            {

                EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo += danyo;
                StartCoroutine(CreateDmgHealText(true, danyo, EnemyList[enemigo], false));

            }
            else
            {

                EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo -= danyo;
                StartCoroutine(CreateDmgHealText(false, danyo, EnemyList[enemigo], false));

                if (VariablesGlobales.GetComponent<VariablesGlobales>().Boss)
                    EnemyList[enemigo].GetComponent<EnemyController>().contAcumulacionDanyoBoss += danyo;

            }

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
            VariablesGlobales.GetComponent<VariablesGlobales>().CardUses[4]++;

            //Ataque 20 de daño
            int danyo = 20 + Player.GetComponent<PlayerController>().Debilidad + Player.GetComponent<PlayerController>().Fuerza;

            if (EnemyList[enemigo].GetComponent<EnemyController>().Transformacion) // Si el enemigo está transformado el ataque le curará
            {

                EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo += danyo;
                StartCoroutine(CreateDmgHealText(true, danyo, EnemyList[enemigo], false));

            }
            else
            {

                EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo -= danyo;
                StartCoroutine(CreateDmgHealText(false, danyo, EnemyList[enemigo], false));

                if (VariablesGlobales.GetComponent<VariablesGlobales>().Boss)
                    EnemyList[enemigo].GetComponent<EnemyController>().contAcumulacionDanyoBoss += danyo;

            }

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
            VariablesGlobales.GetComponent<VariablesGlobales>().CardUses[5]++;

            //Ataque 10 x 2 de daño después de 0,5 seg
            StartCoroutine(DoubleAttack(enemigo, 10, 0.5f, 5));

        }
        else if (tipo == 6)
        {
            VariablesGlobales.GetComponent<VariablesGlobales>().CardUses[6]++;

            // El Jugador gana 2 de maná
            ManaProtagonista += 2;
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

        }
        else if (tipo == 7)
        {
            VariablesGlobales.GetComponent<VariablesGlobales>().CardUses[7]++;

            // El Jugador se cura 5
            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += 5;
            StartCoroutine(CreateDmgHealText(true, 5, Player, false));
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

        }
        else if (tipo == 8)
        {
            VariablesGlobales.GetComponent<VariablesGlobales>().CardUses[8]++;

            // El Jugador se cura 10
            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += 10;
            StartCoroutine(CreateDmgHealText(true, 10, Player, false));
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

        }
        else if (tipo == 9)
        {
            VariablesGlobales.GetComponent<VariablesGlobales>().CardUses[9]++;

            // El Jugador roba 5 de vida a un enemigo
            int danyo = 5 + Player.GetComponent<PlayerController>().Debilidad + Player.GetComponent<PlayerController>().Fuerza;

            if (EnemyList[enemigo].GetComponent<EnemyController>().Transformacion) // Si el enemigo está transformado el ataque le curará
            {

                EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo += danyo;
                StartCoroutine(CreateDmgHealText(true, danyo, EnemyList[enemigo], false));

                VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += 0;
                StartCoroutine(CreateDmgHealText(true, 0, Player, false));

            }
            else
            {

                EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo -= danyo;
                StartCoroutine(CreateDmgHealText(false, danyo, EnemyList[enemigo], false));

                VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += danyo;
                StartCoroutine(CreateDmgHealText(true, danyo, Player, false));

                if (VariablesGlobales.GetComponent<VariablesGlobales>().Boss)
                    EnemyList[enemigo].GetComponent<EnemyController>().contAcumulacionDanyoBoss += danyo;

            }

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

        else if (tipo == 10)
        {
            VariablesGlobales.GetComponent<VariablesGlobales>().CardUses[10]++;

            // El Jugador roba 10 de vida a un enemigo
            int danyo = 10 + Player.GetComponent<PlayerController>().Debilidad + Player.GetComponent<PlayerController>().Fuerza;

            if (EnemyList[enemigo].GetComponent<EnemyController>().Transformacion) // Si el enemigo está transformado el ataque le curará
            {

                EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo += danyo;
                StartCoroutine(CreateDmgHealText(true, danyo, EnemyList[enemigo], false));

                VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += 0;
                StartCoroutine(CreateDmgHealText(true, 0, Player, false));

            }
            else
            {

                EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo -= danyo;
                StartCoroutine(CreateDmgHealText(false, danyo, EnemyList[enemigo], false));

                VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += danyo;
                StartCoroutine(CreateDmgHealText(true, danyo, Player, false));

                if (VariablesGlobales.GetComponent<VariablesGlobales>().Boss)
                    EnemyList[enemigo].GetComponent<EnemyController>().contAcumulacionDanyoBoss += danyo;

            }

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
        else if (tipo == 11)
        {
            VariablesGlobales.GetComponent<VariablesGlobales>().CardUses[11]++;

            // El Jugador roba 5 de vida a todos los enemigos
            int i;
            int danyo = 5 + Player.GetComponent<PlayerController>().Debilidad + Player.GetComponent<PlayerController>().Fuerza;
            int contTransformados = 0;

            for (i = 0; i < EnemyList.Count; i++)
            {

                if (EnemyList[i].GetComponent<EnemyController>().Transformacion) // Si el enemigo está transformado el ataque le curará
                {

                    EnemyList[i].GetComponent<EnemyController>().HealthEnemigo += danyo;
                    StartCoroutine(CreateDmgHealText(true, danyo, EnemyList[i], false));

                    contTransformados++;

                }
                else
                {

                    EnemyList[i].GetComponent<EnemyController>().HealthEnemigo -= danyo;
                    StartCoroutine(CreateDmgHealText(false, danyo, EnemyList[i], false));

                    if (VariablesGlobales.GetComponent<VariablesGlobales>().Boss)
                        EnemyList[i].GetComponent<EnemyController>().contAcumulacionDanyoBoss += danyo;

                }

                EnemyList[i].GetComponent<EnemyController>().RecibirDanyo = true;

                // Ira devolver ataque
                if (EnemyList[i].GetComponent<EnemyController>().Tipo == 0) // Si el enemigo seleccionado es Ira
                {
                    float porcentajeIraDevolver = Random.Range(0f, 11f);

                    if (porcentajeIraDevolver < PorcentajeDevolverIra)
                        StartCoroutine(IraDevolverAtaque(danyo, tipo, i));

                }

            }

            ControlEsperanzado();
            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += (danyo * (i + contTransformados));
            StartCoroutine(CreateDmgHealText(true, (danyo * (i + contTransformados)), Player, false));
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

        }
        else if (tipo == 12)
        {
            VariablesGlobales.GetComponent<VariablesGlobales>().CardUses[12]++;

            // El Jugador roba 10 de vida a todos los enemigos
            int i;
            int danyo = 10 + Player.GetComponent<PlayerController>().Debilidad + Player.GetComponent<PlayerController>().Fuerza;
            int contTransformados = 0;

            for (i = 0; i < EnemyList.Count; i++)
            {

                if (EnemyList[i].GetComponent<EnemyController>().Transformacion) // Si el enemigo está transformado el ataque le curará
                {

                    EnemyList[i].GetComponent<EnemyController>().HealthEnemigo += danyo;
                    StartCoroutine(CreateDmgHealText(true, danyo, EnemyList[i], false));

                    contTransformados++;

                }
                else
                {

                    EnemyList[i].GetComponent<EnemyController>().HealthEnemigo -= danyo;
                    StartCoroutine(CreateDmgHealText(false, danyo, EnemyList[i], false));

                    if (VariablesGlobales.GetComponent<VariablesGlobales>().Boss)
                        EnemyList[i].GetComponent<EnemyController>().contAcumulacionDanyoBoss += danyo;

                }

                EnemyList[i].GetComponent<EnemyController>().RecibirDanyo = true;

                // Ira devolver ataque
                if (EnemyList[i].GetComponent<EnemyController>().Tipo == 0) // Si el enemigo seleccionado es Ira
                {
                    float porcentajeIraDevolver = Random.Range(0f, 11f);

                    if (porcentajeIraDevolver < PorcentajeDevolverIra)
                        StartCoroutine(IraDevolverAtaque(danyo, tipo, i));

                }

            }

            ControlEsperanzado();
            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += (danyo * (i - contTransformados));
            StartCoroutine(CreateDmgHealText(true, (danyo * (i - contTransformados)), Player, false));
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

        }
        else if (tipo == 13)
        {
            VariablesGlobales.GetComponent<VariablesGlobales>().CardUses[13]++;

            // El Jugador bloquea a un enemigo
            EnemyList[enemigo].GetComponent<EnemyController>().Bloqueado = true;
            EnemyList[enemigo].GetComponent<EnemyController>().RecibirDanyo = true;
            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                CreateSpellText("Stunned", EnemyList[enemigo]);
            else                                                                  // Spanish
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
            VariablesGlobales.GetComponent<VariablesGlobales>().CardUses[14]++;

            // El Jugador bloquea a un enemigo pero le cura 10
            EnemyList[enemigo].GetComponent<EnemyController>().Bloqueado = true;
            EnemyList[enemigo].GetComponent<EnemyController>().RecibirDanyo = true;
            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                CreateSpellText("Stunned", EnemyList[enemigo]);
            else                                                                  // Spanish
                CreateSpellText("Bloqueado", EnemyList[enemigo]);
            // StartCoroutine(wait(0.5f)); //para que espere 0.5s antes de poner el otro pop up

            EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo += 10;
            StartCoroutine(CreateDmgHealText(true, 10, EnemyList[enemigo], true));
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
            VariablesGlobales.GetComponent<VariablesGlobales>().CardUses[15]++;

            // El Jugador aplica Débil a un enemigo durante 3 turnos (efecto acumulativo)
            EnemyList[enemigo].GetComponent<EnemyController>().Debilitado = true;
            EnemyList[enemigo].GetComponent<EnemyController>().Debilidad -= 3;
            EnemyList[enemigo].GetComponent<EnemyController>().ContadorDeTurnosDebilitado += 3;
            EnemyList[enemigo].GetComponent<EnemyController>().RecibirDanyo = true;
            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                CreateSpellText("Weakened", EnemyList[enemigo]);
            else                                                                  // Spanish
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

            EnemyList[enemigo].GetComponent<EnemyController>().playerSeBufa = true;

        }
        else if (tipo == 16)
        {
            VariablesGlobales.GetComponent<VariablesGlobales>().CardUses[16]++;

            // El Jugador aplica Débil a todos los enemigos durante 3 turnos (efecto acumulativo)
            for (int i = 0; i < EnemyList.Count; i++)
            {

                EnemyList[i].GetComponent<EnemyController>().Debilitado = true;
                EnemyList[i].GetComponent<EnemyController>().Debilidad -= 3;
                EnemyList[i].GetComponent<EnemyController>().ContadorDeTurnosDebilitado += 3;
                EnemyList[i].GetComponent<EnemyController>().RecibirDanyo = true;
                if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                    CreateSpellText("Weakened", EnemyList[i]);
                else                                                                  // Spanish
                    CreateSpellText("Debilitado", EnemyList[i]);

                // Ira devolver ataque
                if (EnemyList[i].GetComponent<EnemyController>().Tipo == 0) // Si el enemigo seleccionado es Ira
                {
                    float porcentajeIraDevolver = Random.Range(0f, 11f);

                    if (porcentajeIraDevolver < PorcentajeDevolverIra)
                        StartCoroutine(IraDevolverAtaque(-1, tipo, i));

                }

                EnemyList[i].GetComponent<EnemyController>().playerSeBufa = true;

            }

            Debug.Log("Todos los Enemigos Debilitados");
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

        }
        else if (tipo == 17)
        {
            VariablesGlobales.GetComponent<VariablesGlobales>().CardUses[17]++;

            // El Jugador se aplica Fuerte 4 turnos (efecto acumulativo)
            Player.GetComponent<PlayerController>().Fuerte = true;
            Player.GetComponent<PlayerController>().Fuerza += 3;
            Player.GetComponent<PlayerController>().ContadorDeTurnosFuerte += 4;
            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                CreateSpellText("Strong", Player);
            else                                                                  // Spanish
                CreateSpellText("Fuerte", Player);

            Debug.Log("El Jugador obtiene Fuerte");
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

            for (int i = 0; i < EnemyList.Count; i++)
                EnemyList[i].GetComponent<EnemyController>().playerSeBufa = true;

        }
        else if (tipo == 18)
        {
            VariablesGlobales.GetComponent<VariablesGlobales>().CardUses[18]++;

            // El Jugador se aplica Fuerte 4 turnos (efecto acumulativo) pero cura 5 a todos los enemigos
            Player.GetComponent<PlayerController>().Fuerte = true;
            Player.GetComponent<PlayerController>().Fuerza += 3;
            Player.GetComponent<PlayerController>().ContadorDeTurnosFuerte += 4;
            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                CreateSpellText("Strong", Player);
            else                                                                  // Spanish
                CreateSpellText("Fuerte", Player);
            CreateSpellText("Fuerte", Player);

            for (int i = 0; i < EnemyList.Count; i++)
            {

                EnemyList[i].GetComponent<EnemyController>().HealthEnemigo += 5;
                StartCoroutine(CreateDmgHealText(true, 5, EnemyList[i], false));

                EnemyList[i].GetComponent<EnemyController>().playerSeBufa = true;

            }
            Debug.Log("El Jugador obtiene Fuerte");
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

        }
        else if (tipo == 19)
        {
            VariablesGlobales.GetComponent<VariablesGlobales>().CardUses[19]++;
            // El Jugador se aplica Esperanza durante 4 turnos
            Player.GetComponent<PlayerController>().Esperanzado = true;
            Player.GetComponent<PlayerController>().Esperanza += 3;
            Player.GetComponent<PlayerController>().ContadorDeTurnosEsperanzado += 4;
            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                CreateSpellText("Hopeful", Player);
            else                                                                  // Spanish
                CreateSpellText("Esperanzado", Player);
            Debug.Log("El Jugador obtiene Esperanza");
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

            for (int i = 0; i < EnemyList.Count; i++)
                EnemyList[i].GetComponent<EnemyController>().playerSeBufa = true;

        }
        else if (tipo == 20)
        {
            VariablesGlobales.GetComponent<VariablesGlobales>().CardUses[20]++;

            // El Jugador aplica Envenenado a un Enemigo durante 3 turnos (efecto acumulativo)
            EnemyList[enemigo].GetComponent<EnemyController>().Envenenado = true;
            EnemyList[enemigo].GetComponent<EnemyController>().Veneno += 3;
            EnemyList[enemigo].GetComponent<EnemyController>().ContadorDeTurnosEnvenenado += 3;
            EnemyList[enemigo].GetComponent<EnemyController>().RecibirDanyo = true;
            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                CreateSpellText("Poisoned", EnemyList[enemigo]);
            else                                                                  // Spanish
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

            for (int i = 0; i < EnemyList.Count; i++)
                EnemyList[i].GetComponent<EnemyController>().playerSeBufa = true;

        }
        else if (tipo == 21)
        {
            VariablesGlobales.GetComponent<VariablesGlobales>().CardUses[21]++;

            // El Jugador aplica Débil a un enemigo pero le cura 15
            EnemyList[enemigo].GetComponent<EnemyController>().Debilitado = true;
            EnemyList[enemigo].GetComponent<EnemyController>().Debilidad -= 3;
            EnemyList[enemigo].GetComponent<EnemyController>().ContadorDeTurnosDebilitado += 3;
            EnemyList[enemigo].GetComponent<EnemyController>().RecibirDanyo = true;

            EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo += 15;

            //StartCoroutine(wait(0.5f)); //para que espere 0.5s antes de poner el otro pop up
            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                CreateSpellText("Weakened", EnemyList[enemigo]);
            else                                                                  // Spanish
                CreateSpellText("Debilitado", EnemyList[enemigo]);
            StartCoroutine(CreateDmgHealText(true, 15, EnemyList[enemigo], true));

            Debug.Log("Enemigo Debilitado");
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

            // Ira devolver ataque
            if (EnemyList[enemigo].GetComponent<EnemyController>().Tipo == 0) // Si el enemigo seleccionado es Ira
            {
                float porcentajeIraDevolver = Random.Range(0f, 11f);

                if (porcentajeIraDevolver < PorcentajeDevolverIra)
                    StartCoroutine(IraDevolverAtaque(-1, tipo, enemigo));

            }

            for (int i = 0; i < EnemyList.Count; i++)
                EnemyList[i].GetComponent<EnemyController>().playerSeBufa = true;

        }
        else if (tipo == 22)
        {
            VariablesGlobales.GetComponent<VariablesGlobales>().CardUses[22]++;

            // Durante un turno, todos los ataques al Jugador le curan en vez de hacerle daño
            Player.GetComponent<PlayerController>().Transformacion = true;
            Player.GetComponent<PlayerController>().ContadorDeTurnosTransformacion += 1;
            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                CreateSpellText("Transformed", Player);
            else                                                                  // Spanish
                CreateSpellText("Transformado", Player);
            Debug.Log("Jugador Transformado");
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

            for (int i = 0; i < EnemyList.Count; i++)
                EnemyList[i].GetComponent<EnemyController>().playerSeBufa = true;

        }
        else if (tipo == 23)
        {
            VariablesGlobales.GetComponent<VariablesGlobales>().CardUses[23]++;

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
                Player.GetComponent<PlayerController>().ContadorFuerza = 0;
                Player.GetComponent<PlayerController>().fuerte_icon = 0;
                Player.GetComponent<PlayerController>().EliminarSpell(2);

            }

            if (Player.GetComponent<PlayerController>().Esperanzado)    // Esperanza
            {

                Player.GetComponent<PlayerController>().Esperanzado = false;
                Player.GetComponent<PlayerController>().Esperanza = 0;
                Player.GetComponent<PlayerController>().ContadorDeTurnosEsperanzado = 0;
                Player.GetComponent<PlayerController>().ContadorEsperanza = 0;
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

            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                CreateSpellText("Effects Removed", Player);
            else                                                                  // Spanish
                CreateSpellText("Efectos Eliminados", Player);
            Debug.Log("Efectos del Jugador eliminados");
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

        }

        if (tipo != 1 && tipo != 5)
        {

            if (Player.GetComponent<PlayerController>().Envenenado) // Creo que esto tiene que ser cada vez que ataca el Jugador, no cada vez que usa una carta (hablar con Flipper)
            {
                //  yield return new WaitForSeconds(1);
              // StartCoroutine(wait(0.5f));
                VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista -= Player.GetComponent<PlayerController>().Veneno;
                StartCoroutine(CreateDmgHealText(false, Player.GetComponent<PlayerController>().Veneno, Player, true));
            }

        }

        //if(tipo != 1 || tipo != 5) // Ya se hace este control de ambos casos en la función DoubleAttack()
        //{

        //    if (VariablesGlobales.GetComponent<VariablesGlobales>().Boss)
        //        if (EnemyList[0].GetComponent<EnemyController>().contAcumulacionDanyoBoss > 20)
        //            //StartCoroutine(EnemyList[0].GetComponent<EnemyController>().ControlAcumulacionDanyoBoss());
        //            EnemyList[0].GetComponent<EnemyController>().ControlAcumulacionDanyoBoss();

        //}

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
            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
            {

                VictoriaDerrotaText.text = "DEFEAT";
                RecompensaText.text = "¡You have been defeated!";

            }
            else                                                                  // Spanish
            {

                VictoriaDerrotaText.text = "DERROTA";
                RecompensaText.text = "¡Te han derrotado!";

            }
            Time.timeScale = 0f;
            // VariablesGlobales.GetComponent<VariablesGlobales>().EstaEnPausa = true;
            botonTurno.enabled = false;


        }
        //else if (VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista > 0 && enemigosVivos == 0)
        else if (EnemyList.Count == 0 && !VariablesGlobales.GetComponent<VariablesGlobales>().Boss)
        {
            RecompensaVictoria = true;
            Debug.Log("victoria lol");
            VictoriaDerrotaPanel.SetActive(true);

            if (ContadorTurnos < 20) //si los turnos fueron menos de 20, gana más dinero
            {
                RecompensaDinero += RecompensaDinero / 5; //le suma al dinero actual su quinta parte
            }
            else
            {
                RecompensaDinero -= RecompensaDinero / 5; //le resta al dinero actual su quinta parte
            }


            if (VariablesGlobales.GetComponent<VariablesGlobales>().PasivaCurarseCombate)
            {

                VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += VariablesGlobales.GetComponent<VariablesGlobales>().PasivaCuracionCombate;
                StartCoroutine(CreateDmgHealText(true, VariablesGlobales.GetComponent<VariablesGlobales>().PasivaCuracionCombate, Player, false));

            }

            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
            {

                VictoriaDerrotaText.text = "VICTORY";
                if (!VariablesGlobales.GetComponent<VariablesGlobales>().PasivaGanarDinero)
                    RecompensaText.text = "You earn " + RecompensaDinero + " gold";
                else
                    RecompensaText.text = "You earn " + RecompensaDinero + " + (Passive: " + VariablesGlobales.GetComponent<VariablesGlobales>().PasivaDinero + ")" + " gold";

            }
            else                                                                  // Spanish
            {

                VictoriaDerrotaText.text = "VICTORIA";
                if (!VariablesGlobales.GetComponent<VariablesGlobales>().PasivaGanarDinero)
                    RecompensaText.text = "Obtienes " + RecompensaDinero + " de oro";
                else
                    RecompensaText.text = "Obtienes " + RecompensaDinero + " + (Pasiva: " + VariablesGlobales.GetComponent<VariablesGlobales>().PasivaDinero + ")" + " de oro";

            }

            if (victoria_etc == 0 && RecompensaVictoria == true)
            {
                Debug.Log("ganas $" + RecompensaDinero + " de recompensa");
                VariablesGlobales.GetComponent<VariablesGlobales>().DineroAmount += (RecompensaDinero + VariablesGlobales.GetComponent<VariablesGlobales>().PasivaDinero);
                victoria_etc += 1;

            }

            Time.timeScale = 0f;
            // VariablesGlobales.GetComponent<VariablesGlobales>().EstaEnPausa = true;
            botonTurno.enabled = false;

        }
        else if (EnemyList.Count == 0 && VariablesGlobales.GetComponent<VariablesGlobales>().Boss)
        {

            RecompensaVictoria = true;
            Debug.Log("victoria lol");
            VictoriaDerrotaPanel.SetActive(true);
            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
            {

                VictoriaDerrotaText.text = "VICTORY";
                RecompensaText.text = "Congratulations, you have defeated the boss and completed the dungeon";

            }
            else                                                                  // Spanish
            {

                VictoriaDerrotaText.text = "VICTORIA";
                RecompensaText.text = "Enhorabuena, has derrotado al boss y completado la mazmorra";

            }

            //if (victoria_etc == 0 && RecompensaVictoria == true)
            //{
            //    Debug.Log("ganas $" + RecompensaDinero + " de recompensa");
            //    VariablesGlobales.GetComponent<VariablesGlobales>().DineroAmount += RecompensaDinero;
            //    victoria_etc += 1;
            //}

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
        bool salir = false;
        if (danyo < 0)
            danyo = 0;

        int danyoTotal = danyo + Player.GetComponent<PlayerController>().Debilidad + Player.GetComponent<PlayerController>().Fuerza;

        if (EnemyList[enemigo].GetComponent<EnemyController>().Transformacion) // Si el enemigo está transformado el ataque le curará
        {

            EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo += danyoTotal;
            StartCoroutine(CreateDmgHealText(true, danyoTotal, EnemyList[enemigo], false));

        }
        else
        {

            if ((EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo - danyoTotal) <= 0)
            {
                salir = true;
                Debug.Log("                                     salir = true");
            }
            EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo -= danyoTotal;

            StartCoroutine(CreateDmgHealText(false, danyoTotal, EnemyList[enemigo], false));

            if (VariablesGlobales.GetComponent<VariablesGlobales>().Boss)
                EnemyList[enemigo].GetComponent<EnemyController>().contAcumulacionDanyoBoss += danyoTotal;

        }

        ControlEsperanzado();
        if (!salir)
        {
            Debug.Log("                                     !salir");
            EnemyList[enemigo].GetComponent<EnemyController>().RecibirDanyo = true;
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

            if (Player.GetComponent<PlayerController>().Envenenado) // Creo que esto tiene que ser cada vez que ataca el Jugador, no cada vez que usa una carta (hablar con Flipper)
            {
                //  yield return new WaitForSeconds(1);
                VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista -= Player.GetComponent<PlayerController>().Veneno;
                StartCoroutine(CreateDmgHealText(false, Player.GetComponent<PlayerController>().Veneno, Player, false));
            }

            yield return new WaitForSeconds(tiempo);

            if (EnemyList[enemigo].GetComponent<EnemyController>().Transformacion) // Si el enemigo está transformado el ataque le curará
            {

                EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo += danyoTotal;
                StartCoroutine(CreateDmgHealText(true, danyoTotal, EnemyList[enemigo], false));

            }
            else
            {

                EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo -= danyoTotal;
                StartCoroutine(CreateDmgHealText(false, danyoTotal, EnemyList[enemigo], false));

                if (VariablesGlobales.GetComponent<VariablesGlobales>().Boss)
                    EnemyList[enemigo].GetComponent<EnemyController>().contAcumulacionDanyoBoss += danyoTotal;

            }

            ControlEsperanzado();
            //              !!    comento lo de abajo para que la animación no se repita 2 veces        !!

           // EnemyList[enemigo].GetComponent<EnemyController>().RecibirDanyo = true;
           // Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

            if (Player.GetComponent<PlayerController>().Envenenado) // Creo que esto tiene que ser cada vez que ataca el Jugador, no cada vez que usa una carta (hablar con Flipper)
            {
                //  yield return new WaitForSeconds(1);
                VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista -= Player.GetComponent<PlayerController>().Veneno;
                StartCoroutine(CreateDmgHealText(false, Player.GetComponent<PlayerController>().Veneno, Player, true));
            }

            // Ira devolver ataque
            if (EnemyList[enemigo].GetComponent<EnemyController>().Tipo == 0) // Si el enemigo seleccionado es Ira
            {
                float porcentajeIraDevolver = Random.Range(0f, 11f);

                if (porcentajeIraDevolver < PorcentajeDevolverIra)
                    StartCoroutine(IraDevolverAtaque(danyo, tipo, enemigo));

            }
        }

        //if (VariablesGlobales.GetComponent<VariablesGlobales>().Boss)
        //    if (EnemyList[0].GetComponent<EnemyController>().contAcumulacionDanyoBoss > 20)
        //        //StartCoroutine(EnemyList[0].GetComponent<EnemyController>().ControlAcumulacionDanyoBoss());
        //        EnemyList[0].GetComponent<EnemyController>().ControlAcumulacionDanyoBoss();

    }

    public void ControlEsperanzado()
    {

        if (Player.GetComponent<PlayerController>().Esperanzado)
        {

            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += Player.GetComponent<PlayerController>().Esperanza;
          //  StartCoroutine(wait(0.5f)); //para que espere 0.5s antes de poner el otro pop up
            StartCoroutine(CreateDmgHealText(true, Player.GetComponent<PlayerController>().Esperanza, Player, true));

        }

    }

    public IEnumerator IraDevolverAtaque(int danyo, int tipo, int enemigo)
    {

        yield return new WaitForSeconds(0.5f);

        Debug.Log("Ira devuelve el ataque");

        if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
            CreateSpellText("Return Attack", EnemyList[enemigo]);
        else                                                                  // Spanish
            CreateSpellText("Devolver Ataque", EnemyList[enemigo]);

        if (tipo == 0 || tipo == 2 || tipo == 3 || tipo == 4)
        {

            if (Player.GetComponent<PlayerController>().Transformacion)
            {

                VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += danyo;
                StartCoroutine(CreateDmgHealText(true, danyo, Player, false));

            }
            else
            {

                VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista -= danyo;
                StartCoroutine(CreateDmgHealText(false, danyo, Player, false));

            }

            EnemyList[enemigo].GetComponent<EnemyController>().EnemyAnimator.SetBool("atacar", true);

        }
        else if (tipo == 1 || tipo == 5)
        {

            StartCoroutine(EnemyList[enemigo].GetComponent<EnemyController>().DoubleAttack(danyo, 0.5f));

        }
        else if (tipo == 9 || tipo == 10 || tipo == 11 || tipo == 12)
        {

            if (Player.GetComponent<PlayerController>().Transformacion)
            {

                VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += danyo;
                StartCoroutine(CreateDmgHealText(true, danyo, Player, false));

                EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo += 0;
                StartCoroutine(CreateDmgHealText(true, 0, EnemyList[enemigo], false));

            }
            else
            {

                VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista -= danyo;
                StartCoroutine(CreateDmgHealText(false, danyo, Player, false));

                EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo += danyo;
                StartCoroutine(CreateDmgHealText(true, danyo, EnemyList[enemigo], false));

            }

            EnemyList[enemigo].GetComponent<EnemyController>().EnemyAnimator.SetBool("atacar", true);

        }
        else if (tipo == 13)
        {

            Player.GetComponent<PlayerController>().Bloqueado = true;
            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                CreateSpellText("Stunned", Player);
            else                                                                  // Spanish
                CreateSpellText("Bloqueado", Player);

            EnemyList[enemigo].GetComponent<EnemyController>().EnemyAnimator.SetBool("atacar", true);

        }
        else if (tipo == 14)
        {

            Player.GetComponent<PlayerController>().Bloqueado = true;
            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                CreateSpellText("Stunned", Player);
            else                                                                  // Spanish
                CreateSpellText("Bloqueado", Player);

            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += 10;
            StartCoroutine(CreateDmgHealText(true, 10, Player, true));

            EnemyList[enemigo].GetComponent<EnemyController>().EnemyAnimator.SetBool("atacar", true);

        }
        else if (tipo == 15 || tipo == 16)
        {

            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                CreateSpellText("Weakened", Player);
            else                                                                  // Spanish
                CreateSpellText("Debilitado", Player);
            Player.GetComponent<PlayerController>().Debilitado = true;
            Player.GetComponent<PlayerController>().Debilidad -= 3;
            Player.GetComponent<PlayerController>().ContadorDeTurnosDebilitadoDevolverIra += 3;

            EnemyList[enemigo].GetComponent<EnemyController>().EnemyAnimator.SetBool("atacar", true);

        }
        else if (tipo == 20)
        {

            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                CreateSpellText("Poisoned", Player);
            else                                                                  // Spanish
                CreateSpellText("Envenenado", Player);
            Player.GetComponent<PlayerController>().Envenenado = true;
            Player.GetComponent<PlayerController>().Veneno += 3;
            Player.GetComponent<PlayerController>().ContadorDeTurnosEnvenenadoDevolverIra += 3;

            EnemyList[enemigo].GetComponent<EnemyController>().EnemyAnimator.SetBool("atacar", true);

        }
        else if (tipo == 21)
        {

            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                CreateSpellText("Poisoned", Player);
            else                                                                  // Spanish
                CreateSpellText("Envenenado", Player);
            Player.GetComponent<PlayerController>().Envenenado = true;
            Player.GetComponent<PlayerController>().Veneno += 3;
            Player.GetComponent<PlayerController>().ContadorDeTurnosEnvenenadoDevolverIra += 3;

            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += 15;
            StartCoroutine(CreateDmgHealText(true, 15, Player, true));

            EnemyList[enemigo].GetComponent<EnemyController>().EnemyAnimator.SetBool("atacar", true);

        }


    }

    public void UpdateLanguageTexts()
    {

        if(VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
        {

            EndTurnText.text = "End Turn";
            LeaveCombatText.text = "Leave Combat";

        }
        else                                                                  // Spanish
        {

            EndTurnText.text = "Finalizar Turno";
            LeaveCombatText.text = "Salir del Combate";

        }

    }

    //public IEnumerator wait(float sec)
    //{
    //    Debug.Log("wait segundos");
    //    yield return new WaitForSeconds(sec);
    //    Debug.Log("termina wait segundos");
    //}
}
