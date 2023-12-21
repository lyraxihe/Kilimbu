using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public GameObject VariablesGlobales;
    [SerializeField] TMP_Text Dinero_text;

    void Start()
    {
        VariablesGlobales = GameObject.Find("VariablesGlobales");
    }

    void Update()
    {
        Dinero_text.text = "Dinero: " + VariablesGlobales.GetComponent<VariablesGlobales>().DineroAmount;
    }
}
