using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public GameObject VariablesGlobales;
    public GameObject CombatScene;

    public int MaxHealthProtagonista;
    public int HealthProtagonista;
    public Animator PlayerAnimator;
    [SerializeField] bool animation_damage;

    public GameObject Name;
    public int ContadorDeTurnos;

    // Bloqueado
    public bool Bloqueado;


    // Débil
    public bool Debilitado;
    public int Debilidad;
    public int ContadorDeTurnosDebilitado;
    public int ContadorDebilitado;
    public int ContadorDeTurnosDebilitadoDevolverIra;
    public int ContadorDebilitadoDevolverIra;

    // Envenenado
    public bool Envenenado;
    public int Veneno;
    public int ContadorDeTurnosEnvenenado;
    public int ContadorEnvenenado;
    public int ContadorDeTurnosEnvenenadoDevolverIra;
    public int ContadorEnvenenadoDevolverIra;

    // Fuerte
    public bool Fuerte;
    public int Fuerza;
    public int ContadorDeTurnosFuerte;
    public int ContadorFuerza;

    // Esperanza
    public bool Esperanzado;
    public int Esperanza;
    public int ContadorDeTurnosEsperanzado;
    public int ContadorEsperanza;

    // Transformacion (Los ataques al Jugador le curan en vez de hacerle daño)
    public bool Transformacion;
    public int ContadorDeTurnosTransformacion;

    // Reducir Mana (ataque de Tristeza)
    public bool ReducirMana;


    bool[] ActiveSpellGap = new bool[5];
    [SerializeField] public int ActiveSpell;
    Vector2[] SpellCoords = new Vector2[5];
    public GameObject[] ActiveSpellGameobject = new GameObject[5];
    float x_inicial_spell;
    float y_inicial_spell;

    public int debilidad_icon;
    public int veneno_icon;
    public int fuerte_icon;
    public int transformacion_icon;
    public int esperanza_icon;
    public int bloqueado_icon;


    void Start()
    {
        VariablesGlobales = GameObject.Find("VariablesGlobales");

        ContadorDeTurnos = 0;

        //inicializo hechizos
        debilidad_icon = 0;
        veneno_icon = 0;
        fuerte_icon = 0;
        esperanza_icon = 0;
        transformacion_icon = 0;

        ActiveSpell = 0;

        x_inicial_spell = -5.6f;
        y_inicial_spell = 1.2f;


        for (int i = 0; i < VariablesGlobales.GetComponent<VariablesGlobales>().SpellNumber; i++)
        {
            ActiveSpellGap[i] = false;
            SpellCoords[i] = new Vector2(x_inicial_spell += 0.5f, y_inicial_spell);

        }

        // Bloqueado
        Bloqueado = false;
        bloqueado_icon = 0;

        // Débil
        Debilitado = false;
        Debilidad = 0;
        ContadorDeTurnosDebilitado = 0;
        ContadorDebilitado = 0;
        ContadorDeTurnosDebilitadoDevolverIra = 0;
        ContadorDebilitadoDevolverIra = 0;

        // Envenenado
        Envenenado = false;
        Veneno = 0;
        ContadorDeTurnosEnvenenado = 0;
        ContadorEnvenenado = 0;
        ContadorDeTurnosEnvenenadoDevolverIra = 0;
        ContadorEnvenenadoDevolverIra = 0;

        // Fuerte
        Fuerte = false;
        Fuerza = 0;
        ContadorDeTurnosFuerte = 0;
        ContadorFuerza = 0;

        // Esperanza
        Esperanzado = false;
        Esperanza = 0;
        ContadorDeTurnosEsperanzado = 0;
        ContadorEsperanza = 0;

        // Transformacion
        Transformacion = false;
        ContadorDeTurnosTransformacion = 0;

        // Reducir Mana
        ReducirMana = false;

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

        if (valor == 1) // Indica que ha finalizado la animacion de recibir danyo
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
        {
            ContadorDeTurnosFuerte = 0;
        }

        if (ContadorDeTurnosTransformacion < 0)
        {
            ContadorDeTurnosTransformacion = 0;
        }

        if (ContadorDeTurnosEnvenenado < 0)
        {
            ContadorDeTurnosEsperanzado = 0;
        }


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
        //if (Esperanzado && esperanza_icon == 0)
        //{
        //    GameObject ClonSpell;
        //    ClonSpell = Instantiate(CombatScene.GetComponent<CombatController>().PrefabSpell[3]); // Crea el clon del prefab de esperanza ([3])
        //    ClonSpell.transform.position = SpellCoords[ActiveSpell];
        //    ActiveSpellGameobject[ActiveSpell] = ClonSpell;
        //    ActiveSpellGap[ActiveSpell] = true;
        //    ActiveSpell++;
        //    esperanza_icon++;
        //}
        //if (Transformacion && transformacion_icon == 0)
        //{
        //    GameObject ClonSpell;
        //    ClonSpell = Instantiate(CombatScene.GetComponent<CombatController>().PrefabSpell[4]); // Crea el clon del prefab de transformado ([4])
        //    ClonSpell.transform.position = SpellCoords[ActiveSpell];
        //    ActiveSpellGameobject[ActiveSpell] = ClonSpell;
        //    ActiveSpellGap[ActiveSpell] = true;
        //    ActiveSpell++;
        //    transformacion_icon++;
        //}

        if (Bloqueado && bloqueado_icon == 0)
        {
            GameObject ClonSpell;
            ClonSpell = Instantiate(CombatScene.GetComponent<CombatController>().IconSpell);
            ClonSpell.transform.SetParent(GameObject.Find("CanvasEffects").transform, false);
            ClonSpell.GetComponent<Image>().sprite = CombatScene.GetComponent<CombatController>().IconSpellSprites[0];
            ClonSpell.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            ClonSpell.transform.position = SpellCoords[ActiveSpell];
            ClonSpell.GetComponent<EffectIcon>().Personaje = gameObject;
            ClonSpell.GetComponent<EffectIcon>().EsPlayer = true;
            ClonSpell.GetComponent<EffectIcon>().Tipo = 0;
            ActiveSpellGameobject[ActiveSpell] = ClonSpell;
            ActiveSpellGap[ActiveSpell] = true;
            ActiveSpell++;
            bloqueado_icon++;
        }
        if (Debilitado && debilidad_icon == 0)
        {
            GameObject ClonSpell;
            ClonSpell = Instantiate(CombatScene.GetComponent<CombatController>().IconSpell);
            ClonSpell.transform.SetParent(GameObject.Find("CanvasEffects").transform, false);
            ClonSpell.GetComponent<Image>().sprite = CombatScene.GetComponent<CombatController>().IconSpellSprites[1];
            ClonSpell.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            ClonSpell.transform.position = SpellCoords[ActiveSpell];
            ClonSpell.GetComponent<EffectIcon>().Personaje = gameObject;
            ClonSpell.GetComponent<EffectIcon>().EsPlayer = true;
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
            ClonSpell.GetComponent<EffectIcon>().EsPlayer = true;
            ClonSpell.GetComponent<EffectIcon>().Tipo = 2;
            ActiveSpellGameobject[ActiveSpell] = ClonSpell;
            ActiveSpellGap[ActiveSpell] = true;
            ActiveSpell++;
            fuerte_icon++;
        }
        if (Esperanzado && esperanza_icon == 0)
        {
            GameObject ClonSpell;
            ClonSpell = Instantiate(CombatScene.GetComponent<CombatController>().IconSpell);
            ClonSpell.transform.SetParent(GameObject.Find("CanvasEffects").transform, false);
            ClonSpell.GetComponent<Image>().sprite = CombatScene.GetComponent<CombatController>().IconSpellSprites[3];
            ClonSpell.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            ClonSpell.transform.position = SpellCoords[ActiveSpell];
            ClonSpell.GetComponent<EffectIcon>().Personaje = gameObject;
            ClonSpell.GetComponent<EffectIcon>().EsPlayer = true;
            ClonSpell.GetComponent<EffectIcon>().Tipo = 3;
            ActiveSpellGameobject[ActiveSpell] = ClonSpell;
            ActiveSpellGap[ActiveSpell] = true;
            ActiveSpell++;
            esperanza_icon++;
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
            ClonSpell.GetComponent<EffectIcon>().EsPlayer = true;
            ClonSpell.GetComponent<EffectIcon>().Tipo = 4;
            ActiveSpellGameobject[ActiveSpell] = ClonSpell;
            ActiveSpellGap[ActiveSpell] = true;
            ActiveSpell++;
            veneno_icon++;
        }
        if (Transformacion && transformacion_icon == 0)
        {
            GameObject ClonSpell;
            ClonSpell = Instantiate(CombatScene.GetComponent<CombatController>().IconSpell);
            ClonSpell.transform.SetParent(GameObject.Find("CanvasEffects").transform, false);
            ClonSpell.GetComponent<Image>().sprite = CombatScene.GetComponent<CombatController>().IconSpellSprites[5];
            ClonSpell.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            ClonSpell.transform.position = SpellCoords[ActiveSpell];
            ClonSpell.GetComponent<EffectIcon>().Personaje = gameObject;
            ClonSpell.GetComponent<EffectIcon>().EsPlayer = true;
            ClonSpell.GetComponent<EffectIcon>().Tipo = 5;
            ActiveSpellGameobject[ActiveSpell] = ClonSpell;
            ActiveSpellGap[ActiveSpell] = true;
            ActiveSpell++;
            transformacion_icon++;
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

    public void EliminarSpell(int SpellID)
    {
        if (SpellID == 0)
        {
            for (int i = 0; i < ActiveSpell; i++)
            {
                if (ActiveSpellGameobject[i].GetComponent<EffectIcon>().Tipo == 4)
                {
                    ReestructuraIcons(i);
                    break;
                }
            }
        }
        else if (SpellID == 1)
        {
            for (int i = 0; i < ActiveSpell; i++)
            {
                if (ActiveSpellGameobject[i].GetComponent<EffectIcon>().Tipo == 1)
                {
                    ReestructuraIcons(i);
                    break;
                }
            }
        }
        else if (SpellID == 2)
        {
            for (int i = 0; i < ActiveSpell; i++)
            {
                if (ActiveSpellGameobject[i].GetComponent<EffectIcon>().Tipo == 2)
                {
                    ReestructuraIcons(i);
                    break;
                }
            }
        }
        else if (SpellID == 3)
        {
            for (int i = 0; i < ActiveSpell; i++)
            {
                if (ActiveSpellGameobject[i].GetComponent<EffectIcon>().Tipo == 3)
                {
                    ReestructuraIcons(i);
                    break;
                }
            }
        }
        else if (SpellID == 4)
        {
            for (int i = 0; i < ActiveSpell; i++)
            {
                if (ActiveSpellGameobject[i].GetComponent<EffectIcon>().Tipo == 5)
                {
                    ReestructuraIcons(i);
                    break;
                }
            }
        }
        else if (SpellID == 5)
        {
            for (int i = 0; i < ActiveSpell; i++)
            {
                if (ActiveSpellGameobject[i].GetComponent<EffectIcon>().Tipo == 0)
                {
                    ReestructuraIcons(i);
                    break;
                }
            }
        }

    }

    public void OnMouseOver()
    {
        Name.gameObject.SetActive(true);
    }
    public void OnMouseExit()
    {
        Name.gameObject.SetActive(false);
    }

}
