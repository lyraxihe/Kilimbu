using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class ChestCard : MonoBehaviour
{
    public GameObject VariablesGlobales;
    public GameObject ChestScene;
    public GameObject CardMana;

    public GameObject CanvasCards;
    public GameObject CanvasMana;
    public List<GameObject> CardsMana;
    public List<int> ListCardsMana = new List<int>();
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

    List<string> CardDuration = new List<string>() { "", "", "", "", "", "", "", "", "", "", "", "", "", "1", "1", "3", "2", "4", "4", "4", "3", "3", "1", "0" };

    public Animator CardAnimator;

    RectTransform CardPosition;

    public int Id;
    public int Tipo;
    bool AnimationEnd;

    public bool MouseOver;

    bool Abajo;
    public bool ElegibleMana;
    public int RandTipo;

    TMP_Text[] newText;

    // SoundFX Management
    public AudioSource ElegirCartaSound;
    public AudioSource MejoraCofreSound;
    public AudioSource CurarProtaSound;




    void Start()
    {
        
        CardAnimator = GetComponent<Animator>();

        CardPosition = GetComponent<RectTransform>();

        AnimationEnd = false;
        CardAnimator.SetInteger("CardID", Id);
        CardAnimator.SetBool("AnimacionEntrar", true);

        MouseOver = false;
        Abajo = true;

        // Get SounFx's
        ElegirCartaSound = GameObject.Find("ElegirCartaCofre_SoundFX").GetComponent<AudioSource>();
        MejoraCofreSound = GameObject.Find("MejoraCofre_SoundFX").GetComponent<AudioSource>();
        CurarProtaSound = GameObject.Find("CurarProta_SoundFX").GetComponent<AudioSource>();
    }

    void Update()
    {
        // Flotar
        if (AnimationEnd)
        {

            if(Abajo)
            {

                CardPosition.anchoredPosition = Vector2.Lerp(CardPosition.anchoredPosition, new Vector2(CardPosition.anchoredPosition.x, 0), 0.0001f);
                if (Vector2.Distance(CardPosition.anchoredPosition, new Vector2(CardPosition.anchoredPosition.x, 40)) < 2)
                    Abajo = false;

            }   
            else
            {

                CardPosition.anchoredPosition = Vector2.Lerp(CardPosition.anchoredPosition, new Vector2(CardPosition.anchoredPosition.x, 100), 0.0001f);
                if (Vector2.Distance(CardPosition.anchoredPosition, new Vector2(CardPosition.anchoredPosition.x, 60)) < 2)
                    Abajo = true;

            }


        }

    }

    private void OnMouseOver()
    {

        if(AnimationEnd)
        {

            MouseOver = true;
            transform.localScale = new Vector3(2, 2, 2);

        }

    }

    private void OnMouseExit()
    {

        if(AnimationEnd)
        {

            MouseOver = false;
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        }

    }

    private void OnMouseDown()
    {

        int rand = -1;

        if(ElegibleMana)
        {

            VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[RandTipo] -= 1;

            CanvasMana.SetActive(false);
            CanvasCards.SetActive(true);

            CardMana.GetComponent<ChestCard>().ChestScene.GetComponent<ChestController>().CardSelected = true;
            CardMana.GetComponent<ChestCard>().CardAnimator.enabled = true;
            CardMana.GetComponent<ChestCard>().CardAnimator.SetBool("Selected", true);

            MejoraCofreSound.Play();

        }
        else
        {

            if (AnimationEnd && !ChestScene.GetComponent<ChestController>().CardSelected)
            {

                if (Tipo == 3)
                {

                    MouseOver = false;
                    transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                    CanvasCards.SetActive(false);

                    ElegirCartaSound.Play();

                    for (int i = 0; i < 3; i++)
                    {

                        do
                        {
                            
                            rand = Random.Range(0, 24);

                        } while (ListCardsMana.Contains(rand) || VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[rand] <= 0 || VariablesGlobales.GetComponent<VariablesGlobales>().AmountCards[rand] == 0);

                        ListCardsMana.Add(rand);

                        //Implementa los sprites
                        //if (rand <= 5)
                        //    CardsMana[i].GetComponent<Image>().sprite = CardSprites[0];
                        //else if (rand <= 12)
                        //    CardsMana[i].GetComponent<Image>().sprite = CardSprites[1];
                        //else
                        //    CardsMana[i].GetComponent<Image>().sprite = CardSprites[2];
                        CardsMana[i].GetComponent<Image>().sprite = CardSprites[rand];

                        // Actualiza los textos
                        CardsMana[i].GetComponent<ChestCard>().newText = CardsMana[i].GetComponentsInChildren<TMP_Text>();
                        if (!VariablesGlobales.GetComponent<VariablesGlobales>().PasivaGanarDinero)
                        {

                            CardsMana[i].GetComponent<ChestCard>().newText[0].text = CardTitlesEN[rand];
                            CardsMana[i].GetComponent<ChestCard>().newText[1].text = CardDescriptionPlayerEN[rand];
                            CardsMana[i].GetComponent<ChestCard>().newText[2].text = CardDescriptionEnemiesEN[rand];

                        }
                        else
                        {

                            CardsMana[i].GetComponent<ChestCard>().newText[0].text = CardTitlesES[rand];
                            CardsMana[i].GetComponent<ChestCard>().newText[1].text = CardDescriptionPlayerES[rand];
                            CardsMana[i].GetComponent<ChestCard>().newText[2].text = CardDescriptionEnemiesES[rand];

                        }
                        //CardsMana[i].GetComponent<ChestCard>().newText[2].text = "" + VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[rand];

                        if (VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[rand] == VariablesGlobales.GetComponent<VariablesGlobales>().CardCostOriginal[rand])
                            CardsMana[i].GetComponent<ChestCard>().newText[3].text = "" + VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[rand];
                        else if (VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[rand] < VariablesGlobales.GetComponent<VariablesGlobales>().CardCostOriginal[rand])
                            CardsMana[i].GetComponent<ChestCard>().newText[3].text = "<color=green>" + VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[rand] + "</color>";
                        else
                            CardsMana[i].GetComponent<ChestCard>().newText[3].text = "<color=red>" + VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[rand] + "</color>";

                        CardsMana[i].GetComponent<ChestCard>().newText[4].text = CardDuration[rand];

                        CardsMana[i].GetComponent<ChestCard>().ElegibleMana = true;
                        CardsMana[i].GetComponent<ChestCard>().CardMana = gameObject;
                        CardsMana[i].GetComponent<ChestCard>().VariablesGlobales = GameObject.Find("VariablesGlobales");
                        CardsMana[i].GetComponent<ChestCard>().RandTipo = rand;

                    }

                    CanvasMana.SetActive(true);

                }
                else
                {

                    if (Tipo == 0)
                    {

                        VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista = VariablesGlobales.GetComponent<VariablesGlobales>().MaxHealthProtagonista;

                        CurarProtaSound.Play();

                    }
                    else if (Tipo == 1)
                    {

                        VariablesGlobales.GetComponent<VariablesGlobales>().MaxHealthProtagonista += 10;
                        VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += 10;

                        MejoraCofreSound.Play();

                    }
                    else if (Tipo == 2)
                    {

                        VariablesGlobales.GetComponent<VariablesGlobales>().PasivaGanarDinero = true;
                        VariablesGlobales.GetComponent<VariablesGlobales>().PasivaDinero += 10;

                        MejoraCofreSound.Play();

                    }
                    else
                    {

                        VariablesGlobales.GetComponent<VariablesGlobales>().PasivaCurarseCombate = true;
                        VariablesGlobales.GetComponent<VariablesGlobales>().PasivaCuracionCombate += 5;

                        MejoraCofreSound.Play();

                    }

                    ChestScene.GetComponent<ChestController>().CardSelected = true;
                    CardAnimator.enabled = true;
                    CardAnimator.SetBool("Selected", true);

                }

            }

        }

    }

    private void ControlAnimation(int valor)
    {

        if (valor == 0) // Ha terminado la animación de entrada
        {

            CardAnimator.SetBool("AnimacionEntrar", false);
            CardAnimator.enabled = false;

            if(Id == 2)
            {

                for(int i = 0; i < ChestScene.GetComponent<ChestController>().ListCards.Count; i++)
                    ChestScene.GetComponent<ChestController>().ListCards[i].GetComponent<ChestCard>().AnimationEnd = true;

            }

        }
        else if (valor == 1) // Ha terminado la animación de salida de la carta seleccionada
        {

            ChestScene.GetComponent<ChestController>().ListCards.Remove(gameObject);

            for (int i = 0; i < ChestScene.GetComponent<ChestController>().ListCards.Count; i++)
            {

                ChestScene.GetComponent<ChestController>().ListCards[i].GetComponent<ChestCard>().CardAnimator.enabled = true;
                ChestScene.GetComponent<ChestController>().ListCards[i].GetComponent<ChestCard>().CardAnimator.SetBool("AnimacionSalir", true);

            }

            Destroy(gameObject);

        }
        else // Ha terminado la animación de salida de la carta no seleccionada
        {

            ChestScene.GetComponent<ChestController>().Exit = true;
            ChestScene.GetComponent<ChestController>().ListCards.Remove(gameObject);
            Destroy(gameObject);

        }

    }

}
