using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CompraText : MonoBehaviour
{
    TMP_Text textMesh;
    float disappearTimer;
    private Color textColor;
    public string text = "";
    public bool ManaHeal;

    private void Awake()
    {
        textMesh = transform.GetComponent<TMP_Text>();
    }
    void Start()
    {
        disappearTimer = 1f;
        textMesh.text = text;
        textColor = textMesh.color;

        if (ManaHeal)
        {
            gameObject.GetComponent<TMP_Text>().fontSize = 18;
        }
    }


    void Update()
    {
        textMesh.color = textColor;

        float MoveYSpeed = 2f;
        transform.position += new Vector3(0, MoveYSpeed) * Time.deltaTime;
        disappearTimer -= Time.deltaTime;

        if (disappearTimer < 0)
        {
           
            float disappearSpeed = 2f;
            textMesh.color = textColor;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;

            if (textColor.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
