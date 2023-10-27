using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseOver()
    {

        transform.localScale = new Vector3(3, 4, 0);
        transform.position = new Vector3(transform.position.x, -3, 0);
        transform.eulerAngles = new Vector3(0, 0, 0);

    }

}
