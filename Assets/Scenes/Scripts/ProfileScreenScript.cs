using TMPro;
using UnityEngine;

public class ProfileScreenScript : MonoBehaviour
{
    [Header("Main Text")]
    public TMP_Text nameText;
    public TMP_Text ageText;

    [Space]
    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject snakePanel;
    public GameObject spellboundPanel;

    [Space]
    [Header("Snake Levels Score Text")]
    public TMP_Text level1ScoreText;
    public TMP_Text level2ScoreText;
    public TMP_Text level3ScoreText;
    public TMP_Text level4ScoreText;
    public TMP_Text level5ScoreText;

    [Space]
    [Header("Snake Levels Error Text")]
    public TMP_Text level1ErrorText;
    public TMP_Text level2ErrorText;
    public TMP_Text level3ErrorText;
    public TMP_Text level4ErrorText;
    public TMP_Text level5ErrorText;

    [Space]
    [Header("SBQ Levels Score Text")]
    public TMP_Text sbqLevel1ScoreText;
    public TMP_Text sbqLevel2ScoreText;
    public TMP_Text sbqLevel3ScoreText;
    public TMP_Text sbqLevel4ScoreText;
    public TMP_Text sbqLevel5ScoreText;

    DataSaver dataSaver;


    private void Start()
    {
        CloseAllPanels();
        mainPanel.SetActive(true);
        nameText.text = "Name: " + References.userName;
        ageText.text = "Age: " + PlayerPrefs.GetInt("Age").ToString();
        dataSaver = FindFirstObjectByType<DataSaver>();
        
    }

    void CloseAllPanels()
    {
        mainPanel.SetActive(false);
        snakePanel.SetActive(false);
        spellboundPanel.SetActive(false);
    }

    public void OpenMainPanel()
    {
        CloseAllPanels();
        mainPanel.SetActive(true);
    }

    public void OpenSnakePanel()
    {
        CloseAllPanels();
        snakePanel.SetActive(true);
        UpdateSnakePanelScoreAndErrors();
        //dataSaver.LoadData();
    }

    void UpdateSnakePanelScoreAndErrors()
    {
        level1ScoreText.text = "Level 1 Score: " + PlayerPrefs.GetInt("PlayerScore Lv1").ToString();
        level2ScoreText.text = "Level 2 Score: " + PlayerPrefs.GetInt("PlayerScore Lv2").ToString();
        level3ScoreText.text = "Level 3 Score: " + PlayerPrefs.GetInt("PlayerScore Lv3").ToString();
        level4ScoreText.text = "Level 4 Score: " + PlayerPrefs.GetInt("PlayerScore Lv4").ToString();
        level5ScoreText.text = "Level 5 Score: " + PlayerPrefs.GetInt("PlayerScore Lv5").ToString();

        level1ErrorText.text = "Level 1 Error: " + PlayerPrefs.GetInt("Error Count Lv 1").ToString();
        level2ErrorText.text = "Level 2 Error: " + PlayerPrefs.GetInt("Error Count Lv 2").ToString();
        level3ErrorText.text = "Level 3 Error: " + PlayerPrefs.GetInt("Error Count Lv 3").ToString();
        level4ErrorText.text = "Level 4 Error: " + PlayerPrefs.GetInt("Error Count Lv 4").ToString();
        level5ErrorText.text = "Level 5 Error: " + PlayerPrefs.GetInt("Error Count Lv 5").ToString();
    }

    public void OpenSpellboundPanel()
    {
        CloseAllPanels();
        spellboundPanel.SetActive(true);
        UpdateSBQCoins();
    }

    void UpdateSBQCoins()
    {
        sbqLevel1ScoreText.text = "Level 1 Score: " + PlayerPrefs.GetInt("Score 1").ToString();
        sbqLevel2ScoreText.text = "Level 2 Score: " + PlayerPrefs.GetInt("Score 2").ToString();
        sbqLevel3ScoreText.text = "Level 3 Score: " + PlayerPrefs.GetInt("Score 3").ToString();
        sbqLevel4ScoreText.text = "Level 4 Score: " + PlayerPrefs.GetInt("Score 4").ToString();
        sbqLevel5ScoreText.text = "Level 5 Score: " + PlayerPrefs.GetInt("Score 5").ToString();
    }


    
}
