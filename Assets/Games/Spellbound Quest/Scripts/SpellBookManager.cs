using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellBookManager : MonoBehaviour
{
    SBQGameManager SBQGm;
    public Image[] imgs;
    public Button[] optionButtons;

    EnemyController ec;
    PlayerController pc;

    string[] r1_opts = { "Cat", "Kat", "Cet" };
    string[] r2_opts = { "Son", "Sun", "San" };
    string[] r3_opts = { "Red", "Reb", "Rad" };

    int correctOptionIndex;
    

    private void Start()
    {
        SBQGm = gameObject.GetComponent<SBQGameManager>();
        SBQGm.round = 1;
        ShowQuestion(GetOptionsForCurrentRound(), GetCorrectIndexForCurrentRound());
        ec = FindObjectOfType<EnemyController>();
        pc = FindObjectOfType<PlayerController>();

        if (ec != null)
        {
            ec.OnAttackCompleted += HandleEnemyAttackCompleted;
        }
        if (pc != null)
        {
            pc.OnAttackCompleted += HandlePlayerAttackCompleted;
        }
    }

    private void OnDestroy()
    {
        if (ec != null)
        {
            ec.OnAttackCompleted -= HandleEnemyAttackCompleted;
        }
        if(pc!=null)
        {
            pc.OnAttackCompleted -= HandlePlayerAttackCompleted;
        }
    }

    public void ShowQuestion(string[] options, int correctIndex)
    {
        Debug.Log("Showing question for round: " + SBQGm.round);
        CloseAllImages();

        switch (SBQGm.round)
        {
            case 1:
                imgs[0].gameObject.SetActive(true);
                break;
            case 2:
                imgs[1].gameObject.SetActive(true);
                break;
            case 3:
                imgs[2].gameObject.SetActive(true);
                break;
            default:
                Debug.LogWarning("No image available for this round. Ensure that imgs array has enough images.");
                break;
        }

        correctOptionIndex = correctIndex;
        Debug.Log("Setting options for round: " + SBQGm.round);

        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionButtons[i].gameObject.SetActive(true);
            optionButtons[i].GetComponentInChildren<TMP_Text>().text = options[i];
            int index = i;

            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
        }
    }


    void OnOptionSelected(int chosenIndex)
    {
        if (optionButtons == null || optionButtons.Length == 0)
        {
            Debug.LogError("optionButtons array is not assigned or empty.");
        }

        if (SBQGm == null)
        {
            Debug.LogError("SBQGameManager is not assigned.");
        }
        if (chosenIndex == correctOptionIndex)
        {
            Debug.Log("Round: " + SBQGm.round);
            Debug.Log("Correct choice! Player attacks.");
            SBQGm.round++;
            pc.Attack();
        }
        else
        {
            Debug.Log("Incorrect choice! Enemy attacks.");
            ec.Attack();
            optionButtons[chosenIndex].gameObject.SetActive(false);
        }

        SBQGm.CloseBook();
    }

    void CloseAllImages()
    {
        foreach (Image img in imgs)
        {
            img.gameObject.SetActive(false);
        }
    }

    private void HandleEnemyAttackCompleted()
    {
        Debug.Log("Enemy attack completed. Reopening the book.");
        SBQGm.OpenBook();
    }
    private void HandlePlayerAttackCompleted()
    {
        Debug.Log("Player attack completed. Preparing for the next round.");
        if (SBQGm.round <= imgs.Length)
        {
            ShowQuestion(GetOptionsForCurrentRound(), GetCorrectIndexForCurrentRound());
            SBQGm.OpenBook();
        }
        else
        {
            SBQGm.EndGame();
        }
    }

    private string[] GetOptionsForCurrentRound()
    {
        switch (SBQGm.round)
        {
            case 1: return r1_opts;
            case 2: return r2_opts;
            case 3: return r3_opts;
            default:
                Debug.LogWarning("No options found for the current round. Ensure all rounds are defined.");
                return new string[] { };
        }
    }

    private int GetCorrectIndexForCurrentRound()
    {
        switch (SBQGm.round)
        {
            case 1: return 0;
            case 2: return 1;
            case 3: return 0;
            default:
                Debug.LogWarning("No correct index found for the current round. Ensure all rounds are defined.");
                return 0;
        }
    }


}
