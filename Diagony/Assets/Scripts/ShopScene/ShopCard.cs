using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopCard : MonoBehaviour
{
    [SerializeField] GameObject VariablesGlobales;
    [SerializeField] Traduction Traduction; // Script del singleton de Traduction
    [SerializeField] TMP_Text TitleCard;
    [SerializeField] TMP_Text DescriptionCard;
    [SerializeField] TMP_Text ManaCostCard;
    public int Tipo;

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

    List<string> CardDescriptionsES = new List<string>() { "Ataque 5", "Ataque 3x2", "Ataque 5 a todos los enemigos", "Ataque 10", "Ataque 20", "Ataque 10x2", "Gana 2 de maná", "Cura 5",
                                                         "Cura 10", "Roba 5 de vida", "Roba 10 de vida", "Roba 5 de vida a todos los enemigos", "Roba 10 de vida a todos los enemigos", "<b>Bloquear</b> a un enemigo",
                                                         "<b>Bloquear</b> a un enemigo pero le cura 10", "<b>Débil</b> a un enemigo", "<b>Débil</b> a todos los enemigos",
                                                         "<b>Fuerte</b> al jugador", "<b>Fuerte</b> al jugador pero cura 5 a los enemigos", "<b>Esperanza</b> al jugador",
                                                         "<b>Envenenar</b> a un enemigo", "<b>Débil</b> a un enemigo pero le cura 15", "<b>Transformar</b> al jugador",
                                                         "Se eliminan todos los efectos del jugador" };
    List<string> CardDescriptionsEN = new List<string>() { "Attack 5", "Attack 3x2", "Attack 5 to all enemies", "Attack 10", "Attack 20", "Attack 10x2", "Gain 2 mana", "Heal 5",
                                                         "Heal 10", "Drain 5 to an enemy", "Drain 10 to an enemy", "Drain 5 to all enemies", "Drain 10 to all enemies", "<b>Stun</b> to an enemy",
                                                         "<b>Stun</b> to an enemy but heals him 10", "<b>Weak</b> to an enemy", "<b>Weak</b> to all enemies",
                                                         "<b>Strong</b> to the player", "<b>Strong</b> to the player but heals all enemies 5", "<b>Hope</b> to the player",
                                                         "<b>Poison</b> to an enemy", "<b>Weak</b> to an enemy but heals him 15", "<b>Transform</b> to the player",
                                                         "Remove all player's effects" };
    public GameObject EffectContainer;
    public TMP_Text EffectText;
    List<string> CardEffectDescriptionES = new List<string>() { "<b>Bloquear</b>: El enemigo no puede atacar el siguiente turno", "<b>Débil</b>: Los ataques del enemigo hacen -3 de daño",
                                                                "<b>Fuerte</b>: Los ataques del jugador hacen +3 de daño", "<b>Esperanza</b>: Los ataques del jugador restauran +3 de vida",
                                                                "<b>Envenenar</b>: Las acciones del jugador infligen -3 de vida", "<b>Transformar</b>: Los ataques de los enemigos curan en vez de hacer daño" };
    List<string> CardEffectDescriptionEN = new List<string>() { "<b>Stun</b>: The enemy cannot attack the next turn", "<b>Weak</b>: Enemy's attacks deal -3 damage", "<b>Strong</b>: Player's attacks deal +3 damage",
                                                                "<b>Hope</b>: Player's attacks heal +3 health", "<b>Poison</b>: Player's actions deal -3 health", "<b>Transform</b>: Enemy's attacks heal instead of dealing damage" };

    private void Awake()
    {

        VariablesGlobales = GameObject.Find("VariablesGlobales");
        Traduction = GameObject.Find("Traduction").GetComponent<Traduction>(); // Script del singleton de Traduction

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        UpdateCardTexts();

    }

    public void OnMouseOver()
    {

        // Si la opción de los textos descriptivos está activada
        if (Traduction.DescriptiveTexts)
            // Activa el texto explicativo de las cartas que lo necesiten
            if (Tipo == 13 || Tipo == 14 || Tipo == 15 || Tipo == 16 || Tipo == 17 || Tipo == 18 || Tipo == 19 || Tipo == 20 || Tipo == 21 || Tipo == 22)
            {

                EffectContainer.SetActive(true);
                EffectText.gameObject.SetActive(true);

            }

    }

    public void OnMouseExit()
    {

        // Desactiva el texto explicativo de las cartas que lo necesiten
        if (Tipo == 13 || Tipo == 14 || Tipo == 15 || Tipo == 16 || Tipo == 17 || Tipo == 18 || Tipo == 19 || Tipo == 20 || Tipo == 21 || Tipo == 22)
        {

            EffectContainer.SetActive(false);
            EffectText.gameObject.SetActive(false);

        }

    }

    private void UpdateCardTexts()
    {

        // Actualiza el idioma
        if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
        {

            TitleCard.text = CardTitlesEN[Tipo];
            DescriptionCard.text = CardDescriptionsEN[Tipo];

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
        else                                                                   // Spanish
        {

            TitleCard.text = CardTitlesES[Tipo];
            DescriptionCard.text = CardDescriptionsES[Tipo];

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

        // Actualiza con colorines el coste de mana de la carta
        if (VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[Tipo] == VariablesGlobales.GetComponent<VariablesGlobales>().CardCostOriginal[Tipo])
            ManaCostCard.text = "" + VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[Tipo];
        else if (VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[Tipo] < VariablesGlobales.GetComponent<VariablesGlobales>().CardCostOriginal[Tipo])
            ManaCostCard.text = "<color=green>" + VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[Tipo] + "</color>";
        else
            ManaCostCard.text = "<color=red>" + VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[Tipo] + "</color>";
    }

}
