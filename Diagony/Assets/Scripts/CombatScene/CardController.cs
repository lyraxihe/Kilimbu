using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardController : MonoBehaviour
{

    public GameObject CombatScene;
    public GameObject DragZone;

    [SerializeField] Vector3 CardPosition;
    [SerializeField] Vector3 CardRotation;

    [SerializeField] Sprite danyo5;
    [SerializeField] Sprite danyo10;
    [SerializeField] Sprite curar10;

    public Animator CardAnimator;
    [SerializeField] bool AnimacionEntrar;
    public bool AnimacionSalir;
    //private float desireDuration;
    //private float elapsedTime;

    Vector3 MousePositionOffset;
    public bool MouseDrag, MouseOver, IsInDragZone, SePuede;
    public int Id; // ID de la carta en la lista de cartas (para saber su posicion al eliminarla de la lista)
    [SerializeField] int NumCartas; // Número de cartas en el turno actual
    public int Tipo; //por ahora vamos a hacer 3, 0- que haga 5 de daño, 1- que haga 10 y 2- que cure 3 de vida del personaje
 

    void Start()
    {
        MouseDrag = true;
        MouseOver = true;


        //AnimacionCarta();

        //desireDuration = 5f;
        //AnimacionCarta = true;
        AnimacionEntrar = true;
        AnimacionSalir = false;

        CardAnimator = GetComponent<Animator>();
        //CardAnimator.SetBool("AnimacionCarta", AnimacionCarta);
        CardAnimator.SetBool("AnimacionEntrar", AnimacionEntrar);
        CardAnimator.SetBool("AnimacionSalir", AnimacionSalir);
       
        setColor_text();


    }

    void Update()
    {

        NumCartas = CombatScene.GetComponent<CombatController>().CardList.Count;
        CardAnimator.SetInteger("NumCartas", NumCartas);
        CardAnimator.SetInteger("CardID", Id);

        //AnimacionCarta();

    }

    private Vector3 GetMouseWorldPosition()
    {

        return Camera.main.ScreenToWorldPoint(Input.mousePosition);

    }

    private void OnMouseOver()
    {

        if (!MouseDrag && CombatScene.GetComponent<CombatController>().ManaProtagonista > 0)
        {
           if (IsInDragZone)
            {

                CombatScene.GetComponent<CombatController>().ManaProtagonista--;  // Reduce el maná del jugador en 1
                CombatScene.GetComponent<CombatController>().EliminarCarta(Id);
                CombatScene.GetComponent<CombatController>().UsarCarta(Tipo);
                
                Destroy(gameObject);                                              //destruye la carta al colisionar con la dragzone

            }

            MouseOver = true;
            transform.localScale = new Vector3(3, 4, 0);
            transform.position = new Vector3(transform.position.x, -3, 0);
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

    }



    private void OnMouseExit()
    {
        MouseOver = false;
    }

   
    private void OnMouseUp()
    {
        MouseDrag = false;
        MouseOver = false;

    }

    private void OnMouseDown()
    {

        if(CombatScene.GetComponent<CombatController>().ManaProtagonista > 0)
        {

            MousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();

        }

    }

    private void OnMouseDrag()
    {

        if(CombatScene.GetComponent<CombatController>().ManaProtagonista > 0)
        {

            MouseDrag = true;
            transform.position = GetMouseWorldPosition() + MousePositionOffset;

        }

    }

    public void SetPosition()
    {

        CardPosition = transform.position;
        CardRotation = transform.eulerAngles;

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DragZone"))
        {
            IsInDragZone=true;
        }
        if(!collision.gameObject.CompareTag("DragZone"))
        {
            IsInDragZone = false;
        }
    }

    /*
     * Segun termina la animacion de la carta se llama a esta funcion, tiene que ser un int porque Unity es una patata ¯\_(ツ)_/¯
     */
    public void SetAnimacionCarta(int valor)
    {

        //Ha terminado la animacion por lo que se desactiva el animator e impide que la animacion se repita
        if (valor == 0) // Ha terminado la animación de Enter
        {

            //AnimacionCarta = false;
            AnimacionEntrar = false;

            //CardAnimator.SetBool("AnimacionCarta", AnimacionCarta);
            CardAnimator.SetBool("AnimacionEntrar", AnimacionEntrar);
            //CardAnimator.speed *= 0;


            CardAnimator.enabled = false;
            MouseDrag = false;
            MouseOver = false;

            if(Id == 4)
                CombatScene.GetComponent<CombatController>().botonTurno.interactable = true;

        }
        if (valor == 1) // Ha terminado la animación de Exit
        {

            //AnimacionCarta = false;
            AnimacionSalir = false;

            //CardAnimator.SetBool("AnimacionCarta", AnimacionCarta);
            CardAnimator.SetBool("AnimacionSalir", AnimacionSalir);

            CombatScene.GetComponent<CombatController>().EliminarCarta(Id);
            Destroy(gameObject);                                                   //destruye la carta al colisionar con la dragzone

        }

    }
    public void setColor_text()
    {
        Color colorPersonalizado;

        if (Tipo == 0)
        {
            //colorPersonalizado = new Color(0.55f, 0.61f, 0.69f, 1.0f);
            //gameObject.GetComponent<SpriteRenderer>().color = colorPersonalizado;
            //TextMeshProUGUI textCharacter = gameObject.GetComponent<TextMeshProUGUI>();
            //textCharacter.text = "5 ATQ";
            gameObject.GetComponent<SpriteRenderer>().sprite = danyo5;

        }
        else if (Tipo == 1)
        {
            //colorPersonalizado = new Color(0.27f, 0.36f, 0.44f, 1.0f);
            //gameObject.GetComponent<SpriteRenderer>().color = colorPersonalizado;
            //TextMeshProUGUI textCharacter = gameObject.GetComponent<TextMeshProUGUI>();
            //textCharacter.text = "10 ATQ";
            gameObject.GetComponent<SpriteRenderer>().sprite = danyo10;
        }
        else
        {
            //colorPersonalizado = new Color(0.83f, 0.75f, 0.86f, 1.0f);
            //gameObject.GetComponent<SpriteRenderer>().color = colorPersonalizado;
            //TextMeshProUGUI textCharacter = gameObject.GetComponent<TextMeshProUGUI>();
            //textCharacter.text = "+10 HP";
            gameObject.GetComponent<SpriteRenderer>().sprite = curar10;
        }
    }
    //public void AnimacionCarta()
    //{

    //    elapsedTime += Time.deltaTime;
    //    float percentageComplete = elapsedTime / desireDuration;
    //    Vector3 initialPosition;
    //    Vector3 initialAngles;

    //    if(AnimacionEntrar)
    //    {

    //        if (Id == 0)
    //        {

    //            initialPosition = transform.position;
    //            initialAngles = transform.eulerAngles;

    //            transform.position = Vector3.Lerp(initialPosition, new Vector3(3.95f, -4.7f, 0), percentageComplete);
    //            transform.eulerAngles = Vector3.Lerp(initialAngles, new Vector3(0, 0, 340), percentageComplete);

    //            if (Vector3.Equals(transform.position, new Vector3(3.95f, -4.7f, 0)))
    //            {

    //                AnimacionEntrar = false;
    //                MouseDrag = false;
    //                MouseOver = false;

    //            }

    //        }
    //        else if(Id == 1)
    //        {

    //            initialPosition = transform.position;
    //            initialAngles = transform.eulerAngles;

    //            transform.position = Vector3.Lerp(initialPosition, new Vector3(2, -4.15f, 1), percentageComplete);
    //            transform.eulerAngles = Vector3.Lerp(initialAngles, new Vector3(0, 0, 350), percentageComplete);

    //            if (Vector3.Equals(transform.position, new Vector3(2, -4.15f, 1)))
    //            {

    //                AnimacionEntrar = false;
    //                MouseDrag = false;
    //                MouseOver = false;

    //            }

    //        }
    //        else if(Id == 2)
    //        {

    //            initialPosition = transform.position;
    //            initialAngles = transform.eulerAngles;

    //            transform.position = Vector3.Lerp(initialPosition, new Vector3(0, -4, 2), percentageComplete);
    //            transform.eulerAngles = Vector3.Lerp(initialAngles, new Vector3(0, 0, 0), percentageComplete);

    //            if (Vector3.Equals(transform.position, new Vector3(0, -4, 2)))
    //            {

    //                AnimacionEntrar = false;
    //                MouseDrag = false;
    //                MouseOver = false;

    //            }

    //        }
    //        else if (Id == 3)
    //        {

    //            initialPosition = transform.position;
    //            initialAngles = transform.eulerAngles;

    //            transform.position = Vector3.Lerp(initialPosition, new Vector3(-2, -4.15f, 3), percentageComplete);
    //            transform.eulerAngles = Vector3.Lerp(initialAngles, new Vector3(0, 0, 10), percentageComplete);

    //            if (Vector3.Equals(transform.position, new Vector3(-2, -4.15f, 3)))
    //            {

    //                AnimacionEntrar = false;
    //                MouseDrag = false;
    //                MouseOver = false;

    //            }

    //        }
    //        else
    //        {

    //            initialPosition = transform.position;
    //            initialAngles = transform.eulerAngles;

    //            transform.position = Vector3.Lerp(initialPosition, new Vector3(-3.95f, -4.7f, 4), percentageComplete);
    //            transform.eulerAngles = Vector3.Lerp(initialAngles, new Vector3(0, 0, 20), percentageComplete);

    //            if (Vector3.Equals(transform.position, new Vector3(-3.95f, -4.7f, 4)))
    //            {

    //                AnimacionEntrar = false;
    //                MouseDrag = false;
    //                MouseOver = false;

    //            }

    //        }

    //    }

    //    if(AnimacionSalir)
    //    {

    //        initialPosition = transform.position;
    //        initialAngles = transform.eulerAngles;

    //        transform.position = Vector3.Lerp(initialPosition, new Vector3(7.5f, -3f, 0), percentageComplete);
    //        transform.eulerAngles = Vector3.Lerp(initialAngles, new Vector3(0, 0, 0), percentageComplete);

    //        if (Vector3.Equals(transform.position, new Vector3(7.5f, -3f, 0)))
    //        {

    //            CombatScene.GetComponent<CombatController>().EliminarCarta(Id);
    //            Destroy(gameObject);

    //        }

    //    }

    //}

}
