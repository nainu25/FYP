using TMPro;
using UnityEngine;
// to genereate the report of the player performance and evaluate whether the player is dyslexic or not
public class Report : MonoBehaviour
{
    public TMP_Text name;
    public TMP_Text age;

    private int sbqTotalError;
    private int snakeTotalError;
    private int psTotalError;
    private float rnCTotalError;

    DataSaver dataSaver;

    private void Start()
    {
        Screen.SetResolution(1920, 1080, true);
        dataSaver = FindFirstObjectByType<DataSaver>();
        SetNameAge();

    }

    public void SetNameAge()
    {
        name.text = "Name: " + dataSaver.dts.username;
        age.text = "Age: " + dataSaver.dts.age.ToString();
    }

    public int CalculateSBQError()
    {
        sbqTotalError = dataSaver.dts.sbqL1Err +
                        dataSaver.dts.sbqL2Err +
                        dataSaver.dts.sbqL3Err +
                        dataSaver.dts.sbqL4Err +
                        dataSaver.dts.sbqL5Err;
        if(sbqTotalError > 0 && sbqTotalError <= 6 )
        {
            return 1;
        }
        else if(sbqTotalError > 6 && sbqTotalError <= 12)
        {
            return 2;
        }
        else if (sbqTotalError > 12 && sbqTotalError <= 18)
        {
            return 3;
        }
        else if (sbqTotalError > 18 && sbqTotalError <= 24)
        {
            return 4;
        }
        else if (sbqTotalError > 24)
        {
            return 5;
        }
        else return 0;

    }

    public void CalculateSnakeError()
    {
        snakeTotalError = dataSaver.dts.snakeL1ErrorCount +
                          dataSaver.dts.snakeL2ErrorCount +
                          dataSaver.dts.snakeL3ErrorCount +
                          dataSaver.dts.snakeL4ErrorCount +
                          dataSaver.dts.snakeL5ErrorCount;
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
    }

    public void CalculatePsError()
    {
        psTotalError = dataSaver.dts.PSErrL1 +
                       dataSaver.dts.PSErrL2 +
                       dataSaver.dts.PSErrL3;
    }

}
