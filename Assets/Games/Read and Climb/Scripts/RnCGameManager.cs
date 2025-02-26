using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RnCGameManager : MonoBehaviour
{
    public PlayerMovement player;
    public GameObject taskPanel; // Assign in Inspector
    public TMP_Text taskText; // Assign in Inspector
    
    private void Awake()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
        taskPanel.SetActive(false); // Hide at start
    }

    private void Update()
    {
        if (player.turns > 0 && player.turns % 3 == 0) // Show every 3 turns
        {
            ShowTaskPanel();
        }
    }

    void ShowTaskPanel()
    {
        taskPanel.SetActive(true);
        taskText.text = "Reading Task " + (player.turns / 3); // Example text
        Time.timeScale = 0; // Pause game
    }

}
