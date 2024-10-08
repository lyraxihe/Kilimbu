using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalizarTurnoButton : MonoBehaviour
{
    [SerializeField] GameObject VariablesGlobales;
    [SerializeField] GameObject CombatScene;

    // Sound FX Management
    public AudioSource ButtonHoverSound;



    
    // Start is called before the first frame update
    void Start()
    {
        VariablesGlobales = GameObject.Find("VariablesGlobales");

        // Encontrar la m�sica sonando para poder editarla y soundfx para asignarlos
        ButtonHoverSound = GameObject.Find("ButtonHover_SoundFX").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick()
    {
        CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDeTurnos++;
        CombatScene.GetComponent<CombatController>().ContadorTurnos++;
        int numCartas = CombatScene.GetComponent<CombatController>().CardList.Count;

        if(CombatScene.GetComponent<CombatController>().TurnoJugador)
        {

            for (int i = 0; i < numCartas; i++)
            {

                CombatScene.GetComponent<CombatController>().CardList[i].GetComponent<CardController>().CardAnimator.enabled = true;
                //CombatScene.GetComponent<CombatController>().CardList[i].GetComponent<CardController>().AnimacionCarta = true;
                CombatScene.GetComponent<CombatController>().CardList[i].GetComponent<CardController>().AnimacionSalir = true;

                //CombatScene.GetComponent<CombatController>().CardList[i].GetComponent<CardController>().CardAnimator.SetBool("AnimacionCarta", CombatScene.GetComponent<CombatController>().CardList[i].GetComponent<CardController>().AnimacionCarta);
                CombatScene.GetComponent<CombatController>().CardList[i].GetComponent<CardController>().CardAnimator.SetBool("AnimacionSalir", CombatScene.GetComponent<CombatController>().CardList[i].GetComponent<CardController>().AnimacionSalir);

            }

            // Control del estado Bloqueado en el Player
            if(CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Bloqueado)
                CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Bloqueado = false;

            // Control del estado Fuerte en el Player
            if (CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDeTurnosFuerte > 0)
            {

                CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDeTurnosFuerte--;
                CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorFuerza++;

                if (CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorFuerza == 4)
                {

                    CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Fuerza -= 3;
                    CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorFuerza = 0; // Se resetea cada vez que se termina un efecto de D�bil

                }

                if (CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDeTurnosFuerte == 0)
                {
                    CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Fuerte = false;
                    CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Fuerza = 0;
                    CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().fuerte_icon = 0;
                    CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().EliminarSpell(2);
                   
                }
            }

            // control del estado bloqueado en el player
            if (!CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Bloqueado)
            {
                CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().bloqueado_icon = 0;
                CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().EliminarSpell(5);
            }
            // Control del estado Esperanza en el Player
            if (CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDeTurnosEsperanzado > 0)
            {

                CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDeTurnosEsperanzado--;
                CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorEsperanza++;

                if (CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorEsperanza == 4)
                {

                    CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Esperanza -= 3;
                    CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorEsperanza = 0; // Se resetea cada vez que se termina un efecto de D�bil

                }

                if (CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDeTurnosEsperanzado == 0)
                {
                    CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Esperanzado = false;
                    CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Esperanza = 0;
                    CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().esperanza_icon = 0;
                    CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().EliminarSpell(3);
                }
                    

            }

            // Control del estado envenenado en el Player
            if (CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDeTurnosEnvenenado > 0)
            {

                CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDeTurnosEnvenenado--;
                CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorEnvenenado++;

                if (CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorEnvenenado == 4)
                {

                    CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Veneno -= 3;
                    CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorEnvenenado = 0; // Se resetea cada vez que se termina un efecto de D�bil

                }  

            }


            if (CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDeTurnosEnvenenadoDevolverIra > 0)
            {

                CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDeTurnosEnvenenadoDevolverIra--;
                CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorEnvenenadoDevolverIra++;

                if (CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorEnvenenadoDevolverIra == 3)
                {

                    CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Veneno -= 3;
                    CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorEnvenenadoDevolverIra = 0; // Se resetea cada vez que se termina un efecto de D�bil

                }

            }

            if (CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDeTurnosEnvenenado == 0 && CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDeTurnosEnvenenadoDevolverIra == 0)
            {
                CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Envenenado = false;
                CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Veneno = 0;
                CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().veneno_icon = 0;
                CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().EliminarSpell(0);
            }

            // Control del estado debilitado en el Player
            if (CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDeTurnosDebilitado > 0)
            {

                CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDeTurnosDebilitado--;
                CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDebilitado++;

                if (CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDebilitado == 4)
                {

                    CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Debilidad -= 2;
                    CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDebilitado = 0; // Se resetea cada vez que se termina un efecto de D�bil

                }   

            }

            if(CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDeTurnosDebilitadoDevolverIra > 0)
            {

                CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDeTurnosDebilitadoDevolverIra--;
                CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDebilitadoDevolverIra++;

                if (CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDebilitadoDevolverIra == 3)
                {

                    CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Debilidad -= 2;
                    CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDebilitadoDevolverIra = 0; // Se resetea cada vez que se termina un efecto de D�bil

                }

            }

            if (CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDeTurnosDebilitado == 0 && CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().ContadorDeTurnosDebilitadoDevolverIra == 0)
            {
                CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Debilitado = false;
                CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().Debilidad = 0;
                CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().debilidad_icon = 0;
                CombatScene.GetComponent<CombatController>().Player.GetComponent<PlayerController>().EliminarSpell(1);
            }

            // Control Transformado en los enemigos
            List<GameObject> enemies = CombatScene.GetComponent<CombatController>().EnemyList;
            for(int i = 0; i < enemies.Count; i++)
            {

                if (enemies[i].GetComponent<EnemyController>().Transformacion)
                {

                    enemies[i].GetComponent<EnemyController>().ContadorDeTurnosTransformacion--;

                    if (enemies[i].GetComponent<EnemyController>().ContadorDeTurnosTransformacion == 0)
                        enemies[i].GetComponent<EnemyController>().Transformacion = false;

                }

            }

            // Control Curaciones en los enemigos
            for (int i = 0; i < enemies.Count; i++)
            {

                if (enemies[i].GetComponent<EnemyController>().EsperandoHeal)
                {

                    enemies[i].GetComponent<EnemyController>().ContadorDeTurnosHeal--;

                    if (enemies[i].GetComponent<EnemyController>().ContadorDeTurnosHeal == 0)
                        enemies[i].GetComponent<EnemyController>().EsperandoHeal = false;

                }

            }

            CombatScene.GetComponent<CombatController>().CartasCreadas = false;
            CombatScene.GetComponent<CombatController>().TurnoJugador = false;

            CombatScene.GetComponent<CombatController>().botonTurno.interactable = false;
            StartCoroutine(CombatScene.GetComponent<CombatController>().TurnoEnemigo());

        }

    }

    public void PlayButtonHoverSound()
    {
        ButtonHoverSound.Play();
    }
}
