using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomButton : MonoBehaviour
{

    [SerializeField] public float x; // Coordinada X
    [SerializeField] public float y; // Coordinada Y
    [SerializeField] public float id; // id
    [SerializeField] GameObject[] conections = new GameObject[5];
    [SerializeField] int numContections;
  
    static int x_coord = 5;
    static int y_coord = 12;
    [SerializeField] public bool[,] Occupied_Rooms = new bool[x_coord, y_coord];
    [SerializeField] public GameObject[,] RoomsGameobjects = new GameObject[x_coord, y_coord]; // Guarda los clones de las salas
    [SerializeField] public GameObject clonDescanso;
    [SerializeField] public GameObject clonBoss;
    [SerializeField] public GameObject MapController;
    public int ContSalas;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("id:" + id + "cont-2: " + (ContSalas - 2));
        numContections = 0;
         if (id == 0 )
         {
            //for (int i = 0; i < x_coord; i++)
            //{
            //    if (Occupied_Rooms[i, 0])
            //    {
            //        conections[numContections] = RoomsGameobjects[i, 0];
            //        numContections++;
            //    }
            //}
         }

        else if (y != y_coord-1)
        {

        }

        else if (y == y_coord-1)
        {
           // conections[numContections] = clonDescanso;
           // numContections++;
        }

        else if (id == (ContSalas-2))
        {
            Debug.Log("id:" + id + "cont-2: " + (ContSalas - 2));
            conections[numContections] = clonBoss;
            numContections++;
        }
       


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        if (id==0)
        {

        }

        SceneManager.LoadScene("CombatScene");

    }
}
