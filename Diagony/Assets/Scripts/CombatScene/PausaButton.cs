using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausaButton : MonoBehaviour
{
    [SerializeField] GameObject PanelPausa;
    [SerializeField] GameObject VariablesGlobales_;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {

        if (!VariablesGlobales_.GetComponent<VariablesGlobales>().EstaEnPausa)
        {

            PanelPausa.SetActive(true);
            VariablesGlobales_.GetComponent<VariablesGlobales>().EstaEnPausa = true;


        }
        else
        {

            PanelPausa.SetActive(false);
            VariablesGlobales_.GetComponent<VariablesGlobales>().EstaEnPausa = false;

        }

    }

    public void Escapar()
    {

        SceneManager.LoadScene("MainMenu");

    }
}
