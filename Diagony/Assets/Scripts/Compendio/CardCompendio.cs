using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardCompendio : MonoBehaviour
{

    public GameObject VariablesGlobales;
    [SerializeField] GameObject ScrollAreaCompendio;
    [SerializeField] GameObject Compendio;

    [SerializeField] GameObject Parent;
    [SerializeField] int Tipo;
    private GameObject Copy;
    private bool CopyCreated;
    private RectTransform Position;
    private Image Image;
    [SerializeField] TMP_Text CantidadText;

    private Color FullTransparency;
    private Color MidTransparency;

    // Card Interface
    [SerializeField] TMP_Text CardTitle;
    List<string> CardTitlesES = new List<string>() { "Respiro hondo", "Escribo lo que me pasa", "Acepto que me afecta", "Puedo decir que no", "Reconozco lo que siento",
                                                   "Aprendo de lo que siento", "Me divierto con amigos", "Salgo a tomar el sol", "Un paseo por la naturaleza", "Cantar",
                                                   "Cuento hasta diez", "Me ordeno por dentro y por fuera", "Me cuido", "Ahora no, luego s�", "No me pasa nada", "Estoy as� ahora",
                                                   "No pasa nada si sale mal", "Hablo de lo que me pasa", "Hablo todo el tiempo de lo que me pasa", "Estoy en ello",
                                                   "Nada es para siempre", "No s� que hacer", "Todo se transforma", "Soy consciente de como me afecta lo que hago"};
    List<string> CardTitlesEN = new List<string>() { "Deep breath", "I write what happens to me", "I accept that it affects me", "I can say no", "I recognize what I feel",
                                                   "I learn from what I feel", "I have fun with my friends", "I go out to sunbathe", "A walk through nature", "Sing",
                                                   "I count to ten", "I put myself in order inside and outside", "I take care of myself", "Not now, then yes", "Nothing happens to me", "I am like this now",
                                                   "Nothing happens if it goes wrong", "I talk about what happens to me", "I talk all the time about what happens to me", "I am on it",
                                                   "Nothing is forever", "I don't know what to do", "Everything transforms", "I am aware of how what I do affects me"};
    [SerializeField] TMP_Text CardDescription;
    List<string> CardDescriptionsES = new List<string>() { "Ataque 5", "Ataque 3x2", "Ataque 5 a todos los enemigos", "Ataque 10", "Ataque 20", "Ataque 10x2", "Gana 2 de man�", "Cura 5",
                                                         "Cura 10", "Roba 5 de vida", "Roba 10 de vida", "Roba 5 de vida a todos los enemigos", "Roba 10 de vida a todos los enemigos", "<b>Bloquear</b> a un enemigo",
                                                         "<b>Bloquear</b> a un enemigo pero le cura 10", "<b>D�bil</b> a un enemigo", "<b>D�bil</b> a todos los enemigos",
                                                         "<b>Fuerte</b> al jugador", "<b>Fuerte</b> al jugador pero cura 5 a los enemigos", "<b>Esperanza</b> al jugador",
                                                         "<b>Envenenar</b> a un enemigo", "<b>D�bil</b> a un enemigo pero le cura 15", "Los enemigos curan en vez de da�ar",
                                                         "Se eliminan todos los efectos del jugador" };
    List<string> CardDescriptionsEN = new List<string>() { "Attack 5", "Attack 3x2", "Attack 5 to all enemies", "Attack 10", "Attack 20", "Attack 10x2", "Gain 2 mana", "Heal 5",
                                                         "Heal 10", "Drain 5 to an enemy", "Drain 10 to an enemy", "Drain 5 to all enemies", "Drain 10 to all enemies", "<b>Stun</b> to an enemy",
                                                         "<b>Stun</b> to an enemy but heals him 10", "<b>Weak</b> to an enemy", "<b>Weak</b> to all enemies",
                                                         "<b>Strong</b> to the player", "<b>Strong</b> to the player but heals all enemies 5", "<b>Hope</b> to the player",
                                                         "<b>Poison</b> to an enemy", "<b>Weak</b> to an enemy but heals him 15", "Enemies heal instead of dealing damage",
                                                         "Remove all player's effects" };

    [SerializeField] int Fila; // Posici�n en el compendio | 0 - arriba | 1 - abajo 

    TMP_Text[] newText;

    // Start is called before the first frame update
    void Start()
    {

        VariablesGlobales = GameObject.Find("VariablesGlobales");
        Position = GetComponent<RectTransform>();
        Image = GetComponent<Image>();
        CopyCreated = false;

        FullTransparency = Image.color;
        MidTransparency = Image.color;
        MidTransparency.a = 0.3f;

    }

    // Update is called once per frame
    void Update()
    {

        CantidadText.text = "x" + VariablesGlobales.GetComponent<VariablesGlobales>().AmountCards[Tipo];

        if (VariablesGlobales.GetComponent<VariablesGlobales>().AmountCards[Tipo] == 0)
            Image.color = MidTransparency;
        else
            Image.color = FullTransparency;

        newText = GetComponentsInChildren<TMP_Text>();
        if (VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[Tipo] == VariablesGlobales.GetComponent<VariablesGlobales>().CardCostOriginal[Tipo])
            newText[2].text = "" + VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[Tipo];
        else if (VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[Tipo] < VariablesGlobales.GetComponent<VariablesGlobales>().CardCostOriginal[Tipo])
            newText[2].text = "<color=green>" + VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[Tipo] + "</color>";
        else
            newText[2].text = "<color=red>" + VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[Tipo] + "</color>";

        UpdateCardInterface();

    }

    private void OnMouseOver()
    {

        if (!CopyCreated)
        {

            //Color tempColor;

            CopyCreated = true;
            Copy = Instantiate(gameObject, Parent.transform);
            if(Fila == 0)
                Copy.GetComponent<RectTransform>().anchoredPosition = new Vector2(Position.anchoredPosition.x + 320, Position.anchoredPosition.y - 70);
            else
                Copy.GetComponent<RectTransform>().anchoredPosition = new Vector2(Position.anchoredPosition.x + 320, Position.anchoredPosition.y + 70);
            Copy.transform.localScale = new Vector3(2, 2, 2);

        }

    }

    private void OnMouseExit()
    {

        Destroy(Copy);
        CopyCreated = false;

    }

    private void UpdateCardInterface()
    {

        if(VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
        {

            CardTitle.text = CardTitlesEN[Tipo];
            CardDescription.text = CardDescriptionsEN[Tipo];

        }
        else                                                                 // Spanish
        {

            CardTitle.text = CardTitlesES[Tipo];
            CardDescription.text = CardDescriptionsES[Tipo];

        }

    }

}
