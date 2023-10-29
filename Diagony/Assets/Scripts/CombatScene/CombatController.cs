using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct ListCards{

    public GameObject[] cards; //Array de cartas
    public int cont;           //Contador de cartas en la lista

};

public class CombatController : MonoBehaviour
{
    [SerializeField] GameObject Player;   // Prefab del Player
    [SerializeField] GameObject Enemy1;   // Prefab del Enemigo 1
    [SerializeField] GameObject Enemy2;   // Prefab del Enemigo 2
    [SerializeField] GameObject Enemy3;   // Prefab del Enemigo 3
    [SerializeField] GameObject Card;     // Carta de combate
    [SerializeField] ListCards CardList;  // Lista de cartas en el combate
    [SerializeField] GameObject DragZone; // Zona en la que se eliminarán las cartas usadas

    //nos conviene hacer esto en un array con los enemigos guardados así el random solo elige un hueco

    // Start is called before the first frame update
    void Start()
    {
        CardList.cards = new GameObject[5];
        CardList.cont = 0; // Inicializa el contador de la lista

        CreatePlayer();  // Crea al jugador
        CreateEnemies(); // Crea los enemigos
        CreateCards();   // Crea las cartas

    }

    // Update is called once per frame
    void Update()
    {

        CardsPosition();

    }

    /*
     * Crea al jugador
     */
    public void CreatePlayer()
    {
        
        GameObject clonPlayer = Instantiate(Player); // Crea el clon del Player

    }

    /*
     * Crea de 1 a 3 enemigos de manera aleatoria
     */
    public void CreateEnemies()
    {
        
        int rand = Random.Range(1, 4);           // Random de 1 a 3
        GameObject clonEnemy;                    // Declara el clon del prefab

        for (int i = 0; i < rand; i++)           // Bucle dependiendo del número de enemigos que hay en la sala
        {

            if (i == 0)                          // Si es el primer enemigo
                clonEnemy = Instantiate(Enemy1); // Crea el clon del prefab Enemigo 1
            else if (i == 1)                     // Si es el segundo enemigo
                clonEnemy = Instantiate(Enemy2); // Crea el clon del prefab Enemigo 2
            else                                 // Si es el primer enemigo
                clonEnemy = Instantiate(Enemy3); // Crea el clon del prefab Enemigo 3

        }

    }

    /*
     * Coloca las cartas del combate
     */
    public void CreateCards()
    {

        GameObject clon;

        for(int i = 0; i < 5; i++)
        {

            clon = Instantiate(Card);                                          // Crea una carta
            clon.GetComponent<CardController>().CombatController = gameObject; // Almacena el controlador del combate en cada carta para acceder a sus variables
           // clon.GetComponent<CardController>().DragZone = DragZone;         // Almacena la DragZone en cada carta para poder eliminarla una vez se acerque a ella
            clon.GetComponent<CardController>().Id = i;                        // Almacena el ID de cada carta (para saber su posicion al eliminarla de la lista)
            CardList.cards[CardList.cont] = clon;                              // Almacena la carta en la lista
            CardList.cont++;                                                   // Aumenta el contador de la lista

        }

    }

