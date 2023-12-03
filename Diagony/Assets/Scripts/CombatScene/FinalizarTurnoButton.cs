using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalizarTurnoButton : MonoBehaviour
{
    [SerializeField] GameObject VariablesGlobales;
    [SerializeField] GameObject CombatScene;
    
    // Start is called before the first frame update
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
        CombatScene.GetComponent<CombatController>().ContadorDeTurnos++;
        int numCartas = CombatScene.GetComponent<CombatController>().CardList.Count;

        if(CombatScene.GetComponent<CombatController>().TurnoJugador)
        {

            for (int i = 0; i < numCartas; i++)
            {


                CombatScene.GetComponent<CombatController>().CardList[i].GetComponent<CardController>().CardAnimator.enabled = true;
                //CombatScene.GetComponent<CombatController>().CardList[i].GetComponent<CardController>().AnimacionCarta = true;
                CombatScene.GetComponent<CombatController>().CardList[i].GetComponent<CardController>().AnimacionSalir = true;

                //CombatScene.GetComponent<CombatController>().CardList[i].GetComponent<CardController>().CardAnimator.SetBool("AnimacionCarta", CombatScene.GetComponent<CombatController>().CardList[i].GetComponent<CardController>().AnimacionCarta);
                CombatScene.GetComponent<CombatController>().CardList[i].GetComponent<CardController>().CardAnimator.SetBool("AnimacionSalir", CombatScene.GetComponent<CombatController>().CardList[i].GetComponent<CardController>().AnimacionSalir);

            }

            CombatScene.GetComponent<CombatController>().CartasCreadas = false;
            CombatScene.GetComponent<CombatController>().TurnoJugador = false;

            CombatScene.GetComponent<CombatController>().botonTurno.interactable = false;
            StartCoroutine(CombatScene.GetComponent<CombatController>().TurnoEnemigo());

        }

    }
}
