using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VariablesGlobales : MonoBehaviour
{
    // declaración de variables personajes
    public int VidasProtagonista;
    public int HealthProtagonista;
    public int MaxHealthProtagonista;
    public int MaxManaProtagonista;     // Maná máximo del jugador con el que podrá usar las cartas (se resetea en cada turno)
    public int DineroAmount;
    public TMP_Text Dinero_text;

    public bool EstaEnPausa = false;

    public static VariablesGlobales instance;
    private void Awake() //sigleton
    {
        DineroAmount = 0;
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
    void Start()
    {
       
        VidasProtagonista = 3;
        MaxHealthProtagonista = 100;
        HealthProtagonista = 100;
        MaxManaProtagonista = 3;     // Maná máximo del jugador con el que podrá usar las cartas (se resetea en cada turno)

    }


   
    void Update()
    {
        Dinero_text.text = "Dinero: " + DineroAmount;
        // ControlMaxManaProtagonista();
        ControlHealthProtagonista();

    }

    public void ControlMaxManaProtagonista()
    {

        if (HealthProtagonista <= 25)
            MaxManaProtagonista = 1;
        else if (HealthProtagonista <= 50)
            MaxManaProtagonista = 2;
        else if (HealthProtagonista <= 100)
            MaxManaProtagonista = 3;
        else if (HealthProtagonista <= 150)
            MaxManaProtagonista = 4;
        else
            MaxManaProtagonista = 5;

    }

    public void ControlHealthProtagonista()
    {

        if (HealthProtagonista < 0)
            HealthProtagonista = 0;

        if (HealthProtagonista > MaxHealthProtagonista)
            HealthProtagonista = MaxHealthProtagonista;

    }

}
