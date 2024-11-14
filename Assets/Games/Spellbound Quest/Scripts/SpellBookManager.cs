using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellBookManager : MonoBehaviour
{
    SBQGameManager SBQGm;
    public Image[] imgs;
    public Button[] optionButtons;

    string[] r1_opts = { "Cat", "Kat", "Cet" };
    string[] r2_opts = { "Son", "Sun", "San" };
    string[] r3_opts = { "Red", "Reb", "Rad" };

    int correctOptionIndex;
    int round;
    private void Start()
    {
        SBQGm = gameObject.GetComponent<SBQGameManager>();
        round = 1;
        ShowQuestion(r1_opts, 1);
    }

    public void ShowQuestion(string[] options, int correctIndex)
    {
        if(round==1)
        {
            CloseAllImages();
            imgs[0].gameObject.SetActive(true);
        }
        else if(round==2)
        {
            CloseAllImages();
            imgs[1].gameObject.SetActive(true);
        }
        else if(round==3)
        {
            CloseAllImages();
            imgs[2].gameObject.SetActive(true);
        }
        correctOptionIndex = correctIndex;

        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionButtons[i].GetComponentInChildren<TMP_Text>().text = options[i];
            int index = i; // Capture the loop variable for the lambda
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
        }
    }
    void OnOptionSelected(int chosenIndex)
    {
        if (chosenIndex == correctOptionIndex)
        {
            Debug.Log("Correct choice! Player attacks.");
            //Instantiate(playerAttackEffect, transform.position, Quaternion.identity);
            // Trigger attack animation/effects from the player
            SBQGm.CloseBook();
            round++;
        }
        else
        {
            Debug.Log("Incorrect choice! Enemy attacks.");
            SBQGm.CloseBook();
            Destroy(optionButtons[chosenIndex]);
            //Instantiate(enemyAttackEffect, transform.position, Quaternion.identity);
            // Trigger attack animation/effects from the enemy
        }
    }

    void CloseAllImages()
    {
        foreach(Image img in imgs)
        {
            img.gameObject.SetActive(false);
        }
    }

}
