using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopButton : MonoBehaviour
{
    [SerializeField] GameObject CanvasSingleton;
   // [SerializeField] GameObject Panel;
    bool mapavisible;
    void Start()
    {
        CanvasSingleton = GameObject.Find("CanvasSingleton");
        mapavisible = false;
    }

    public void VolverAlMapa()
    {

        SceneManager.LoadScene("MapScene");

    }

    public void MirarMapa()
    {
        if (!mapavisible)
        {
          //  Panel.SetActive(true);
            CanvasSingleton.GetComponent<CanvasSingleton>().MapaVisible(true);
            mapavisible = true;
        }
        else
        {
          //  Panel.SetActive(false);
            CanvasSingleton.GetComponent<CanvasSingleton>().MapaVisible(false);
            mapavisible = false;
        }
      
    }
   
}
