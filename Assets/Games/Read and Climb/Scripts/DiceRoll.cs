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

    private int diceResult;

    void Start()
    {
        rollDiceButton.onClick.AddListener(RollDice);
    }

    public void RollDice()
    {
        rollDiceButton.interactable = false;
        StartCoroutine(AnimateDiceRoll());
    }

    public void OppRollDice()
    {
        rollDiceButton.interactable = false;
        StartCoroutine(OpponentAnimateDiceRoll());
    }

    IEnumerator AnimateDiceRoll()
    {
        int rollCount = Random.Range(8, 15);

        for (int i = 0; i < rollCount; i++)
        {
            int randomFace = Random.Range(0, 6);
            diceImage.sprite = diceFaces[randomFace];
            yield return new WaitForSeconds(0.1f);
        }

        diceResult = Random.Range(1, 7);
        diceImage.sprite = diceFaces[diceResult - 1]; 

        yield return new WaitForSeconds(0.5f);
        player.RollDice(diceResult); 
        rollDiceButton.interactable = true;
    }
    IEnumerator OpponentAnimateDiceRoll()
    {
        int rollCount = Random.Range(8, 15);

        for (int i = 0; i < rollCount; i++)
        {
            int randomFace = Random.Range(0, 6);
            diceImage.sprite = diceFaces[randomFace];
            yield return new WaitForSeconds(0.1f);
        }

        diceResult = Random.Range(1, 7);
        diceImage.sprite = diceFaces[diceResult - 1];

        yield return new WaitForSeconds(0.5f);
        opp.TakeTurn(diceResult);
        rollDiceButton.interactable = true;
    }
}
