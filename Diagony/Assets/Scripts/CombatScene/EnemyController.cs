using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{

    public GameObject VariablesGlobales;
    public GameObject CombatScene;
    public GameObject Player;
    public GameObject AuraEnemigo;
    
    public GameObject ArrowEmitter;
    public bool OverEnemy;

    public int Tipo; // 0 = Ira | 1 = Miedo | 2 = Tristeza | 3 = Boss
    public int Id;   // ID que tiene en la lista de enemigos
    public int HealthEnemigo;
    public int MaxHealthEnemigo;
    [SerializeField] private int AttackEnemigo;
    public Animator EnemyAnimator;
    [SerializeField] bool animation_damage;

    public bool RecibirDanyo; // Indica que el enemigo debe realizar la animación de recibir danyo


    [SerializeField] public bool Bloqueado; // Indica si el jugador a bloqueado a este enemigo

    [SerializeField] public bool Debilitado;
    public int Debilidad;
    public int ContadorDeTurnosDebilitado;
    public int ContadorDebilitado;

    [SerializeField] public bool Envenenado;
    public int Veneno;
    public int ContadorDeTurnosEnvenenado;
    public int ContadorEnvenenado;


    [SerializeField] public bool Fuerte;
    public int Fuerza;
    public int ContadorDeTurnosFuerte;
    public int ContadorFuerza;

    public int ContadorDeTurnosHeal;
    [SerializeField] public bool EsperandoHeal;

    // Esperanza
    public bool Esperanzado;
    public int Esperanza;
    public int ContadorDeTurnosEsperanzado;
    public int ContadorEsperanza;

    // Transformacion (Los ataques al Boss le curan en vez de hacerle daño)
    public bool Transformacion;
    public int ContadorDeTurnosTransformacion;

    bool[] ActiveSpellGap = new bool [5];
    [SerializeField] int ActiveSpell;
    Vector2[] SpellCoords = new Vector2 [5];
    GameObject[] ActiveSpellGameobject = new GameObject[5];
    float x_inicial_spell;
    float y_inicial_spell;

    int debilidad_icon;
    int veneno_icon;
    int fuerte_icon;
    public bool SoloTristeza;

    public bool playerSeBufa;

    public int contAcumulacionDanyoBoss;

    void Start()
    {
        SoloTristeza = false;

        VariablesGlobales = GameObject.Find("VariablesGlobales");

        OverEnemy = false;
        RecibirDanyo = false;

        SetHealthEnemigo();
        //SetAtackEnenmigo();
        AttackEnemigo = 1;
        EnemyAnimator.SetInteger("enemy_type", Tipo);
        EnemyAnimator.SetBool("atacar", animation_damage);


        //inicializo los hechizos
        debilidad_icon = 0;
        veneno_icon = 0;
        fuerte_icon = 0;

        ActiveSpell = 0;
        if (Id == 0)
        {
            //sumarle a la x 0.4f
            x_inicial_spell = 0.9f;
            y_inicial_spell = 2.5f;
        }
        else if (Id == 1)
        {
            x_inicial_spell = 2.9f;
            y_inicial_spell = 1.5f;
        }
        else
        {
            x_inicial_spell = 4.9f;
            y_inicial_spell = 0.5f;
        }

        for (int i = 0; i < VariablesGlobales.GetComponent<VariablesGlobales>().SpellNumber; i++)
        {
            ActiveSpellGap[i] = false;
            SpellCoords[i] = new Vector2(x_inicial_spell += 0.4f, y_inicial_spell);
           
        }

        //                                              PRUEBAAAAAAAAAAAAAAAAAAAAAAAAAAAAA !!!!!!!!!!!!!!!

                //for (int i = 0; i < VariablesGlobales.GetComponent<VariablesGlobales>().SpellNumber; i++)
                //{
                //    GameObject ClonSpell;
                //    ClonSpell = Instantiate(CombatScene.GetComponent<CombatController>().PrefabSpell[i]); // Crea el clon del prefab
                //    ClonSpell.transform.position = SpellCoords[ActiveSpell];
                //    ActiveSpellGap[ActiveSpell] = true;
                //    ActiveSpell++;
                //}
        //                                              PRUEBAAAAAAAAAAAAAAAAAAAAAAAAAAAAA !!!!!!!!!!!!!!!


        Bloqueado = false;

        Debilitado = false;
        Debilidad = 0;
        ContadorDeTurnosDebilitado = 0;
        ContadorDebilitado = 0;

        Envenenado = false;
        Veneno = 0;
        ContadorDeTurnosEnvenenado = 0;
        ContadorEnvenenado = 0;

        Fuerte = false;
        Fuerza = 0;
        ContadorDeTurnosFuerte = 0;
        ContadorFuerza = 0;

        ContadorDeTurnosHeal = 0;
        EsperandoHeal = false;

        // Esperanza
        Esperanzado = false;
        Esperanza = 0;
        ContadorDeTurnosEsperanzado = 0;
        ContadorEsperanza = 0;

        playerSeBufa = false;

        contAcumulacionDanyoBoss = 0;

    }

    // Update is called once per frame
    void Update()
    {

        ControlHealthEnemigo();
        ControlStatus();

        //esto no se debería hacer en el update pero no sé
        if (CombatScene.GetComponent<CombatController>().numTristeza == CombatScene.GetComponent<CombatController>().EnemyList.Count)
        {
            SoloTristeza = true;
        }


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
        else if (Tipo == 2)    // Tristeza
            MaxHealthEnemigo = 50;
        else
            MaxHealthEnemigo = 200;

        HealthEnemigo = MaxHealthEnemigo;

    }

    public void ControlHealthEnemigo()
    {

        if (HealthEnemigo <= 0)
        {

            HealthEnemigo = 0;
            CombatScene.GetComponent<CombatController>().EliminarEnemig0(Id);
            for (int i = 0; i < ActiveSpell; i++)
            {
                Destroy(ActiveSpellGameobject[i]);
            }
            CombatScene.GetComponent<CombatController>().victoriaDerrota();
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

            float AttackType = Random.Range(1f, 11f);
            int damageAmount = 0;

            if (Tipo == 0)         // Ira
            {
                if (ContadorDeTurnosFuerte > 0)
                {

                    ContadorDeTurnosFuerte--;
                    ContadorFuerza++;


                    if (ContadorFuerza == 4)
                    {

                        Fuerza += 3;
                        ContadorFuerza = 0; // Se resetea cada vez que se termina un efecto de Fuerza

                    }

                    if (ContadorDeTurnosFuerte == 0)
                    {
                        Fuerte = false;
                        Fuerza = 0;
                        ContadorDeTurnosFuerte = 0;
                        fuerte_icon = 0;
                       
                    }
                        
                }

                if (AttackType >= 5f && ContadorDeTurnosFuerte <= 0)
                {
                    
                    Debug.Log("aumenta la fuerza");

                    Fuerte = true;
                    Fuerza += 3;
                    ContadorDeTurnosFuerte += 3;
                    CombatScene.GetComponent<CombatController>().CreateSpellText("Fuerte", gameObject);

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

                EnemyAnimator.SetBool("atacar", true);
            }


            else if (Tipo == 1)    // Miedo
            {
                if (EsperandoHeal)
                {
                    ContadorDeTurnosHeal--;
                    if (ContadorDeTurnosHeal <= 0)
                    {
                        ContadorDeTurnosHeal = 0;
                        EsperandoHeal = false;
                    }
                }

                if (AttackType >= 8 && HealthEnemigo < 35 && !EsperandoHeal)
                {
                    ContadorDeTurnosHeal = 2;
                    EsperandoHeal = true;

                    damageAmount = 10;
                    Debug.Log("se cura miedo");
                    HealthEnemigo += damageAmount;

                    CombatScene.GetComponent<CombatController>().CreateDmgHealText(true, damageAmount, gameObject);

                    EnemyAnimator.SetBool("atacar", true);

                }
                else
                {
                    
                    damageAmount = Random.Range(3, 5) + AttackEnemigo + Debilidad;
                    if (damageAmount < 0)
                        damageAmount = 0;
                    //if (Player.GetComponent<PlayerController>().Transformacion) // Si el Jugador está transformado el ataque le curará
                    //{
                    //    Debug.Log("ataque que cura al player");
                    //    VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += damageAmount;
                    //    CombatScene.GetComponent<CombatController>().CreateDmgHealText(true, damageAmount, Player);

                    //}
                    //else
                    //{
                    //    Debug.Log("atq normal");
                    //    StartCoroutine(DoubleAttack(damageAmount, 0.5f));


                    //}
                    Debug.Log("atq normal");
                    StartCoroutine(DoubleAttack(damageAmount, 0.5f));

                }

            }
            else if (Tipo == 2)                   // Tristeza
            {
                if (SoloTristeza)
                {
                    if (Player.GetComponent<PlayerController>().Transformacion) // Si el Jugador está transformado el ataque le curará
                    {
                        
                        damageAmount = 2;
                        VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += damageAmount;
                        CombatScene.GetComponent<CombatController>().CreateDmgHealText(true, damageAmount, Player);

                    }
                    else
                    {
                        damageAmount = 2;
                        VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista -= damageAmount;
                        CombatScene.GetComponent<CombatController>().CreateDmgHealText(false, damageAmount, Player);

                    }

                }

                else if (AttackType <= 3.3f && CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDeTurnosEnvenenado <=0)
                {
                    
                    Debug.Log("Jugador Envenenado");
                    CombatScene.GetComponent<CombatController>().CreateSpellText("Envenenado", Player);
                    CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Envenenado = true;
                    CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Veneno += 3;
                    CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDeTurnosEnvenenado += 1;

                }
                else if (AttackType > 3.3f && AttackType <= 6.6 && CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDeTurnosDebilitado <=0)
                {
                    
                    Debug.Log("Jugador Debilitado");
                    CombatScene.GetComponent<CombatController>().CreateSpellText("Debilitado", Player);
                    CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Debilitado = true;
                    CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Debilidad -= 3;
                    CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDeTurnosDebilitado +=2;
                }
                else
                {
                    
                    Debug.Log("Reducir 1 de mana");
                    //damageAmount = Random.Range(1, 5) + AttackEnemigo + Debilidad;

                    //if (damageAmount < 0)
                    //    damageAmount = 0;

                    //if (Player.GetComponent<PlayerController>().Transformacion) // Si el Jugador está transformado el ataque le curará
                    //{
                    //    Debug.Log("ataque que cura al player");
                    //    VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += damageAmount;
                    //    CombatScene.GetComponent<CombatController>().CreateDmgHealText(true, damageAmount, Player);

                    //}
                    //else
                    //{
                    //    Debug.Log("atq normal");
                    //    VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista -= damageAmount;
                    //    CombatScene.GetComponent<CombatController>().CreateDmgHealText(false, damageAmount, Player);

                    //}

                    CombatScene.GetComponent<CombatController>().CreateSpellText("Reducir Maná", Player);
                    Player.GetComponent<PlayerController>().ReducirMana = true;

                }

                EnemyAnimator.SetBool("atacar", true);

            }
            else // Boss
            {

                if(HealthEnemigo <= 10 && !EsperandoHeal) // Si tiene poca vida se cura
                {

                    ContadorDeTurnosHeal = 3;
                    EsperandoHeal = true;

                    damageAmount = 30;
                    HealthEnemigo += damageAmount;

                    CombatScene.GetComponent<CombatController>().CreateDmgHealText(true, damageAmount, gameObject);

                }
                else // Comportamiento normal
                {

                    if (playerSeBufa) // Intercambia estados con el Jugador
                    {

                        GameObject player = CombatScene.GetComponent<CombatController>().Player;

                        // Debilidad
                        if(player.GetComponent<PlayerController>().Debilitado || Debilitado)
                        {

                            bool DebilitadoAux = player.GetComponent<PlayerController>().Debilitado;
                            int DebilidadAux = player.GetComponent<PlayerController>().Debilidad;
                            int ContadorDeTurnoDebilitadoAux = player.GetComponent<PlayerController>().ContadorDeTurnosDebilitado;
                            int ContadorDebilitadoAux = player.GetComponent<PlayerController>().ContadorDebilitado;

                            player.GetComponent<PlayerController>().Debilitado = Debilitado;
                            player.GetComponent<PlayerController>().Debilidad = Debilidad;
                            player.GetComponent<PlayerController>().ContadorDeTurnosDebilitado = ContadorDeTurnosDebilitado;
                            player.GetComponent<PlayerController>().ContadorDebilitado = ContadorDebilitado;

                            Debilitado = DebilitadoAux;
                            Debilidad = DebilidadAux;
                            ContadorDeTurnosDebilitado = ContadorDeTurnoDebilitadoAux;
                            ContadorDebilitado = ContadorDebilitadoAux;

                        }

                        // Envenenado
                        if(player.GetComponent<PlayerController>().Envenenado || Envenenado)
                        {

                            bool EnvenenadoAux = player.GetComponent<PlayerController>().Envenenado;
                            int VenenoAux = player.GetComponent<PlayerController>().Veneno;
                            int ContadorDeTurnosEnvenenadoAux = player.GetComponent<PlayerController>().ContadorDeTurnosEnvenenado;
                            int ContadorEnvenenadoAux = player.GetComponent<PlayerController>().ContadorEnvenenado;

                            player.GetComponent<PlayerController>().Envenenado = Envenenado;
                            player.GetComponent<PlayerController>().Veneno = Veneno;
                            player.GetComponent<PlayerController>().ContadorDeTurnosEnvenenado = ContadorDeTurnosEnvenenado;
                            player.GetComponent<PlayerController>().ContadorEnvenenado = ContadorEnvenenado;

                            Envenenado = EnvenenadoAux;
                            Veneno = VenenoAux;
                            ContadorDeTurnosEnvenenado = ContadorDeTurnosEnvenenadoAux;
                            ContadorEnvenenado = ContadorEnvenenadoAux;

                        }

                        // Fuerte
                        if(player.GetComponent<PlayerController>().Fuerte || Fuerte)
                        {

                            bool FuerteAux = player.GetComponent<PlayerController>().Fuerte;
                            int FuerzaAux = player.GetComponent<PlayerController>().Fuerza;
                            int ContadorDeTurnosFuerteAux = player.GetComponent<PlayerController>().ContadorDeTurnosFuerte;
                            int ContadorFuerteAux = player.GetComponent<PlayerController>().ContadorFuerza;

                            player.GetComponent<PlayerController>().Fuerte = Fuerte;
                            player.GetComponent<PlayerController>().Fuerza = Fuerza;
                            player.GetComponent<PlayerController>().ContadorDeTurnosFuerte = ContadorDeTurnosFuerte;
                            player.GetComponent<PlayerController>().ContadorFuerza = ContadorFuerza;

                            Fuerte = FuerteAux;
                            Fuerza = FuerzaAux;
                            ContadorDeTurnosFuerte = ContadorDeTurnosFuerteAux;
                            ContadorFuerza = ContadorFuerteAux;

                        }

                        // Esperanza
                        if(player.GetComponent<PlayerController>().Esperanzado || Esperanzado)
                        {

                            bool EsperanzadoAux = player.GetComponent<PlayerController>().Esperanzado;
                            int EsperanzaAux = player.GetComponent<PlayerController>().Esperanza;
                            int ContadorDeTurnosEsperanzadoAux = player.GetComponent<PlayerController>().ContadorDeTurnosEsperanzado;
                            int ContadorEsperanzaAux = player.GetComponent<PlayerController>().ContadorEsperanza;

                            player.GetComponent<PlayerController>().Esperanzado = Esperanzado;
                            player.GetComponent<PlayerController>().Esperanza = Esperanza;
                            player.GetComponent<PlayerController>().ContadorDeTurnosEsperanzado = ContadorDeTurnosEsperanzado;
                            player.GetComponent<PlayerController>().ContadorEsperanza = ContadorEsperanza;

                            Esperanzado = EsperanzadoAux;
                            Esperanza = EsperanzaAux;
                            ContadorDeTurnosEsperanzado = ContadorDeTurnosEsperanzadoAux;
                            ContadorEsperanza = ContadorEsperanzaAux;

                        }

                        // Transformacion (Los ataques al Jugador le curan en vez de hacerle daño)
                        if(player.GetComponent<PlayerController>().Transformacion || Transformacion)
                        {

                            bool TransformacionAux = player.GetComponent<PlayerController>().Transformacion;
                            int ContadorDeTurnosTransformacionAux = player.GetComponent<PlayerController>().ContadorDeTurnosTransformacion;

                            player.GetComponent<PlayerController>().Transformacion = Transformacion;
                            player.GetComponent<PlayerController>().ContadorDeTurnosTransformacion = ContadorDeTurnosTransformacion;

                            Transformacion = TransformacionAux;
                            ContadorDeTurnosTransformacion = ContadorDeTurnosTransformacionAux;

                        }

                        CombatScene.GetComponent<CombatController>().CreateSpellText("Intercambio", gameObject);

                    }
                    else // Ataques normales
                    {

                        if (AttackType <= 5) // Ataque de daño
                        {

                            damageAmount = Random.Range(10, 15) + Fuerza + Debilidad;
                            if (damageAmount < 0)
                                damageAmount = 0;
                            if (Player.GetComponent<PlayerController>().Transformacion) // Si el Jugador está transformado el ataque le curará
                            {

                                VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += damageAmount;
                                CombatScene.GetComponent<CombatController>().CreateDmgHealText(true, damageAmount, Player);

                            }
                            else
                            {

                                VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista -= damageAmount;
                                CombatScene.GetComponent<CombatController>().CreateDmgHealText(false, damageAmount, Player);

                            }

                        }
                        else if (AttackType <= 8) // Pone Débil al Jugador
                        {

                            CombatScene.GetComponent<CombatController>().CreateSpellText("Debilitado", Player);
                            CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Debilitado = true;
                            CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Debilidad -= 3;
                            CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDeTurnosDebilitado += 2;

                        }
                        else if (AttackType <= 8.5f) // Se pone transformado
                        {

                            Transformacion = true;
                            ContadorDeTurnosTransformacion += 1;
                            CombatScene.GetComponent<CombatController>().CreateSpellText("Transformado", gameObject);

                        }
                        else
                        {

                            CombatScene.GetComponent<CombatController>().CreateSpellText("Reducir Maná", Player);
                            Player.GetComponent<PlayerController>().ReducirMana = true;

                        }

                    }

                }

                EnemyAnimator.SetBool("atacar", true);

            }

            ControlEsperanzado();

        }
        else
        {

            Debug.Log("El Enemigo está Bloqueado");
            CombatScene.GetComponent<CombatController>().CreateSpellText("Bloqueado", gameObject);
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
            ContadorDebilitado++;

            if (ContadorDebilitado == 3)
            {

                Debilidad += 3;
                ContadorDebilitado = 0; // Se resetea cada vez que se termina un efecto de Débil

            }

            if (ContadorDeTurnosDebilitado == 0)
            {
                Debilidad = 0;
                Debilitado = false;
                debilidad_icon = 0;
                for (int i = 0; i < ActiveSpell; i++)
                {
                    if (ActiveSpellGameobject[i].tag == "Debilitado")
                    {
                        ReestructuraIcons(i);
                        break;
                    }
                }
            }
                

        }

        // Envenenado (Enemigo)
        if (ContadorDeTurnosEnvenenado > 0)
        {

            ContadorDeTurnosEnvenenado--;
            ContadorEnvenenado++;

            if (ContadorEnvenenado == 3)
            {

                Veneno -= 3;
                ContadorEnvenenado = 0; // Se resetea cada vez que se termina un efecto de Envenenado

            }

            if (ContadorDeTurnosEnvenenado == 0)
            {
                Veneno = 0;
                Envenenado = false;
                veneno_icon = 0;
                for (int i = 0; i < ActiveSpell; i++)
                {
                    if (ActiveSpellGameobject[i].tag == "Envenenado")
                    {
                        ReestructuraIcons(i);
                        break;
                    }
                }
            }
               

        }

        // Fuerza (Enemigo ira)
        if (ContadorDeTurnosFuerte > 0)
        {

            ContadorDeTurnosFuerte--;
            ContadorFuerza++;

            if (ContadorFuerza == 3)
            {

                Fuerza += 3;
                ContadorFuerza = 0; // Se resetea cada vez que se termina un efecto de fuerza

            }

            if (ContadorDeTurnosFuerte == 0)
            {
                Fuerza = 0;
                Fuerte = false;
                fuerte_icon = 0;
                for (int i = 0; i < ActiveSpell; i++)
                {
                    if (ActiveSpellGameobject[i].tag == "Fuerza")
                    {
                        ReestructuraIcons(i);
                        break;
                    }
                }
            }     

        }

        // Control del estado Esperanza en el Enemigo
        if (ContadorDeTurnosEsperanzado > 0)
        {

            ContadorDeTurnosEsperanzado--;
            ContadorEsperanza++;

            if (ContadorEsperanza == 4)
            {

                Esperanza -= 3;
                ContadorEsperanza = 0; // Se resetea cada vez que se termina un efecto de Débil

            }

            if (ContadorDeTurnosEsperanzado == 0)
            {
                
                Esperanzado = false;
                Esperanza = 0;

            }


        }

        playerSeBufa = false;

        CombatScene.GetComponent<CombatController>().victoriaDerrota();

    }

    public void ControlStatus()
    {
        if (Envenenado && veneno_icon == 0)
        {
            GameObject ClonSpell;
            ClonSpell = Instantiate(CombatScene.GetComponent<CombatController>().PrefabSpell[0]); // Crea el clon del prefab de veneno ([0])
            ClonSpell.transform.position = SpellCoords[ActiveSpell];
            ActiveSpellGameobject[ActiveSpell] = ClonSpell;
            ActiveSpellGap[ActiveSpell] = true;
            ActiveSpell++;
            veneno_icon++;
        }
        if (Debilitado && debilidad_icon == 0)
        {
            GameObject ClonSpell;
            ClonSpell = Instantiate(CombatScene.GetComponent<CombatController>().PrefabSpell[1]); // Crea el clon del prefab de debilidad ([1])
            ClonSpell.transform.position = SpellCoords[ActiveSpell];
            ActiveSpellGameobject[ActiveSpell] = ClonSpell;
            ActiveSpellGap[ActiveSpell] = true;
            ActiveSpell++;
            debilidad_icon++;
        }
        if (Fuerte && fuerte_icon == 0)
        {
            GameObject ClonSpell;
            ClonSpell = Instantiate(CombatScene.GetComponent<CombatController>().PrefabSpell[2]); // Crea el clon del prefab de fuerza ([2])
            ClonSpell.transform.position = SpellCoords[ActiveSpell];
            ActiveSpellGameobject[ActiveSpell] = ClonSpell;
            ActiveSpellGap[ActiveSpell] = true;
            ActiveSpell++;
            fuerte_icon++;
        }


        if (ContadorDeTurnosEnvenenado < 0)
        {
            ContadorDeTurnosEnvenenado = 0;
            Veneno = 0;
            Envenenado = false;
            veneno_icon = 0;
            for (int i = 0; i<ActiveSpell; i++)
            {
                if (ActiveSpellGameobject[i].tag == "Envenenado")
                {
                    ReestructuraIcons(i);
                    break;
                }
            }

        }
        if (ContadorDeTurnosDebilitado < 0)
        {
            ContadorDeTurnosDebilitado = 0;
            Debilidad = 0;
            Debilitado = false;
            debilidad_icon = 0;
            for (int i = 0; i < ActiveSpell; i++)
            {
                if (ActiveSpellGameobject[i].tag == "Debilitado")
                {
                    ReestructuraIcons(i);
                    break;
                }
            }
        }
        if (ContadorDeTurnosFuerte < 0)
        {
            ContadorDeTurnosFuerte = 0;
            Fuerza = 0;
            Fuerte = false;
            fuerte_icon = 0;
            for (int i = 0; i < ActiveSpell; i++)
            {
                if (ActiveSpellGameobject[i].tag == "Fuerza")
                {
                    ReestructuraIcons(i);
                    break;
                }
            }
        }

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
    public IEnumerator DoubleAttack(int damageAmount, float tiempo)
    {

        Debug.Log("pasa por el coso de segundos !!!!!");

        if(Player.GetComponent<PlayerController>().Transformacion)
        {

            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += damageAmount;
            CombatScene.GetComponent<CombatController>().CreateDmgHealText(true, damageAmount, Player);
            EnemyAnimator.SetBool("atacar", true);

            yield return new WaitForSeconds(tiempo);

            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += damageAmount;
            CombatScene.GetComponent<CombatController>().CreateDmgHealText(true, damageAmount, Player);
            EnemyAnimator.SetBool("atacar", true);

        }
        else
        {

            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista -= damageAmount;
            CombatScene.GetComponent<CombatController>().CreateDmgHealText(false, damageAmount, Player);
            EnemyAnimator.SetBool("atacar", true);

            yield return new WaitForSeconds(tiempo);

            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista -= damageAmount;
            CombatScene.GetComponent<CombatController>().CreateDmgHealText(false, damageAmount, Player);
            EnemyAnimator.SetBool("atacar", true);

        }

    }

    public void ReestructuraIcons(int IconEliminar)
    {
      
        // Elimina el icono en la posición IconEliminar
        Destroy(ActiveSpellGameobject[IconEliminar]);
        ActiveSpellGap[ActiveSpell] = false;

        // Reorganiza las posiciones y actualiza el array
        for (int i = IconEliminar; i < ActiveSpell - 1; i++)
        {
            ActiveSpellGameobject[i] = ActiveSpellGameobject[i + 1];
            ActiveSpellGameobject[i].transform.position = SpellCoords[i];
        }

        // Marca el último elemento del array como nulo
        ActiveSpellGameobject[ActiveSpell - 1] = null;

        // Decrementa la cantidad de hechizos activos
        ActiveSpell--;

    }

    public void ControlEsperanzado()
    {

        if (Esperanzado)
        {

            HealthEnemigo += Esperanza;
            CombatScene.GetComponent<CombatController>().CreateDmgHealText(true, Esperanza, gameObject);

        }

    }

    public void ControlAcumulacionDanyoBoss()
    {

        //yield return new WaitForSeconds(0.5f);

        CombatScene.GetComponent<CombatController>().CreateSpellText("Se ha cansado de ti", gameObject);
        CombatScene.GetComponent<CombatController>().botonTurno.GetComponent<FinalizarTurnoButton>().OnClick();

    }

}
