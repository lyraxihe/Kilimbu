using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardCompendio : MonoBehaviour
{

    public GameObject VariablesGlobales;
    [SerializeField] Traduction Traduction; // Script del singleton de Traduction
    [SerializeField] GameObject CanvasSingleton;
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
    
    [SerializeField] TMP_Text DescriptionPlayerCard;
    List<string> CardDescriptionPlayerES = new List<string>() { "", "", "", "", "", "", "+2 Maná", "+5", "+10", "+5", "+10",
                                                              "xN +5", "xN +10", "", "-8", "", "", "Fuerte", "Fuerte y -8",
                                                              "Esperanza", "", "-8", "Transformado", "-Efectos" };
    List<string> CardDescriptionPlayerEN = new List<string>() { "", "", "", "", "", "", "+2 Mana", "+5", "+10", "+5", "+10",
                                                              "xN +5", "xN +10", "", "-8", "", "", "Strong", "Strong and -8",
                                                              "Hope", "", "-8", "Transformed", "-Effects" };

    [SerializeField] TMP_Text DescriptionEnemiesCard;
    List<string> CardDescriptionEnemiesES = new List<string>() { "-5", "x2 -3", "-5 a todos", "-10", "-20", "x2 -10", "", "",
                                                               "", "-5", "-10", "-5 a todos", "-10 a todos", "Aturdido",
                                                               "Aturdido", "Débil", "Débil a todos", "", "", "",
                                                               "Envenenado", "Débil", "", "" };
    List<string> CardDescriptionEnemiesEN = new List<string>() { "-5", "x2 -3", "-5 to everyone", "-10", "-20", "x2 -10", "", "",
                                                               "", "-5", "-10", "-5 to everyone", "-10 to everyone", "Stunned",
                                                               "Stunned", "Weak", "Weak to everyone", "", "", "",
                                                               "Poisoned", "Weak", "", "" };
    List<string> CardDuration = new List<string>() { "", "", "", "", "", "", "", "", "", "", "", "", "", "1", "1", "3", "2", "4", "4", "4", "3", "3", "1", "0" };

    public TMP_Text CardCost;
    public TMP_Text CardDuracion;
    public GameObject EffectContainer; // Container del texto que explica el efecto de la carta
    public TMP_Text EffectText;        // Texto que explica el efecto de la carta
    List<string> CardEffectDescriptionES = new List<string>() { "<b>Aturdido</b>: El enemigo no puede atacar el siguiente turno", "<b>Débil</b>: Los ataques del enemigo hacen -2 de daño",
                                                                "<b>Fuerte</b>: Los ataques del jugador hacen +3 de daño", "<b>Esperanza</b>: Cada turno restaura vida (+3) al jugador",
                                                                "<b>Envenenado</b>: Cada turno inflige daño (-3) al enemigo", "<b>Transformado</b>: Los ataques de los enemigos curan en vez de hacer daño" };
    List<string> CardEffectDescriptionEN = new List<string>() { "<b>Stunned</b>: The enemy cannot attack the next turn", "<b>Weak</b>: Enemy's attacks deal -2 damage", "<b>Strong</b>: Player's attacks deal +3 damage",
                                                                "<b>Hope</b>: Each turn restores health (+3) to the player", "<b>Poisoned</b>: Each turn deals damage (-3) to the enemy", "<b>Transformed</b>: Enemy's attacks heal instead of dealing damage" };

    [SerializeField] int Fila; // Posición en el compendio | 0 - arriba | 1 - abajo 

    TMP_Text[] newText;

    // Card click
    private float ClickTime = 0f;
    private float Delay = 0.3f;
    public Vector3 CompendioPosition = new Vector3 (-1, -1, -1);
    private bool CardIncrease = false;
    public bool CardDecrease = false;
    private Vector3 OriginalScale;
    public Vector3 FinalScale;
    [SerializeField] List<GameObject>ListCompendioCards = new List<GameObject>();

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

        newText = GetComponentsInChildren<TMP_Text>();
        newText[4].text = "" + CardDuration[Tipo];

        OriginalScale = gameObject.transform.localScale;
        FinalScale = OriginalScale * 1.7f;

        ListCompendioCards = CanvasSingleton.GetComponent<CanvasSingleton>().ListCompendioCards;

    }

    // Update is called once per frame
    void Update()
    {

        CantidadText.text = "x" + VariablesGlobales.GetComponent<VariablesGlobales>().AmountCards[Tipo];

        if (VariablesGlobales.GetComponent<VariablesGlobales>().AmountCards[Tipo] == 0)
        {

            Image.color = MidTransparency;
            CardTitle.color = new Color (CardTitle.color.r, CardTitle.color.g, CardTitle.color.b, MidTransparency.a);
            DescriptionPlayerCard.color = new Color(DescriptionPlayerCard.color.r, DescriptionPlayerCard.color.g, DescriptionPlayerCard.color.b, MidTransparency.a);
            DescriptionEnemiesCard.color = new Color(DescriptionEnemiesCard.color.r, DescriptionEnemiesCard.color.g, DescriptionEnemiesCard.color.b, MidTransparency.a);
            CardCost.color = new Color(CardCost.color.r, CardCost.color.g, CardCost.color.b, MidTransparency.a);
            CardDuracion.color = new Color(CardDuracion.color.r, CardDuracion.color.g, CardDuracion.color.b, MidTransparency.a);

        }
        else
        {

            Image.color = FullTransparency;
            CardTitle.color = new Color(CardTitle.color.r, CardTitle.color.g, CardTitle.color.b, 1);
            DescriptionPlayerCard.color = new Color(DescriptionPlayerCard.color.r, DescriptionPlayerCard.color.g, DescriptionPlayerCard.color.b, 1);
            DescriptionEnemiesCard.color = new Color(DescriptionEnemiesCard.color.r, DescriptionEnemiesCard.color.g, DescriptionEnemiesCard.color.b, 1);
            CardCost.color = new Color(CardCost.color.r, CardCost.color.g, CardCost.color.b, 1);
            CardDuracion.color = new Color(CardDuracion.color.r, CardDuracion.color.g, CardDuracion.color.b, 1);

        }

        if (VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[Tipo] == VariablesGlobales.GetComponent<VariablesGlobales>().CardCostOriginal[Tipo])
            newText[3].text = "" + VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[Tipo];
        else if (VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[Tipo] < VariablesGlobales.GetComponent<VariablesGlobales>().CardCostOriginal[Tipo])
            newText[3].text = "<color=green>" + VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[Tipo] + "</color>";
        else
            newText[3].text = "<color=red>" + VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[Tipo] + "</color>";

        UpdateCardInterface();

        // Card Click
        IncreaseScale();
        DecreaseScale();

        if (CompendioPosition != Parent.transform.position && CompendioPosition != new Vector3(-1, -1, -1))
        {

            CardIncrease = false;
            CardDecrease = true;

        }

    }

    private void OnMouseOver()
    {

        //if (!CopyCreated)
        //{

        //    //Color tempColor;

        //    CopyCreated = true;
        //    Copy = Instantiate(gameObject, Parent.transform);
        //    if (Fila == 0)
        //        Copy.GetComponent<RectTransform>().anchoredPosition = new Vector2(Position.anchoredPosition.x + 320, Position.anchoredPosition.y - 70);
        //    else
        //        Copy.GetComponent<RectTransform>().anchoredPosition = new Vector2(Position.anchoredPosition.x + 320, Position.anchoredPosition.y + 70);
        //    Copy.transform.localScale = new Vector3(2, 2, 2);

        //    // Si la opción de los textos descriptivos está activada
        //    if (Traduction.DescriptiveTexts)
        //        // Activa el texto explicativo de las cartas que lo necesiten
        //        if (Tipo == 13 || Tipo == 14 || Tipo == 15 || Tipo == 16 || Tipo == 17 || Tipo == 18 || Tipo == 19 || Tipo == 20 || Tipo == 21 || Tipo == 22)
        //        {

        //            Copy.GetComponent<CardCompendio>().EffectContainer.SetActive(true);
        //            Copy.GetComponent<CardCompendio>().EffectText.gameObject.SetActive(true);

        //        }

        //}

        // Si la opción de los textos descriptivos está activada
        if (Traduction.DescriptiveTexts)
        {

            if (gameObject.transform.localScale != OriginalScale)
            {

                // Activa el texto explicativo de las cartas que lo necesiten
                if (Tipo == 13 || Tipo == 14 || Tipo == 15 || Tipo == 16 || Tipo == 17 || Tipo == 18 || Tipo == 19 || Tipo == 20 || Tipo == 21 || Tipo == 22)
                {

                    EffectContainer.SetActive(true);
                    EffectText.gameObject.SetActive(true);

                }

            }
            else
            {

                EffectContainer.SetActive(false);
                EffectText.gameObject.SetActive(false);

            }

        }


    }

    private void OnMouseExit()
    {

        // Desactiva el texto explicativo de las cartas que lo necesiten
        if (Tipo == 13 || Tipo == 14 || Tipo == 15 || Tipo == 16 || Tipo == 17 || Tipo == 18 || Tipo == 19 || Tipo == 20 || Tipo == 21 || Tipo == 22)
        {

            //Copy.GetComponent<CardCompendio>().EffectContainer.SetActive(false);
            //Copy.GetComponent<CardCompendio>().EffectText.gameObject.SetActive(false);
            EffectContainer.SetActive(false);
            EffectText.gameObject.SetActive(false);

        }

        //Destroy(Copy);
        //CopyCreated = false;

    }

    private void OnMouseDown()
    {

        ClickTime = Time.time;
        CompendioPosition = Parent.transform.position;

        for (int i = 0; i < ListCompendioCards.Count; i++)
        {

            if (ListCompendioCards[i] != this && ListCompendioCards[i].GetComponent<CardCompendio>().transform.localScale != ListCompendioCards[i].GetComponent<CardCompendio>().OriginalScale)
            {

                ListCompendioCards[i].GetComponent<CardCompendio>().CardIncrease = false;
                ListCompendioCards[i].GetComponent<CardCompendio>().CardDecrease = true;

            }

        }

    }

    private void OnMouseUp()
    {
        
        if (Time.time - ClickTime <= Delay && Parent.transform.position == CompendioPosition)
        {

            //if(CardSelected)
            //{

            //    CardSelected = false;
            //    CardDeselected = true;

            //}
            //else
            //{

            //    CardDeselected = false;
            //    CardSelected = true;

            //}

            if(gameObject.transform.localScale == OriginalScale)
                CardIncrease = true;
            else if (gameObject.transform.localScale == FinalScale)
                CardDecrease = true;

        }

    }

    private void UpdateCardInterface()
    {

        if(VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
        {

            CardTitle.text = CardTitlesEN[Tipo];
            DescriptionPlayerCard.text = CardDescriptionPlayerEN[Tipo];
            DescriptionEnemiesCard.text = CardDescriptionEnemiesEN[Tipo];

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
        else                                                                 // Spanish
        {

            CardTitle.text = CardTitlesES[Tipo];
            DescriptionPlayerCard.text = CardDescriptionPlayerES[Tipo];
            DescriptionEnemiesCard.text = CardDescriptionEnemiesES[Tipo];

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

    public void IncreaseScale()
    {

        if (gameObject.transform.localScale != FinalScale && CardIncrease)
        {

            gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, FinalScale, 0.01f);
            if (Vector3.Distance(gameObject.transform.localScale, FinalScale) <= 0.01f)
            {

                gameObject.transform.localScale = FinalScale;
                CardIncrease = false;

            }

        }

    }

    public void DecreaseScale()
    {

        if (gameObject.transform.localScale != OriginalScale && CardDecrease)
        {

            gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, OriginalScale, 0.01f);
            if (Vector3.Distance(gameObject.transform.localScale, OriginalScale) <= 0.01f)
            {

                gameObject.transform.localScale = OriginalScale;
                CardDecrease = false;
                CompendioPosition = new Vector3(-1, -1, -1);

            }

        }

    }

}
