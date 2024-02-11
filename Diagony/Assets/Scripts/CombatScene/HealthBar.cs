using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBar : MonoBehaviour
{
    public GameObject VariablesGlobales;
    [SerializeField] public TMP_Text HealthLabel;
    [SerializeField] UnityEngine.UI.Slider HealthBarSlider;
    [SerializeField] UnityEngine.UI.Image fill;
    /*[SerializeField] public int Identify;*/ // 0 ira, 1 miedo, 2 tristeza, 3 prota
    [SerializeField] public bool EsPlayer;
    [SerializeField] public GameObject _player;
    [SerializeField] public GameObject _enemy;
    [SerializeField] public GameObject TextCharacter;

    private int _maxHealth; // Para controlar la vida Máxima en el script

    private void Awake()
    {

        VariablesGlobales = GameObject.Find("VariablesGlobales");

    }

    void Start()
    {
        if (EsPlayer)
        {
            TextCharacter.SetActive(false);
        }
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

        UpdateLanguageTexts();

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

    public void UpdateLanguageTexts()
    {

        if (VariablesGlobales.GetComponent<VariablesGlobales>().Language == 0) // English
        {

            if (EsPlayer)
                TextCharacter.GetComponent<TMP_Text>().text = "YOU";
            else
            {

                if (_enemy.GetComponent<EnemyController>().Tipo == 0)
                    TextCharacter.GetComponent<TMP_Text>().text = "ANGER";
                else if (_enemy.GetComponent<EnemyController>().Tipo == 1)
                    TextCharacter.GetComponent<TMP_Text>().text = "FEAR";
                else if (_enemy.GetComponent<EnemyController>().Tipo == 2)
                    TextCharacter.GetComponent<TMP_Text>().text = "SADNESS";
                else
                    TextCharacter.GetComponent<TMP_Text>().text = "BOSS";

            }

        }
        else                                                                  // Spanish
        {

            if (EsPlayer)
                TextCharacter.GetComponent<TMP_Text>().text = "TÚ";
            else
            {

                if (_enemy.GetComponent<EnemyController>().Tipo == 0)
                    TextCharacter.GetComponent<TMP_Text>().text = "IRA";
                else if (_enemy.GetComponent<EnemyController>().Tipo == 1)
                    TextCharacter.GetComponent<TMP_Text>().text = "MIEDO";
                else if (_enemy.GetComponent<EnemyController>().Tipo == 2)
                    TextCharacter.GetComponent<TMP_Text>().text = "TRISTEZA";
                else
                    TextCharacter.GetComponent<TMP_Text>().text = "JEFE";

            }

        }

    }
}
