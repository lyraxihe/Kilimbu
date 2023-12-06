using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DmgText : MonoBehaviour
{
    /* [SerializeField] */
    TMP_Text textMesh;
  
    float disappearTimer;
    private Color textColor;
    public bool IsHeal;
    public int Amount;
    //[SerializeField] public TMP_Text DmgHeal_text;

    private void Awake()
    {
        textMesh = transform.GetComponent<TMP_Text>();
    }
    private void Start()
    {

        // DmgHeal_text.color = textColor;

       

        disappearTimer = 1f;
        if (IsHeal)
        {
            textMesh.text = "+" + (Amount.ToString());
            textMesh.color = Color.green;
        }
        else
        {
            textMesh.text = "-" + (Amount.ToString());
            textMesh.color = Color.red;
        }

        textColor = textMesh.color;

    }
    private void Update()
    {
       // DmgHeal_text.color = textColor;
        textMesh.color = textColor;

        float MoveYSpeed = 2f;
        transform.position += new Vector3(0, MoveYSpeed) * Time.deltaTime;
        disappearTimer -= Time.deltaTime;

        if(disappearTimer < 0 )
        {
            //Debug.Log("timer menor que 0");
            float disappearSpeed = 2f;
            textMesh.color = textColor;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;

            if (textColor.a < 0)
            {
               // Debug.Log("texto a menor que 0");
                Destroy(gameObject);
            }
        }
    }
}
