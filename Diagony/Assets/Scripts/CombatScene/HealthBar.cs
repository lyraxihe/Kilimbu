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

    void Start()
    {
     
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
        
        if (Health <= 15)
            fill.color = Color.red; //colorea de rojo en caso de tener menos de 15 de vida
        else if (Health <=35)
            fill.color = Color.yellow;
           // fill.color = new Color { 1, 1, 1, 1 };
        else
            fill.color = Color.green;
    }

    public void ControlHealthbar(int Health)
    {

        if (Health <= 0)
        {
            if (EsPlayer)
            {
                Destroy(_player);
                Destroy(TextCharacter);

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
