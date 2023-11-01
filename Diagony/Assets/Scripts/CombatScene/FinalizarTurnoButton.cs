using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalizarTurnoButton : MonoBehaviour
{
    [SerializeField] GameObject CombatScene;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {

        int numCartas = CombatScene.GetComponent<CombatController>().CardList.cont;

        if(CombatScene.GetComponent<CombatController>().TurnoJugador)
        {

            //for(int i = 0; i < numCartas; i++)
            //{

            //    CombatScene.GetComponent<CombatController>().CardList.cards[i].GetComponent<CardController>().CardAnimator.enabled = true;
            //    CombatScene.GetComponent<CombatController>().CardList.cards[i].GetComponent<CardController>().AnimacionCarta = true;
            //    CombatScene.GetComponent<CombatController>().CardList.cards[i].GetComponent<CardController>().AnimacionSalir = true;

            //}

            CombatScene.GetComponent<CombatController>().TurnoJugador = false;

        }

    }
}
