using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour
{
   // public List<int> CardPrecio = new List<int>() { 10, 10, 15, 15, 20, 20, 20, 10, 15, 15, 20, 20, 30, 10, 5, 10, 15, 10, 5, 15, 10, 5, 25, 20 };
    public GameObject VariablesGlobales;
    public int ID;
    public int Precio;
    [SerializeField] bool EsMana;
    [SerializeField] bool EsVida;
    [SerializeField] RectTransform canvas;
    [SerializeField] GameObject FeedbackText;

    // SoundFX management
    public AudioSource AnyadirCartaSound;
    public AudioSource ButtonHoverSound;




    void Start()
    {
        VariablesGlobales = GameObject.Find("VariablesGlobales");
        if (EsMana)
        {
            if (VariablesGlobales.GetComponent<VariablesGlobales>().MaxManaProtagonista == 3)
            {
                Precio = 40;
                gameObject.GetComponentInChildren<TMP_Text>().text = Precio.ToString() + "xp";
            }
           
            else if (VariablesGlobales.GetComponent<VariablesGlobales>().MaxManaProtagonista == 4)
            {
                Precio = 70;
                gameObject.GetComponentInChildren<TMP_Text>().text = Precio.ToString() + "xp";
            }
            
            else
            {
                gameObject.GetComponent<Button>().interactable = false;
                if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                    gameObject.GetComponentInChildren<TMP_Text>().text = "Mana limit";
                else                                                                   // Spanish
                    gameObject.GetComponentInChildren<TMP_Text>().text = "L�mite man�";

            }
            

           

        }
        else if (EsVida)
        {
            Precio = 15;
            if (VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista == VariablesGlobales.GetComponent<VariablesGlobales>().MaxHealthProtagonista)
                gameObject.GetComponent<Button>().interactable = false;
        }
        //VariablesGlobales.GetComponent<VariablesGlobales>().AmountCards[cardType].ToString();

        // Encontrar efecto de sonido para asignarlo en el bot�n cada vez que se hace click
        AnyadirCartaSound = GameObject.Find("AnyadirCarta_SoundFX").GetComponent<AudioSource>();
        ButtonHoverSound = GameObject.Find("ButtonHover_SoundFX").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
       
        if (VariablesGlobales.GetComponent<VariablesGlobales>().DineroAmount < Precio)
            gameObject.GetComponent<Button>().interactable = false;

    }

    public void OnClick()
    {
        if (EsMana)
        {
            if (VariablesGlobales.GetComponent<VariablesGlobales>().DineroAmount >= Precio)
            {
                VariablesGlobales.GetComponent<VariablesGlobales>().DineroAmount -= Precio;
                VariablesGlobales.GetComponent<VariablesGlobales>().MaxManaProtagonista++;
                gameObject.GetComponent<Button>().interactable = false;
                gameObject.GetComponentInChildren<TMP_Text>().text = "X";
                //gameObject.GetComponent<TMP_Text>().text = "X";                                                                REVISAR POR ERROR?
                if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                    CreateFeedbackText("+1 mana");
                else                                                                   // Spanish
                    CreateFeedbackText("+1 de man�");
                Debug.Log("se aumento el mana");
            }
          
        }
        else if (EsVida)
        {
            if (VariablesGlobales.GetComponent<VariablesGlobales>().DineroAmount >= Precio)
            {
                VariablesGlobales.GetComponent<VariablesGlobales>().DineroAmount -= Precio;
                VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += 10;
                if (VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista == VariablesGlobales.GetComponent<VariablesGlobales>().MaxHealthProtagonista)
                {
                    gameObject.GetComponent<Button>().interactable = false;
                }
                if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                    CreateFeedbackText("+10 health");
                else                                                                   // Spanish
                    CreateFeedbackText("+10 de vida");

            }
            

        }
        else
        {
            if (VariablesGlobales.GetComponent<VariablesGlobales>().DineroAmount >= Precio)
            {
                VariablesGlobales.GetComponent<VariablesGlobales>().DineroAmount -= Precio;
                VariablesGlobales.GetComponent<VariablesGlobales>().AmountCards[ID]++;
                if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                    CreateFeedbackText("+1 card to the deck");
                else                                                                   // Spanish
                    CreateFeedbackText("+1 carta al mazo");

                if (ID==6) //Si es la carta de man� solo te deja comprarla una vez
                {
                    gameObject.GetComponent<Button>().interactable = false;
                    gameObject.GetComponentInChildren<TMP_Text>().text = "X";
                    // gameObject.GetComponent<TMP_Text>().text = "X";
                }
            }
                  
        }
       
    }

    public void CreateFeedbackText(string feedback)
    {
        GameObject Text = Instantiate(FeedbackText);
        Text.GetComponent<CompraText>().text = feedback;
        Text.transform.SetParent(canvas, false);
        if (EsMana || EsVida)
        {
            Text.GetComponent<CompraText>().ManaHeal = true;
            Text.transform.position = new Vector2(gameObject.transform.position.x + 2, gameObject.transform.position.y + 1);
        }
        else
        {
            Text.GetComponent<CompraText>().ManaHeal = false;
            Text.transform.position = new Vector2(gameObject.transform.position.x + 0.5f, gameObject.transform.position.y + 0.5f);
        }
        
    }

    public void PlayAnyadirCartaSound()
    {
        AnyadirCartaSound.Play();
    }

    public void PlayButtonHoverSound()
    {
        ButtonHoverSound.Play();
    }

}
