using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausaButton : MonoBehaviour
{
    [SerializeField] GameObject PanelPausa;
    [SerializeField] GameObject VariablesGlobales_;
    [SerializeField] GameObject CombatScene;
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
            Time.timeScale = 0f;
            VariablesGlobales_.GetComponent<VariablesGlobales>().EstaEnPausa = true;
            CombatScene.GetComponent<CombatController>().botonTurno.enabled = false;

        }
        else
        {

            PanelPausa.SetActive(false);
            Time.timeScale = 1f;
            VariablesGlobales_.GetComponent<VariablesGlobales>().EstaEnPausa = false;
            CombatScene.GetComponent<CombatController>().botonTurno.enabled = true;

        }

    }

    public void Escapar()
    {

        VariablesGlobales_.GetComponent<VariablesGlobales>().EstaEnPausa = false;
        CombatScene.GetComponent<CombatController>().botonTurno.enabled = true;
        SceneManager.LoadScene("MainMenu");

    }

}
