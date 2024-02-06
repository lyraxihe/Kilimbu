using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traduction : MonoBehaviour
{
    public static Traduction instance;
    // Idiomas
    public int Language; // 0 - Inglés || 1 - Español

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
        Language = 0; // Inglés por defecto
    }

    void Start()
    {
      
    }

   
    void Update()
    {
        
    }
}
