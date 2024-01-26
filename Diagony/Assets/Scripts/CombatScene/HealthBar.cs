using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBar : MonoBehaviour
{
    [SerializeField] public TMP_Text HealthLabel;
    [SerializeField] UnityEngine.UI.Slider HealthBarSlider;
    [SerializeField] UnityEngine.UI.Image fill;
    /*[SerializeField] public int Identify;*/ // 0 ira, 1 miedo, 2 tristeza, 3 prota
    [SerializeField] public bool EsPlayer;
    [SerializeField] public GameObject _player;
    [SerializeField] public GameObject _enemy;
    [SerializeField] public GameObject TextCharacter;

    private int _maxHealth; // Para controlar la vida Máxima en el script

    void Start()
    {
        
        //Color verde = new Color(0.541f, 0.658f, 0.596f, 1.0f);
        //Color amarillo = new Color(0.86f, 0.89f, 0.776f, 1.0f);
        //Color rojo = new Color(0.447f, 0.149f, 0.27f, 1.0f);

    }

    
    void Update()
    {
        
        if (EsPlayer)
        {
            ValueHealthBar(_player.GetComponent<PlayerController>().HealthProtagonista, _player.GetComponent<PlayerController>().MaxHealthProtagonista);
            ControlHealthbar(_player.GetComponent<PlayerController>().HealthProtagonista);
        }
       else
        {
            ValueHealthBar(_enemy.GetComponent<EnemyController>().HealthEnemigo, _enemy.GetComponent<EnemyController>().MaxHealthEnemigo);
            ControlHealthbar(_enemy.GetComponent<EnemyController>().HealthEnemigo);
        }

    }

    public void ValueHealthBar(int Health, int MaxHealth)
    {

        HealthLabel.text = Health + "/" + MaxHealth; // representa los valores en el texto
        HealthBarSlider.maxValue = MaxHealth;
        HealthBarSlider.value = Health; //asigna el valor al slider
        
        if (Health <= (MaxHealth * 0.3f))
            fill.color = new Color(0.447f, 0.149f, 0.27f, 1.0f); // colorea de rojo en caso de tener menos de 15 de vida
        else if (Health <= (MaxHealth * 0.5f))
            fill.color = new Color(0.86f, 0.89f, 0.776f, 1.0f); // amarillo
        else
            fill.color = new Color(0.541f, 0.658f, 0.596f, 1.0f); // verde
    }

    public void ControlHealthbar(int Health)
    {

        if (Health <= 0)
        {
            if (EsPlayer)
            {
                //Destroy(_player);
                //Destroy(TextCharacter);

            }
            else
            {
                Destroy(_enemy);
                Destroy(TextCharacter);
            }

            Destroy(gameObject);
        }
           

    }
}
