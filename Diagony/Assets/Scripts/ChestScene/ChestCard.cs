using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestCard : MonoBehaviour
{
    public GameObject VariablesGlobales;
    public GameObject ChestScene;

    public Animator CardAnimator;

    RectTransform CardPosition;

    public int Id;
    public int Tipo;
    bool AnimationEnd;

    public bool MouseOver;

    bool Abajo;

    void Start()
    {
        
        CardAnimator = GetComponent<Animator>();

        CardPosition = GetComponent<RectTransform>();

        AnimationEnd = false;
        CardAnimator.SetInteger("CardID", Id);
        CardAnimator.SetBool("AnimacionEntrar", true);

        MouseOver = false;
        Abajo = true;

    }

    void Update()
    {
        // Flotar
        if (AnimationEnd)
        {

            if(Abajo)
            {

                CardPosition.anchoredPosition = Vector2.Lerp(CardPosition.anchoredPosition, new Vector2(CardPosition.anchoredPosition.x, 0), 0.0001f);
                if (Vector2.Distance(CardPosition.anchoredPosition, new Vector2(CardPosition.anchoredPosition.x, 40)) < 2)
                    Abajo = false;

            }   
            else
            {

                CardPosition.anchoredPosition = Vector2.Lerp(CardPosition.anchoredPosition, new Vector2(CardPosition.anchoredPosition.x, 100), 0.0001f);
                if (Vector2.Distance(CardPosition.anchoredPosition, new Vector2(CardPosition.anchoredPosition.x, 60)) < 2)
                    Abajo = true;

            }


        }

    }

    private void OnMouseOver()
    {

        if(AnimationEnd)
        {

            MouseOver = true;
            transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

        }

    }

    private void OnMouseExit()
    {

        if(AnimationEnd)
        {

            MouseOver = false;
            transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

        }

    }

    private void OnMouseDown()
    {

        if (AnimationEnd && !ChestScene.GetComponent<ChestController>().CardSelected)
        {

            ChestScene.GetComponent<ChestController>().CardSelected = true;
            CardAnimator.enabled = true;
            CardAnimator.SetBool("Selected", true);

        }

    }

    private void ControlAnimation(int valor)
    {

        if (valor == 0) // Ha terminado la animación de entrada
        {

            CardAnimator.SetBool("AnimacionEntrar", false);
            CardAnimator.enabled = false;

            if(Id == 2)
            {

                for(int i = 0; i < ChestScene.GetComponent<ChestController>().ListCards.Count; i++)
                    ChestScene.GetComponent<ChestController>().ListCards[i].GetComponent<ChestCard>().AnimationEnd = true;

            }

        }
        else if (valor == 1) // Ha terminado la animación de salida de la carta seleccionada
        {

            ChestScene.GetComponent<ChestController>().ListCards.Remove(gameObject);

            for (int i = 0; i < ChestScene.GetComponent<ChestController>().ListCards.Count; i++)
            {

                ChestScene.GetComponent<ChestController>().ListCards[i].GetComponent<ChestCard>().CardAnimator.enabled = true;
                ChestScene.GetComponent<ChestController>().ListCards[i].GetComponent<ChestCard>().CardAnimator.SetBool("AnimacionSalir", true);

            }

            VariablesGlobales.GetComponent<VariablesGlobales>().AmountCards[Tipo]++;

            Destroy(gameObject);

        }
        else // Ha terminado la animación de salida de la carta no seleccionada
        {

            ChestScene.GetComponent<ChestController>().Exit = true;
            ChestScene.GetComponent<ChestController>().ListCards.Remove(gameObject);
            Destroy(gameObject);

        }

    }

}
