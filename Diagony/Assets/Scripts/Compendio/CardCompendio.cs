using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardCompendio : MonoBehaviour
{

    public GameObject VariablesGlobales;
    [SerializeField] GameObject ScrollAreaCompendio;
    [SerializeField] GameObject Compendio;

    [SerializeField] GameObject Parent;
    [SerializeField] int Tipo;
    private GameObject Copy;
    private bool CopyCreated;
    private RectTransform Position;
    private Image Image;
    [SerializeField] TMP_Text CantidadText;

    private Color FullTransparency;
    private Color MidTransparency;

    [SerializeField] int Fila; // Posición en el compendio | 0 - arriba | 1 - abajo 

    // Start is called before the first frame update
    void Start()
    {

        VariablesGlobales = GameObject.Find("VariablesGlobales");
        Position = GetComponent<RectTransform>();
        Image = GetComponent<Image>();
        CopyCreated = false;

        FullTransparency = Image.color;
        MidTransparency = Image.color;
        MidTransparency.a = 0.3f;

    }

    // Update is called once per frame
    void Update()
    {

        CantidadText.text = "x" + VariablesGlobales.GetComponent<VariablesGlobales>().AmountCards[Tipo];

        if (VariablesGlobales.GetComponent<VariablesGlobales>().AmountCards[Tipo] == 0)
            Image.color = MidTransparency;
        else
            Image.color = FullTransparency;

    }

    private void OnMouseOver()
    {

        if (!CopyCreated)
        {

            Color tempColor;

            CopyCreated = true;
            Copy = Instantiate(gameObject, Parent.transform);
            if(Fila == 0)
                Copy.GetComponent<RectTransform>().anchoredPosition = new Vector2(Position.anchoredPosition.x + 160, Position.anchoredPosition.y - 35);
            else
                Copy.GetComponent<RectTransform>().anchoredPosition = new Vector2(Position.anchoredPosition.x + 160, Position.anchoredPosition.y + 35);
            Copy.transform.localScale = new Vector3(1, 1, 1);

        }

    }

    private void OnMouseExit()
    {

        Destroy(Copy);
        CopyCreated = false;

    }

}
