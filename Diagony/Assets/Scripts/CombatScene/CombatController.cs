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

    public bool EnemigosRecibirDanyo; // Para controlar que al usar la carta de curarse los enemigos no hagan la animacion de recibir daño

    void Start()
    {
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

        EnemigosRecibirDanyo = false;

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

        victoriaDerrota();
        
    }

    /*
    * Crea las barras de vida
    */
    public void CreateHealthBar(float x, float y, bool EsPlayer, GameObject personaje)
    {
        GameObject clonHealthBar = Instantiate(HealthBar);          //crea el prefab de la barra de vida
        clonHealthBar.transform.SetParent(canvas, false);            //declara el canvas como padre para que sea visible
        clonHealthBar.transform.position = new Vector2(x, y);    //lo coloca arriba del personaje
        clonHealthBar.GetComponent<HealthBar>().EsPlayer = EsPlayer;
       

        

      
             if (EsPlayer)
             {
                clonHealthBar.GetComponent<HealthBar>()._player = personaje;
             }
             else
             {
                clonHealthBar.GetComponent<HealthBar>()._enemy = personaje;
             }


    }

    public void CreateCharacterText(float x, float y, string nombre)
    {
        GameObject clonCharacterText = Instantiate(GameObject_Character_text);
        clonCharacterText.transform.SetParent(canvas, false);
        clonCharacterText.transform.position = new Vector2(x, y - 1.5f);
        TextMeshProUGUI textCharacter = clonCharacterText.GetComponent<TextMeshProUGUI>();
        textCharacter.text = nombre;
        
    }

    /*
     * Crea al jugador
     */
    public void CreatePlayer()
    {
        
        GameObject clonPlayer = Instantiate(PrefabPlayer); // Crea el clon del Player
        clonPlayer.transform.position = new Vector2(-4, -0.8f);
        CreateHealthBar(clonPlayer.transform.position.x, clonPlayer.transform.position.y + 1.5f, true, clonPlayer); //tipo 3 = player
        clonPlayer.GetComponent<PlayerController>().VariablesGlobales = VariablesGlobales;
        clonPlayer.GetComponent<PlayerController>().CombatScene = gameObject;
        CreateCharacterText(clonPlayer.transform.position.x, clonPlayer.transform.position.y, "PLAYER");
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
                clonEnemy.transform.position = new Vector3(2.5f, 0.5f, 1);
                clonEnemy.GetComponent<EnemyController>().Tipo = tipo;
                clonEnemy.GetComponent<EnemyController>().VariablesGlobales = VariablesGlobales;
                clonEnemy.GetComponent<EnemyController>().CombatScene = gameObject;
                clonEnemy.GetComponent<EnemyController>().Id = i;
                EnemyList.Add(clonEnemy);
                CreateHealthBar(clonEnemy.transform.position.x, clonEnemy.transform.position.y + 1.5f, false, clonEnemy);
                CreateCharacterText(clonEnemy.transform.position.x, clonEnemy.transform.position.y, EnemyName);
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
                clonEnemy.transform.position = new Vector3(4.5f, -0.5f, 1);
                clonEnemy.GetComponent<EnemyController>().Tipo = tipo;
                clonEnemy.GetComponent<EnemyController>().VariablesGlobales = VariablesGlobales;
                clonEnemy.GetComponent<EnemyController>().CombatScene = gameObject;
                clonEnemy.GetComponent<EnemyController>().Id = i;
                EnemyList.Add(clonEnemy);
                CreateHealthBar(clonEnemy.transform.position.x, clonEnemy.transform.position.y + 1.5f, false, clonEnemy);
                CreateCharacterText(clonEnemy.transform.position.x, clonEnemy.transform.position.y, EnemyName);
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
                clonEnemy.transform.position = new Vector3(6.5f, -1.5f, 1);
                clonEnemy.GetComponent<EnemyController>().Tipo = tipo;
                clonEnemy.GetComponent<EnemyController>().VariablesGlobales = VariablesGlobales;
                clonEnemy.GetComponent<EnemyController>().CombatScene = gameObject;
                clonEnemy.GetComponent<EnemyController>().Id = i;
                EnemyList.Add(clonEnemy);
                CreateHealthBar(clonEnemy.transform.position.x, clonEnemy.transform.position.y + 1.5f, false, clonEnemy);
                CreateCharacterText(clonEnemy.transform.position.x, clonEnemy.transform.position.y, EnemyName);
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
                clon.GetComponent<CardController>().Tipo = Random.Range(0, 3); //hace que la carta sea de alguna de las del tipo
                clon.GetComponent<CardController>().VariablesGlobales = VariablesGlobales; // Almacena las variables globales en la carta
                clon.GetComponent<CardController>().ArrowEmitter = ArrowEmitter;

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
            else
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

    }

    public void UsarCarta(int tipo)
    {
        
        if (tipo == 0)
        {
            //hace 5 de daño
            for (int i = 0; i < EnemyList.Count; i++)
            {

                EnemyList[i].GetComponent<EnemyController>().HealthEnemigo-=5;
                EnemigosRecibirDanyo = true;
                Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

            }

        }
        else if (tipo == 1)
        {
            //hace 10 de daño
            for (int i = 0; i < EnemyList.Count; i++)
            {

                EnemyList[i].GetComponent<EnemyController>().HealthEnemigo -= 10;
                EnemigosRecibirDanyo = true;
                Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);

            }
        }
        else
        {
            //cura 10 al personaje
            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += 10;

            Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("atacar", true);


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

}
