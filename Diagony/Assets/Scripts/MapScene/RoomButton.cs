using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour
{

    [SerializeField] public float x; // Coordinada X
    [SerializeField] public float y; // Coordinada Y


    [SerializeField] public float id; // id
    [SerializeField] public GameObject[] conections = new GameObject[4];
    [SerializeField] public int numContections;
    [SerializeField] public Button SetInteractuable;

    void Start()
    {

        


    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void OnClick()
    {
        SetInteractuable.interactable = false;
        for (int i = 0; i < numContections; i++)
        {
            conections[i].GetComponent<Button>().interactable = true;
        }
        SceneManager.LoadScene("CombatScene");

    }
}
