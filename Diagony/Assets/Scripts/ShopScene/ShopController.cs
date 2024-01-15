using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
using System.Linq;

public class ShopController : MonoBehaviour
{
    public GameObject VariablesGlobales;
    [SerializeField] RectTransform CanvasCards;

    // Cartas
    // [SerializeField] GameObject prefabCarta;
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

    List<string> CardDuration = new List<string>() { "", "", "", "", "", "", "", "", "", "", "", "", "", "1", "1", "3", "2", "4", "4", "4", "3", "3", "1", "0" };
   
    public List<int> CardPrecio = new List<int>() { 10, 10, 15, 15, 20, 20, 5, 10, 15, 15, 20, 20, 30, 10, 5, 10, 15, 10, 5, 15, 10, 5, 25, 20 };

    List<int> maxCardTypes = new List<int>(); // para guardar las cartas más usadas

    public List<GameObject> ListCards = new List<GameObject>(); // Lista de los IDs de las cartas creadas

    int numCards; // Número de cartas implementadas



    void Start()
    {
        VariablesGlobales = GameObject.Find("VariablesGlobales");
        numCards = 24;
        BuscarCartasMasRepetidas();
        CrearCartas();
    }

    void Update()
    {

    }

    public void CrearCartas()
    {
        TMP_Text[] newText;
        int cardType;
        List<int> CardsCreated = new List<int>(); // Lista con las cartas que se van creando para que no se repitan

        for (int i = 0; i < 6; i++)
        {
                do
                {
                    if (i < 3)
                    {
                        cardType = CartaMasUsada();
                    }
                    else
                    {
                        cardType = Random.Range(0, numCards);
                    }
                } while (CardsCreated.Contains(cardType));
                // Repite el aleatorio hasta que encuentre uno que no haya salido para que no se repitan
            


            CardsCreated.Add(cardType);
   
             //Implementa los sprites
            if (cardType <= 5)
                ListCards[i].GetComponent<Image>().sprite = CardSprites[0];
            else if (cardType <= 12)
                ListCards[i].GetComponent<Image>().sprite = CardSprites[1];
            else
                ListCards[i].GetComponent<Image>().sprite = CardSprites[2];

            // Actualiza los textos
            newText = ListCards[i].GetComponentsInChildren<TMP_Text>();
            newText[0].text = CardTitles[cardType];
            newText[1].text = CardDescriptions[cardType];
            newText[2].text = CardCost[cardType];
            newText[3].text = CardDuration[cardType];
            newText[4].text = "Tienes: " + VariablesGlobales.GetComponent<VariablesGlobales>().AmountCards[cardType].ToString();
            newText[5].text = "$" + CardPrecio[cardType].ToString();

        }
    }

    public void BuscarCartasMasRepetidas()
    {
        List<int> cardUses = VariablesGlobales.GetComponent<VariablesGlobales>().CardUses;
      
        //obtiene el valor máximo de usos
        int maxCount = cardUses.Max();

       //obtiene los id de cartas que tienen el máximo número de usos
        for (int cardType = 0; cardType < cardUses.Count; cardType++)
        {
            if (cardUses[cardType] == maxCount)
            {
                maxCardTypes.Add(cardType); //agrega las cartas con mayor numero de usos
            }

        }

        int buscarMax = 0, max2 = -1, max3 = -1;
        if (maxCardTypes.Count < 3) //en caso de que el numero de cartas con mayor uso sea menor a 3 
        {
            //pasa por un bucle para agregar más cartas a la lista de cartas con mayores usos
            do
            {
                for (int cardType = 0; cardType < cardUses.Count; cardType++)
                {
                    if (buscarMax == 0 && cardUses[cardType] > max2 && cardUses[cardType] != maxCount)
                    {
                       max2 = cardUses[cardType]; //busca el segundo número de usos más alto
                    }
                    else if (buscarMax == 1 && cardUses[cardType] == max2)
                    {
                        maxCardTypes.Add(cardType);  //agrega las cartas con segundo mayor numero de usos
                    }
                    else if (buscarMax == 2 && cardUses[cardType] > max3 && cardUses[cardType] != maxCount && cardUses[cardType] != max2)
                    {
                        max3 = cardUses[cardType]; // en caso de ser necesario porque la cantidad de cartas con mayor uso
                                                   // es menor a 3 busca el segundo número de usos más alto
                    }
                    else if (buscarMax == 3 && cardUses[cardType] == max3)
                    {
                        maxCardTypes.Add(cardType); //agrega las cartas con tercer mayor numero de usos
                    }

                }
                if (maxCardTypes.Count >= 3) //en caso que con las cartas del segundo mayor número de usos ya sean 3 o más, entonces sale del bucle
                {
                    buscarMax = 4;
                    break;
                }
                else //caso contrario suma +1 en "buscarMax" para así entrar en los "else if" cuando vale 2 y cuando vale 3
                                                                         //(es decir, busca el tercer mayor número de usos)
                {
                    buscarMax++;
                }

                Debug.Log("pasó con " + buscarMax);
            } while (buscarMax == 1 || buscarMax == 2 || buscarMax == 3);
        }

    }

    public int CartaMasUsada()
    {
        int chosenCardType;

        //seleccionar de forma aleatoria cual carta pasar de las de mayor uso
        chosenCardType = maxCardTypes[Random.Range(0, maxCardTypes.Count)];

        Debug.Log(" id:" + chosenCardType);
        return chosenCardType;
    }

}
