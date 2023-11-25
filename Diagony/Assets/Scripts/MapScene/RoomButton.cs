using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomButton : MonoBehaviour
{

    [SerializeField] public float x; // Coordinada X
    [SerializeField] public float y; // Coordinada Y
    [SerializeField] public float id; // id
    [SerializeField] public GameObject conection1;
    [SerializeField] public GameObject conection2;
    [SerializeField] public GameObject conection3;
    [SerializeField] public GameObject conection4;
    [SerializeField] public int numContections;
    [SerializeField] public bool[,] Occupied_Rooms = new bool[3, 4];


    // Start is called before the first frame update
    void Start()
    {

     
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {

        SceneManager.LoadScene("CombatScene");

    }
}
