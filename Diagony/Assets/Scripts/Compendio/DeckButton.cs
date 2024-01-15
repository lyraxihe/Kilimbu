using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckButton : MonoBehaviour
{

    [SerializeField] GameObject ScrollAreaCompendio;
    [SerializeField] Sprite VolverIcon;
    [SerializeField] Sprite CartasIcon;
    //[SerializeField] TMP_Text ButtonText;

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

        if (!ScrollAreaCompendio.activeSelf)
        {
            //ButtonText.text = "Volver";
            gameObject.GetComponent<Image>().sprite = VolverIcon;
        }
        else
        {
            //ButtonText.text = "Mis Cartas";
            gameObject.GetComponent<Image>().sprite = CartasIcon;
        }

        ScrollAreaCompendio.SetActive(!ScrollAreaCompendio.activeSelf);


    }

}
