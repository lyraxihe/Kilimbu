using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LanguageDropdown : MonoBehaviour
{

    [SerializeField] GameObject Traduction;

    private void Awake()
    {
        Traduction = GameObject.Find("Traduction");
    }
    // Start is called before the first frame update
    void Start()
    {

        gameObject.GetComponent<TMP_Dropdown>().value = Traduction.GetComponent<Traduction>().Language;

    }

    // Update is called once per frame
    void Update()
    {

        Traduction.GetComponent<Traduction>().Language = gameObject.GetComponent<TMP_Dropdown>().value;

    }
}
