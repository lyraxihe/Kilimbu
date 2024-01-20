using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    [SerializeField] GameObject CanvasSingleton;
    // [SerializeField] GameObject Panel;
    [SerializeField] Sprite verMapa;
    [SerializeField] Sprite Volver;
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
            gameObject.GetComponent<Image>().sprite = Volver;
            CanvasSingleton.GetComponent<CanvasSingleton>().MapaVisible(true);
            mapavisible = true;
        }
        else
        {
            //  Panel.SetActive(false);
            gameObject.GetComponent<Image>().sprite = verMapa;
            CanvasSingleton.GetComponent<CanvasSingleton>().MapaVisible(false);
            mapavisible = false;
        }
      
    }
   
}