    /*
     * Controla la posicion de las cartas en funcion del numero de ellas que haya en pantalla
     */
    public void CardsPosition()
    {

        if (CardList.cont == 1)
        {

            if(!CardList.cards[0].GetComponent<CardController>().MouseDrag && !CardList.cards[0].GetComponent<CardController>().MouseOver)
            {

                CardList.cards[0].transform.position = new Vector3(0, -4, 0);
                CardList.cards[0].transform.eulerAngles = new Vector3(0, 0, 0);
                CardList.cards[0].GetComponent<CardController>().SetPosition();
                CardList.cards[0].transform.localScale = new Vector3(2, 3, 1);

            }

        }
        else if (CardList.cont == 2)
        {

            if(!CardList.cards[0].GetComponent<CardController>().MouseDrag && !CardList.cards[0].GetComponent<CardController>().MouseOver)
            {

                CardList.cards[0].transform.position = new Vector3(1, -4, 0);
                CardList.cards[0].transform.eulerAngles = new Vector3(0, 0, -5);
                CardList.cards[0].transform.localScale = new Vector3(2, 3, 1);
                CardList.cards[0].GetComponent<CardController>().SetPosition();

            }
            if(!CardList.cards[1].GetComponent<CardController>().MouseDrag && !CardList.cards[1].GetComponent<CardController>().MouseOver)
            {

                CardList.cards[1].transform.position = new Vector3(-1, -4, 1);
                CardList.cards[1].transform.eulerAngles = new Vector3(0, 0, 5);
                CardList.cards[1].transform.localScale = new Vector3(2, 3, 1);
                CardList.cards[1].GetComponent<CardController>().SetPosition();

            }

        }
        else if (CardList.cont == 3)
        {

            if(!CardList.cards[0].GetComponent<CardController>().MouseDrag && !CardList.cards[0].GetComponent<CardController>().MouseOver)
            {

                CardList.cards[0].transform.position = new Vector3(2, -4.15f, 0);
                CardList.cards[0].transform.eulerAngles = new Vector3(0, 0, -10);
                CardList.cards[0].transform.localScale = new Vector3(2, 3, 1);
                CardList.cards[0].GetComponent<CardController>().SetPosition();

            }
            if(!CardList.cards[1].GetComponent<CardController>().MouseDrag && !CardList.cards[1].GetComponent<CardController>().MouseOver)
            {

                CardList.cards[1].transform.position = new Vector3(0, -4, 1);
                CardList.cards[1].transform.eulerAngles = new Vector3(0, 0, 0);
                CardList.cards[1].transform.localScale = new Vector3(2, 3, 1);
                CardList.cards[1].GetComponent<CardController>().SetPosition();

            }
            if(!CardList.cards[2].GetComponent<CardController>().MouseDrag && !CardList.cards[2].GetComponent<CardController>().MouseOver)
            {

                CardList.cards[2].transform.position = new Vector3(-2, -4.15f, 2);
                CardList.cards[2].transform.eulerAngles = new Vector3(0, 0, 10);
                CardList.cards[2].transform.localScale = new Vector3(2, 3, 1);
                CardList.cards[2].GetComponent<CardController>().SetPosition();

            }

        }
        else if (CardList.cont == 4)
        {

            if(!CardList.cards[0].GetComponent<CardController>().MouseDrag && !CardList.cards[0].GetComponent<CardController>().MouseOver)
            {

                CardList.cards[0].transform.position = new Vector3(3, -4.35f, 0);
                CardList.cards[0].transform.eulerAngles = new Vector3(0, 0, -15);
                CardList.cards[0].transform.localScale = new Vector3(2, 3, 1);
                CardList.cards[0].GetComponent<CardController>().SetPosition();

            }
            if(!CardList.cards[1].GetComponent<CardController>().MouseDrag && !CardList.cards[1].GetComponent<CardController>().MouseOver)
            {

                CardList.cards[1].transform.position = new Vector3(1, -4, 1);
                CardList.cards[1].transform.eulerAngles = new Vector3(0, 0, -5);
                CardList.cards[1].transform.localScale = new Vector3(2, 3, 1);
                CardList.cards[1].GetComponent<CardController>().SetPosition();

            }
            if(!CardList.cards[2].GetComponent<CardController>().MouseDrag && !CardList.cards[2].GetComponent<CardController>().MouseOver)
            {

                CardList.cards[2].transform.position = new Vector3(-1, -4, 2);
                CardList.cards[2].transform.eulerAngles = new Vector3(0, 0, 5);
                CardList.cards[2].transform.localScale = new Vector3(2, 3, 1);
                CardList.cards[2].GetComponent<CardController>().SetPosition();

            }
            if(!CardList.cards[3].GetComponent<CardController>().MouseDrag && !CardList.cards[3].GetComponent<CardController>().MouseOver)
            {

                CardList.cards[3].transform.position = new Vector3(-3, -4.35f, 3);
                CardList.cards[3].transform.eulerAngles = new Vector3(0, 0, 15);
                CardList.cards[3].transform.localScale = new Vector3(2, 3, 1);
                CardList.cards[3].GetComponent<CardController>().SetPosition();

            }

        }
        else
        {

            if(!CardList.cards[0].GetComponent<CardController>().MouseDrag && !CardList.cards[0].GetComponent<CardController>().MouseOver)
            {

                CardList.cards[0].transform.position = new Vector3(3.95f, -4.7f, 0);
                CardList.cards[0].transform.eulerAngles = new Vector3(0, 0, -20);
                CardList.cards[0].transform.localScale = new Vector3(2, 3, 1);
                CardList.cards[0].GetComponent<CardController>().SetPosition();

            }
            if(!CardList.cards[1].GetComponent<CardController>().MouseDrag && !CardList.cards[1].GetComponent<CardController>().MouseOver)
            {

                CardList.cards[1].transform.position = new Vector3(2, -4.15f, 1);
                CardList.cards[1].transform.eulerAngles = new Vector3(0, 0, -10);
                CardList.cards[1].transform.localScale = new Vector3(2, 3, 1);
                CardList.cards[1].GetComponent<CardController>().SetPosition();

            }
            if(!CardList.cards[2].GetComponent<CardController>().MouseDrag && !CardList.cards[2].GetComponent<CardController>().MouseOver)
            {

                CardList.cards[2].transform.position = new Vector3(0, -4, 2);
                CardList.cards[2].transform.eulerAngles = new Vector3(0, 0, 0);
                CardList.cards[2].transform.localScale = new Vector3(2, 3, 1);
                CardList.cards[2].GetComponent<CardController>().SetPosition();

            }
            if(!CardList.cards[3].GetComponent<CardController>().MouseDrag && !CardList.cards[3].GetComponent<CardController>().MouseOver)
            {

                CardList.cards[3].transform.position = new Vector3(-2, -4.15f, 3);
                CardList.cards[3].transform.eulerAngles = new Vector3(0, 0, 10);
                CardList.cards[3].transform.localScale = new Vector3(2, 3, 1);
                CardList.cards[3].GetComponent<CardController>().SetPosition();

            }
            if(!CardList.cards[4].GetComponent<CardController>().MouseDrag && !CardList.cards[4].GetComponent<CardController>().MouseOver)
            {

                CardList.cards[4].transform.position = new Vector3(-3.95f, -4.7f, 4);
                CardList.cards[4].transform.eulerAngles = new Vector3(0, 0, 20);
                CardList.cards[4].transform.localScale = new Vector3(2, 3, 1);
                CardList.cards[4].GetComponent<CardController>().SetPosition();

            }

        }

    }

    /*
     * Elimina una carta de la lista de cartas (La elimina de la lista, recoloca la lista y reduce el contador de la lista)
     */
    public void EliminarCarta(int id)
    {

        // Elimina la carta
        CardList.cards[id] = CardList.cards[id + 1];

        // Recoloca los siguientes elementos rellenando el hueco dejado
        for (int i = id + 1; i < CardList.cont - 1; i++ )
        {

            CardList.cards[i] = CardList.cards[i + 1];

        }

        CardList.cards[CardList.cont - 1] = null; // Elimina el último elemento ya que este ha pasado a la penúltima posición
        CardList.cont--;                          // Reduce el contado de la lista de cartas

        // Recoloca el ID de cada carta de la lista
        for (int i = 0; i < CardList.cont; i++)
            CardList.cards[i].GetComponent<CardController>().Id = i;

    }

}
