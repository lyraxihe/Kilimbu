using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public GameObject VariablesGlobales;
    public GameObject CombatScene;

    public int Tipo; // 0 = Ira | 1 = Miedo | 2 = Tristeza
    public int Id; // ID que tiene en la lista de enemigos
    public int HealthEnemigo;
    public int MaxHealthEnemigo;
    private int AtackEnemigo;

    // Start is called before the first frame update
    void Start()
    {

        SetHealthEnemigo();
        SetAtackEnenmigo();

    }

    // Update is called once per frame
    void Update()
    {

        ControlHealthEnemigo();

    }

    public void SetHealthEnemigo()
    {

        if (Tipo == 0)         // Ira
            MaxHealthEnemigo = 5;
        else if (Tipo == 1)    // Miedo
            MaxHealthEnemigo = 5;
        else                   // Tristeza
            MaxHealthEnemigo = 5;

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

    public void SetAtackEnenmigo()
    {

        if (Tipo == 0)         // Ira
            AtackEnemigo = 10;
        else if (Tipo == 1)    // Miedo
            AtackEnemigo = 10;
        else                   // Tristeza
            AtackEnemigo = 10;

    }

    public void Atacar()
    {

        VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista -= AtackEnemigo;

    }

}
