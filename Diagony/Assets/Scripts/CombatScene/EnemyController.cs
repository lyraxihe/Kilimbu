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
    public Animator EnemyAnimator;
    [SerializeField] bool animation_damage;
   


    // Start is called before the first frame update
    void Start()
    {
        VariablesGlobales = GameObject.Find("VariablesGlobales");

        SetHealthEnemigo();
        SetAtackEnenmigo();
        EnemyAnimator.SetInteger("enemy_id", Id);
        EnemyAnimator.SetBool("atacar", animation_damage);

    }

    // Update is called once per frame
    void Update()
    {

        ControlHealthEnemigo();

    }

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

    public void SetAtackEnenmigo()
    {

        if (Tipo == 0)         // Ira
            AtackEnemigo = 20;
        else if (Tipo == 1)    // Miedo
            AtackEnemigo = 15;
        else                   // Tristeza
            AtackEnemigo = 10;

    }

    public void Atacar()
    {

        EnemyAnimator.SetBool("atacar", true);
        VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista -= Random.Range(AtackEnemigo - 5, AtackEnemigo + 1); //hace un golpe entre atq-5 y atq

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

}
