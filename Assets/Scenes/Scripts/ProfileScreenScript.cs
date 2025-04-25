using TMPro;
using Unity.VisualScripting;
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
    public GameObject rncPanel;
    public GameObject psPanel;

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

    [Space]
    [Header("SBQ Levels Error Text")]
    public TMP_Text sbqLevel1ErrorText;
    public TMP_Text sbqLevel2ErrorText;
    public TMP_Text sbqLevel3ErrorText;
    public TMP_Text sbqLevel4ErrorText;
    public TMP_Text sbqLevel5ErrorText;

    [Space]
    [Header("RnC Tasks Accuracy")]
    public TMP_Text RnCT1Acc;
    public TMP_Text RnCT2Acc;
    public TMP_Text RnCT3Acc;
    public TMP_Text RnCT4Acc;
    public TMP_Text RnCT5Acc;
    public TMP_Text RnCT6Acc;
    public TMP_Text RnCT7Acc;

    [Space]
    [Header("PS Levels Score Text")]
    public TMP_Text psL1ScoreText;
    public TMP_Text psL2ScoreText;
    public TMP_Text psL3ScoreText;

    [Space]
    [Header("PS Levels Error Text")]
    public TMP_Text psL1ErrorText;
    public TMP_Text psL2ErrorText;
    public TMP_Text psL3ErrorText;

    DataSaver dataSaver;


    private void Start()
    {
        CloseAllPanels();
        mainPanel.SetActive(true);
        nameText.text = "Name: " + References.userName;
        ageText.text = "Age: " + PlayerPrefs.GetInt("Age").ToString();
        dataSaver = FindFirstObjectByType<DataSaver>();
        dataSaver.SaveData();
        dataSaver.LoadData();
        
    }

    void CloseAllPanels()
    {
        mainPanel.SetActive(false);
        snakePanel.SetActive(false);
        spellboundPanel.SetActive(false);
        rncPanel.SetActive(false);
        psPanel.SetActive(false);
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
        sbqLevel1ScoreText.text = "Level 1 Score: " + PlayerPrefs.GetInt("SBQ Score 1").ToString();
        sbqLevel2ScoreText.text = "Level 2 Score: " + PlayerPrefs.GetInt("SBQ Score 2").ToString();
        sbqLevel3ScoreText.text = "Level 3 Score: " + PlayerPrefs.GetInt("SBQ Score 3").ToString();
        sbqLevel4ScoreText.text = "Level 4 Score: " + PlayerPrefs.GetInt("SBQ Score 4").ToString();
        sbqLevel5ScoreText.text = "Level 5 Score: " + PlayerPrefs.GetInt("SBQ Score 5").ToString();

        sbqLevel1ErrorText.text = "Level 1 Error: " + PlayerPrefs.GetInt("SBQ Error 1").ToString();
        sbqLevel2ErrorText.text = "Level 2 Error: " + PlayerPrefs.GetInt("SBQ Error 2").ToString();
        sbqLevel3ErrorText.text = "Level 3 Error: " + PlayerPrefs.GetInt("SBQ Error 3").ToString();
        sbqLevel4ErrorText.text = "Level 4 Error: " + PlayerPrefs.GetInt("SBQ Error 4").ToString();
        sbqLevel5ErrorText.text = "Level 5 Error: " + PlayerPrefs.GetInt("SBQ Error 5").ToString();

    }

    public void OpenRnCPanel()
    {
        CloseAllPanels();
        rncPanel.SetActive(true);
        UpdateRnCScore();
    }

    void UpdateRnCScore()
    {
        RnCT1Acc.text = "Task 1 Accuracy: " + PlayerPrefs.GetFloat("RnC Task 1").ToString();
        RnCT2Acc.text = "Task 2 Accuracy: " + PlayerPrefs.GetFloat("RnC Task 2").ToString();
        RnCT3Acc.text = "Task 3 Accuracy: " + PlayerPrefs.GetFloat("RnC Task 3").ToString();
        RnCT4Acc.text = "Task 4 Accuracy: " + PlayerPrefs.GetFloat("RnC Task 4").ToString();
        RnCT5Acc.text = "Task 5 Accuracy: " + PlayerPrefs.GetFloat("RnC Task 5").ToString();
        RnCT6Acc.text = "Task 6 Accuracy: " + PlayerPrefs.GetFloat("RnC Task 6").ToString();
        RnCT7Acc.text = "Task 7 Accuracy: " + PlayerPrefs.GetFloat("RnC Task 7").ToString();
    }

    public void OpenPSPanel()
    {
        CloseAllPanels();
        psPanel.SetActive(true);
        UpdatePSScoreAndErrors();
    }

    void UpdatePSScoreAndErrors()
    {
        psL1ScoreText.text = "Level 1 Score: " + PlayerPrefs.GetInt("PS L1 Score").ToString();
        psL2ScoreText.text = "Level 2 Score: " + PlayerPrefs.GetInt("PS L2 Score").ToString();
        psL3ScoreText.text = "Level 3 Score: " + PlayerPrefs.GetInt("PS L3 Score").ToString();

        psL1ErrorText.text = "Level 1 Error: " + PlayerPrefs.GetInt("PS L1 Err").ToString();
        psL2ErrorText.text = "Level 2 Error: " + PlayerPrefs.GetInt("PS L2 Err").ToString();
        psL3ErrorText.text = "Level 3 Error: " + PlayerPrefs.GetInt("PS L3 Err").ToString();
    }



}
