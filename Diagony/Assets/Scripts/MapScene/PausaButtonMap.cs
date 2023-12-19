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
    [SerializeField] GameObject ScrollArea;

    private ScrollRect DesplazamientoMapa;

    TMP_Text textButton;

    void Start()
    {
        VariablesGlobales = GameObject.Find("VariablesGlobales");
        textButton = gameObject.GetComponentInChildren<TextMeshPro>();
        DesplazamientoMapa = ScrollArea.GetComponent<ScrollRect>();
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
            DesplazamientoMapa.enabled = false;

        }
        else
        {

            PanelPausa.SetActive(false);
            Time.timeScale = 1f;
            VariablesGlobales.GetComponent<VariablesGlobales>().EstaEnPausa = false;
            DesplazamientoMapa.enabled = true;

        }

    }

    public void Escapar()
    {


        VariablesGlobales.GetComponent<VariablesGlobales>().EstaEnPausa = false;
        DesplazamientoMapa.enabled = true;
        SceneManager.LoadScene("MainMenu");

    }

}
