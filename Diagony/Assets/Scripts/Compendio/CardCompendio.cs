using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardCompendio : MonoBehaviour
{

    public GameObject VariablesGlobales;
    [SerializeField] Traduction Traduction; // Script del singleton de Traduction
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
                                                   "Cuento hasta diez", "Me ordeno por dentro y por fuera", "Me cuido", "Ahora no, luego sí", "No me pasa nada", "Estoy así ahora",
                                                   "No pasa nada si sale mal", "Hablo de lo que me pasa", "Hablo todo el tiempo de lo que me pasa", "Estoy en ello",
                                                   "Nada es para siempre", "No sé que hacer", "Todo se transforma", "Soy consciente de como me afecta lo que hago"};
    List<string> CardTitlesEN = new List<string>() { "Deep breath", "I write what happens to me", "I accept that it affects me", "I can say no", "I recognize what I feel",
                                                   "I learn from what I feel", "I have fun with my friends", "I go out to sunbathe", "A walk through nature", "Sing",
                                                   "I count to ten", "I put myself in order inside and outside", "I take care of myself", "Not now, then yes", "Nothing happens to me", "I am like this now",
                                                   "Nothing happens if it goes wrong", "I talk about what happens to me", "I talk all the time about what happens to me", "I am on it",
                                                   "Nothing is forever", "I don't know what to do", "Everything transforms", "I am aware of how what I do affects me"};
    [SerializeField] TMP_Text CardDescription;
    List<string> CardDescriptionsES = new List<string>() { "Ataque 5", "Ataque 3x2", "Ataque 5 a todos los enemigos", "Ataque 10", "Ataque 20", "Ataque 10x2", "Gana 2 de maná", "Cura 5",
                                                         "Cura 10", "Roba 5 de vida", "Roba 10 de vida", "Roba 5 de vida a todos los enemigos", "Roba 10 de vida a todos los enemigos", "<b>Bloquear</b> a un enemigo",
                                                         "<b>Bloquear</b> a un enemigo pero le cura 7", "<b>Débil</b> a un enemigo", "<b>Débil</b> a todos los enemigos",
                                                         "<b>Fuerte</b> al jugador", "<b>Fuerte</b> al jugador pero cura 5 a los enemigos", "<b>Esperanza</b> al jugador",
                                                         "<b>Envenenar</b> a un enemigo", "<b>Débil</b> a un enemigo pero le cura 8", "<b>Transformar</b> al jugador",
                                                         "Se eliminan todos los efectos del jugador" };
    List<string> CardDescriptionsEN = new List<string>() { "Attack 5", "Attack 3x2", "Attack 5 to all enemies", "Attack 10", "Attack 20", "Attack 10x2", "Gain 2 mana", "Heal 5",
                                                         "Heal 10", "Drain 5 to an enemy", "Drain 10 to an enemy", "Drain 5 to all enemies", "Drain 10 to all enemies", "<b>Stun</b> to an enemy",
                                                         "<b>Stun</b> to an enemy but heals him 7", "<b>Weak</b> to an enemy", "<b>Weak</b> to all enemies",
                                                         "<b>Strong</b> to the player", "<b>Strong</b> to the player but heals all enemies 5", "<b>Hope</b> to the player",
                                                         "<b>Poison</b> to an enemy", "<b>Weak</b> to an enemy but heals him 8", "<b>Transform</b> to the player",
                                                         "Remove all player's effects" };
    public TMP_Text CardCost;
    public TMP_Text CardDuracion;
    public GameObject EffectContainer; // Container del texto que explica el efecto de la carta
    public TMP_Text EffectText;        // Texto que explica el efecto de la carta
    List<string> CardEffectDescriptionES = new List<string>() { "<b>Bloquear</b>: El enemigo no puede atacar el siguiente turno", "<b>Débil</b>: Los ataques del enemigo hacen -3 de daño",
                                                                "<b>Fuerte</b>: Los ataques del jugador hacen +3 de daño", "<b>Esperanza</b>: Los ataques del jugador restauran +3 de vida",
                                                                "<b>Envenenar</b>: Las acciones del jugador infligen -3 de vida", "<b>Transformar</b>: Los ataques de los enemigos curan en vez de hacer daño" };
    List<string> CardEffectDescriptionEN = new List<string>() { "<b>Stun</b>: The enemy cannot attack the next turn", "<b>Weak</b>: Enemy's attacks deal -3 damage", "<b>Strong</b>: Player's attacks deal +3 damage",
                                                                "<b>Hope</b>: Player's attacks heal +3 health", "<b>Poison</b>: Player's actions deal -3 health", "<b>Transform</b>: Enemy's attacks heal instead of dealing damage" };

    [SerializeField] int Fila; // Posición en el compendio | 0 - arriba | 1 - abajo 

    TMP_Text[] newText;

    // Start is called before the first frame update
    void Start()
    {

        VariablesGlobales = GameObject.Find("VariablesGlobales");
        Traduction = GameObject.Find("Traduction").GetComponent<Traduction>(); // Script del singleton de Traduction
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
        {

            Image.color = MidTransparency;
            CardTitle.color = new Color (CardTitle.color.r, CardTitle.color.g, CardTitle.color.b, MidTransparency.a);
            CardDescription.color = new Color(CardDescription.color.r, CardDescription.color.g, CardDescription.color.b, MidTransparency.a);
            CardCost.color = new Color(CardCost.color.r, CardCost.color.g, CardCost.color.b, MidTransparency.a);
            CardDuracion.color = new Color(CardDuracion.color.r, CardDuracion.color.g, CardDuracion.color.b, MidTransparency.a);

        }
        else
        {

            Image.color = FullTransparency;
            CardTitle.color = new Color(CardTitle.color.r, CardTitle.color.g, CardTitle.color.b, 1);
            CardDescription.color = new Color(CardDescription.color.r, CardDescription.color.g, CardDescription.color.b, 1);
            CardCost.color = new Color(CardCost.color.r, CardCost.color.g, CardCost.color.b, 1);
            CardDuracion.color = new Color(CardDuracion.color.r, CardDuracion.color.g, CardDuracion.color.b, 1);

        }

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

            // Si la opción de los textos descriptivos está activada
            if(Traduction.DescriptiveTexts)
                // Activa el texto explicativo de las cartas que lo necesiten
                if (Tipo == 13 || Tipo == 14 || Tipo == 15 || Tipo == 16 || Tipo == 17 || Tipo == 18 || Tipo == 19 || Tipo == 20 || Tipo == 21 || Tipo == 22)
                {

                    Copy.GetComponent<CardCompendio>().EffectContainer.SetActive(true);
                    Copy.GetComponent<CardCompendio>().EffectText.gameObject.SetActive(true);

                }

        }

    }

    private void OnMouseExit()
    {

        // Desactiva el texto explicativo de las cartas que lo necesiten
        if (Tipo == 13 || Tipo == 14 || Tipo == 15 || Tipo == 16 || Tipo == 17 || Tipo == 18 || Tipo == 19 || Tipo == 20 || Tipo == 21 || Tipo == 22)
        {

            Copy.GetComponent<CardCompendio>().EffectContainer.SetActive(false);
            Copy.GetComponent<CardCompendio>().EffectText.gameObject.SetActive(false);

        }

        Destroy(Copy);
        CopyCreated = false;

    }

    private void UpdateCardInterface()
    {

        if(VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
        {

            CardTitle.text = CardTitlesEN[Tipo];
            CardDescription.text = CardDescriptionsEN[Tipo];

            // Traducción inglesa de los textos explicativos de los efectos
            if (Tipo == 13 || Tipo == 14)
                EffectText.text = CardEffectDescriptionEN[0];
            else if (Tipo == 15 || Tipo == 16 || Tipo == 21)
                EffectText.text = CardEffectDescriptionEN[1];
            else if (Tipo == 17 || Tipo == 18)
                EffectText.text = CardEffectDescriptionEN[2];
            else if (Tipo == 19)
                EffectText.text = CardEffectDescriptionEN[3];
            else if (Tipo == 20)
                EffectText.text = CardEffectDescriptionEN[4];
            else if (Tipo == 21)
                EffectText.text = CardEffectDescriptionEN[5];


        }
        else                                                                 // Spanish
        {

            CardTitle.text = CardTitlesES[Tipo];
            CardDescription.text = CardDescriptionsES[Tipo];

            // Traducción española de los textos explicativos de los efectos
            if (Tipo == 13 || Tipo == 14)
                EffectText.text = CardEffectDescriptionES[0];
            else if (Tipo == 15 || Tipo == 16 || Tipo == 21)
                EffectText.text = CardEffectDescriptionES[1];
            else if (Tipo == 17 || Tipo == 18)
                EffectText.text = CardEffectDescriptionES[2];
            else if (Tipo == 19)
                EffectText.text = CardEffectDescriptionES[3];
            else if (Tipo == 20)
                EffectText.text = CardEffectDescriptionES[4];
            else if (Tipo == 21)
                EffectText.text = CardEffectDescriptionES[5];

        }

    }

}
