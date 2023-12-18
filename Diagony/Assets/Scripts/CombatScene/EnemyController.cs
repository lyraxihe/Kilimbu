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

    public int Tipo; // 0 = Ira | 1 = Miedo | 2 = Tristeza
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
    public int ContadorDebilitados;

    [SerializeField] public bool Envenenado;
    public int Veneno;
    public int ContadorDeTurnosEnvenenado;
    public int ContadorEnvenenados;


    [SerializeField] public bool Fuerte;
    public int Fuerza;
    public int ContadorDeTurnosFuerte;
    public int ContadorFuerte;

    public int ContadorDeTurnosHeal;
    [SerializeField] public bool EsperandoHeal;

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
        ContadorDebilitados = 0;

        Envenenado = false;
        Veneno = 0;
        ContadorDeTurnosEnvenenado = 0;
        ContadorEnvenenados = 0;

        Fuerte = false;
        Fuerza = 0;
        ContadorDeTurnosFuerte = 0;
        ContadorFuerte = 0;

        ContadorDeTurnosHeal = 0;
        EsperandoHeal = false;

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
                        ContadorFuerte = 0; // Se resetea cada vez que se termina un efecto de Fuerza

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


            else                   // Tristeza
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
            ContadorDebilitados++;

            if (ContadorDebilitados == 3)
            {

                Debilidad += 3;
                ContadorDebilitados = 0; // Se resetea cada vez que se termina un efecto de Débil

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
            ContadorEnvenenados++;

            if (ContadorEnvenenados == 3)
            {

                Veneno -= 3;
                ContadorEnvenenados = 0; // Se resetea cada vez que se termina un efecto de Envenenado

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
            ContadorFuerte++;

            if (ContadorFuerte == 3)
            {

                Fuerza += 3;
                ContadorFuerte = 0; // Se resetea cada vez que se termina un efecto de fuerza

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
}
