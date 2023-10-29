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
            ValueHealthBar(_ira.GetComponent<Ira>().Health, _ira.GetComponent<Ira>().MaxHealth);
        }
        else if (Identify == 1)
        {
            ValueHealthBar(_miedo.GetComponent<Miedo>().Health, _miedo.GetComponent<Miedo>().MaxHealth);
        }
        else if (Identify == 2)
        {
            ValueHealthBar(_tristeza.GetComponent<Tristeza>().Health, _tristeza.GetComponent<Tristeza>().MaxHealth);
        }
        else
        {
            ValueHealthBar(_player.GetComponent<VariablesGlobales>().HealthProtagonista, _player.GetComponent<VariablesGlobales>().MaxHealthProtagonista);
        }

    }

    public void ValueHealthBar(int Health, int MaxHealth)
    {

        HealthLabel.text = Health + "/" + MaxHealth; // representa los valores en el texto
        HealthBarSlider.value = Health; //asigna el valor al slider
        if (Health > 50)
        {
            fill.color = Color.green;
        }
        else if (Health < 50 && Health > 30)
        {
            fill.color = Color.yellow;
        }
        else if (Health < 30)
        {
            fill.color = Color.red; //colorea de rojo en caso de tener menos de 30 de vida
        }
    }
}
