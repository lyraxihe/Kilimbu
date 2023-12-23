using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckButton : MonoBehaviour
{

    [SerializeField] GameObject ScrollAreaCompendio;
    [SerializeField] TMP_Text ButtonText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {

        if(!ScrollAreaCompendio.activeSelf)
            ButtonText.text = "Volver";
        else
            ButtonText.text = "Mis Cartas";

        ScrollAreaCompendio.SetActive(!ScrollAreaCompendio.activeSelf);


    }

}
