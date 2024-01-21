using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour
{
    public List<int> CardPrecio = new List<int>() { 10, 10, 15, 15, 20, 20, 5, 10, 15, 15, 20, 20, 30, 10, 5, 10, 15, 10, 5, 15, 10, 5, 25, 20 };
    public GameObject VariablesGlobales;
    public int ID;
    public int Precio;
    [SerializeField] bool EsMana;
    [SerializeField] bool EsVida;
    void Start()
    {
        VariablesGlobales = GameObject.Find("VariablesGlobales");
        if (EsMana)
        {
            if (VariablesGlobales.GetComponent<VariablesGlobales>().MaxManaProtagonista == 3)
            Precio = 40;
            else if (VariablesGlobales.GetComponent<VariablesGlobales>().MaxManaProtagonista == 4)
            Precio = 70;
            else
            gameObject.GetComponent<Button>().interactable = false;

            gameObject.GetComponentInChildren<TMP_Text>().text = "$" + Precio.ToString();

        }
        else if (EsVida)
        {
            Precio = 15;
            if (VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista == VariablesGlobales.GetComponent<VariablesGlobales>().MaxHealthProtagonista)
                gameObject.GetComponent<Button>().interactable = false;
        }
        //VariablesGlobales.GetComponent<VariablesGlobales>().AmountCards[cardType].ToString();
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
                   
            }
            

        }
        else
        {
            if (VariablesGlobales.GetComponent<VariablesGlobales>().DineroAmount >= Precio)
            {
                VariablesGlobales.GetComponent<VariablesGlobales>().DineroAmount -= Precio;
                VariablesGlobales.GetComponent<VariablesGlobales>().AmountCards[ID]++;
            }
        }
       
    }

}
