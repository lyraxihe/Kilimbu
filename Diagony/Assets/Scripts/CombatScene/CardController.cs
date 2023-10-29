using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{

    public GameObject CombatController;
  //  public GameObject DragZone;

    [SerializeField] Vector3 CardPosition;
    [SerializeField] Vector3 CardRotation;

    [SerializeField] Animator CardAnimator;
    [SerializeField] bool AnimacionCarta;

    Vector3 MousePositionOffset;
    public bool MouseDrag, MouseOver, IsInDragZone;
    public int Id; // ID de la carta en la lista de cartas (para saber su posicion al eliminarla de la lista)

    void Start()
    {

        MouseDrag = false;
        MouseOver = false;

        AnimacionCarta = true;
        CardAnimator.SetInteger("Card ID", Id);

    }

    void Update()
    {

        CardAnimator.SetBool("AnimacionCarta", AnimacionCarta);

    }

    private Vector3 GetMouseWorldPosition()
    {

        return Camera.main.ScreenToWorldPoint(Input.mousePosition);

    }

    private void OnMouseOver()
    {

        if (!MouseDrag)
        {
           if (IsInDragZone)
            {
                CombatController.GetComponent<CombatController>().EliminarCarta(Id);
                Destroy(gameObject); //destruye la carta al colisionar con la dragzone
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

        MousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();

    }

    private void OnMouseDrag()
    {

        MouseDrag = true;
        transform.position = GetMouseWorldPosition() + MousePositionOffset;

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

    public void SetAnimacionCarta(int valor)
    {

        if(valor == 0)
            AnimacionCarta = false;
        else
            AnimacionCarta = true;

    }

}
