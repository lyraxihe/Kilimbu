using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardManaText : MonoBehaviour
{

    [SerializeField] GameObject VariablesGlobales;
    [SerializeField] Traduction Traduction; // Script del singleton de Traduction
    [SerializeField] TMP_Text TitleCard;
    [SerializeField] TMP_Text DescriptionPlayerCard;
    [SerializeField] TMP_Text DescriptionEnemiesCard;
    private int Tipo;

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
    public GameObject EffectContainer;
    public TMP_Text EffectText;
    List<string> CardEffectDescriptionES = new List<string>() { "<b>Aturdido</b>: El enemigo no puede atacar el siguiente turno", "<b>Débil</b>: Los ataques del enemigo hacen -2 de daño",
                                                                "<b>Fuerte</b>: Los ataques del jugador hacen +3 de daño", "<b>Esperanza</b>: Cada turno restaura vida (+3) al jugador",
                                                                "<b>Envenenado</b>: Cada turno inflige daño (-3) al enemigo", "<b>Transformado</b>: Los ataques de los enemigos curan en vez de hacer daño" };
    List<string> CardEffectDescriptionEN = new List<string>() { "<b>Stunned</b>: The enemy cannot attack the next turn", "<b>Weak</b>: Enemy's attacks deal -2 damage", "<b>Strong</b>: Player's attacks deal +3 damage",
                                                                "<b>Hope</b>: Each turn restores health (+3) to the player", "<b>Poisoned</b>: Each turn deals damage (-3) to the enemy", "<b>Transformed</b>: Enemy's attacks heal instead of dealing damage" };

    // SoundFX Management
    public AudioSource MejoraCofreSound;




    private void Awake()
    {

        VariablesGlobales = GameObject.Find("VariablesGlobales");
        Traduction = GameObject.Find("Traduction").GetComponent<Traduction>(); // Script del singleton de Traduction
        Tipo = gameObject.GetComponent<ChestCard>().RandTipo;

    }

    // Start is called before the first frame update
    void Start()
    {
        // Get SounFx's
        MejoraCofreSound = GameObject.Find("MejoraCofre_SoundFX").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        UpdateTexts();

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

    private void UpdateTexts()
    {

        if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
        {

            TitleCard.text = CardTitlesEN[gameObject.GetComponent<ChestCard>().RandTipo];
            DescriptionPlayerCard.text = CardDescriptionPlayerEN[gameObject.GetComponent<ChestCard>().RandTipo];
            DescriptionEnemiesCard.text = CardDescriptionEnemiesEN[gameObject.GetComponent<ChestCard>().RandTipo];

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
            else if (Tipo == 22)
                EffectText.text = CardEffectDescriptionEN[5];

        }
        else                                                                   // Spanish
        {

            TitleCard.text = CardTitlesES[gameObject.GetComponent<ChestCard>().RandTipo];
            DescriptionPlayerCard.text = CardDescriptionPlayerES[gameObject.GetComponent<ChestCard>().RandTipo];
            DescriptionEnemiesCard.text = CardDescriptionEnemiesES[gameObject.GetComponent<ChestCard>().RandTipo];

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
            else if (Tipo == 22)
                EffectText.text = CardEffectDescriptionES[5];

        }

    }

}
