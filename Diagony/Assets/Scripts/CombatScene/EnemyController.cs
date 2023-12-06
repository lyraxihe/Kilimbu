using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{

    public GameObject VariablesGlobales;
    public GameObject CombatScene;
    public GameObject Player;
    public GameObject AuraEnemigo;
    
    public GameObject ArrowEmitter;
    public bool OverEnemy;

    public int Tipo; // 0 = Ira | 1 = Miedo | 2 = Tristeza
    public int Id;   // ID que tiene en la lista de enemigos
    public int HealthEnemigo;
    public int MaxHealthEnemigo;
    [SerializeField] private int AttackEnemigo;
    public Animator EnemyAnimator;
    [SerializeField] bool animation_damage;

    public bool RecibirDanyo; // Indica que el enemigo debe realizar la animación de recibir danyo


    public bool Bloqueado; // Indica si el jugador a bloqueado a este enemigo
    
    public bool Debilitado;
    public int Debilidad;
    public int ContadorDeTurnosDebilitado;
    public int ContadorDebilitados;

    public bool Envenenado;
    public int Veneno;
    public int ContadorDeTurnosEnvenenado;
    public int ContadorEnvenenados;


    public bool Fuerte;
    public int Fuerza;
    public int ContadorDeTurnosFuerte;
    public int ContadorFuerte;

    // Start is called before the first frame update
    void Start()
    {
        VariablesGlobales = GameObject.Find("VariablesGlobales");

        OverEnemy = false;
        RecibirDanyo = false;

        SetHealthEnemigo();
        //SetAtackEnenmigo();
        AttackEnemigo = 1;
        EnemyAnimator.SetInteger("enemy_id", Id);
        EnemyAnimator.SetBool("atacar", animation_damage);

        Bloqueado = false;

        Debilitado = false;
        Debilidad = 0;
        ContadorDeTurnosDebilitado = 0;
        ContadorDebilitados = 0;

        Envenenado = false;
        Veneno = 0;
        ContadorDeTurnosEnvenenado = 0;
        ContadorEnvenenados = 0;

        Fuerte = false;
        Fuerza = 0;
        ContadorDeTurnosFuerte = 0;
        ContadorFuerte = 0;
}

    // Update is called once per frame
    void Update()
    {

        ControlHealthEnemigo();
        ControlStatus();
    
    }

    //public void OnMouseOver()
    //{
    //    Debug.Log("Pasando por encima.");
    //    if (CombatScene.GetComponent<CombatController>().MovingArrow)
    //    {

    //        ArrowEmitter.GetComponent<ArrowEmitter>().OverEnemy = true;

    //    }

    //}

    //public void OnMouseExit()
    //{
    //    Debug.Log("Saliendo de encima.");
    //    if (CombatScene.GetComponent<CombatController>().MovingArrow)
    //    {

    //        ArrowEmitter.GetComponent<ArrowEmitter>().OverEnemy = false;

    //    }

    //}

    //public void OnTriggerEnter2D(Collider2D collision)
    //{

    //    if(collision.tag == "Arrow")
    //        ArrowEmitter.GetComponent<ArrowEmitter>().OverEnemy = true;

    //}

    //public void OnTriggerExit2D(Collider2D collision)
    //{

    //    if(collision.tag == "Arrow")
    //        ArrowEmitter.GetComponent<ArrowEmitter>().OverEnemy = true;

    //}

    public void SetHealthEnemigo()
    {

        if (Tipo == 0)         // Ira
            MaxHealthEnemigo = 50;
        else if (Tipo == 1)    // Miedo
            MaxHealthEnemigo = 50;
        else                   // Tristeza
            MaxHealthEnemigo = 50;

        HealthEnemigo = MaxHealthEnemigo;

    }

    public void ControlHealthEnemigo()
    {

        if (HealthEnemigo <= 0)
        {

            HealthEnemigo = 0;
            CombatScene.GetComponent<CombatController>().EliminarEnemig0(Id);
            Destroy(gameObject);

        }

        if (HealthEnemigo > MaxHealthEnemigo)
            HealthEnemigo = MaxHealthEnemigo;

    }

    //public void SetAtackEnenmigo()
    //{

    //    if (Tipo == 0)         // Ira
    //        AttackEnemigo = 20;
    //    else if (Tipo == 1)    // Miedo
    //        AttackEnemigo = 15;
    //    else                   // Tristeza
    //        AttackEnemigo = 10;

    //}

    public void Atacar()
    {
        
        if(!Bloqueado)
        {

            EnemyAnimator.SetBool("atacar", true);
            float AttackType = Random.Range(0f, 11f);
            int damageAmount = 0;

            if (Tipo == 0)         // Ira
            {
                if (ContadorDeTurnosFuerte > 0)
                {

                    ContadorDeTurnosFuerte--;
                    ContadorFuerte++;

                    if (ContadorFuerte == 4)
                    {

                        Fuerza += 3;
                        ContadorFuerte = 0; // Se resetea cada vez que se termina un efecto de Débil

                    }

                    if (ContadorDeTurnosFuerte == 0)
                        Fuerte = false;
                }

                if (AttackType >= 5f)
                {
                    
                    Debug.Log("aumenta la fuerza");

                    Fuerte = true;
                    Fuerza += 3;
                    ContadorDeTurnosFuerte += 3;

                }
                else
                {
                    
                    damageAmount = Random.Range(5, 8) + Fuerza + Debilidad;
                    if(damageAmount < 0)
                        damageAmount = 0;
                    if (Player.GetComponent<PlayerController>().Transformacion) // Si el Jugador está transformado el ataque le curará
                    {
                        Debug.Log("ataque que cura al player");
                        VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += damageAmount;
                        CombatScene.GetComponent<CombatController>().CreateDmgHealText(true, damageAmount, Player);

                    }
                    else
                    {
                        Debug.Log("atq normal");
                        VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista -= damageAmount;
                        CombatScene.GetComponent<CombatController>().CreateDmgHealText(false, damageAmount, Player);

                    }

                }
            }


            else if (Tipo == 1)    // Miedo
            {
                if (AttackType >= 8 && HealthEnemigo < 35)
                {
                    
                    damageAmount = 10;
                    Debug.Log("se cura miedo");
                    HealthEnemigo += damageAmount;

                    CombatScene.GetComponent<CombatController>().CreateDmgHealText(true, damageAmount, gameObject);

                }
                else
                {
                    
                    damageAmount = Random.Range(6, 8) + AttackEnemigo + Debilidad;
                    if (damageAmount < 0)
                        damageAmount = 0;
                    if (Player.GetComponent<PlayerController>().Transformacion) // Si el Jugador está transformado el ataque le curará
                    {
                        Debug.Log("ataque que cura al player");
                        VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += damageAmount;
                        CombatScene.GetComponent<CombatController>().CreateDmgHealText(true, damageAmount, Player);

                    }
                    else
                    {
                        Debug.Log("atq normal");
                        VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista -= damageAmount;
                        CombatScene.GetComponent<CombatController>().CreateDmgHealText(false, damageAmount, Player);

                    }

                }

            }


            else                   // Tristeza
            {
                if (AttackType <= 3.3f)
                {
                    
                    Debug.Log("Jugador Envenenado");
                    CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Envenenado = true;
                    CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Veneno += 3;
                    CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDeTurnosEnvenenado += 3;

                }
                else if (AttackType > 3.3f && AttackType <= 6.6)
                {
                    
                    Debug.Log("Jugador Debilitado");
                    CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Debilitado = true;
                    CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Debilidad -= 3;
                    CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDeTurnosDebilitado +=3;
                }
                else
                {
                    
                    Debug.Log("lo de la carta que quiere felipe kjsdf");
                    damageAmount = Random.Range(1, 5) + AttackEnemigo + Debilidad;

                    if (damageAmount < 0)
                        damageAmount = 0;

                    if (Player.GetComponent<PlayerController>().Transformacion) // Si el Jugador está transformado el ataque le curará
                    {
                        Debug.Log("ataque que cura al player");
                        VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += damageAmount;
                        CombatScene.GetComponent<CombatController>().CreateDmgHealText(true, damageAmount, Player);

                    }
                    else
                    {
                        Debug.Log("atq normal");
                        VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista -= damageAmount;
                        CombatScene.GetComponent<CombatController>().CreateDmgHealText(false, damageAmount, Player);

                    }
                    //sustituye una carta pero ni idea como hacerlo

                }
            }

        }
        else
        {

            Debug.Log("El Enemigo está Bloqueado");
            Bloqueado = false;

        }

        if (Envenenado) // Creo que esto tiene que ser cada vez que ataca el Jugador, no cada vez que usa una carta (hablar con Flipper)
        {

            HealthEnemigo -= Veneno;
            CombatScene.GetComponent<CombatController>().CreateDmgHealText(false, Veneno, gameObject);
        
        }

        // Debil (Enemigo)
        if (ContadorDeTurnosDebilitado > 0)
        {

            ContadorDeTurnosDebilitado--;
            ContadorDebilitados++;

            if (ContadorDebilitados == 3)
            {

                Debilidad += 3;
                ContadorDebilitados = 0; // Se resetea cada vez que se termina un efecto de Débil

            }

            if (ContadorDeTurnosDebilitado == 0)
                Debilitado = false;

        }

        // Envenenado (Enemigo)
        if (ContadorDeTurnosEnvenenado > 0)
        {

            ContadorDeTurnosEnvenenado--;
            ContadorEnvenenados++;

            if (ContadorEnvenenados == 3)
            {

                Veneno -= 3;
                ContadorEnvenenados = 0; // Se resetea cada vez que se termina un efecto de Débil

            }

            if (ContadorDeTurnosEnvenenado == 0)
                Envenenado = false;

        }

        // Fuerza (Enemigo ira)
        if (ContadorDeTurnosFuerte > 0)
        {

            ContadorDeTurnosFuerte--;
            ContadorFuerte++;

            if (ContadorFuerte == 3)
            {

                Fuerza += 3;
                ContadorFuerte = 0; // Se resetea cada vez que se termina un efecto de fuerza

            }

            if (ContadorDeTurnosFuerte == 0)
                Fuerte = false;

        }

    }

    public void ControlStatus()
    {

        //if (Debilitado)
        //{
        //    debilidad = -3;
        //}
        //else
        //{
        //    debilidad = 0;
        //}

        if (ContadorDeTurnosDebilitado < 0)
            ContadorDeTurnosDebilitado = 0;

        //if (Envenenado)
        //{
        //    veneno = 3;
        //}
        //else
        //{
        //    veneno = 0;
        //}

        if (ContadorDeTurnosEnvenenado < 0)
            ContadorDeTurnosEnvenenado = 0;

        if (ContadorDeTurnosFuerte < 0)
            ContadorDeTurnosFuerte = 0;

    }

    public void ControlEnemyAnimation(int valor)
    {

        if (valor == 0) // Si la animacion de atacar ha terminado
        {

            EnemyAnimator.SetBool("atacar", false);
            CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("danyo", true);

        }

        if (valor == 1) // Si la animacion de recibir daño ha terminado
            EnemyAnimator.SetBool("danyo", false);

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Arrow")
        {
            for (int j = 0; j < ArrowEmitter.GetComponent<ArrowEmitter>().arrowNodes.Count; j++)
            {

                ArrowEmitter.GetComponent<ArrowEmitter>().arrowNodes[j].GetComponent<Image>().color = Color.red;
                

            }

            //ArrowEmitter.GetComponent<ArrowEmitter>().OverEnemy = true;
            OverEnemy = true;
            ArrowEmitter.GetComponent<ArrowEmitter>().Carta.GetComponent<CardController>().EnemigoSeleccionado = Id;
            AuraEnemigo.SetActive(true);
            

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Arrow")
        {
            for (int j = 0; j < ArrowEmitter.GetComponent<ArrowEmitter>().arrowNodes.Count; j++)
            {

                ArrowEmitter.GetComponent<ArrowEmitter>().arrowNodes[j].GetComponent<Image>().color = Color.grey;


            }

            //ArrowEmitter.GetComponent<ArrowEmitter>().OverEnemy = false;
            OverEnemy = false;
            AuraEnemigo.SetActive(false);
            ArrowEmitter.GetComponent<ArrowEmitter>().Carta.GetComponent<CardController>().EnemigoSeleccionado = -1;

        }
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Debug.Log("detecta col");
    //    if (collision.gameObject.tag == "Arrow")
    //    {
    //        Debug.Log("toca la felcha idkdkdk");
    //    }
    //}

    //private void OnMouseUp()
    //{
        
    //    if(OverEnemy)
    //    {
    //        Debug.Log("OnMouseUp");
    //        // Deshabilita la flecha
    //        OverEnemy = false;

    //        for (int j = 0; j < ArrowEmitter.GetComponent<ArrowEmitter>().arrowNodes.Count; j++)
    //        {

    //            ArrowEmitter.GetComponent<ArrowEmitter>().arrowNodes[j].GetComponent<Image>().color = Color.grey;


    //        }

    //        ArrowEmitter.SetActive(false);
    //        CombatScene.GetComponent<CombatController>().MovingArrow = false;
    //        Cursor.visible = true;

    //        // Realiza el efecto de la carta
    //        CombatScene.GetComponent<CombatController>().UsarCarta(ArrowEmitter.GetComponent<ArrowEmitter>().IdCarta, Id);

    //    }

    //}

}
