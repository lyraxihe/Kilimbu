using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    //public Sprite ArrowHeadPrefab_A; //ACTIVO
    //public Sprite ArrowNodePrefab_A;

    //public Sprite ArrowHeadPrefab_D; //DESACTIVO
    //public Sprite ArrowNodePrefab_D;

    public GameObject VariablesGlobales;
    public GameObject CombatScene;
    public GameObject Player;
    public GameObject AuraEnemigo;
    public GameObject Name;
    bool showName;
    bool showName2;

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
    public bool IntegrarEmocion = false; // Indica que el enemigo debe realizar la animación de integrarse


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
    int bloqueado_icon;
    public bool SoloTristeza;

    public bool playerSeBufa;

    public int contAcumulacionDanyoBoss;

    public TMP_Text EnemyNameText;

    public bool BossDanyoComprobado; // Controla que el "se ha casado de ti" del Boss sólo se ejecute una vez, sobretodo para los DoubleAttack

    private bool PlayerRecibeDanyo; // Cuando el enemigo realiza la animación de ataque, indica si el player debe realizar la animación de recibir daño | false - no recibe daño | true - si recibe daño

    // Animation Muerte
    private bool EndMuerteTransformacion;
    private List<Vector3> ListPositionsMuerte;
    private Vector3 PositionMuerte;
    public bool Derrotado; // Controla si el enemigo ha sido derrotado, para que no haga acciones antes de ejecutar su animación de derrota

    // Partículas
    public ParticleSystem HealParticle;
    public ParticleSystem EffectParticle;
    public ParticleSystem DamageParticle;

    //SoundFX Management
    public AudioSource AtaqueEmocionSound;
    public AudioSource AtaqueDobleEmocionSound;
    public AudioSource CurarEmocionSound;
    public AudioSource AplicarEfectoDeEmocionSound;
    public AudioSource AtaqueBossSound;
    public AudioSource CurarBossSound;
    public AudioSource AplicarEfectoDeBossSound;
    public AudioSource IntegrarEmocionSound;
    public AudioSource BloqueadoSound;




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
            x_inicial_spell = 1.18f;
            y_inicial_spell = 2.4f;
        }
        else if (Id == 1)
        {
            x_inicial_spell = 3.18f;
            y_inicial_spell = 1.4f;
        }
        else
        {
            x_inicial_spell = 4.18f;
            y_inicial_spell = 0.4f;
        }

        for (int i = 0; i < VariablesGlobales.GetComponent<VariablesGlobales>().SpellNumber; i++)
        {
            ActiveSpellGap[i] = false;
           
            if (Tipo == 3)
            {
                SpellCoords[i] = new Vector2(x_inicial_spell += 0.4f, y_inicial_spell + 1.5f);
            }
            else
            {
                SpellCoords[i] = new Vector2(x_inicial_spell += 0.4f, y_inicial_spell);
            }
           
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
        BossDanyoComprobado = false;

        PlayerRecibeDanyo = false;

        // Animation Muerte
        EndMuerteTransformacion = false;
        ListPositionsMuerte = new List<Vector3> { new Vector3(-5, -1, transform.position.z), new Vector3(-6, -1, transform.position.z), new Vector3(-7, -1, transform.position.z) };
        Derrotado = false;

        //Find SoundFxs from scene
        AtaqueEmocionSound = GameObject.Find("AtaqueEmocion_SoundFX").GetComponent<AudioSource>();
        AtaqueDobleEmocionSound = GameObject.Find("AtaqueDobleEmocion_SoundFX").GetComponent<AudioSource>();
        CurarEmocionSound = GameObject.Find("CurarEmocion_SoundFX").GetComponent<AudioSource>();
        AplicarEfectoDeEmocionSound = GameObject.Find("AplicarEfectoDeEmocion_SoundFX").GetComponent<AudioSource>();
        AtaqueBossSound = GameObject.Find("AtaqueBoss_SoundFX").GetComponent<AudioSource>();
        CurarBossSound = GameObject.Find("CurarBoss_SoundFX").GetComponent<AudioSource>();
        AplicarEfectoDeBossSound = GameObject.Find("AplicarEfectoDeBoss_SoundFX").GetComponent<AudioSource>();
        IntegrarEmocionSound = GameObject.Find("IntegrarEmocion_SoundFX").GetComponent<AudioSource>();
        BloqueadoSound = GameObject.Find("Bloqueado_SoundFX").GetComponent<AudioSource>();
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

        // Comprueba el daño recibido por el Boss en el turno del Jugador para su "Se ha cansado de ti"
        if (VariablesGlobales.GetComponent<VariablesGlobales>().Boss) // Si es el Boss
            if (contAcumulacionDanyoBoss > 20 && !BossDanyoComprobado) // Si el daño recibido es mayor que 20 y no se ha comprobado previamente
                //StartCoroutine(EnemyList[0].GetComponent<EnemyController>().ControlAcumulacionDanyoBoss());
                ControlAcumulacionDanyoBoss();
        ShowName();

        if(EndMuerteTransformacion)
        {

            transform.position = Vector3.Lerp(transform.position, PositionMuerte, 2.5f * Time.deltaTime);

            if (Vector3.Distance(transform.position, PositionMuerte) < 0.05f)
            {

                EndMuerteTransformacion = false;
                transform.position = PositionMuerte;

                EnemyAnimator.SetTrigger("muerte_idle");

                CombatScene.GetComponent<CombatController>().EliminarEnemig0(Id);
                CombatScene.GetComponent<CombatController>().victoriaDerrota();

            }

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

        if (VariablesGlobales.GetComponent<VariablesGlobales>().Tutorial)
            MaxHealthEnemigo = 50;
        else
        {

            if (Tipo == 0)         // Ira
                MaxHealthEnemigo = 50;
            else if (Tipo == 1)    // Miedo
                MaxHealthEnemigo = 50;
            else if (Tipo == 2)    // Tristeza
                MaxHealthEnemigo = 50;
            else                   // Boss
                MaxHealthEnemigo = 200;

        }

        HealthEnemigo = MaxHealthEnemigo;

    }

    public void ControlHealthEnemigo()
    {

        if (HealthEnemigo <= 0)
        {

            Derrotado = true;
            HealthEnemigo = 0;
            EnemyAnimator.SetTrigger("muerte");
            //CombatScene.GetComponent<CombatController>().EliminarEnemig0(Id);
            for (int i = 0; i < ActiveSpell; i++)
            {
                Destroy(ActiveSpellGameobject[i]);
            }
            //CombatScene.GetComponent<CombatController>().victoriaDerrota();
            //Destroy(gameObject);

            if(!IntegrarEmocion)
            {
                IntegrarEmocion = true;
                IntegrarEmocionSound.Play();
            }

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

                    if (ContadorDeTurnosFuerte <= 0)
                    {
                        Fuerza = 0;
                        Fuerte = false;
                        fuerte_icon = 0;
                        for (int i = 0; i < ActiveSpell; i++)
                        {
                            if (ActiveSpellGameobject[i].GetComponent<EffectIcon>().Tipo == 2)
                            {
                                ReestructuraIcons(i);
                                break;
                            }
                        }
                    }
                        
                }

                if (AttackType >= 5f && ContadorFuerza <= 0)
                {
                    
                    Debug.Log("aumenta la fuerza");

                    Fuerte = true;
                    Fuerza += 3;
                    ContadorDeTurnosFuerte += 3;
                    if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                        CombatScene.GetComponent<CombatController>().CreateSpellText("Strong", gameObject);
                    else                                                                  // Spanish
                        CombatScene.GetComponent<CombatController>().CreateSpellText("Fuerte", gameObject);

                    EffectParticle.Play();
                    AplicarEfectoDeEmocionSound.Play();

                }
                else
                {

                    damageAmount = Random.Range(4, 7) + Fuerza + Debilidad;
                    if(damageAmount < 0)
                        damageAmount = 0;
                    if (Player.GetComponent<PlayerController>().Transformacion) // Si el Jugador está transformado el ataque le curará
                    {
                        Debug.Log("ataque que cura al player");
                        VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += damageAmount;
                        StartCoroutine(CombatScene.GetComponent<CombatController>().CreateDmgHealText(true, damageAmount, Player, false));

                        Player.GetComponent<PlayerController>().HealParticle.Play();
                        CurarEmocionSound.Play();

                    }
                    else
                    {
                        Debug.Log("atq normal");
                        VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista -= damageAmount;
                        StartCoroutine(CombatScene.GetComponent<CombatController>().CreateDmgHealText(false, damageAmount, Player, false));
                        PlayerRecibeDanyo = true; // Indica que el player deberá realizar la animación de recibir daño

                        Player.GetComponent<PlayerController>().DamageParticle.Play();
                        AtaqueEmocionSound.Play();

                    }

                }

                EnemyAnimator.SetBool("atacar", true);
            }


            else if (Tipo == 1)    // Miedo
            {

                if (HealthEnemigo <= (MaxHealthEnemigo * 0.35f) && !EsperandoHeal)
                {
                    ContadorDeTurnosHeal = 3;
                    EsperandoHeal = true;

                    damageAmount = 10;
                    Debug.Log("se cura miedo");
                    HealthEnemigo += damageAmount;

                    StartCoroutine(CombatScene.GetComponent<CombatController>().CreateDmgHealText(true, damageAmount, gameObject, false));

                    EnemyAnimator.SetBool("atacar", true);

                    HealParticle.Play();
                    CurarEmocionSound.Play();

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
                    // StartCoroutine(DoubleAttack(damageAmount, 1f));
                    PlayerRecibeDanyo = true; // Indica que el player deberá realizar la animación de recibir daño
                    StartCoroutine(DoubleAttack(damageAmount, 0.5f));

                }

            }
            else if (Tipo == 2)                   // Tristeza
            {

                bool repetirTirada;

                do
                {

                    repetirTirada = false;

                    if (SoloTristeza)
                    {
                        if (Player.GetComponent<PlayerController>().Transformacion) // Si el Jugador está transformado el ataque le curará
                        {

                            damageAmount = 2 + Debilidad;
                            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += damageAmount;
                            StartCoroutine(CombatScene.GetComponent<CombatController>().CreateDmgHealText(true, damageAmount, Player, false));
                            Player.GetComponent<PlayerController>().HealParticle.Play();
                            CurarEmocionSound.Play();

                        }
                        else
                        {
                            damageAmount = 2 + Debilidad;
                            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista -= damageAmount;
                            StartCoroutine(CombatScene.GetComponent<CombatController>().CreateDmgHealText(false, damageAmount, Player, false));
                            PlayerRecibeDanyo = true; // Indica que el player deberá realizar la animación de recibir daño
                            Player.GetComponent<PlayerController>().DamageParticle.Play();
                            AtaqueEmocionSound.Play();

                        }

                    }

                    else if (AttackType <= 3.3f)
                    {

                        Debug.Log("Jugador Envenenado");
                        if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                            CombatScene.GetComponent<CombatController>().CreateSpellText("Poisoned", Player);
                        else                                                                  // Spanish
                            CombatScene.GetComponent<CombatController>().CreateSpellText("Envenenado", Player);
                        CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Envenenado = true;
                        CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Veneno += 3;
                        CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDeTurnosEnvenenado += 1;

                        Player.GetComponent<PlayerController>().EffectParticle.Play();
                        AplicarEfectoDeEmocionSound.Play();

                    }
                    else if (AttackType > 3.3f && AttackType <= 6.6 && CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDeTurnosDebilitado <= 0)
                    {

                        Debug.Log("Jugador Debilitado");
                        if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                            CombatScene.GetComponent<CombatController>().CreateSpellText("Weakened", Player);
                        else                                                                  // Spanish
                            CombatScene.GetComponent<CombatController>().CreateSpellText("Debilitado", Player);
                        CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Debilitado = true;
                        CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Debilidad -= 2;
                        CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDeTurnosDebilitado += 2;

                        Player.GetComponent<PlayerController>().EffectParticle.Play();
                        AplicarEfectoDeEmocionSound.Play();

                    }
                    else if (AttackType > 6.6f && !CombatScene.GetComponent<CombatController>().ManaReducido)
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

                        CombatScene.GetComponent<CombatController>().ManaReducido = true; // Indica que el maná ya se ha reducido una vez en este turno

                        if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                            CombatScene.GetComponent<CombatController>().CreateSpellText("Reduce Mana", Player);
                        else                                                                  // Spanish
                            CombatScene.GetComponent<CombatController>().CreateSpellText("Reducir Maná", Player);
                        Player.GetComponent<PlayerController>().ReducirMana = true;

                        Player.GetComponent<PlayerController>().EffectParticle.Play();
                        AplicarEfectoDeEmocionSound.Play();

                    }
                    else // Caso muerto
                    {

                        Debug.Log("Ataca pero no hace nada");
                        repetirTirada = true;               // Indica que la tirada se tiene que repetir
                        AttackType = Random.Range(1f, 11f); // La tirada se repite

                    }

                } while (repetirTirada);

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

                    StartCoroutine(CombatScene.GetComponent<CombatController>().CreateDmgHealText(true, damageAmount, gameObject, false));

                    HealParticle.Play();
                    CurarBossSound.Play();

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

                        if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                            CombatScene.GetComponent<CombatController>().CreateSpellText("Swap", gameObject);
                        else                                                                  // Spanish
                            CombatScene.GetComponent<CombatController>().CreateSpellText("Intercambio", gameObject);

                        EffectParticle.Play();
                        Player.GetComponent<PlayerController>().EffectParticle.Play();
                        AplicarEfectoDeBossSound.Play();

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
                                StartCoroutine(CombatScene.GetComponent<CombatController>().CreateDmgHealText(true, damageAmount, Player, false));

                                Player.GetComponent<PlayerController>().HealParticle.Play();
                                CurarBossSound.Play();

                            }
                            else
                            {

                                VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista -= damageAmount;
                                StartCoroutine(CombatScene.GetComponent<CombatController>().CreateDmgHealText(false, damageAmount, Player, false));
                                PlayerRecibeDanyo = true; // Indica que el player deberá realizar la animación de recibir daño

                                Player.GetComponent<PlayerController>().DamageParticle.Play();
                                AtaqueBossSound.Play();

                            }

                        }
                        else if (AttackType <= 8) // Pone Débil al Jugador
                        {

                            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                                CombatScene.GetComponent<CombatController>().CreateSpellText("Weakened", Player);
                            else                                                                  // Spanish
                                CombatScene.GetComponent<CombatController>().CreateSpellText("Debilitado", Player);
                            CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Debilitado = true;
                            CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Debilidad -= 3;
                            CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDeTurnosDebilitado += 2;

                            Player.GetComponent<PlayerController>().EffectParticle.Play();
                            AplicarEfectoDeBossSound.Play();

                        }
                        else if (AttackType <= 8.5f) // Se pone transformado
                        {

                            Transformacion = true;
                            ContadorDeTurnosTransformacion += 1;
                            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                                CombatScene.GetComponent<CombatController>().CreateSpellText("Transformed", gameObject);
                            else                                                                  // Spanish
                                CombatScene.GetComponent<CombatController>().CreateSpellText("Transformado", gameObject);

                            EffectParticle.Play();
                            AplicarEfectoDeBossSound.Play();

                        }
                        else
                        {

                            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                                CombatScene.GetComponent<CombatController>().CreateSpellText("Reduce Mana", Player);
                            else                                                                  // Spanish
                                CombatScene.GetComponent<CombatController>().CreateSpellText("Reducir Maná", Player);
                            Player.GetComponent<PlayerController>().ReducirMana = true;

                            Player.GetComponent<PlayerController>().EffectParticle.Play();
                            AplicarEfectoDeBossSound.Play();

                        }

                    }

                }

                EnemyAnimator.SetBool("atacar", true);

            }

            //ControlEsperanzado();

        }
        else
        {

            Debug.Log("El Enemigo está Bloqueado");
            if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
                CombatScene.GetComponent<CombatController>().CreateSpellText("Stunned", gameObject);
            else                                                                  // Spanish
                CombatScene.GetComponent<CombatController>().CreateSpellText("Bloqueado", gameObject);
            Bloqueado = false;

            BloqueadoSound.Play();

        }

        // Debil (Enemigo)
        if (ContadorDeTurnosDebilitado > 0)
        {

            ContadorDeTurnosDebilitado--;
            ContadorDebilitado++;

            if (ContadorDebilitado == 3)
            {

                Debilidad += 2;
                ContadorDebilitado = 0; // Se resetea cada vez que se termina un efecto de Débil

            }

            if (ContadorDeTurnosDebilitado == 0)
            {
                Debilidad = 0;
                Debilitado = false;
                debilidad_icon = 0;
                for (int i = 0; i < ActiveSpell; i++)
                {
                    if (ActiveSpellGameobject[i].GetComponent<EffectIcon>().Tipo == 1)
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
                    if (ActiveSpellGameobject[i].GetComponent<EffectIcon>().Tipo == 4)
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
                    if (ActiveSpellGameobject[i].GetComponent<EffectIcon>().Tipo == 2)
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
        //if (Envenenado && veneno_icon == 0)
        //{
        //    GameObject ClonSpell;
        //    ClonSpell = Instantiate(CombatScene.GetComponent<CombatController>().PrefabSpell[0]); // Crea el clon del prefab de veneno ([0])
        //    ClonSpell.transform.position = SpellCoords[ActiveSpell];
        //    ActiveSpellGameobject[ActiveSpell] = ClonSpell;
        //    ActiveSpellGap[ActiveSpell] = true;
        //    ActiveSpell++;
        //    veneno_icon++;
        //}
        //if (Debilitado && debilidad_icon == 0)
        //{
        //    GameObject ClonSpell;
        //    ClonSpell = Instantiate(CombatScene.GetComponent<CombatController>().PrefabSpell[1]); // Crea el clon del prefab de debilidad ([1])
        //    ClonSpell.transform.position = SpellCoords[ActiveSpell];
        //    ActiveSpellGameobject[ActiveSpell] = ClonSpell;
        //    ActiveSpellGap[ActiveSpell] = true;
        //    ActiveSpell++;
        //    debilidad_icon++;
        //}
        //if (Fuerte && fuerte_icon == 0)
        //{
        //    GameObject ClonSpell;
        //    ClonSpell = Instantiate(CombatScene.GetComponent<CombatController>().PrefabSpell[2]); // Crea el clon del prefab de fuerza ([2])
        //    ClonSpell.transform.position = SpellCoords[ActiveSpell];
        //    ActiveSpellGameobject[ActiveSpell] = ClonSpell;
        //    ActiveSpellGap[ActiveSpell] = true;
        //    ActiveSpell++;
        //    fuerte_icon++;
        //}
        if (Debilitado && debilidad_icon == 0)
        {
            GameObject ClonSpell;
            ClonSpell = Instantiate(CombatScene.GetComponent<CombatController>().IconSpell);
            ClonSpell.transform.SetParent(GameObject.Find("CanvasEffects").transform, false);
            ClonSpell.GetComponent<Image>().sprite = CombatScene.GetComponent<CombatController>().IconSpellSprites[1];
            ClonSpell.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            ClonSpell.transform.position = SpellCoords[ActiveSpell];
            ClonSpell.GetComponent<EffectIcon>().Personaje = gameObject;
            ClonSpell.GetComponent<EffectIcon>().EsPlayer = false;
            ClonSpell.GetComponent<EffectIcon>().Tipo = 1;
            ActiveSpellGameobject[ActiveSpell] = ClonSpell;
            ActiveSpellGap[ActiveSpell] = true;
            ActiveSpell++;
            debilidad_icon++;
        }
        if (Fuerte && fuerte_icon == 0)
        {
            GameObject ClonSpell;
            ClonSpell = Instantiate(CombatScene.GetComponent<CombatController>().IconSpell);
            ClonSpell.transform.SetParent(GameObject.Find("CanvasEffects").transform, false);
            ClonSpell.GetComponent<Image>().sprite = CombatScene.GetComponent<CombatController>().IconSpellSprites[2];
            ClonSpell.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            ClonSpell.transform.position = SpellCoords[ActiveSpell];
            ClonSpell.GetComponent<EffectIcon>().Personaje = gameObject;
            ClonSpell.GetComponent<EffectIcon>().EsPlayer = false;
            ClonSpell.GetComponent<EffectIcon>().Tipo = 2;
            ActiveSpellGameobject[ActiveSpell] = ClonSpell;
            ActiveSpellGap[ActiveSpell] = true;
            ActiveSpell++;
            fuerte_icon++;
        }
        if (Envenenado && veneno_icon == 0)
        {
            GameObject ClonSpell;
            ClonSpell = Instantiate(CombatScene.GetComponent<CombatController>().IconSpell);
            ClonSpell.transform.SetParent(GameObject.Find("CanvasEffects").transform, false);
            ClonSpell.GetComponent<Image>().sprite = CombatScene.GetComponent<CombatController>().IconSpellSprites[4];
            ClonSpell.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            ClonSpell.transform.position = SpellCoords[ActiveSpell];
            ClonSpell.GetComponent<EffectIcon>().Personaje = gameObject;
            ClonSpell.GetComponent<EffectIcon>().EsPlayer = false;
            ClonSpell.GetComponent<EffectIcon>().Tipo = 4;
            ActiveSpellGameobject[ActiveSpell] = ClonSpell;
            ActiveSpellGap[ActiveSpell] = true;
            ActiveSpell++;
            veneno_icon++;
        }
        if (Bloqueado && bloqueado_icon == 0)
        {
            GameObject ClonSpell;
            ClonSpell = Instantiate(CombatScene.GetComponent<CombatController>().IconSpell);
            ClonSpell.transform.SetParent(GameObject.Find("CanvasEffects").transform, false);
            ClonSpell.GetComponent<Image>().sprite = CombatScene.GetComponent<CombatController>().IconSpellSprites[0];
            ClonSpell.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            ClonSpell.transform.position = SpellCoords[ActiveSpell];
            ClonSpell.GetComponent<EffectIcon>().Personaje = gameObject;
            ClonSpell.GetComponent<EffectIcon>().EsPlayer = false;
            ClonSpell.GetComponent<EffectIcon>().Tipo = 0;
            ActiveSpellGameobject[ActiveSpell] = ClonSpell;
            ActiveSpellGap[ActiveSpell] = true;
            ActiveSpell++;
            bloqueado_icon++;
        }

        if (!Bloqueado)
        {
            bloqueado_icon = 0;
            for (int i = 0; i < ActiveSpell; i++)
            {
                if(ActiveSpellGameobject[i] != null)
                {

                    if (ActiveSpellGameobject[i].GetComponent<EffectIcon>().Tipo == 0)
                    {
                        ReestructuraIcons(i);
                        break;
                    }

                }
            }
        }
        if (ContadorDeTurnosEnvenenado <= 0)
        {
            ContadorDeTurnosEnvenenado = 0;
            Veneno = 0;
            Envenenado = false;
            veneno_icon = 0;
            for (int i = 0; i<ActiveSpell; i++)
            {
                if(ActiveSpellGameobject[i] != null)
                {

                    if (ActiveSpellGameobject[i].GetComponent<EffectIcon>().Tipo == 4)
                    {
                        ReestructuraIcons(i);
                        break;
                    }

                }
            }

        }
        if (ContadorDeTurnosDebilitado <= 0)
        {
            ContadorDeTurnosDebilitado = 0;
            Debilidad = 0;
            Debilitado = false;
            debilidad_icon = 0;
            for (int i = 0; i < ActiveSpell; i++)
            {
                if(ActiveSpellGameobject[i] != null)
                {

                    if (ActiveSpellGameobject[i].GetComponent<EffectIcon>().Tipo == 1)
                    {
                        ReestructuraIcons(i);
                        break;
                    }

                }
            }
        }
        if (ContadorDeTurnosFuerte <= 0)
        {
            ContadorDeTurnosFuerte = 0;
            Fuerza = 0;
            Fuerte = false;
            fuerte_icon = 0;
            for (int i = 0; i < ActiveSpell; i++)
            {
                if(ActiveSpellGameobject[i] != null)
                {

                    if (ActiveSpellGameobject[i].GetComponent<EffectIcon>().Tipo == 2)
                    {
                        ReestructuraIcons(i);
                        break;
                    }

                }
            }
        }

    }

    public void ControlEnemyAnimation(int valor)
    {

        if (valor == 0) // Si la animacion de atacar ha terminado
        {

            EnemyAnimator.SetBool("atacar", false);
            
            // Controla si el Player debe ejecutar
            if (PlayerRecibeDanyo)
            {

                CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().PlayerAnimator.SetBool("danyo", true);
                PlayerRecibeDanyo = false;

            }

        }

        if (valor == 1) // Si la animacion de recibir daño ha terminado
            EnemyAnimator.SetBool("danyo", false);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Arrow")
        {
            showName = true;
            //ArrowEmitter.GetComponent<ArrowEmitter>().ArrowHead.GetComponent<Image>().sprite = ArrowEmitter.GetComponent<ArrowEmitter>().ArrowHeadPrefab_A;
            for (int j = 0; j < ArrowEmitter.GetComponent<ArrowEmitter>().arrowNodes.Count; j++)
            {

                //ArrowEmitter.GetComponent<ArrowEmitter>().arrowNodes[j].GetComponent<Image>().color = Color.red;
                // ArrowEmitter.GetComponent<ArrowEmitter>().arrowNodes[j].GetComponent<Image>().sprite = ArrowEmitter.GetComponent<ArrowEmitter>().ArrowNodePrefab_A;
                if (j == ArrowEmitter.GetComponent<ArrowEmitter>().arrowNodes.Count - 1)
                    ArrowEmitter.GetComponent<ArrowEmitter>().arrowNodes[j].GetComponent<Image>().sprite = ArrowEmitter.GetComponent<ArrowEmitter>().ArrowHeadEnabled;
                else
                    ArrowEmitter.GetComponent<ArrowEmitter>().arrowNodes[j].GetComponent<Image>().sprite = ArrowEmitter.GetComponent<ArrowEmitter>().ArrowNodeEnabled;

            }
           // ArrowEmitter.GetComponent<ArrowEmitter>().arrowNodes[ArrowEmitter.GetComponent<ArrowEmitter>().arrowNodes.Count].GetComponent<Image>().sprite = ArrowEmitter.GetComponent<ArrowEmitter>().ArrowHeadPrefab_A;

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
            showName = false;
            // ArrowEmitter.GetComponent<ArrowEmitter>().ArrowHeadPrefab.GetComponent<Image>().sprite = ArrowEmitter.GetComponent<ArrowEmitter>().ArrowHeadPrefab_D;
            for (int j = 0; j < ArrowEmitter.GetComponent<ArrowEmitter>().arrowNodes.Count; j++)
            {

                //ArrowEmitter.GetComponent<ArrowEmitter>().arrowNodes[j].GetComponent<Image>().color = Color.grey;
                // ArrowEmitter.GetComponent<ArrowEmitter>().arrowNodes[j].GetComponent <Image>().sprite = ArrowEmitter.GetComponent<ArrowEmitter>().ArrowNodePrefab_D;
                if (j == ArrowEmitter.GetComponent<ArrowEmitter>().arrowNodes.Count - 1)
                    ArrowEmitter.GetComponent<ArrowEmitter>().arrowNodes[j].GetComponent<Image>().sprite = ArrowEmitter.GetComponent<ArrowEmitter>().ArrowHeadDisabled;
                else
                    ArrowEmitter.GetComponent<ArrowEmitter>().arrowNodes[j].GetComponent<Image>().sprite = ArrowEmitter.GetComponent<ArrowEmitter>().ArrowNodeDisabled;

            }
           // ArrowEmitter.GetComponent<ArrowEmitter>().arrowNodes[ArrowEmitter.GetComponent<ArrowEmitter>().arrowNodes.Count].GetComponent<Image>().sprite = ArrowEmitter.GetComponent<ArrowEmitter>().ArrowHeadPrefab_D;



            //ArrowEmitter.GetComponent<ArrowEmitter>().OverEnemy = false;
            OverEnemy = false;
            AuraEnemigo.SetActive(false);
            ArrowEmitter.GetComponent<ArrowEmitter>().Carta.GetComponent<CardController>().EnemigoSeleccionado = -1;

        }
    }

    public IEnumerator DoubleAttack(int damageAmount, float tiempo)
    {

       

        if(Player.GetComponent<PlayerController>().Transformacion)
        {

            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += damageAmount;
            StartCoroutine(CombatScene.GetComponent<CombatController>().CreateDmgHealText(true, damageAmount, Player, false));
            Player.GetComponent<PlayerController>().HealParticle.Play();
            EnemyAnimator.SetBool("atacar", true);
            CurarEmocionSound.Play();

            yield return new WaitForSeconds(tiempo);

            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista += damageAmount;
            StartCoroutine(CombatScene.GetComponent<CombatController>().CreateDmgHealText(true, damageAmount, Player, false));
            //Player.GetComponent<PlayerController>().HealParticle.Play();
            // EnemyAnimator.SetBool("atacar", true);
            CurarEmocionSound.Play();

        }
        else
        {

            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista -= damageAmount;
            StartCoroutine(CombatScene.GetComponent<CombatController>().CreateDmgHealText(false, damageAmount, Player, false));
            Player.GetComponent<PlayerController>().DamageParticle.Play();
            EnemyAnimator.SetBool("atacar", true);
            AtaqueDobleEmocionSound.Play();

            yield return new WaitForSeconds(tiempo);

            VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista -= damageAmount;
            StartCoroutine(CombatScene.GetComponent<CombatController>().CreateDmgHealText(false, damageAmount, Player, false));
            //Player.GetComponent<PlayerController>().DamageParticle.Play();
            // EnemyAnimator.SetBool("atacar", true);
            AtaqueDobleEmocionSound.Play();

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

    public void ControlEsperanzado(bool esperar)
    {

        if (Esperanzado)
        {

            HealthEnemigo += Esperanza;
           // CombatScene.GetComponent<CombatController>().wait(0.5f);
            StartCoroutine(CombatScene.GetComponent<CombatController>().CreateDmgHealText(true, Esperanza, gameObject, esperar));

        }

    }

    public void ControlEnvenenado(bool esperar)
    {

        if (Envenenado) // Creo que esto tiene que ser cada vez que ataca el Jugador, no cada vez que usa una carta (hablar con Flipper)
        {

            HealthEnemigo -= Veneno;
            EnemyAnimator.SetBool("danyo", true);
            StartCoroutine(CombatScene.GetComponent<CombatController>().CreateDmgHealText(false, Veneno, gameObject, esperar));
            DamageParticle.Play();

        }

    }

    public void ControlAcumulacionDanyoBoss()
    {

        contAcumulacionDanyoBoss = 0; // Resetea el daño recibido
        BossDanyoComprobado = true;   // Indica que ya se ha controlado para que si en este mismo turno se vuelve a intentar comprobar, no lo haga, sobretodo para los DoubleAttack()

        //yield return new WaitForSeconds(0.5f);
        // CombatScene.GetComponent<CombatController>().wait(0.5f);
        if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
            CombatScene.GetComponent<CombatController>().CreateSpellText("He is tired of you", gameObject);
        else                                                                  // Spanish
            CombatScene.GetComponent<CombatController>().CreateSpellText("Se ha cansado de ti", gameObject);
        CombatScene.GetComponent<CombatController>().botonTurno.GetComponent<FinalizarTurnoButton>().OnClick();

    }

    public void OnMouseEnter()
    {
       showName2 = true;
    }

    public void OnMouseExit()
    {
        showName2 = false;
    }

    void ShowName()
    {
        if(Name != null)
        {

            if (showName || showName2)
                Name.gameObject.SetActive(true);
            else if (!showName && !showName2)
                Name.gameObject.SetActive(false);

        }
    }

    public void EndMuerteAnimation()
    {

        EndMuerteTransformacion = true;
        CombatScene.GetComponent<CombatController>().NumEnemiesDefeat++;
        PositionMuerte = ListPositionsMuerte[CombatScene.GetComponent<CombatController>().NumEnemiesDefeat - 1];

    }

}
