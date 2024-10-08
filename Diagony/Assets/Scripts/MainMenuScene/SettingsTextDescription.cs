using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsTextDescription : MonoBehaviour
{

    [SerializeField] Traduction _Traduction;
    [SerializeField] TMP_Text SettingDescriptionText;
  //  [SerializeField] TMP_Text SettingTurnsText;
    [SerializeField] int Id;

    public void Awake()
    {

        _Traduction = GameObject.Find("Traduction").GetComponent<Traduction>();
        SettingDescriptionText = GameObject.Find("CanvasSettings").transform.GetChild(0).GetChild(6).GetChild(0).GetComponent<TMP_Text>();
       // SettingTurnsText = GameObject.Find("CanvasSettings").transform.GetChild(0).GetChild(8).GetChild(0).GetComponent<TMP_Text>();

    }

    public void OnMouseOver()
    {
        
        // Traductions
        if (_Traduction.Language == 0) // English
        {

            if (SettingDescriptionText == null)
                SettingDescriptionText = GameObject.Find("SettingDescriptionText").GetComponent<TMP_Text>();

            if (gameObject.name == "LanguageText")
                SettingDescriptionText.text = "Change the language of the texts.";
            else if (gameObject.name == "DescriptiveTextsText")
                SettingDescriptionText.text = "Enable/Disable texts that explain the effects of special cards or their icons.";
            else if (gameObject.name == "TurnsText")
                SettingDescriptionText.text = "Enable/Disable text that shows turns during combat";

        }
        else                          // Spanish
        {

            if (SettingDescriptionText == null)
                SettingDescriptionText = GameObject.Find("SettingDescriptionText").GetComponent<TMP_Text>();

            if (gameObject.name == "LanguageText")
                SettingDescriptionText.text = "Cambia el idioma de los textos.";
            else if (gameObject.name == "DescriptiveTextsText")
                SettingDescriptionText.text = "Habilita/Deshabilita los textos que explican los efectos de las cartas especiales o sus iconos.";
            else if (gameObject.name == "TurnsText")
                SettingDescriptionText.text = "Habilita/Deshabilita el texto que muestra los turnos durante el combate";
        }

    }

    public void OnMouseExit()
    {
        
        SettingDescriptionText.text = "";

    }

}
