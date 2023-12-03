using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
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

    public List<TMP_Text> CardTitles;
    public List<TMP_Text> CardDescriptions;

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
    [SerializeField] GameObject GameObject_Character_text;
    [SerializeField] TMP_Text Character_text;

    [SerializeField] GameObject GameObject_Dmg_text;
    // [SerializeField] TMP_Text Dmg_text;
    [SerializeField] public bool Debilitado;
    int debilidad = 0;
    
    [SerializeField] public bool Envenenado;
    int veneno = 0;

    [SerializeField] public int ContadorDeTurnos = 0;


    void Start()
    {
        Debilitado = false;
        Envenenado = false;

        VariablesGlobales = GameObject.Find("VariablesGlobales");
        //CardList.cards = new GameObject[5];
        //CardList.cont = 0; // Inicializa el contador de la lista

        if (VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista <= 0)
        {
            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista = VariablesGlobales.GetComponent<VariablesGlobales>().MaxHealthProtagonista;
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
        if (Debilitado)
        {
            debilidad = -3;
        }
        else
        {
            debilidad = 0;
        }
        if (Envenenado)
        {
            veneno = 3;
        }
        else
        {
            veneno = 0;
        }

        Mana.text = ManaProtagonista + " / " + VariablesGlobales.GetComponent<VariablesGlobales>().MaxManaProtagonista; // Actualiza el texto que indica el maná del jugador
        CardsPosition();

        victoriaDerrota();
        
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
                clonEnemy.GetComponent<EnemyController>().AuraEnemigo.transform.position = new Vector3(2.5f, 0.5f, 0);

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
                clonEnemy.GetComponent<EnemyController>().AuraEnemigo.transform.position = new Vector3(4.5f, -0.5f, 0);

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
                clonEnemy.GetComponent<EnemyController>().AuraEnemigo.transform.position = new Vector3(6.5f, -1.5f, 0);

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

            for (int i = 0; i < 5; i++)
            {

                clon = Instantiate(Card);                                          // Crea una carta
                clon.GetComponent<CardController>().CombatScene = gameObject; // Almacena el controlador del combate en cada carta para acceder a sus variables
                clon.GetComponent<CardController>().DragZone = DragZone;         // Almacena la DragZone en cada carta para poder eliminarla una vez se acerque a ella
                clon.GetComponent<CardController>().Id = i;                        // Almacena el ID de cada carta (para saber su posicion al eliminarla de la lista)
                clon.GetComponent<CardController>().Tipo = Random.Range(0, 13); //hace que la carta sea de alguna de las del tipo
                clon.GetComponent<CardController>().VariablesGlobales = VariablesGlobales; // Almacena las variables globales en la carta
                clon.GetComponent<CardController>().ArrowEmitter = ArrowEmitter;
                clon.GetComponent<CardController>().CosteMana = CardManaCost(clon.GetComponent<CardController>().Tipo);
                //clon.GetComponent<CardController>().TextTitle = CardTitles[i];
                //clon.GetComponent<CardController>().TextDescription = CardDescriptions[i];

                CardList.Add(clon);                                         // Almacena la carta en la lista
                yield return new WaitForSeconds(0.05f);
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

                    CardList[0].transform.position = new Vector3(0, -4, 0);
                    CardList[0].transform.eulerAngles = new Vector3(0, 0, 0);
                    CardList[0].GetComponent<CardController>().SetPosition();
                    CardList[0].transform.localScale = new Vector3(2, 3, 1);

                }

            }
            else if (CardList.Count == 2)
            {

                if (!CardList[0].GetComponent<CardController>().MouseDrag && !CardList[0].GetComponent<CardController>().MouseOver)
                {

                    CardList[0].transform.position = new Vector3(1, -4, 0);
                    CardList[0].transform.eulerAngles = new Vector3(0, 0, -5);
                    CardList[0].transform.localScale = new Vector3(2, 3, 1);
                    CardList[0].GetComponent<CardController>().SetPosition();

                }
                if (!CardList[1].GetComponent<CardController>().MouseDrag && !CardList[1].GetComponent<CardController>().MouseOver)
                {

                    CardList[1].transform.position = new Vector3(-1, -4, 1);
                    CardList[1].transform.eulerAngles = new Vector3(0, 0, 5);
                    CardList[1].transform.localScale = new Vector3(2, 3, 1);
                    CardList[1].GetComponent<CardController>().SetPosition();

                }

            }
            else if (CardList.Count == 3)
            {

                if (!CardList[0].GetComponent<CardController>().MouseDrag && !CardList[0].GetComponent<CardController>().MouseOver)
                {

                    CardList[0].transform.position = new Vector3(2, -4.15f, 0);
                    CardList[0].transform.eulerAngles = new Vector3(0, 0, -10);
                    CardList[0].transform.localScale = new Vector3(2, 3, 1);
                    CardList[0].GetComponent<CardController>().SetPosition();

                }
                if (!CardList[1].GetComponent<CardController>().MouseDrag && !CardList[1].GetComponent<CardController>().MouseOver)
                {

                    CardList[1].transform.position = new Vector3(0, -4, 1);
                    CardList[1].transform.eulerAngles = new Vector3(0, 0, 0);
                    CardList[1].transform.localScale = new Vector3(2, 3, 1);
                    CardList[1].GetComponent<CardController>().SetPosition();

                }
                if (!CardList[2].GetComponent<CardController>().MouseDrag && !CardList[2].GetComponent<CardController>().MouseOver)
                {

                    CardList[2].transform.position = new Vector3(-2, -4.15f, 2);
                    CardList[2].transform.eulerAngles = new Vector3(0, 0, 10);
                    CardList[2].transform.localScale = new Vector3(2, 3, 1);
                    CardList[2].GetComponent<CardController>().SetPosition();

                }

            }
            else if (CardList.Count == 4)
            {

                if (!CardList[0].GetComponent<CardController>().MouseDrag && !CardList[0].GetComponent<CardController>().MouseOver)
                {

                    CardList[0].transform.position = new Vector3(3, -4.35f, 0);
                    CardList[0].transform.eulerAngles = new Vector3(0, 0, -15);
                    CardList[0].transform.localScale = new Vector3(2, 3, 1);
                    CardList[0].GetComponent<CardController>().SetPosition();

                }
                if (!CardList[1].GetComponent<CardController>().MouseDrag && !CardList[1].GetComponent<CardController>().MouseOver)
                {

                    CardList[1].transform.position = new Vector3(1, -4, 1);
                    CardList[1].transform.eulerAngles = new Vector3(0, 0, -5);
                    CardList[1].transform.localScale = new Vector3(2, 3, 1);
                    CardList[1].GetComponent<CardController>().SetPosition();

                }
                if (!CardList[2].GetComponent<CardController>().MouseDrag && !CardList[2].GetComponent<CardController>().MouseOver)
                {

                    CardList[2].transform.position = new Vector3(-1, -4, 2);
                    CardList[2].transform.eulerAngles = new Vector3(0, 0, 5);
                    CardList[2].transform.localScale = new Vector3(2, 3, 1);
                    CardList[2].GetComponent<CardController>().SetPosition();

                }
                if (!CardList[3].GetComponent<CardController>().MouseDrag && !CardList[3].GetComponent<CardController>().MouseOver)
                {

                    CardList[3].transform.position = new Vector3(-3, -4.35f, 3);
                    CardList[3].transform.eulerAngles = new Vector3(0, 0, 15);
                    CardList[3].transform.localScale = new Vector3(2, 3, 1);
                    CardList[3].GetComponent<CardController>().SetPosition();

                }

            }
            else if (CardList.Count == 5)
            {

                if (!CardList[0].GetComponent<CardController>().MouseDrag && !CardList[0].GetComponent<CardController>().MouseOver)
                {

                    CardList[0].transform.position = new Vector3(3.95f, -4.7f, 0);
                    CardList[0].transform.eulerAngles = new Vector3(0, 0, -20);
                    CardList[0].transform.localScale = new Vector3(2, 3, 1);
                    CardList[0].GetComponent<CardController>().SetPosition();

                }
                if (!CardList[1].GetComponent<CardController>().MouseDrag && !CardList[1].GetComponent<CardController>().MouseOver)
                {

                    CardList[1].transform.position = new Vector3(2, -4.15f, 1);
                    CardList[1].transform.eulerAngles = new Vector3(0, 0, -10);
                    CardList[1].transform.localScale = new Vector3(2, 3, 1);
                    CardList[1].GetComponent<CardController>().SetPosition();

                }
                if (!CardList[2].GetComponent<CardController>().MouseDrag && !CardList[2].GetComponent<CardController>().MouseOver)
                {

                    CardList[2].transform.position = new Vector3(0, -4, 2);
                    CardList[2].transform.eulerAngles = new Vector3(0, 0, 0);
                    CardList[2].transform.localScale = new Vector3(2, 3, 1);
                    CardList[2].GetComponent<CardController>().SetPosition();

                }
                if (!CardList[3].GetComponent<CardController>().MouseDrag && !CardList[3].GetComponent<CardController>().MouseOver)
                {

                    CardList[3].transform.position = new Vector3(-2, -4.15f, 3);
                    CardList[3].transform.eulerAngles = new Vector3(0, 0, 10);
                    CardList[3].transform.localScale = new Vector3(2, 3, 1);
                    CardList[3].GetComponent<CardController>().SetPosition();

                }
                if (!CardList[4].GetComponent<CardController>().MouseDrag && !CardList[4].GetComponent<CardController>().MouseOver)
                {

                    CardList[4].transform.position = new Vector3(-3.95f, -4.7f, 4);
                    CardList[4].transform.eulerAngles = new Vector3(0, 0, 20);
                    CardList[4].transform.localScale = new Vector3(2, 3, 1);
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

            ManaProtagonista = VariablesGlobales.GetComponent<VariablesGlobales>().MaxManaProtagonista; // Se resetea el maná del jugador                                                                      // Cambia de turno
            TurnoJugador = true;
            StartCoroutine(CreateCards());

        }
        ContadorDeTurnos++;

    }

    //crea el popUp de texto con la cantidad de daño o de heal
    public void CreateDmgHealText(bool IsHeal, int Amount, GameObject Character)
    {
        GameObject DmgText = Instantiate(GameObject_Dmg_text);
        DmgText.transform.SetParent(canvas, false);
        DmgText.GetComponent<DmgText>().IsHeal = IsHeal;
        DmgText.GetComponent<DmgText>().Amount = Amount;
        DmgText.transform.position = new Vector2(Character.transform.position.x + 2, Character.transform.position.y + 1);
    }

    public void UsarCarta(int tipo, int enemigo) // enemigo -> 0: EnemyList[0] | 1: EnemyList[1] | 2: EnemyList[2] | 3: Jugador | 4: TODOS los enemigos (casi no se usa)
    {
        

        if (tipo == 0)
        {
            
            //Ataque 5 de daño
            EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo -= (5 + debilidad);

            // en esta fución como ultimo parametro debería pasarse el gameobject del enemigo al que ataca la flecha
            //le puse "EnemyList[0]" porque es el que marca arriba para restarle la vida
            CreateDmgHealText(false, (5 + debilidad), EnemyList[enemigo]);

            EnemyList[enemigo].GetComponent<EnemyController>().RecibirDanyo = true;
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);



        }
        else if (tipo == 1)
        {
            //Ataque 3 x 2 de daño después de 0,5 seg
            StartCoroutine(DoubleAttack(enemigo, 3, 0.5f));

            //for (int i = 0; i < EnemyList.Count; i++)
            //{

            //    EnemyList[i].GetComponent<EnemyController>().HealthEnemigo -= (10 + debilidad);
            //    EnemyList[i].GetComponent<EnemyController>().RecibirDanyo = true;
            //    CreateDmgHealText(false, (10 + debilidad), EnemyList[i]);

            //}

        }
        else if (tipo == 2)
        {
            //Ataque 5 de daño a todos
            for (int i = 0; i < EnemyList.Count; i++)
            {

                EnemyList[i].GetComponent<EnemyController>().HealthEnemigo -= (5 + debilidad);
                EnemyList[i].GetComponent<EnemyController>().RecibirDanyo = true;
                CreateDmgHealText(false, (5 + debilidad), EnemyList[i]);

            }

            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

            //VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += (10 + debilidad);
            //CreateDmgHealText(true, (10 + debilidad), Player);
            //Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);
        }
        else if (tipo == 3)
        {

            //Ataque 10 de daño
            EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo -= (10 + debilidad);

            CreateDmgHealText(false, (10 + debilidad), EnemyList[enemigo]);

            EnemyList[enemigo].GetComponent<EnemyController>().RecibirDanyo = true;
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

        }
        else if (tipo == 4)
        {

            //Ataque 20 de daño
            EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo -= (20 + debilidad);

            CreateDmgHealText(false, (20 + debilidad), EnemyList[enemigo]);

            EnemyList[enemigo].GetComponent<EnemyController>().RecibirDanyo = true;
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

        }
        else if (tipo == 5)
        {

            //Ataque 10 x 2 de daño después de 0,5 seg
            StartCoroutine(DoubleAttack(enemigo, 10, 0.5f));

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
            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += (5 + debilidad);
            CreateDmgHealText(true, (5 + debilidad), Player);
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

        }
        else if (tipo == 8)
        {

            // El Jugador se cura 10
            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += (10 + debilidad);
            CreateDmgHealText(true, (10 + debilidad), Player);
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

        }
        else if (tipo == 9)
        {

            // El roba 5 de vida a un enemigo
            EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo -= (5 + debilidad);
            CreateDmgHealText(false, (5 + debilidad), EnemyList[enemigo]);
            EnemyList[enemigo].GetComponent<EnemyController>().RecibirDanyo = true;

            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += (5 + debilidad);
            CreateDmgHealText(true, (5 + debilidad), Player);
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

        }

        else if (tipo == 10)
        {

            // El Jugador roba 10 de vida a un enemigo
            EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo -= (10 + debilidad);
            CreateDmgHealText(false, (10 + debilidad), EnemyList[enemigo]);
            EnemyList[enemigo].GetComponent<EnemyController>().RecibirDanyo = true;

            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += (10 + debilidad);
            CreateDmgHealText(true, (10 + debilidad), Player);
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

        }
        else if (tipo == 11)
        {

            // El Jugador roba 5 de vida a todos los enemigos
            int i;

            for ( i = 0; i < EnemyList.Count; i++)
            {

                EnemyList[i].GetComponent<EnemyController>().HealthEnemigo -= (5 + debilidad);
                EnemyList[i].GetComponent<EnemyController>().RecibirDanyo = true;
                CreateDmgHealText(false, (5 + debilidad), EnemyList[i]);

            }

            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += ((5 + debilidad) * i);
            CreateDmgHealText(true, ((5 + debilidad) * i), Player);
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

        }
        else if (tipo == 12)
        {

            // El Jugador roba 10 de vida a todos los enemigos
            int i;

            for (i = 0; i < EnemyList.Count; i++)
            {

                EnemyList[i].GetComponent<EnemyController>().HealthEnemigo -= (10 + debilidad);
                EnemyList[i].GetComponent<EnemyController>().RecibirDanyo = true;
                CreateDmgHealText(false, (10 + debilidad), EnemyList[i]);

            }

            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += ((10 + debilidad) * i);
            CreateDmgHealText(true, ((10 + debilidad) * i), Player);
            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

        }

        if (Envenenado)
        {
          //  yield return new WaitForSeconds(1);
            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista -= (veneno);
            CreateDmgHealText(false, veneno, Player);
        }
       
    }

    public void victoriaDerrota()
    {
        int enemigosVivos = EnemyList.Count;
        for (int i = 0; i < EnemyList.Count; i++)
        {
            if (EnemyList[i].GetComponent<EnemyController>().HealthEnemigo == 0)
            {
                enemigosVivos--;
            }

        }

        if (VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista <= 0)
        {
           
            VictoriaDerrotaPanel.SetActive(true);
            VictoriaDerrotaText.text = "DERROTA";
            Time.timeScale = 0f;
           // VariablesGlobales.GetComponent<VariablesGlobales>().EstaEnPausa = true;
            botonTurno.enabled = false;

        }
        else if (VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista > 0 && enemigosVivos == 0)
        {
           
            VictoriaDerrotaPanel.SetActive(true);
            VictoriaDerrotaText.text = "VICTORIA";
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

    public IEnumerator DoubleAttack(int enemigo, int danyo, float tiempo)
    {

        EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo -= (danyo + debilidad);
        CreateDmgHealText(false, (danyo + debilidad), EnemyList[enemigo]);
        EnemyList[enemigo].GetComponent<EnemyController>().RecibirDanyo = true;
        Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

        yield return new WaitForSeconds(tiempo);

        EnemyList[enemigo].GetComponent<EnemyController>().HealthEnemigo -= (danyo + debilidad);
        CreateDmgHealText(false, (danyo + debilidad), EnemyList[enemigo]);
        EnemyList[enemigo].GetComponent<EnemyController>().RecibirDanyo = true;
        Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

    }

}
