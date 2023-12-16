using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausaButton : MonoBehaviour
{
    [SerializeField] GameObject PanelPausa;
    [SerializeField] GameObject VariablesGlobales;
    [SerializeField] GameObject CombatScene;
    public bool victoria;
    TMP_Text textButton;
    void Start()
    {
        victoria = false;
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

    public void VictoriaDerrota()
    {
        victoria = CombatScene.GetComponent<CombatController>().RecompensaVictoria;

        if (victoria)
        {
            Debug.Log("victoria boton pausa");
            SceneManager.LoadScene("MapScene");
        }
        else
        {
            Debug.Log("derrota boton pausa");
            SceneManager.LoadScene("MainMenu");
        }

    }

}
