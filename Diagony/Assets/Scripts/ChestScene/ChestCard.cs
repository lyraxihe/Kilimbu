using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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

    void Start()
    {
        
        CardAnimator = GetComponent<Animator>();

        CardPosition = GetComponent<RectTransform>();

        AnimationEnd = false;
        CardAnimator.SetInteger("CardID", Id);
        CardAnimator.SetBool("AnimacionEntrar", true);

        MouseOver = false;
        Abajo = true;

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

                    for (int i = 0; i < 3; i++)
                    {

                        do
                        {
                            
                            rand = Random.Range(0, 24);

                        } while (ListCardsMana.Contains(rand) || VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[rand] <= 0);

                        ListCardsMana.Add(rand);

                        //Implementa los sprites
                        if (rand <= 5)
                            CardsMana[i].GetComponent<Image>().sprite = CardSprites[0];
                        else if (rand <= 12)
                            CardsMana[i].GetComponent<Image>().sprite = CardSprites[1];
                        else
                            CardsMana[i].GetComponent<Image>().sprite = CardSprites[2];

                        // Actualiza los textos
                        CardsMana[i].GetComponent<ChestCard>().newText = CardsMana[i].GetComponentsInChildren<TMP_Text>();
                        CardsMana[i].GetComponent<ChestCard>().newText[0].text = CardTitles[rand];
                        CardsMana[i].GetComponent<ChestCard>().newText[1].text = CardDescriptions[rand];
                        //CardsMana[i].GetComponent<ChestCard>().newText[2].text = "" + VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[rand];

                        if (VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[rand] == VariablesGlobales.GetComponent<VariablesGlobales>().CardCostOriginal[rand])
                            CardsMana[i].GetComponent<ChestCard>().newText[2].text = "" + VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[rand];
                        else if (VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[rand] < VariablesGlobales.GetComponent<VariablesGlobales>().CardCostOriginal[rand])
                            CardsMana[i].GetComponent<ChestCard>().newText[2].text = "<color=green>" + VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[rand] + "</color>";
                        else
                            CardsMana[i].GetComponent<ChestCard>().newText[2].text = "<color=red>" + VariablesGlobales.GetComponent<VariablesGlobales>().CardCost[rand] + "</color>";

                        CardsMana[i].GetComponent<ChestCard>().newText[3].text = CardDuration[rand];

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
                        VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista = VariablesGlobales.GetComponent<VariablesGlobales>().MaxHealthProtagonista;
                    else if (Tipo == 1)
                        VariablesGlobales.GetComponent<VariablesGlobales>().MaxHealthProtagonista += 10;
                    else if (Tipo == 2)
                    {

                        VariablesGlobales.GetComponent<VariablesGlobales>().PasivaGanarDinero = true;
                        VariablesGlobales.GetComponent<VariablesGlobales>().PasivaDinero += 10;

                    }
                    else
                    {

                        VariablesGlobales.GetComponent<VariablesGlobales>().PasivaCurarseCombate = true;
                        VariablesGlobales.GetComponent<VariablesGlobales>().PasivaCuracionCombate += 5;

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
