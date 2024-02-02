using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LanguageDropdown : MonoBehaviour
{

    [SerializeField] GameObject VariablesGlobales;

    // Start is called before the first frame update
    void Start()
    {

        VariablesGlobales = GameObject.Find("VariablesGlobales");
        gameObject.GetComponent<TMP_Dropdown>().value = VariablesGlobales.GetComponent<VariablesGlobales>().Language;

    }

    // Update is called once per frame
    void Update()
    {

        VariablesGlobales.GetComponent<VariablesGlobales>().Language = gameObject.GetComponent<TMP_Dropdown>().value;

    }
}
