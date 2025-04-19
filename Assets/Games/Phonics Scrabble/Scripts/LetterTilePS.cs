using TMPro;
using UnityEngine;

public class LetterTilePS : MonoBehaviour
{
    public TMP_Text letterText;

    public void SetLetter(string newLetter)
    {
        if (letterText != null)
        {
            letterText.text = newLetter;
        }
        else
        {
            Debug.LogError("letterText is not assigned on " + gameObject.name);
        }
    }
}
