using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChestController : MonoBehaviour
{

    public GameObject VariablesGlobales;
    [SerializeField] RectTransform CanvasCards;

    // Cartas
    [SerializeField] GameObject prefabCarta;
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

    public List<GameObject> ListCards = new List<GameObject>(); // Lista de los IDs de las cartas creadas

    int numCards; // Número de cartas implementadas
    bool CartasCreadas;
    public bool CardSelected;
    public bool Exit;
    
    // Start is called before the first frame update
    void Start()
    {

        VariablesGlobales = GameObject.Find("VariablesGlobales");
        numCards = 24;
        CartasCreadas = false;
        CardSelected = false;
        Exit = false;

    }

    // Update is called once per frame
    void Update()
    {
        
        if(Exit)
        {

            if(ListCards.Count == 0)
                SceneManager.LoadScene("MapScene");

        }

    }

    public void OnMouseDown()
    {

        if(!CartasCreadas)
            StartCoroutine(CreateCards());

    }

    public IEnumerator CreateCards()
    {

        TMP_Text[] newText;
        int cardType;
        //int posX = -500, posY = 100;
        List<int> CardsCreated = new List<int>(); // Lista con las cartas que se van creando para que no se repitan

        for (int i = 0; i < 3; i++)
        {

            GameObject clon = Instantiate(prefabCarta);

            // Repite el aleatorio hasta que encuentre uno que no haya salido para que no se repitan
            do
            {

                cardType = Random.Range(0, numCards);

            } while (CardsCreated.Contains(cardType));
            CardsCreated.Add(cardType);

            clon.GetComponent<ChestCard>().Tipo = cardType;
            clon.GetComponent<ChestCard>().VariablesGlobales = VariablesGlobales; // Almacena las variables globales en la carta
            clon.GetComponent<ChestCard>().Id = i;
            clon.GetComponent<ChestCard>().ChestScene = gameObject;
            clon.transform.SetParent(CanvasCards, false);

            // Implementa los sprites
            if (cardType <= 5)
                clon.GetComponent<Image>().sprite = CardSprites[0];
            else if (cardType <= 12)
                clon.GetComponent<Image>().sprite = CardSprites[1];
            else
                clon.GetComponent<Image>().sprite = CardSprites[2];

            // Actualiza los textos
            newText = clon.GetComponentsInChildren<TMP_Text>();
            newText[0].text = CardTitles[cardType];
            newText[1].text = CardDescriptions[cardType];
            newText[2].text = CardCost[cardType];
            newText[3].text = CardDuration[cardType];

            ListCards.Add(clon);

            yield return new WaitForSeconds(0.25f);

        }

        CartasCreadas = true;

    }

}
