using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{

    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject Levels;

    public void Play()
    {

        MainMenu.SetActive(false);
        Levels.SetActive(true);

    }

    public void Level1()
    {

        SceneManager.LoadScene("MapScene");

    }

}
