using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FelipeBarridoMap : MonoBehaviour
{

    [SerializeField] Animator Animator;  // Animator Barrido
    [SerializeField] Button DeckButton;  // Bot�n compendio, para controlar que no se puede tocar hasta que acabe la animaci�n
    [SerializeField] Button PauseButton;  // Bot�n compendio, para controlar que no se puede tocar hasta que acabe la animaci�n
    public GameObject FirstRoom;   // Primera sala, para que no puedas acceder hasta que termine la animaci�n

    // Start is called before the first frame update
    void Start()
    {

        Animator.SetBool("Barrido", true);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EndAnimation(int valor)
    {

        Animator.enabled = false;
        DeckButton.GetComponent<Button>().interactable = true; // Activa el bot�n
        PauseButton.GetComponent<Button>().interactable = true; // Activa el bot�n
        FirstRoom.GetComponent<Button>().interactable = true; // Activa el bot�n
    }

}
