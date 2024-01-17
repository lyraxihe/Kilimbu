using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausaButtonMap : MonoBehaviour
{
    [SerializeField] GameObject PanelPausa;
    [SerializeField] GameObject VariablesGlobales;

    TMP_Text textButton;

    void Start()
    {
        VariablesGlobales = GameObject.Find("VariablesGlobales");
        textButton = gameObject.GetComponentInChildren<TextMeshPro>();
    }


    void Update()
    {

    }

    public void OnClick()
    {

        if (!VariablesGlobales.GetComponent<VariablesGlobales>().EstaEnPausa)
        {

            PanelPausa.SetActive(true);
            Time.timeScale = 0f;
            VariablesGlobales.GetComponent<VariablesGlobales>().EstaEnPausa = true;

        }
        else
        {

            PanelPausa.SetActive(false);
            Time.timeScale = 1f;
            VariablesGlobales.GetComponent<VariablesGlobales>().EstaEnPausa = false;

        }

    }

    public void Escapar()
    {


        VariablesGlobales.GetComponent<VariablesGlobales>().EstaEnPausa = false;
        SceneManager.LoadScene("MainMenu");

    }

}
