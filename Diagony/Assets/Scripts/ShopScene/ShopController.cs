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
    List<string> CardTitlesES = new List<string>() { "Respiro hondo", "Escribo lo que me pasa", "Acepto que me afecta", "Puedo decir que no", "Reconozco lo que siento",
                                                   "Aprendo de lo que siento", "Me divierto con amigos", "Salgo a tomar el sol", "Un paseo por la naturaleza", "Cantar",
                                                   "Cuento hasta diez", "Me ordeno por dentro y por fuera", "Me cuido", "Ahora no, luego sí", "No me pasa nada", "Estoy así ahora",
                                                   "No pasa nada si sale mal", "Hablo de lo que me pasa", "Hablo todo el tiempo de lo que me pasa", "Estoy en ello",
                                                   "Nada es para siempre", "No sé que hacer", "Todo se transforma", "Soy consciente de como me afecta lo que hago"};
    List<string> CardTitlesEN = new List<string>() { "Deep breath", "I write what happens to me", "I accept that it affects me", "I can say no", "I recognize what I feel",
                                                   "I learn from what I feel", "I have fun with my friends", "I go out to sunbathe", "A walk through nature", "Sing",
                                                   "I count to ten", "I put myself in order inside and outside", "I take care of myself", "Not now, then yes", "Nothing happens to me", "I am like this now",
                                                   "Nothing happens if it goes wrong", "I talk about what happens to me", "I talk all the time about what happens to me", "I am on it",
                                                   "Nothing is forever", "I don't know what to do", "Everything transforms", "I am aware of how what I do affects me"};

    List<string> CardDescriptionPlayerES = new List<string>() { "", "", "", "", "", "", "+2 Maná", "+5", "+10", "+5", "+10",
                                                              "xN +5", "xN +10", "", "-8", "", "", "Fuerte", "Fuerte y -8",
                                                              "Esperanza", "", "-8", "Transformado", "-Efectos" };
    List<string> CardDescriptionPlayerEN = new List<string>() { "", "", "", "", "", "", "+2 Mana", "+5", "+10", "+5", "+10",
                                                              "xN +5", "xN +10", "", "-8", "", "", "Strong", "Strong and -8",
                                                              "Hope", "", "-8", "Transformed", "-Effects" };

    List<string> CardDescriptionEnemiesES = new List<string>() { "-5", "x2 -3", "-5 a todos", "-10", "-20", "x2 -10", "", "",
                                                               "", "-5", "-10", "-5 a todos", "-10 a todos", "Aturdido",
                                                               "Aturdido", "Débil", "Débil a todos", "", "", "",
                                                               "Envenenado", "Débil", "", "" };
    List<string> CardDescriptionEnemiesEN = new List<string>() { "-5", "x2 -3", "-5 to everyone", "-10", "-20", "x2 -10", "", "",
                                                               "", "-5", "-10", "-5 to everyone", "-10 to everyone", "Stunned",
                                                               "Stunned", "Weak", "Weak to everyone", "", "", "",
                                                               "Poisoned", "Weak", "", "" };

    List<string> CardCost = new List<string>() { "1", "1", "2", "2", "3", "3", "0", "1", "2", "2", "3", "3", "5", "1", "0", "1", "2", "1", "0", "2", "1", "0", "4", "1" };

    List<string> CardDuration = new List<string>() { "", "", "", "", "", "", "", "", "", "", "", "", "", "1", "1", "3", "2", "4", "4", "4", "3", "3", "1", "0" };
   
    public List<int> CardPrecio = new List<int>() { 10, 10, 15, 15, 20, 20, 20, 10, 15, 15, 20, 20, 30, 10, 5, 10, 15, 10, 5, 15, 10, 5, 25, 10 };

    [SerializeField] List<int> maxCardTypes = new List<int>(); // para guardar las cartas más usadas

    public List<GameObject> ListCards = new List<GameObject>(); // Lista de los IDs de las cartas creadas
    public List<GameObject> ListButton = new List<GameObject>(); //lista con los botones para poder asignarles sus id con los respectivos precios.

    int numCards; // Número de cartas implementadas

    List<int> CardsCreated = new List<int>(); // Lista con las cartas que se van creando para que no se repitan
    TMP_Text[] newText;



    void Start()
    {
        VariablesGlobales = GameObject.Find("VariablesGlobales");
        numCards = 24;
       
        CrearCartas();

    }

    void Update()
    {

        for (int i = 0; i < CardsCreated.Count; i++)
        {
            newText = ListCards[i].GetComponentsInChildren<TMP_Text>();
            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                newText[5].text = "Have: " + VariablesGlobales.GetComponent<VariablesGlobales>().AmountCards[CardsCreated[i]].ToString();
            else                                                                   // Spanish
                newText[5].text = "Tienes: " + VariablesGlobales.GetComponent<VariablesGlobales>().AmountCards[CardsCreated[i]].ToString();
        }
      
    }

    public void CrearCartas()
    {
        BuscarCartasMasRepetidas();
        int cardType;
        


        for (int i = 0; i < 8; i++)
        {
                do
                {
                    if (i < 2)
                    {
                        cardType = CartaMasUsada();
                    }
                    else
                    {
                        cardType = CartaNoObtenida();
                    }
                   
                    
                } while (CardsCreated.Contains(cardType));
                // Repite el aleatorio hasta que encuentre uno que no haya salido para que no se repitan
 
                 CardsCreated.Add(cardType);

            // Implementa los sprites
            ListCards[i].GetComponent<Image>().sprite = CardSprites[cardType];

            // Actualiza los textos
            newText = ListCards[i].GetComponentsInChildren<TMP_Text>();
            if (!VariablesGlobales.GetComponent<VariablesGlobales>().PasivaGanarDinero)
            {

                newText[0].text = CardTitlesEN[cardType];
                newText[1].text = CardDescriptionPlayerEN[cardType];
                newText[2].text = CardDescriptionEnemiesEN[cardType];
            }
            else
            {

                newText[0].text = CardTitlesES[cardType];
                newText[1].text = CardDescriptionPlayerES[cardType];
                newText[2].text = CardDescriptionEnemiesES[cardType];
            }
            newText[3].text = CardCost[cardType];
            newText[4].text = CardDuration[cardType];
            newText[6].text = "$" + CardPrecio[cardType].ToString();

            ListCards[i].GetComponent<ShopCard>().Tipo = cardType;
            ListButton[i].GetComponent<BuyButton>().ID = cardType;
            ListButton[i].GetComponent<BuyButton>().Precio = CardPrecio[cardType];
        }
    }

    public void BuscarCartasMasRepetidas()
    {
        List<int> cardUses = VariablesGlobales.GetComponent<VariablesGlobales>().CardUses;

        //obtiene el valor máximo de usos
        int maxCount = cardUses.Max();

        bool hayOtra = false;
        if (cardUses[6] == maxCount)
        {
            for (int cardType = 0; cardType < cardUses.Count; cardType++)
            {
                if (cardUses[cardType] == maxCount && cardType != 6)
                {
                    Debug.Log("hay otra");
                    hayOtra = true;
                    break;
                }
            }

            if (!hayOtra)
            {
                Debug.Log("no hay otra");
                int max_no6 = -1;
                for (int cardType = 0; cardType < cardUses.Count; cardType++)
                {
                    if (cardUses[cardType] > max_no6 && cardType != 6)
                    {
                        max_no6 = cardUses[cardType]; //busca cartas con mayor numero de usos que no sea carta id 6
                    }
                }
                maxCount = max_no6;
                Debug.Log("encontró el " + maxCount + " como num más usado");
            }
        }
            
       

       //obtiene los id de cartas que tienen el máximo número de usos
        for (int cardType = 0; cardType < cardUses.Count; cardType++)
        {
            if (cardUses[cardType] == maxCount && cardType!=6)
            {
                maxCardTypes.Add(cardType); //agrega las cartas con mayor numero de usos
            }

        }

        int buscarMax = 0, max2 = -1;
        if (maxCardTypes.Count < 3) //en caso de que el numero de cartas con mayor uso sea menor a 3 
        {
            //pasa por un bucle para agregar más cartas a la lista de cartas con mayores usos
            do
            {
                for (int cardType = 0; cardType < cardUses.Count; cardType++)
                {
                    if (buscarMax == 0 && cardUses[cardType] > max2 && cardUses[cardType] != maxCount && cardType!=6)
                    {
                       max2 = cardUses[cardType]; //busca el segundo número de usos más alto
                    }
                    else if (buscarMax == 1 && cardUses[cardType] == max2 && cardType!=6)
                    {
                        maxCardTypes.Add(cardType);  //agrega las cartas con segundo mayor numero de usos
                    }

                    //else if (buscarMax == 2 && cardUses[cardType] > max3 && cardUses[cardType] != maxCount && cardUses[cardType] != max2)
                    //{
                    //    max3 = cardUses[cardType]; // en caso de ser necesario porque la cantidad de cartas con mayor uso
                    //                               // es menor a 3 busca el segundo número de usos más alto
                    //}
                    //else if (buscarMax == 3 && cardUses[cardType] == max3)
                    //{
                    //    maxCardTypes.Add(cardType); //agrega las cartas con tercer mayor numero de usos
                    //}

                }
                buscarMax++;
                //if (maxCardTypes.Count >= 3) //en caso que con las cartas del segundo mayor número de usos ya sean 3 o más, entonces sale del bucle
                //{
                //    buscarMax = 4;
                //    break;
                //}
                //else //caso contrario suma +1 en "buscarMax" para así entrar en los "else if" cuando vale 2 y cuando vale 3
                //                                                         //(es decir, busca el tercer mayor número de usos)
                //{

                //}

                Debug.Log("pasó con " + buscarMax);
            } while (buscarMax == 1 /*|| buscarMax == 2 || buscarMax == 3*/);
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

    public int CartaNoObtenida()
    {
        List<int> NoObtenidas = new List<int>();

        for (int i = 0; i < VariablesGlobales.GetComponent<VariablesGlobales>().AmountCards.Count; i++)
        {
            if (VariablesGlobales.GetComponent<VariablesGlobales>().AmountCards[i] == 0)
            {
                NoObtenidas.Add(i);
            }
        }

        if (NoObtenidas.Count > 0)
        {
            return NoObtenidas[Random.Range(0, NoObtenidas.Count)];
        }
        else
        {
            int random = -1;
            do
            {
               random = Random.Range(0, VariablesGlobales.GetComponent<VariablesGlobales>().AmountCards.Count);
            } while (random == 6);
            Debug.Log("no hay cartas no obtenidas así que devuelve: " + random);
            return random;
        }
           
    }
}
