using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FelipeBarridoMap : MonoBehaviour
{

    [SerializeField] Animator Animator;  // Animator Barrido
    [SerializeField] Button DeckButton;  // Botón compendio, para controlar que no se puede tocar hasta que acabe la animación
    [SerializeField] Button PauseButton;  // Botón compendio, para controlar que no se puede tocar hasta que acabe la animación
    public GameObject FirstRoom;   // Primera sala, para que no puedas acceder hasta que termine la animación

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
        DeckButton.GetComponent<Button>().interactable = true; // Activa el botón
        PauseButton.GetComponent<Button>().interactable = true; // Activa el botón
        FirstRoom.GetComponent<Button>().interactable = true; // Activa el botón
    }

}
