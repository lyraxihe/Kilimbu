using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject VariablesGlobales;
    public GameObject CombatScene;

    public int MaxHealthProtagonista;
    public int HealthProtagonista;
    public Animator PlayerAnimator;
    [SerializeField] bool animation_damage;

    public int ContadorDeTurnos;

    // Débil
    public bool Debilitado;
    public int Debilidad;
    public int ContadorDeTurnosDebilitado;
    public int ContadorDebilitado;

    // Envenenado
    public bool Envenenado;
    public int Veneno;
    public int ContadorDeTurnosEnvenenado;
    public int ContadorEnvenenado;

    // Fuerte
    public bool Fuerte;
    public int Fuerza;
    public int ContadorDeTurnosFuerte;
    public int ContadorFuertes;

    // Esperanza
    public bool Esperanzado;
    public int Esperanza;
    public int ContadorDeTurnosEsperanzado;
    public int ContadorEsperanzas;

    //Transformacion (Los ataques al Jugador le curan en vez de hacerle daño)
    public bool Transformacion;
    public int ContadorDeTurnosTransformacion;

    void Start()
    {
        VariablesGlobales = GameObject.Find("VariablesGlobales");

        ContadorDeTurnos = 0;


        // Débil
        Debilitado = false;
        Debilidad = 0;
        ContadorDeTurnosDebilitado = 0;
        ContadorDebilitado = 0;

        // Envenenado
        Envenenado = false;
        Veneno = 0;
        ContadorDeTurnosEnvenenado = 0;
        ContadorEnvenenado = 0;

        // Fuerte
        Fuerte = false;
        Fuerza = 0;
        ContadorDeTurnosFuerte = 0;
        ContadorFuertes = 0;

        // Esperanza
        Esperanzado = false;
        Esperanza = 0;
        ContadorDeTurnosEsperanzado = 0;
        ContadorEsperanzas = 0;

        // Transformacion
        Transformacion = false;
        ContadorDeTurnosTransformacion = 0;

    }

    // Update is called once per frame
    void Update()
    {

        MaxHealthProtagonista = VariablesGlobales.GetComponent<VariablesGlobales>().MaxHealthProtagonista;
        HealthProtagonista = VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista;

        ControlStatus();

    }

    public void ControlAnimation(int valor)
    {

        if (valor == 0) // Indica que ha finalizado la aniamcion de atacar
        {

            PlayerAnimator.SetBool("atacar", false);
            
            for (int i = 0; i < CombatScene.GetComponent<CombatController>().EnemyList.Count(); i++)
            {

                if (CombatScene.GetComponent<CombatController>().EnemyList[i].GetComponent<EnemyController>().RecibirDanyo)
                {

                    CombatScene.GetComponent<CombatController>().EnemyList[i].GetComponent<EnemyController>().EnemyAnimator.SetBool("danyo", true);


                    CombatScene.GetComponent<CombatController>().EnemyList[i].GetComponent<EnemyController>().RecibirDanyo = false;
                }

            }

        }

        if(valor == 1) // Indica que ha finalizado la animacion de recibir danyo
            PlayerAnimator.SetBool("danyo", false);

    }

    public void ControlStatus()
    {

        if (ContadorDeTurnosDebilitado < 0)
        {
            ContadorDeTurnosDebilitado = 0;
        }

        if (ContadorDeTurnosEnvenenado < 0)
        {
            ContadorDeTurnosEnvenenado = 0;
        }

        if (ContadorDeTurnosFuerte < 0)
            ContadorDeTurnosFuerte = 0;

        if (ContadorDeTurnosTransformacion < 0)
            ContadorDeTurnosTransformacion = 0;

    }

}
