using TMPro;
using UnityEngine;
// to genereate the report of the player performance and evaluate whether the player is dyslexic or not
public class Report : MonoBehaviour
{
    public TMP_Text name;
    public TMP_Text age;

    //30 slots for each eval

    public GameObject[] slots;

    private int sbqTotalError;
    private int snakeTotalError;
    private int psTotalError;
    private float rnCTotalError;
    private float rnCTotalTime;

    DataSaver dataSaver;

    private void Start()
    {
        Screen.SetResolution(1920, 1080, true);
        dataSaver = FindFirstObjectByType<DataSaver>();
        SetNameAge();
        foreach (GameObject slot in slots)
        {
            slot.SetActive(false);
        }
    }

    public void SetNameAge()
    {
        name.text = "Name: " + dataSaver.dts.username;
        age.text = "Age: " + dataSaver.dts.age.ToString();
    }

    public void CalculateSBQError()
    {
        sbqTotalError = dataSaver.dts.sbqL1Err +
                        dataSaver.dts.sbqL2Err +
                        dataSaver.dts.sbqL3Err +
                        dataSaver.dts.sbqL4Err +
                        dataSaver.dts.sbqL5Err;
        if(sbqTotalError > 0 && sbqTotalError <= 6 )
        {
            slots[0].SetActive(true);
        }
        else if(sbqTotalError > 6 && sbqTotalError <= 12)
        {
            slots[1].SetActive(true);
        }
        else if (sbqTotalError > 12 && sbqTotalError <= 18)
        {
            slots[2].SetActive(true);
        }
        else if (sbqTotalError > 18 && sbqTotalError <= 24)
        {
            slots[3].SetActive(true);
        }
        else if (sbqTotalError > 24)
        {
            slots[4].SetActive(true);
        }

    }

    public void CalculateSnakeError()
    {
        snakeTotalError = dataSaver.dts.snakeL1ErrorCount +
                          dataSaver.dts.snakeL2ErrorCount +
                          dataSaver.dts.snakeL3ErrorCount +
                          dataSaver.dts.snakeL4ErrorCount +
                          dataSaver.dts.snakeL5ErrorCount;
        if(snakeTotalError > 0 && snakeTotalError <= 21)
        {
            slots[5].SetActive(true);
        }
        else if(snakeTotalError > 21 && snakeTotalError <= 42)
        {
            slots[6].SetActive(true);
        }
        else if (snakeTotalError > 42 && snakeTotalError <= 63)
        {
            slots[7].SetActive(true);
        }
        else if (snakeTotalError > 63 && snakeTotalError <= 84)
        {
            slots[8].SetActive(true);
        }
        else if (snakeTotalError > 84)
        {
            slots[9].SetActive(true);
        }
    }

    public void CalculatePsError()
    {
        psTotalError = dataSaver.dts.PSErrL1 +
                       dataSaver.dts.PSErrL2 +
                       dataSaver.dts.PSErrL3;

        if (psTotalError > 0 && psTotalError <= 5)
        {
            slots[10].SetActive(true);
        }
        else if (psTotalError > 5 && psTotalError <= 10)
        {
            slots[11].SetActive(true);
        }
        else if (psTotalError > 10 && psTotalError <= 15)
        {
            slots[12].SetActive(true);
        }
        else if (psTotalError > 15 && psTotalError <= 20)
        {
            slots[13].SetActive(true);
        }
        else if (psTotalError > 20)
        {
            slots[14].SetActive(true);
        }
    }

    public void CalculateRncError()
    {
        rnCTotalError = dataSaver.dts.RnCT1Score +
                        dataSaver.dts.RnCT2Score +
                        dataSaver.dts.RnCT3Score +
                        dataSaver.dts.RnCT4Score +
                        dataSaver.dts.RnCT5Score +
                        dataSaver.dts.RnCT6Score +
                        dataSaver.dts.RnCT7Score;
        if (rnCTotalError > 0 && rnCTotalError <= 140)
        {
            slots[15].SetActive(true);
        }
        else if (rnCTotalError > 140 && rnCTotalError <= 280)
        {
            slots[16].SetActive(true);
        }
        else if (rnCTotalError > 280 && rnCTotalError <= 420)
        {
            slots[17].SetActive(true);
        }
        else if (rnCTotalError > 420 && rnCTotalError <= 560)
        {
            slots[18].SetActive(true);
        }
        else if (rnCTotalError > 560)
        {
            slots[19].SetActive(true);
        }

        rnCTotalTime = dataSaver.dts.RnCT1Time +
                       dataSaver.dts.RnCT2Time +
                       dataSaver.dts.RnCT3Time +
                       dataSaver.dts.RnCT4Time +
                       dataSaver.dts.RnCT5Time +
                       dataSaver.dts.RnCT6Time +
                       dataSaver.dts.RnCT7Time;

    }

    

}
