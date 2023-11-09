using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject VariablesGlobales;

    public int MaxHealthProtagonista;
    public int HealthProtagonista;
    public Animator PlayerAnimator;
    [SerializeField] bool animation_damage;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        MaxHealthProtagonista = VariablesGlobales.GetComponent<VariablesGlobales>().MaxHealthProtagonista;
        HealthProtagonista = VariablesGlobales.GetComponent<VariablesGlobales>().HealthProtagonista;

    }
}
