using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EffectIcon : MonoBehaviour
{

    [SerializeField] Traduction _Traduction;
    [SerializeField] Transform DescriptionContainer; // Container del texto descriptivo
    [SerializeField] TMP_Text DescriptionText;       // Texto que describe el efecto
    public GameObject Personaje;                     // Personaje asociado a este efecto
    public bool EsPlayer;                            // Indica si el personaje asociado es el Player
    public int Tipo;                                 // 0 - Bloqueado | 1 - Débil | 2 - Fuerte | 3 - Esperanza | 4 - Envenenado | 5 - Transformado

    public void Start()
    {

        _Traduction = GameObject.Find("Traduction").GetComponent<Traduction>();
        DescriptionContainer = gameObject.transform.GetChild(0);
        DescriptionText = gameObject.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();

    }

    public void OnMouseOver()
    {

        DescriptionContainer.gameObject.SetActive(true);

        if(_Traduction.Language == 0) // English
        {

            if (Tipo == 0)
                DescriptionText.text = " Cannot attack next turn / 1 turn ";
            else if (Tipo == 1)
            {

                if(EsPlayer)
                    DescriptionText.text = /*"-" +*/ Personaje.GetComponent<PlayerController>().Debilidad + " damage / " + (Personaje.GetComponent<PlayerController>().ContadorDeTurnosDebilitado + Personaje.GetComponent<PlayerController>().ContadorDeTurnosDebilitadoDevolverIra) + " turns ";
                else
                    DescriptionText.text = /*"-" + */Personaje.GetComponent<EnemyController>().Debilidad + " damage / " + Personaje.GetComponent<EnemyController>().ContadorDeTurnosDebilitado + " turns ";

            }
            else if (Tipo == 2)
            {

                if (EsPlayer)
                    DescriptionText.text = " +" + Personaje.GetComponent<PlayerController>().Fuerza + " damage / " + Personaje.GetComponent<PlayerController>().ContadorDeTurnosFuerte + " turns ";
                else
                    DescriptionText.text = " +" + Personaje.GetComponent<EnemyController>().Fuerza + " damage / " + Personaje.GetComponent<EnemyController>().ContadorDeTurnosFuerte + " turns ";

            }
            else if (Tipo == 3)
            {

                if (EsPlayer)
                    DescriptionText.text = " +" + Personaje.GetComponent<PlayerController>().Esperanza + " health when attacking / " + Personaje.GetComponent<PlayerController>().ContadorDeTurnosEsperanzado + " turns ";
                else
                    DescriptionText.text = " +" + Personaje.GetComponent<EnemyController>().Esperanza + " health when attacking / " + Personaje.GetComponent<EnemyController>().ContadorDeTurnosEsperanzado + " turns ";

            }
            else if (Tipo == 4)
            {

                if (EsPlayer)
                    DescriptionText.text = " -" + Personaje.GetComponent<PlayerController>().Veneno + " health when attacking / " + (Personaje.GetComponent<PlayerController>().ContadorDeTurnosEnvenenado + Personaje.GetComponent<PlayerController>().ContadorDeTurnosEnvenenadoDevolverIra) + " turns ";
                else
                    DescriptionText.text = " -" + Personaje.GetComponent<EnemyController>().Veneno + " health when attacking / " + Personaje.GetComponent<EnemyController>().ContadorDeTurnosEnvenenado + " turns ";

            }
            else if (Tipo == 5)
            {

                if (EsPlayer)
                    DescriptionText.text = " Attacks heal instead of damage / " + Personaje.GetComponent<PlayerController>().ContadorDeTurnosTransformacion + " turns ";
                else
                    DescriptionText.text = " Attacks heal instead of damage" + Personaje.GetComponent<EnemyController>().ContadorDeTurnosTransformacion + " turns ";

            }

        }
        else                          // Español
        {

            if (Tipo == 0)
                DescriptionText.text = " No puede atacar el siguiente turno / 1 turno ";
            else if (Tipo == 1)
            {

                if (EsPlayer)
                    DescriptionText.text = " " + Personaje.GetComponent<PlayerController>().Debilidad + " de daño / " + (Personaje.GetComponent<PlayerController>().ContadorDeTurnosDebilitado + Personaje.GetComponent<PlayerController>().ContadorDeTurnosDebilitadoDevolverIra) + " turnos ";
                else
                    DescriptionText.text = " " + Personaje.GetComponent<EnemyController>().Debilidad + " de daño / " + Personaje.GetComponent<EnemyController>().ContadorDeTurnosDebilitado + " turnos ";

            }
            else if (Tipo == 2)
            {

                if (EsPlayer)
                    DescriptionText.text = " +" + Personaje.GetComponent<PlayerController>().Fuerza + " de daño / " + Personaje.GetComponent<PlayerController>().ContadorDeTurnosFuerte + " turnos ";
                else
                    DescriptionText.text = " +" + Personaje.GetComponent<EnemyController>().Fuerza + " de daño / " + Personaje.GetComponent<EnemyController>().ContadorDeTurnosFuerte + " turnos ";

            }
            else if (Tipo == 3)
            {

                if (EsPlayer)
                    DescriptionText.text = " +" + Personaje.GetComponent<PlayerController>().Esperanza + " de vida al atacar / " + Personaje.GetComponent<PlayerController>().ContadorDeTurnosEsperanzado + " turnos ";
                else
                    DescriptionText.text = "+" + Personaje.GetComponent<EnemyController>().Esperanza + " de vida al atacar / " + Personaje.GetComponent<EnemyController>().ContadorDeTurnosEsperanzado + " turnos ";

            }
            else if (Tipo == 4)
            {

                if (EsPlayer)
                    DescriptionText.text = " -" + Personaje.GetComponent<PlayerController>().Veneno + " de vida al atacar / " + (Personaje.GetComponent<PlayerController>().ContadorDeTurnosEnvenenado + Personaje.GetComponent<PlayerController>().ContadorDeTurnosEnvenenadoDevolverIra) + " turnos ";
                else
                    DescriptionText.text = " -" + Personaje.GetComponent<EnemyController>().Veneno + " de vida al atacar / " + Personaje.GetComponent<EnemyController>().ContadorDeTurnosEnvenenado + " turnos ";

            }
            else if (Tipo == 5)
            {

                if (EsPlayer)
                    DescriptionText.text = " Los ataques curan en vez de dañar / " + Personaje.GetComponent<PlayerController>().ContadorDeTurnosTransformacion + " turnos ";
                else
                    DescriptionText.text = " Los ataques curan en vez de dañar / " + Personaje.GetComponent<EnemyController>().ContadorDeTurnosTransformacion + " turnos ";

            }

        }

    }

    public void OnMouseExit()
    {

        DescriptionText.text = "";
        DescriptionContainer.gameObject.SetActive(false);

    }

}
