using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoll : MonoBehaviour
{
    public Image diceImage;
    public Sprite[] diceFaces;
    public Button rollDiceButton;
    public PlayerMovement player;
    public OpponentMovement opp;
    public AudioSource audioSource;

    private int diceResult;

    void Start()
    {
        rollDiceButton.onClick.AddListener(() => StartDiceRoll(isOpponent: false));
    }

    public void OppRollDice()
    {
        StartDiceRoll(isOpponent: true);
    }

    private void StartDiceRoll(bool isOpponent)
    {
        rollDiceButton.interactable = false;
        StartCoroutine(AnimateDiceRoll(isOpponent));
    }

    private IEnumerator AnimateDiceRoll(bool isOpponent)
    {
        int rollCount = Random.Range(8, 15);
        audioSource.Play();

        for (int i = 0; i < rollCount; i++)
        {
            diceImage.sprite = diceFaces[Random.Range(0, 6)];
            yield return new WaitForSeconds(0.1f);
        }

        diceResult = Random.Range(1, 7);
        diceImage.sprite = diceFaces[diceResult - 1];

        yield return new WaitForSeconds(0.5f);

        if (isOpponent)
        {
            if (opp.currentPosition <= 99 - diceResult)
            {
                opp.TakeTurn(diceResult);
            }
        }
        else
        {
            //if (player.currentPosition <= 99 - diceResult)
            //{
            //    player.RollDice(diceResult);
            //}
            player.RollDice(diceResult);
        }

        rollDiceButton.interactable = true;
    }
}
