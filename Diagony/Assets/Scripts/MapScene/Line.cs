using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    LineRenderer lineRenderer;
    public bool porVisitar;
    public bool visitado;

    void Start()
    {
      lineRenderer = GetComponent<LineRenderer>();
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AsignPositions(Vector2 index0, Vector2 index1)
    {
        gameObject.GetComponent<LineRenderer>().SetPosition(0, index0);
        gameObject.GetComponent<LineRenderer>().SetPosition(1, index1);
    }

    public void ChangeColor(int type) // 0: sin visitar  1: por visitar  2: visitado
    {
        if (type == 1) //por visitar
        {
            Debug.Log("por visitar color 1");
           // lineRenderer.material.SetColor("_EmissionColor", new Color(0.8f, 0.07f, 0.4f)); //violeta
           // lineRenderer.material.SetColor("_EmissionColor", new Color(0.5f, 0.8f, 0.3f)); //verde
            lineRenderer.material.SetColor("_EmissionColor", new Color(0.96f, 0.9f, 1f)); //blanco-violeta
        }
        else if (type == 2) //visitado
        {
            Debug.Log("visitado color 2");
           // lineRenderer.material.SetColor("_EmissionColor", new Color(0.4f, 0.4f, 0.5f)); //azul-gris
           // lineRenderer.material.SetColor("_EmissionColor", new Color(0.11f, 0.12f, 0.14f)); //azul-gris oscuro
            lineRenderer.material.SetColor("_EmissionColor", new Color(0.96f, 0.9f, 1f)); //blanco-violeta
        }
        else // 0 --> sin visitar
        {
            Debug.Log("sin visitar color 0");
           // lineRenderer.material.SetColor("_EmissionColor", new Color(0.96f, 0.9f, 1f)); //blanco-violeta
            //lineRenderer.material.SetColor("_EmissionColor", new Color(0.4f, 0.4f, 0.5f));
            lineRenderer.material.SetColor("_EmissionColor", new Color(0.2746774f, 0.1384317f, 0.3324516f)); // violeta oscuro
        }
    }
}
