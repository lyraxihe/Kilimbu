using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traduction : MonoBehaviour
{
    public static Traduction instance;
    public GameObject Settings;

    // Idiomas
    public int Language; // 0 - Ingl�s || 1 - Espa�ol

    // Textos descriptivos
    public bool DescriptiveTexts;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(Settings);
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(Settings);
                Destroy(gameObject);
            }
        }
        
        Language = 0; // Ingl�s por defecto
        DescriptiveTexts = true; // Textos descriptivos activados

    }

    void Start()
    {
      
    }

   
    void Update()
    {
        
    }

    public void ChangeDescriptiveTexts()
    {

        DescriptiveTexts = !DescriptiveTexts;

    }

}
