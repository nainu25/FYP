using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoll : MonoBehaviour
{
    public Image diceImage;
    public Sprite[] diceFaces; // Assign 6 dice face sprites in Inspector
    public Button rollDiceButton;
    public PlayerMovement player; // Reference to the movement script

    private int diceResult;

    void Start()
    {
        rollDiceButton.onClick.AddListener(RollDice);
    }

    public void RollDice()
    {
        rollDiceButton.interactable = false; // Disable button during roll
        StartCoroutine(AnimateDiceRoll());
    }

    IEnumerator AnimateDiceRoll()
    {
        int rollCount = Random.Range(8, 15); // Number of roll animations

        for (int i = 0; i < rollCount; i++)
        {
            int randomFace = Random.Range(0, 6);
            diceImage.sprite = diceFaces[randomFace];
            yield return new WaitForSeconds(0.1f);
        }

        // Final dice result
        diceResult = Random.Range(1, 7);
        diceImage.sprite = diceFaces[diceResult - 1]; // Update UI

        yield return new WaitForSeconds(0.5f);
        player.RollDice(diceResult); // Call player movement
        rollDiceButton.interactable = true;
    }
}
