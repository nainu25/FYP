using TMPro;
using UnityEngine;

public class Report : MonoBehaviour
{
    public TMP_Text name;
    public TMP_Text age;
    public TMP_Text riskLevel;


    //30 slots for each eval

    public GameObject[] slots;

    private int sbqTotalError;
    private int snakeTotalError;
    private int psTotalError;
    private float rnCTotalError;
    private float rnCTotalTime;

    private int never, rarely, sometimes, frequently, always;

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

        CalculateSBQError();
        CalculateSnakeError();
        CalculatePsError();
        CalculateRncError();
        RequiresHelp();
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
        Debug.Log("SBQ Total Error: " + sbqTotalError);
        if (sbqTotalError > 0 && sbqTotalError <= 6 )
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
            slots[20].SetActive(true);
        }
        else if (rnCTotalError > 140 && rnCTotalError <= 280)
        {
            slots[16].SetActive(true);
            slots[21].SetActive(true);
        }
        else if (rnCTotalError > 280 && rnCTotalError <= 420)
        {
            slots[17].SetActive(true);
            slots[22].SetActive(true);
        }
        else if (rnCTotalError > 420 && rnCTotalError <= 560)
        {
            slots[18].SetActive(true);
            slots[23].SetActive(true);
        }
        else if (rnCTotalError > 560)
        {
            slots[19].SetActive(true);
            slots[24].SetActive(true);
        }

    }

    public void RequiresHelp()
    {
        int total = 0;

        // Check SBQError
        if (sbqTotalError > 0 && sbqTotalError <= 6) total += 0;
        else if (sbqTotalError > 6 && sbqTotalError <= 12) total += 1;
        else if (sbqTotalError > 12 && sbqTotalError <= 18) total += 2;
        else if (sbqTotalError > 18 && sbqTotalError <= 24) total += 3;
        else if (sbqTotalError > 24) total += 4;

        // Check SnakeError
        if (snakeTotalError > 0 && snakeTotalError <= 21) total += 0;
        else if (snakeTotalError > 21 && snakeTotalError <= 42) total += 1;
        else if (snakeTotalError > 42 && snakeTotalError <= 63) total += 2;
        else if (snakeTotalError > 63 && snakeTotalError <= 84) total += 3;
        else if (snakeTotalError > 84) total += 4;

        // Check PsError
        if (psTotalError > 0 && psTotalError <= 5) total += 0;
        else if (psTotalError > 5 && psTotalError <= 10) total += 1;
        else if (psTotalError > 10 && psTotalError <= 15) total += 2;
        else if (psTotalError > 15 && psTotalError <= 20) total += 3;
        else if (psTotalError > 20) total += 4;

        // Check RnCError (reads slowly)
        if (rnCTotalError > 0 && rnCTotalError <= 140) total += 0;
        else if (rnCTotalError > 140 && rnCTotalError <= 280) total += 1;
        else if (rnCTotalError > 280 && rnCTotalError <= 420) total += 2;
        else if (rnCTotalError > 420 && rnCTotalError <= 560) total += 3;
        else if (rnCTotalError > 560) total += 4;

        // Check RnCError again (reads below grade level)
        if (rnCTotalError > 0 && rnCTotalError <= 140) total += 0;
        else if (rnCTotalError > 140 && rnCTotalError <= 280) total += 1;
        else if (rnCTotalError > 280 && rnCTotalError <= 420) total += 2;
        else if (rnCTotalError > 420 && rnCTotalError <= 560) total += 3;
        else if (rnCTotalError > 560) total += 4;

        // Average score (out of 5 categories)
        float average = total / 5f;

        if (average < 0.5f)
        {
            slots[25].SetActive(true); // Never
        }
        else if (average >= 0.5f && average < 1.5f)
        {
            slots[26].SetActive(true); // Rarely
        }
        else if (average >= 1.5f && average < 2.5f)
        {
            slots[27].SetActive(true); // Sometimes
        }
        else if (average >= 2.5f && average < 3.5f)
        {
            slots[28].SetActive(true); // Frequently
        }
        else if (average >= 3.5f)
        {
            slots[29].SetActive(true); // Always
        }

        // Determine Risk Level
        if (average < 1.5f)
        {
            riskLevel.text = "Minimal";
        }
        else if (average >= 1.5f && average < 2.5f)
        {
            riskLevel.text = "Moderate";
        }
        else
        {
            riskLevel.text = "sSignificant";
        }

    }
}
