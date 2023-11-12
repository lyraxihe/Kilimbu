using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausaButton : MonoBehaviour
{
    [SerializeField] GameObject PanelPausa;
    [SerializeField] GameObject VariablesGlobales;
    [SerializeField] GameObject CombatScene;
    void Start()
    {
        VariablesGlobales = GameObject.Find("VariablesGlobales");
    }

    // Update is called once per frame
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
            CombatScene.GetComponent<CombatController>().botonTurno.enabled = false;

        }
        else
        {

            PanelPausa.SetActive(false);
            Time.timeScale = 1f;
            VariablesGlobales.GetComponent<VariablesGlobales>().EstaEnPausa = false;
            CombatScene.GetComponent<CombatController>().botonTurno.enabled = true;

        }

    }

    public void Escapar()
    {

        VariablesGlobales.GetComponent<VariablesGlobales>().EstaEnPausa = false;
        CombatScene.GetComponent<CombatController>().botonTurno.enabled = true;
        SceneManager.LoadScene("MainMenu");

    }

}
