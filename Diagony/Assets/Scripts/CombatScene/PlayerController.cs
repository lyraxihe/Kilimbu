using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject VariablesGlobales;
    public GameObject CombatScene;

    public int MaxHealthProtagonista;
    public int HealthProtagonista;
    public Animator PlayerAnimator;
    [SerializeField] bool animation_damage;

    void Start()
    {
        VariablesGlobales = GameObject.Find("VariablesGlobales");
    }

    // Update is called once per frame
    void Update()
    {

        MaxHealthProtagonista = VariablesGlobales.GetComponent<VariablesGlobales>().MaxHealthProtagonista;
        HealthProtagonista = VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista;

    }

    public void ControlAnimation(int valor)
    {

        if (valor == 0) // Indica que ha finalizado la aniamcion de atacar
        {

            PlayerAnimator.SetBool("atacar", false);
            if(CombatScene.GetComponent<CombatController>().EnemigosRecibirDanyo)
            {

                for (int i = 0; i < CombatScene.GetComponent<CombatController>().EnemyList.Count; i++)
                    CombatScene.GetComponent<CombatController>().EnemyList[i].GetComponent<EnemyController>().EnemyAnimator.SetBool("danyo", true);


                CombatScene.GetComponent<CombatController>().EnemigosRecibirDanyo = false;
            }

        }

        if(valor == 1) // Indica que ha finalizado la animacion de recibir danyo
            PlayerAnimator.SetBool("danyo", false);

    }
}
