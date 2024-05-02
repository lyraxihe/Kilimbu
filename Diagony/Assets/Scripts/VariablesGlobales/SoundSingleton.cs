using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSingleton : MonoBehaviour
{
    public static SoundSingleton instance;

    private void Awake() //sigleton
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
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
