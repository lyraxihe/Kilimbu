﻿
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class CardController : MonoBehaviour
{

    public GameObject VariablesGlobales;
    [SerializeField] Traduction Traduction; // Script del singleton de Traduction
    public GameObject CombatScene;
    public GameObject Player;
    public GameObject DragZone;
    public GameObject ArrowEmitter;
    public int EnemigoSeleccionado; // -1; Ninguno | 0: EnemyList[0] | 1: EnemyList[1] | 2: EnemyList[2]

    [SerializeField] Vector3 CardPosition;
    [SerializeField] Vector3 CardRotation;

    public TMP_Text TextTitle;
    public TMP_Text TextDescription;
    public GameObject EffectContainer; // Container del texto que explica el efecto de la carta
    public TMP_Text EffectText;        // Texto que explica el efecto de la carta

    // Sprites de las cartas
    //[SerializeField] Sprite danyo5;
    //[SerializeField] Sprite danyo10;
    //[SerializeField] Sprite curar10;

    public Animator CardAnimator;
    [SerializeField] bool AnimacionEntrar;
    public bool AnimacionSalir;
    //private float desireDuration;
    //private float elapsedTime;

    Vector3 MousePositionOffset;
    public bool MouseDrag, MouseOver, IsInDragZone, SePuede;
    public int Id; // ID de la carta en la lista de cartas (para saber su posicion al eliminarla de la lista)
    [SerializeField] int NumCartas; // Número de cartas en el turno actual
    public int Tipo; 
    public int CosteMana;
    private int numEnemies; // Nº enemies in combat

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

    List<string> CardEffectDescriptionES = new List<string>() { "<b>Aturdido</b>: El enemigo no puede atacar el siguiente turno", "<b>Débil</b>: Los ataques del enemigo hacen -2 de daño",
                                                                "<b>Fuerte</b>: Los ataques del jugador hacen +3 de daño", "<b>Esperanza</b>: Cada turno restaura vida (+3) al jugador",
                                                                "<b>Envenenado</b>: Cada turno inflige daño (-3) al enemigo", "<b>Transformado</b>: Los ataques de los enemigos curan en vez de hacer daño" };
    List<string> CardEffectDescriptionEN = new List<string>() { "<b>Stunned</b>: The enemy cannot attack the next turn", "<b>Weak</b>: Enemy's attacks deal -2 damage", "<b>Strong</b>: Player's attacks deal +3 damage",
                                                                "<b>Hope</b>: Each turn restores health (+3) to the player", "<b>Poisoned</b>: Each turn deals damage (-3) to the enemy", "<b>Transformed</b>: Enemy's attacks heal instead of dealing damage" };

    List<string> CardDuration = new List<string>() { "", "", "", "", "", "", "", "", "", "", "", "", "", "1", "1", "3", "2", "4", "4", "4", "3", "3", "1", "0" };

    TMP_Text[] newText;

    // Efectos de sonido
    public AudioSource hoverCardSound;
    public AudioSource SinEnergiaSound; 

    void Start()
    {

        Traduction = GameObject.Find("Traduction").GetComponent<Traduction>(); // Script del singleton de Traduction

        MouseDrag = true;
        MouseOver = true;

        EnemigoSeleccionado = -1;

        //AnimacionCarta();

        //desireDuration = 5f;
        //AnimacionCarta = true;
        AnimacionEntrar = true;
        AnimacionSalir = false;

        CardAnimator = GetComponent<Animator>();
        //CardAnimator.SetBool("AnimacionCarta", AnimacionCarta);
        CardAnimator.SetBool("AnimacionEntrar", AnimacionEntrar);
        CardAnimator.SetBool("AnimacionSalir", AnimacionSalir);
       
        setColor_text();

        //TextTitle.GetComponent<MeshRenderer>().enabled = true;
        //TextDescription.GetComponent<MeshRenderer>().enabled = true;

        newText = GetComponentsInChildren<TMP_Text>();
        CosteMana = VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[Tipo];

        hoverCardSound = GameObject.Find("SoundFX/CartaHover_SoundFX").GetComponent<AudioSource>(); //Get SoundFX
        SinEnergiaSound = GameObject.Find("SinEnergia_SoundFX").GetComponent<AudioSource>(); //Get SoundFX

    }

    void Update()
    {

        //TextTitle.transform.position = transform.position;
        //TextTitle.transform.eulerAngles = transform.eulerAngles;
        //TextDescription.transform.position = transform.position;
        //TextDescription.transform.eulerAngles = transform.eulerAngles;

        NumCartas = CombatScene.GetComponent<CombatController>().CardList.Count;
        CardAnimator.SetInteger("NumCartas", NumCartas);
        CardAnimator.SetInteger("CardID", Id);

        numEnemies = CombatScene.GetComponent<CombatController>().EnemyList.Count;

        //AnimacionCarta();

        SetText();

    }

    private Vector3 GetMouseWorldPosition()
    {

        return Camera.main.ScreenToWorldPoint(Input.mousePosition);

    }

    private void OnMouseOver()
    {
       // hoverCardSound.Play();

        if (!MouseDrag && /*CombatScene.GetComponent<CombatController>().ManaProtagonista > 0 &&*/ !VariablesGlobales.GetComponent<VariablesGlobales>().EstaEnPausa && !CombatScene.GetComponent<CombatController>().MovingArrow)
        {
           if (IsInDragZone)
            {

                if(CombatScene.GetComponent<CombatController>().ManaProtagonista - CosteMana >= 0)
                {

                    if (CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Bloqueado)
                    {

                        if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                            CombatScene.GetComponent<CombatController>().CreateSpellText("Stunned", CombatScene.GetComponent<CombatController>().Player);
                        else                                                                  // Spanish
                            CombatScene.GetComponent<CombatController>().CreateSpellText("Bloqueado", CombatScene.GetComponent<CombatController>().Player);

                    }
                    else
                    {

                        CombatScene.GetComponent<CombatController>().ManaProtagonista -= CosteMana;  // Reduce el maná del jugador
                        CombatScene.GetComponent<CombatController>().EliminarCarta(Id);
                        CombatScene.GetComponent<CombatController>().UsarCarta(Tipo, 4);

                        Destroy(gameObject);                                              //destruye la carta al colisionar con la dragzone

                    }

                }
                else
                {
                    SinEnergiaSound.Play();
                    Debug.Log("SIN ENERGIA");
                }

           }

            // Si la opción de los textos descriptivos está activada
            if (Traduction.DescriptiveTexts)
                // Activa el texto explicativo de las cartas que lo necesiten
                if (Tipo == 13 || Tipo == 14 || Tipo == 15 || Tipo == 16 || Tipo == 17 || Tipo == 18 || Tipo == 19 || Tipo == 20 || Tipo == 21 || Tipo == 22)
                {

                    EffectContainer.SetActive(true);
                    EffectText.gameObject.SetActive(true);

                }

           MouseOver = true;
           transform.localScale = new Vector3(1, 1, 1);
           transform.position = new Vector3(transform.position.x, -3.62f, 0);
           transform.eulerAngles = new Vector3(0, 0, 0);
        }

    }

    private void OnMouseExit()
    {

        // Desactiva el texto explicativo de las cartas que lo necesiten
        if (Tipo == 13 || Tipo == 14 || Tipo == 15 || Tipo == 16 || Tipo == 17 || Tipo == 18 || Tipo == 19 || Tipo == 20 || Tipo == 21 || Tipo == 22)
        {

            EffectContainer.SetActive(false);
            EffectText.gameObject.SetActive(false);

        }

        MouseOver = false;

    }


    private void OnMouseUp()
    {
        MouseDrag = false;
        MouseOver = false;
        //ArrowEmitter.GetComponent<ArrowEmitter>().OverEnemy = false;
        //if(ArrowEmitter.GetComponent<ArrowEmitter>().OverEnemy)
        //{

        //    CombatScene.GetComponent<CombatController>().ManaProtagonista--;  // Reduce el maná del jugador en 1
        //    CombatScene.GetComponent<CombatController>().EliminarCarta(Id);
        //    CombatScene.GetComponent<CombatController>().UsarCarta(Tipo);

        //    Destroy(gameObject);                                              //destruye la carta al colisionar con la dragzone

        //}

        if (EnemigoSeleccionado != -1)
        {

            if (CombatScene.GetComponent<CombatController>().ManaProtagonista - CosteMana >= 0)
            {

                // Deshabilita la flecha
                //OverEnemy = false;

                for (int j = 0; j < ArrowEmitter.GetComponent<ArrowEmitter>().arrowNodes.Count; j++)
                {

                    //ArrowEmitter.GetComponent<ArrowEmitter>().arrowNodes[j].GetComponent<Image>().color = Color.grey;
                    if(j == ArrowEmitter.GetComponent<ArrowEmitter>().arrowNodes.Count - 1)
                        ArrowEmitter.GetComponent<ArrowEmitter>().arrowNodes[j].GetComponent<Image>().sprite = ArrowEmitter.GetComponent<ArrowEmitter>().ArrowHeadDisabled;
                    else
                        ArrowEmitter.GetComponent<ArrowEmitter>().arrowNodes[j].GetComponent<Image>().sprite = ArrowEmitter.GetComponent<ArrowEmitter>().ArrowNodeDisabled;

                }

                //ArrowEmitter.SetActive(false);
                //CombatScene.GetComponent<CombatController>().MovingArrow = false;
                //Cursor.visible = true;
                CombatScene.GetComponent<CombatController>().EnemyList[EnemigoSeleccionado].GetComponent<EnemyController>().AuraEnemigo.SetActive(false);

                // Realiza el efecto de la carta
                if (CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Bloqueado)
                {

                    if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                        CombatScene.GetComponent<CombatController>().CreateSpellText("Stunned", CombatScene.GetComponent<CombatController>().Player);
                    else                                                                  // Spanish
                        CombatScene.GetComponent<CombatController>().CreateSpellText("Bloqueado", CombatScene.GetComponent<CombatController>().Player);

                }
                else
                {

                    CombatScene.GetComponent<CombatController>().ManaProtagonista -= CosteMana;  // Reduce el maná del jugador
                    CombatScene.GetComponent<CombatController>().EliminarCarta(Id);
                    CombatScene.GetComponent<CombatController>().UsarCarta(Tipo, EnemigoSeleccionado);
                    EnemigoSeleccionado = -1;

                    Destroy(gameObject);

                }

            }
            else
            {
                SinEnergiaSound.Play();
                Debug.Log("SIN ENERGIA");
            }

        }

        ArrowEmitter.SetActive(false);
        CombatScene.GetComponent<CombatController>().MovingArrow = false;
        Cursor.visible = true;

    }

    private void OnMouseDown()
    {

        //if(CombatScene.GetComponent<CombatController>().ManaProtagonista > 0)
        //{

        //    MousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();

        //}

        MousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();

    }

    private void OnMouseDrag()
    {

        if (/*CombatScene.GetComponent<CombatController>().ManaProtagonista > 0 &&*/ !VariablesGlobales.GetComponent<VariablesGlobales>().EstaEnPausa)
        {

            MouseDrag = true;

            if (Tipo == 0 || Tipo == 1 || Tipo == 3 || Tipo == 4 || Tipo == 5 || Tipo == 9 || Tipo == 10 || Tipo == 13 || Tipo == 14 || Tipo == 15 || Tipo == 20 || Tipo == 21)
            {

                if (transform.position.y > -3)
                {

                    ArrowEmitter.GetComponent<ArrowEmitter>().IdCarta = Id;
                    ArrowEmitter.GetComponent<ArrowEmitter>().Carta = gameObject;
                    ArrowEmitter.GetComponent<RectTransform>().position = this.GetComponent<RectTransform>().position;
                    ArrowEmitter.SetActive(true);
                    CombatScene.GetComponent<CombatController>().MovingArrow = true;
                    Cursor.visible = false;

                }
                else
                    transform.position = GetMouseWorldPosition() + MousePositionOffset;

            }
            else
                transform.position = GetMouseWorldPosition() + MousePositionOffset;

            // Desactiva el texto explicativo de las cartas que lo necesiten
            if (Tipo == 13 || Tipo == 14 || Tipo == 15 || Tipo == 16 || Tipo == 17 || Tipo == 18 || Tipo == 19 || Tipo == 20 || Tipo == 21 || Tipo == 22)
            {

                EffectContainer.SetActive(false);
                EffectText.gameObject.SetActive(false);

            }

        }

    }

    public void SetPosition()
    {

        CardPosition = transform.position;
        CardRotation = transform.eulerAngles;

    }

    //public void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (Id == 0)
    //        Debug.Log("Colision con " + collision.gameObject.tag);
    //    if (collision.gameObject.CompareTag("DragZone"))
    //    {
    //        IsInDragZone=true;
    //    }
    //    if(!collision.gameObject.CompareTag("DragZone"))
    //    {
    //        IsInDragZone = false;
    //    }
    //}

    private void OnTriggerStay2D(Collider2D collision)
    {

        //if (Id == 0)
        //    Debug.Log("Colision con " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("DragZone"))
        {
            IsInDragZone = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("DragZone"))
        {
            IsInDragZone = false;
        }

    }

    /*
     * Segun termina la animacion de la carta se llama a esta funcion, tiene que ser un int porque Unity es una patata ¯\_(ツ)_/¯
     */
    public void SetAnimacionCarta(int valor)
    {

        //Ha terminado la animacion por lo que se desactiva el animator e impide que la animacion se repita
        if (valor == 0) // Ha terminado la animación de Enter
        {

            //AnimacionCarta = false;
            AnimacionEntrar = false;

            //CardAnimator.SetBool("AnimacionCarta", AnimacionCarta);
            CardAnimator.SetBool("AnimacionEntrar", AnimacionEntrar);
            //CardAnimator.speed *= 0;


            CardAnimator.enabled = false;
            MouseDrag = false;
            MouseOver = false;
            
            if(Id == 1)
            {

                VariablesGlobales.GetComponent<VariablesGlobales>().EstaEnPausa = false;
                CombatScene.GetComponent<CombatController>().botonTurno.enabled = true;

            }

            if(Id == 4)
                CombatScene.GetComponent<CombatController>().botonTurno.interactable = true;

        }
        if (valor == 1) // Ha terminado la animación de Exit
        {

            //AnimacionCarta = false;
            AnimacionSalir = false;

            //CardAnimator.SetBool("AnimacionCarta", AnimacionCarta);
            CardAnimator.SetBool("AnimacionSalir", AnimacionSalir);

            CombatScene.GetComponent<CombatController>().EliminarCarta(Id);
            Destroy(gameObject);                                                   //destruye la carta al colisionar con la dragzone

        }

    }

    private void SetText()
    {

        int danyo;

        // Actualiza los textos
        if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
            newText[0].text = CardTitlesEN[Tipo];
        else                                                                  // Spanish
            newText[0].text = CardTitlesES[Tipo];

        if (Tipo == 0)
        {

            danyo = (5 + Player.GetComponent<PlayerController>().Fuerza + Player.GetComponent<PlayerController>().Debilidad);
            if (danyo < 0)
                danyo = 0;
            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
            {

                if (danyo < 5)
                {

                    newText[1].text = CardDescriptionPlayerEN[Tipo];
                    newText[2].text = "<color=red>-" + danyo + "</color>";

                }
                else if (danyo > 5)
                {

                    newText[1].text = CardDescriptionPlayerEN[Tipo];
                    newText[2].text = "<color=green>-" + danyo + "</color>";

                }
                else
                {

                    newText[1].text = CardDescriptionPlayerEN[Tipo];
                    newText[2].text = "-" + danyo;

                }

            }
            else                                                                  // Spanish
            {

                if (danyo < 5)
                {

                    newText[1].text = CardDescriptionPlayerES[Tipo];
                    newText[2].text = "<color=red>-" + danyo + "</color>";

                }
                else if (danyo > 5)
                {

                    newText[1].text = CardDescriptionPlayerES[Tipo];
                    newText[2].text = "<color=green>-" + danyo + "</color>";

                }
                else
                {

                    newText[1].text = CardDescriptionPlayerES[Tipo];
                    newText[2].text = "-" + danyo;

                }

            }

        }
        else if (Tipo == 1)
        {

            danyo = (3 + Player.GetComponent<PlayerController>().Fuerza + Player.GetComponent<PlayerController>().Debilidad);
            if (danyo < 0)
                danyo = 0;
            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
            {

                if (danyo < 3)
                {

                    newText[1].text = CardDescriptionPlayerEN[Tipo];
                    newText[2].text = "x2 <color=red>-" + danyo + "</color>";

                }
                else if (danyo > 3)
                {

                    newText[1].text = CardDescriptionPlayerEN[Tipo];
                    newText[2].text = "x2 <color=green>-" + danyo + "</color>";

                }
                else
                {

                    newText[1].text = CardDescriptionPlayerEN[Tipo];
                    newText[2].text = "x2 -" + danyo;

                }

            }
            else                                                                  // Spanish
            {

                if (danyo < 3)
                {

                    newText[1].text = CardDescriptionPlayerES[Tipo];
                    newText[2].text = "x2 <color=red>-" + danyo + "</color>";

                }
                else if (danyo > 3)
                {

                    newText[1].text = CardDescriptionPlayerES[Tipo];
                    newText[2].text = "x2 <color=green>-" + danyo + "</color>";

                }
                else
                {

                    newText[1].text = CardDescriptionPlayerES[Tipo];
                    newText[2].text = "x2 -" + danyo;

                }

            }

        }
        else if(Tipo == 2)
        {

            danyo = (5 + Player.GetComponent<PlayerController>().Fuerza + Player.GetComponent<PlayerController>().Debilidad);
            if (danyo < 0)
                danyo = 0;
            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
            {

                if (danyo < 5)
                {

                    newText[1].text = CardDescriptionPlayerEN[Tipo];
                    newText[2].text = "<color=red>-" + danyo + "</color> to everyone";

                }
                else if (danyo > 5)
                {

                    newText[1].text = CardDescriptionPlayerEN[Tipo];
                    newText[2].text = "<color=green>-" + danyo + "</color> to everyone";

                }
                else
                {

                    newText[1].text = CardDescriptionPlayerEN[Tipo];
                    newText[2].text = "-" + danyo + " to everyone";

                }

            }
            else                                                                  // Spanish
            {

                if (danyo < 5)
                {

                    newText[1].text = CardDescriptionPlayerES[Tipo];
                    newText[2].text = "<color=red>-" + danyo + "</color> a todos";

                }
                else if (danyo > 5)
                {

                    newText[1].text = CardDescriptionPlayerES[Tipo];
                    newText[2].text = "<color=green>-" + danyo + "</color> a todos";

                }
                else
                {

                    newText[1].text = CardDescriptionPlayerES[Tipo];
                    newText[2].text = "-" + danyo + " a todos";

                }

            }

        }
        else if(Tipo == 3)
        {

            danyo = (10 + Player.GetComponent<PlayerController>().Fuerza + Player.GetComponent<PlayerController>().Debilidad);
            if (danyo < 0)
                danyo = 0;
            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
            {

                if (danyo < 10)
                {

                    newText[1].text = CardDescriptionPlayerEN[Tipo];
                    newText[2].text = "<color=red>-" + danyo + "</color>";

                }
                else if (danyo > 10)
                {

                    newText[1].text = CardDescriptionPlayerEN[Tipo];
                    newText[2].text = "<color=green>-" + danyo + "</color>";

                }
                else
                {

                    newText[1].text = CardDescriptionPlayerEN[Tipo];
                    newText[2].text = "-" + danyo;

                }

            }
            else                                                                  // Spanish
            {

                if (danyo < 10)
                {

                    newText[1].text = CardDescriptionPlayerES[Tipo];
                    newText[2].text = "<color=red>-" + danyo + "</color>";

                }
                else if (danyo > 10)
                {

                    newText[1].text = CardDescriptionPlayerES[Tipo];
                    newText[2].text = "<color=green>-" + danyo + "</color>";

                }
                else
                {

                    newText[1].text = CardDescriptionPlayerES[Tipo];
                    newText[2].text = "-" + danyo;

                }

            }

        }
        else if (Tipo == 4)
        {
            
            danyo = (20 + Player.GetComponent<PlayerController>().Fuerza + Player.GetComponent<PlayerController>().Debilidad);
            if (danyo < 0)
                danyo = 0;
            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
            {

                if (danyo < 20)
                {

                    newText[1].text = CardDescriptionPlayerEN[Tipo];
                    newText[2].text = "<color=red>-" + danyo + "</color>";

                }
                else if (danyo > 20)
                {

                    newText[1].text = CardDescriptionPlayerEN[Tipo];
                    newText[2].text = "<color=green>-" + danyo + "</color>";

                }
                else
                {

                    newText[1].text = CardDescriptionPlayerEN[Tipo];
                    newText[2].text = "-" + danyo;

                }

            }
            else                                                                  // Spanish
            {

                if (danyo < 20)
                {

                    newText[1].text = CardDescriptionPlayerES[Tipo];
                    newText[2].text = "<color=red>-" + danyo + "</color>";

                }
                else if (danyo > 20)
                {

                    newText[1].text = CardDescriptionPlayerES[Tipo];
                    newText[2].text = "<color=green>-" + danyo + "</color>";

                }
                else
                {

                    newText[1].text = CardDescriptionPlayerES[Tipo];
                    newText[2].text = "-" + danyo;

                }

            }

        }
        else if (Tipo == 5)
        {

            danyo = (10 + Player.GetComponent<PlayerController>().Fuerza + Player.GetComponent<PlayerController>().Debilidad);
            if (danyo < 0)
                danyo = 0;
            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
            {

                if (danyo < 10)
                {

                    newText[1].text = CardDescriptionPlayerEN[Tipo];
                    newText[2].text = "x2 <color=red>-" + danyo + "</color>";

                }
                else if (danyo > 10)
                {

                    newText[1].text = CardDescriptionPlayerEN[Tipo];
                    newText[2].text = "x2 <color=green>-" + danyo + "</color>";

                }
                else
                {

                    newText[1].text = CardDescriptionPlayerEN[Tipo];
                    newText[2].text = "x2 -" + danyo;

                }

            }
            else                                                                  // Spanish
            {

                if (danyo < 10)
                {

                    newText[1].text = CardDescriptionPlayerES[Tipo];
                    newText[2].text = "x2 <color=red>-" + danyo + "</color>";

                }
                else if (danyo > 10)
                {

                    newText[1].text = CardDescriptionPlayerES[Tipo];
                    newText[2].text = "x2 <color=green>-" + danyo + "</color>";

                }
                else
                {

                    newText[1].text = CardDescriptionPlayerES[Tipo];
                    newText[2].text = "x2 -" + danyo;

                }

            }

        }
        else if (Tipo == 9)
        {

            danyo = (5 + Player.GetComponent<PlayerController>().Fuerza + Player.GetComponent<PlayerController>().Debilidad);
            if (danyo < 0)
                danyo = 0;
            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
            {

                if (danyo < 5)
                {

                    newText[1].text = "<color=red>+" + danyo + "</color>";
                    newText[2].text = "<color=red>-" + danyo + "</color>";

                }
                else if (danyo > 5)
                {

                    newText[1].text = "<color=green>+" + danyo + "</color>";
                    newText[2].text = "<color=green>-" + danyo + "</color>";

                }
                else
                {

                    newText[1].text = "+" + danyo;
                    newText[2].text = "-" + danyo;

                }

            }
            else                                                                  // Spanish
            {

                if (danyo < 5)
                {

                    newText[1].text = "<color=red>+" + danyo + "</color>";
                    newText[2].text = "<color=red>-" + danyo + "</color>";

                }
                else if (danyo > 5)
                {

                    newText[1].text = "<color=green>+" + danyo + "</color>";
                    newText[2].text = "<color=green>-" + danyo + "</color>";

                }
                else
                {

                    newText[1].text = "+" + danyo;
                    newText[2].text = "-" + danyo;

                }

            }

        }
        else if (Tipo == 10)
        {

            danyo = (10 + Player.GetComponent<PlayerController>().Fuerza + Player.GetComponent<PlayerController>().Debilidad);
            if (danyo < 0)
                danyo = 0;
            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
            {

                if (danyo < 10)
                {

                    newText[1].text = "<color=red>+" + danyo + "</color>";
                    newText[2].text = "<color=red>+" + danyo + "</color>";

                }
                else if (danyo > 10)
                {

                    newText[1].text = "<color=green>-" + danyo + "</color>";
                    newText[2].text = "<color=green>-" + danyo + "</color>";

                }
                else
                {

                    newText[1].text = "+" + danyo;
                    newText[2].text = "-" + danyo;

                }

            }
            else                                                                  // Spanish
            {

                if (danyo < 10)
                {

                    newText[1].text = "<color=red>+" + danyo + "</color>";
                    newText[2].text = "<color=red>+" + danyo + "</color>";

                }
                else if (danyo > 10)
                {

                    newText[1].text = "<color=green>-" + danyo + "</color>";
                    newText[2].text = "<color=green>-" + danyo + "</color>";

                }
                else
                {

                    newText[1].text = "+" + danyo;
                    newText[2].text = "-" + danyo;

                }

            }

        }
        else if (Tipo == 11)
        {

            danyo = (5 + Player.GetComponent<PlayerController>().Fuerza + Player.GetComponent<PlayerController>().Debilidad);
            if (danyo < 0)
                danyo = 0;
            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
            {

                if (danyo < 5)
                {

                    newText[1].text = "x" + numEnemies + " <color=red>+" + danyo + "</color>";
                    newText[2].text = "<color=red>-" + danyo + "</color> to everyone";

                }
                else if (danyo > 5)
                {

                    newText[1].text = "x" + numEnemies + " <color=green>+" + danyo + "</color>";
                    newText[2].text = "<color=green>-" + danyo + "</color> to everyone";

                }
                else
                {

                    newText[1].text = "x" + numEnemies + " +" + danyo;
                    newText[2].text = "-" + danyo + " to everyone";

                }

            }
            else                                                                  // Spanish
            {

                if (danyo < 5)
                {

                    newText[1].text = "x" + numEnemies + " <color=red>+" + danyo + "</color>";
                    newText[2].text = "<color=red>-" + danyo + "</color> a todos";

                }
                else if (danyo > 5)
                {

                    newText[1].text = "x" + numEnemies + " <color=green>+" + danyo + "</color>";
                    newText[2].text = "<color=green>-" + danyo + "</color> a todos";

                }
                else
                {

                    newText[1].text = "x" + numEnemies + " +" + danyo;
                    newText[2].text = "-" + danyo + " a todos";

                }

            }

        }
        else if (Tipo == 12)
        {

            danyo = (10 + Player.GetComponent<PlayerController>().Fuerza + Player.GetComponent<PlayerController>().Debilidad);
            if (danyo < 0)
                danyo = 0;
            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
            {

                if (danyo < 10)
                {

                    newText[1].text = "x" + numEnemies + " <color=red>+" + danyo + "</color>";
                    newText[2].text = "<color=red>-" + danyo + "</color> to everyone";

                }
                else if (danyo > 10)
                {

                    newText[1].text = "x" + numEnemies + " <color=green>+" + danyo + "</color>";
                    newText[2].text = "<color=green>-" + danyo + "</color> to everyone";

                }
                else
                {

                    newText[1].text = "x" + numEnemies + " +" + danyo;
                    newText[2].text = "-" + danyo + " to everyone";

                }

            }
            else                                                                  // Spanish
            {

                if (danyo < 10)
                {

                    newText[1].text = "x" + numEnemies + " <color=red>+" + danyo + "</color>";
                    newText[2].text = "<color=red>-" + danyo + "</color> a todos";

                }
                else if (danyo > 10)
                {

                    newText[1].text = "x" + numEnemies + " <color=green>+" + danyo + "</color>";
                    newText[2].text = "<color=green>-" + danyo + "</color> a todos";

                }
                else
                {

                    newText[1].text = "x" + numEnemies + " +" + danyo;
                    newText[2].text = "-" + danyo + " a todos";

                }

            }

        }
        else if (Tipo == 13 || Tipo == 14)
        {

            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
            {

                newText[1].text = CardDescriptionPlayerEN[Tipo];
                newText[2].text = CardDescriptionEnemiesEN[Tipo];
                EffectText.text = CardEffectDescriptionEN[0];

            }
            else                                                                  // Spanish
            {

                newText[1].text = CardDescriptionPlayerES[Tipo];
                newText[2].text = CardDescriptionEnemiesES[Tipo];
                EffectText.text = CardEffectDescriptionES[0];

            }

        }
        else if (Tipo == 15 || Tipo == 16 || Tipo == 21)
        {

            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
            {

                newText[1].text = CardDescriptionPlayerEN[Tipo];
                newText[2].text = CardDescriptionEnemiesEN[Tipo];
                EffectText.text = CardEffectDescriptionEN[1];

            }
            else                                                                  // Spanish
            {

                newText[1].text = CardDescriptionPlayerES[Tipo];
                newText[2].text = CardDescriptionEnemiesES[Tipo];
                EffectText.text = CardEffectDescriptionES[1];

            }

        }
        else if (Tipo == 17 || Tipo == 18)
        {

            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
            {

                newText[1].text = CardDescriptionPlayerEN[Tipo];
                newText[2].text = CardDescriptionEnemiesEN[Tipo];
                EffectText.text = CardEffectDescriptionEN[2];

            }
            else                                                                  // Spanish
            {

                newText[1].text = CardDescriptionPlayerES[Tipo];
                newText[2].text = CardDescriptionEnemiesES[Tipo];
                EffectText.text = CardEffectDescriptionES[2];

            }

        }
        else if (Tipo == 19)
        {

            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
            {

                newText[1].text = CardDescriptionPlayerEN[Tipo];
                newText[2].text = CardDescriptionEnemiesEN[Tipo];
                EffectText.text = CardEffectDescriptionEN[3];

            }
            else                                                                  // Spanish
            {

                newText[1].text = CardDescriptionPlayerES[Tipo];
                newText[2].text = CardDescriptionEnemiesES[Tipo];
                EffectText.text = CardEffectDescriptionES[3];

            }

        }
        else if (Tipo == 20)
        {

            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
            {

                newText[1].text = CardDescriptionPlayerEN[Tipo];
                newText[2].text = CardDescriptionEnemiesEN[Tipo];
                EffectText.text = CardEffectDescriptionEN[4];

            }
            else                                                                  // Spanish
            {

                newText[1].text = CardDescriptionPlayerES[Tipo];
                newText[2].text = CardDescriptionEnemiesES[Tipo];
                EffectText.text = CardEffectDescriptionES[4];

            }

        }
        else if (Tipo == 22)
        {

            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
            {

                newText[1].text = CardDescriptionPlayerEN[Tipo];
                newText[2].text = CardDescriptionEnemiesEN[Tipo];
                EffectText.text = CardEffectDescriptionEN[5];

            }
            else                                                                  // Spanish
            {

                newText[1].text = CardDescriptionPlayerES[Tipo];
                newText[2].text = CardDescriptionEnemiesES[Tipo];
                EffectText.text = CardEffectDescriptionES[5];

            }

        }
        else
        {

            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
            {

                newText[1].text = CardDescriptionPlayerEN[Tipo];
                newText[2].text = CardDescriptionEnemiesEN[Tipo];

            }
            else                                                                  // Spanish
            {

                newText[1].text = CardDescriptionPlayerES[Tipo];
                newText[2].text = CardDescriptionEnemiesES[Tipo];

            }
                

        }

        if (VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[Tipo] == VariablesGlobales.GetComponent<VariablesGlobales>().CardCostOriginal[Tipo])
            newText[3].text = "" + VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[Tipo];
        else if (VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[Tipo] < VariablesGlobales.GetComponent<VariablesGlobales>().CardCostOriginal[Tipo])
            newText[3].text = "<color=green>" + VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[Tipo] + "</color>";
        else
            newText[3].text = "<color=red>" + VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[Tipo] + "</color>";
        newText[4].text = CardDuration[Tipo];

    }

    public void setColor_text()
    {
        //Color colorPersonalizado;

        if (Tipo == 0)
        {
            //colorPersonalizado = new Color(0.55f, 0.61f, 0.69f, 1.0f);
            //gameObject.GetComponent<SpriteRenderer>().color = colorPersonalizado;
            //TextMeshProUGUI textCharacter = gameObject.GetComponent<TextMeshProUGUI>();
            //textCharacter.text = "5 ATQ";
            //gameObject.GetComponent<SpriteRenderer>().sprite = danyo5;

        }
        else if (Tipo == 1)
        {
            //colorPersonalizado = new Color(0.27f, 0.36f, 0.44f, 1.0f);
            //gameObject.GetComponent<SpriteRenderer>().color = colorPersonalizado;
            //TextMeshProUGUI textCharacter = gameObject.GetComponent<TextMeshProUGUI>();
            //textCharacter.text = "10 ATQ";
            //gameObject.GetComponent<SpriteRenderer>().sprite = danyo10;
        }
        else
        {
            //colorPersonalizado = new Color(0.83f, 0.75f, 0.86f, 1.0f);
            //gameObject.GetComponent<SpriteRenderer>().color = colorPersonalizado;
            //TextMeshProUGUI textCharacter = gameObject.GetComponent<TextMeshProUGUI>();
            //textCharacter.text = "+10 HP";
            //gameObject.GetComponent<SpriteRenderer>().sprite = curar10;
        }
    }

    //public void AnimacionCarta()
    //{

    //    elapsedTime += Time.deltaTime;
    //    float percentageComplete = elapsedTime / desireDuration;
    //    Vector3 initialPosition;
    //    Vector3 initialAngles;

    //    if(AnimacionEntrar)
    //    {

    //        if (Id == 0)
    //        {

    //            initialPosition = transform.position;
    //            initialAngles = transform.eulerAngles;

    //            transform.position = Vector3.Lerp(initialPosition, new Vector3(3.95f, -4.7f, 0), percentageComplete);
    //            transform.eulerAngles = Vector3.Lerp(initialAngles, new Vector3(0, 0, 340), percentageComplete);

    //            if (Vector3.Equals(transform.position, new Vector3(3.95f, -4.7f, 0)))
    //            {

    //                AnimacionEntrar = false;
    //                MouseDrag = false;
    //                MouseOver = false;

    //            }

    //        }
    //        else if(Id == 1)
    //        {

    //            initialPosition = transform.position;
    //            initialAngles = transform.eulerAngles;

    //            transform.position = Vector3.Lerp(initialPosition, new Vector3(2, -4.15f, 1), percentageComplete);
    //            transform.eulerAngles = Vector3.Lerp(initialAngles, new Vector3(0, 0, 350), percentageComplete);

    //            if (Vector3.Equals(transform.position, new Vector3(2, -4.15f, 1)))
    //            {

    //                AnimacionEntrar = false;
    //                MouseDrag = false;
    //                MouseOver = false;

    //            }

    //        }
    //        else if(Id == 2)
    //        {

    //            initialPosition = transform.position;
    //            initialAngles = transform.eulerAngles;

    //            transform.position = Vector3.Lerp(initialPosition, new Vector3(0, -4, 2), percentageComplete);
    //            transform.eulerAngles = Vector3.Lerp(initialAngles, new Vector3(0, 0, 0), percentageComplete);

    //            if (Vector3.Equals(transform.position, new Vector3(0, -4, 2)))
    //            {

    //                AnimacionEntrar = false;
    //                MouseDrag = false;
    //                MouseOver = false;

    //            }

    //        }
    //        else if (Id == 3)
    //        {

    //            initialPosition = transform.position;
    //            initialAngles = transform.eulerAngles;

    //            transform.position = Vector3.Lerp(initialPosition, new Vector3(-2, -4.15f, 3), percentageComplete);
    //            transform.eulerAngles = Vector3.Lerp(initialAngles, new Vector3(0, 0, 10), percentageComplete);

    //            if (Vector3.Equals(transform.position, new Vector3(-2, -4.15f, 3)))
    //            {

    //                AnimacionEntrar = false;
    //                MouseDrag = false;
    //                MouseOver = false;

    //            }

    //        }
    //        else
    //        {

    //            initialPosition = transform.position;
    //            initialAngles = transform.eulerAngles;

    //            transform.position = Vector3.Lerp(initialPosition, new Vector3(-3.95f, -4.7f, 4), percentageComplete);
    //            transform.eulerAngles = Vector3.Lerp(initialAngles, new Vector3(0, 0, 20), percentageComplete);

    //            if (Vector3.Equals(transform.position, new Vector3(-3.95f, -4.7f, 4)))
    //            {

    //                AnimacionEntrar = false;
    //                MouseDrag = false;
    //                MouseOver = false;

    //            }

    //        }

    //    }

    //    if(AnimacionSalir)
    //    {

    //        initialPosition = transform.position;
    //        initialAngles = transform.eulerAngles;

    //        transform.position = Vector3.Lerp(initialPosition, new Vector3(7.5f, -3f, 0), percentageComplete);
    //        transform.eulerAngles = Vector3.Lerp(initialAngles, new Vector3(0, 0, 0), percentageComplete);

    //        if (Vector3.Equals(transform.position, new Vector3(7.5f, -3f, 0)))
    //        {

    //            CombatScene.GetComponent<CombatController>().EliminarCarta(Id);
    //            Destroy(gameObject);

    //        }

    //    }

    //}

}
