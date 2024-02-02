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
    public GameObject CanvasCardsObject;
    public GameObject CanvasMana;
    public List<GameObject> CardsMana;

    [SerializeField] Sprite[] Sprites;

    // Cartas
    [SerializeField] GameObject prefabCarta;
    public List<Sprite> CardSprites;

    List<string> CardDescriptionsES = new List<string>() { "Te curas por completo", "+10 a la vida máxima del personaje", "Ganas 10 de oro en cada combate",
                                                         "Una carta de tu elección cuesta 1 menos de maná", "+5 de vida después de cada combate" };
    List<string> CardDescriptionsEN = new List<string>() { "You heal completely", "+10 to player's maximum health", "You earn +10 gold in every combat",
                                                         "A card of your choice costs 1 less mana", "+5 health after every combat" };

    public List<GameObject> ListCards = new List<GameObject>(); // Lista de los IDs de las cartas creadas

    int numCards; // Número de cartas implementadas
    public bool CartasCreadas;
    public bool CardSelected;
    public bool Exit;

    [SerializeField] TMP_Text SelectCardManaText;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        VariablesGlobales = GameObject.Find("VariablesGlobales");
        numCards = 5;
        CartasCreadas = false;
        CardSelected = false;
        Exit = false;

    }

    // Update is called once per frame
    void Update()
    {
        
        UpdateTexts();

        if(Exit)
        {

            if(ListCards.Count == 0)
                SceneManager.LoadScene("MapScene");

        }

    }

    public void OnMouseDown()
    {

        if(!CartasCreadas)
        {

            CartasCreadas = true;
            GetComponent<SpriteRenderer>().sprite = Sprites[1];
            StartCoroutine(CreateCards());

        }

    }

    public IEnumerator CreateCards()
    {

        TMP_Text[] newText;
        int cardType;
        //int posX = -500, posY = 100;
        List<int> CardsCreated = new List<int>(); // Lista con las cartas que se van creando para que no se repitan
        int idCartaCurarse = -1; // Cuando tenga menos de 30 de vida, aparecerá siempre, pues para saber la posición que tiene y que no sea siempre la misma

        if(VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista <= 30)
            idCartaCurarse = Random.Range(0, 3);

        for (int i = 0; i < 3; i++)
        {

            GameObject clon = Instantiate(prefabCarta);

            // Repite el aleatorio hasta que encuentre uno que no haya salido para que no se repitan
            do
            {

                if (i == idCartaCurarse)
                    cardType = 0;
                else
                {

                    int rand = Random.Range(0, 101);

                    if (rand <= 35)
                        cardType = 1;
                    else if (rand <= 55)
                        cardType = 2;
                    else if (rand <= 80)
                        cardType = 3;
                    else
                        cardType = 4;

                }

            } while (CardsCreated.Contains(cardType));

            CardsCreated.Add(cardType);

            clon.GetComponent<ChestCard>().Tipo = cardType;
            clon.GetComponent<ChestCard>().VariablesGlobales = VariablesGlobales; // Almacena las variables globales en la carta
            clon.GetComponent<ChestCard>().Id = i;
            clon.GetComponent<ChestCard>().ChestScene = gameObject;
            clon.GetComponent<ChestCard>().CanvasCards = CanvasCardsObject;
            clon.GetComponent<ChestCard>().CanvasMana = CanvasMana;
            clon.GetComponent<ChestCard>().CardsMana = CardsMana;
            clon.GetComponent<ChestCard>().CardSprites = CardSprites;
            clon.transform.SetParent(CanvasCards, false);

            // Implementa los sprites
            //if (cardType <= 5)
            //    clon.GetComponent<Image>().sprite = CardSprites[0];
            //else if (cardType <= 12)
            //    clon.GetComponent<Image>().sprite = CardSprites[1];
            //else
            //    clon.GetComponent<Image>().sprite = CardSprites[2];

            // Actualiza los textos
            newText = clon.GetComponentsInChildren<TMP_Text>();
            //newText[0].text = CardTitles[cardType];
            if (!VariablesGlobales.GetComponent<VariablesGlobales>().PasivaGanarDinero)
                newText[0].text = CardDescriptionsEN[cardType];
            else
                newText[0].text = CardDescriptionsES[cardType];
            //newText[2].text = CardCost[cardType];
            //newText[3].text = CardDuration[cardType];

            ListCards.Add(clon);

            yield return new WaitForSeconds(0.25f);

        }

    }

    public void UpdateTexts()
    {

        for (int i = 0; i < ListCards.Count; i++)
        {

            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                ListCards[i].GetComponentsInChildren<TMP_Text>()[0].text = CardDescriptionsEN[ListCards[i].GetComponent<ChestCard>().Tipo];
            else                                                                   // Spanish
                ListCards[i].GetComponentsInChildren<TMP_Text>()[0].text = CardDescriptionsES[ListCards[i].GetComponent<ChestCard>().Tipo];

        }

        if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
            SelectCardManaText.text = "Choose a card to reduce its mana cost by 1";
        else                                                                   // Spanish
            SelectCardManaText.text = "Elige una carta a la que reducir su coste en 1";

    }

}
