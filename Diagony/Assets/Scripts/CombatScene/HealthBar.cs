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
    [SerializeField] public int Identify; // 0 ira, 1 miedo, 2 tristeza, 3 prota
    [SerializeField] public GameObject _player;
    [SerializeField] public GameObject _ira;
    [SerializeField] public GameObject _miedo;
    [SerializeField] public GameObject _tristeza;

    void Start()
    {
        
    }

    
    void Update()
    {
        if (Identify == 0)
        {
            ValueHealthBar(_ira.GetComponent<EnemyController>().HealthEnemigo, _ira.GetComponent<EnemyController>().MaxHealthEnemigo);
            ControlHealthbar(_ira.GetComponent<EnemyController>().HealthEnemigo);
        }
        else if (Identify == 1)
        {
            ValueHealthBar(_miedo.GetComponent<EnemyController>().HealthEnemigo, _miedo.GetComponent<EnemyController>().MaxHealthEnemigo);
            ControlHealthbar(_miedo.GetComponent<EnemyController>().HealthEnemigo);
        }
        else if (Identify == 2)
        {
            ValueHealthBar(_tristeza.GetComponent<EnemyController>().HealthEnemigo, _tristeza.GetComponent<EnemyController>().MaxHealthEnemigo);
            ControlHealthbar(_tristeza.GetComponent<EnemyController>().HealthEnemigo);
        }
        else
        {
            ValueHealthBar(_player.GetComponent<PlayerController>().HealthProtagonista, _player.GetComponent<PlayerController>().MaxHealthProtagonista);
            ControlHealthbar(_player.GetComponent<PlayerController>().HealthProtagonista);
        }

    }

    public void ValueHealthBar(int Health, int MaxHealth)
    {

        HealthLabel.text = Health + "/" + MaxHealth; // representa los valores en el texto
        HealthBarSlider.value = Health; //asigna el valor al slider
        
        if (Health <= 1)
            fill.color = Color.red; //colorea de rojo en caso de tener menos de 1 de vida
        else if (Health <=3)
            fill.color = Color.yellow;
        else
            fill.color = Color.green;
    }

    public void ControlHealthbar(int Health)
    {

        if (Health <= 0)
            Destroy(gameObject);

    }
}
