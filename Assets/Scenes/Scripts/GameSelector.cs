using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSelector : MonoBehaviour
{
    [SerializeField]
    GameObject[] games;
    int index = 0;

    void CloseAll()
    {
        foreach (GameObject game in games)
        {
            game.SetActive(false);
        }
    }

    public void NextGame()
    {
        CloseAll();
        index++;
        if (index < games.Length)
        {
            games[index].SetActive(true);
        }
        else
        {
            index = 0;
            games[index].SetActive(true);
        }
    }

    public void PreviousGame()
    {
        CloseAll();
        index--;
        if (index >= 0)
        {
            games[index].SetActive(true);
        }
        else
        {
            index = games.Length - 1;
            games[index].SetActive(true);
        }
    }

    public void Play()
    {
        if(index == 0)
        {
            SceneManager.LoadScene("SBQ Level 1");
        }
        else if (index == 1)
        {
            SceneManager.LoadScene("Snake Game Lv 1");
        }
        else if (index == 2)
        {
            SceneManager.LoadScene("Read and Climb");
        }
        else if (index == 3)
        {
            SceneManager.LoadScene("Game4");
        }
    }
}
