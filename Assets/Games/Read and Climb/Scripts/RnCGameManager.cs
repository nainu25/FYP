using UnityEngine;

public class RnCGameManager : MonoBehaviour
{
    private void Awake()
    {
        Screen.SetResolution(1080, 1920, FullScreenMode.FullScreenWindow);
        Screen.orientation = ScreenOrientation.Portrait;
        UnityEditor.PlayModeWindow.SetCustomRenderingResolution(1080, 1920, "Portrait");
    }
}
