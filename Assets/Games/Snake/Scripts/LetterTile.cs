using UnityEngine;

public class LetterTile : MonoBehaviour
{
    public string letter;
    private LetterPronunciationManager manager;
    GameManager gm;
    int errorCount = 0;

    public void Setup(string letter, LetterPronunciationManager manager)
    {
        this.letter = letter;
        this.manager = manager;
    }
    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (letter != manager.CurrentLetter)
            {
                Debug.Log($"Incorrect selection: {letter}. Expected: {manager.CurrentLetter}");
                errorCount++;
                Debug.Log(errorCount);
                if(gm.level==1)
                {
                    PlayerPrefs.SetInt("Error Count Lv 1", errorCount);
                    PlayerPrefs.Save();
                }
                else if (gm.level == 2)
                {
                    PlayerPrefs.SetInt("Error Count Lv 2", errorCount);
                    PlayerPrefs.Save();
                }
                else if (gm.level == 3)
                {
                    PlayerPrefs.SetInt("Error Count Lv 3", errorCount);
                    PlayerPrefs.Save();
                }
                else if (gm.level == 4)
                {
                    PlayerPrefs.SetInt("Error Count Lv 4", errorCount);
                    PlayerPrefs.Save();
                }
                else if (gm.level == 5)
                {
                    PlayerPrefs.SetInt("Error Count Lv 5", errorCount);
                    PlayerPrefs.Save();
                }
            }
            else
            {
                manager.CorrectSelection();
            }
        }
    }
}
