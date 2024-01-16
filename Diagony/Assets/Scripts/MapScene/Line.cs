using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
   // LineRenderer lineRenderer;

    void Start()
    {
      //  lineRenderer = GetComponent<LineRenderer>();
       
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
}
