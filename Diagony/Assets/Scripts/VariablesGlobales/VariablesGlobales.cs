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
    public bool EstaEnPausa = false;
    public int SpellNumber = 5;

    public static VariablesGlobales instance;

    public List<int> AmountCards = new List<int>() {3, 2, 2, 0, 0, 0, 0, 3, 2, 1, 0, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 0, 0}; // Lista con las cantidades de cada carta (posición = ID carta en el excel)
    public List<int> CardCostOriginal = new List<int>() { 1, 1, 2, 2, 3, 3, 0, 1, 2, 2, 3, 3, 5, 1, 0, 1, 2, 1, 0, 2, 1, 0, 4, 3 };
    public List<int> CardCost = new List<int>() { 1, 1, 2, 2, 3, 3, 0, 1, 2, 2, 3, 3, 5, 1, 0, 1, 2, 1, 0, 2, 1, 0, 4, 3 };

    public GameObject Traduction;

    // Boss
    public bool Boss; // Si es true significa que el combate será contra el Boss, si es false es un combate normal

    // Tutorial
    public bool Tutorial; // Si es true significa que el combate es el tutorial, si es false es un combate normal

    // Pasivas
    public bool PasivaGanarDinero;
    public int PasivaDinero;
    public bool PasivaCurarseCombate;
    public int PasivaCuracionCombate;

    // Array con numero de usos en cada carta, están acomodadas según su id y aumentan en (+1) cada vez que se usa una en combate
    public List<int> CardUses = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    // Controla si los botones de la interfaz se pueden tocar
    public bool ButtonsEnabled;

    //// Idiomas
    public int Language; // 0 - Inglés || 1 - Español

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

        // Controla si los botones de la interfaz se pueden tocar
        ButtonsEnabled = true;

        //Language = 0; // Inglés por defecto
        Traduction = GameObject.Find("Traduction");
       
    }
    void Start()
    {

        Boss = false;
        VidasProtagonista = 3;
        MaxHealthProtagonista = 100;
        HealthProtagonista = 100;
        MaxManaProtagonista = 3;     // Maná máximo del jugador con el que podrá usar las cartas (se resetea en cada turno)

        // Pasivas
        PasivaGanarDinero = false;
        PasivaDinero = 0;
        PasivaCurarseCombate = false;
        PasivaCuracionCombate = 0;

    }


   
    void Update()
    {
        Language = Traduction.GetComponent<Traduction>().Language;
        //if (EstaEnPausa)
        //    Time.timeScale = 0;
        //else
        //    Time.timeScale = 1;

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
