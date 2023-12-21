using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour
{

    [SerializeField] GameObject VariablesGlobales;

    [SerializeField] public float x; // Coordinada X
    [SerializeField] public float y; // Coordinada Y


    [SerializeField] public float id; // id
    [SerializeField] public GameObject[] conections = new GameObject[4];
    [SerializeField] public int numContections;
    [SerializeField] public GameObject MapController_;

    public int RoomType; // 0: Combate - 1: Cofre

    void Start()
    {

        VariablesGlobales = GameObject.Find("VariablesGlobales");

    }


    void Update()
    {
       // ComprobarConexionesActivas();

        if(VariablesGlobales.GetComponent<VariablesGlobales>().EstaEnPausa)
            GetComponent<Button>().enabled = false;
        else
            GetComponent<Button>().enabled = true;

    }

    public void OnClick()
    {

        gameObject.GetComponent<Button>().interactable = false;
        for (int i = 0; i < numContections; i++)
        {
            conections[i].GetComponent<Button>().interactable = true;
        }
        MapController_.GetComponent<MapController>().ComprobarInactivos(); //llama a la funcion de comprobar inactivos en map controller para desactivar las salas necesarias
        
        if(RoomType == 0)
            SceneManager.LoadScene("CombatScene");
        else if(RoomType == 1)
            SceneManager.LoadScene("ChestScene");

    }

    //public void ComprobarConexionesActivas()
    //{
    //    for (int i = 0; i < numContections; i++)
    //    {
    //        if (conections[i].GetComponent<Button>().interactable)
    //        {
    //            SetInteractuable.interactable = false;
    //        }
    //    }
    //}
}
