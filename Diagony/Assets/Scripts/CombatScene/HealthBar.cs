using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBar : MonoBehaviour
{
    //[SerializeField] public Slider HealthSlider;
    //[SerializeField] public Image fill;
    [SerializeField] public TMP_Text HealthLabel;
    void Start()
    {
        
    }

    
    void Update()
    {

    }

    public void ValueHealthBar(float Health, float MaxHealth)
    {

        HealthLabel.text = Mathf.RoundToInt(Health) + "/" + Mathf.RoundToInt(MaxHealth); // redondea los valores de la vida para representarlos en texto
        //HealthSlider.value = Health;

        //if (Health < 30)
        //{
        //  fill.tintColor = Color.red;
        //}
    }
}
