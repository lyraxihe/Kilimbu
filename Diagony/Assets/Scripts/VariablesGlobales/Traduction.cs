using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traduction : MonoBehaviour
{
    public static Traduction instance;
    // Idiomas
    public int Language; // 0 - Ingl�s || 1 - Espa�ol

    private void Awake()
    {
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
        Language = 0; // Ingl�s por defecto
    }

    void Start()
    {
      
    }

   
    void Update()
    {
        
    }
}
