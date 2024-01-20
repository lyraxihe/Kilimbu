using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MapHealthbar : MonoBehaviour
{

    public GameObject VariablesGlobales;

    [SerializeField] public TMP_Text HealthLabel;
    [SerializeField] UnityEngine.UI.Slider HealthBarSlider;
    [SerializeField] UnityEngine.UI.Image fill;

    void Awake()
    {

        VariablesGlobales = GameObject.Find("VariablesGlobales");

    }
    void Update()
    {

        ValueHealthBar(VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista, VariablesGlobales.GetComponent<VariablesGlobales>().MaxHealthProtagonista);
        
    }

    public void ValueHealthBar(int Health, int MaxHealth)
    {

        HealthLabel.text = Health + "/" + MaxHealth; // representa los valores en el texto
        HealthBarSlider.maxValue = MaxHealth;
        HealthBarSlider.value = Health; //asigna el valor al slider

        if (Health <= 15)
            fill.color = new Color(0.447f, 0.149f, 0.27f, 1.0f); // colorea de rojo en caso de tener menos de 15 de vida
        else if (Health <= 35)
            fill.color = new Color(0.86f, 0.89f, 0.776f, 1.0f); // amarillo
        else
            fill.color = new Color(0.541f, 0.658f, 0.596f, 1.0f); // verde
    }

}
