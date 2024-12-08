using UnityEngine;

public class LoginPageManager : MonoBehaviour
{
    public static LoginPageManager instance;

    public GameObject selectLogin;
    public GameObject loginPanel;
    public GameObject signUpPanel;
    public GameObject forgotPasswordPanel;

    private void Awake()
    {
        CloseAllPanels();
        selectLogin.SetActive(true);
    }

    void CloseAllPanels()
    {
        selectLogin.SetActive(false);
        loginPanel.SetActive(false);
        signUpPanel.SetActive(false); 
        forgotPasswordPanel.SetActive(false);
    }

    public void LoginPanel()
    {
        CloseAllPanels();
        loginPanel.SetActive(true);
    }

    public void SignUpPanel()
    {
        CloseAllPanels();
        signUpPanel.SetActive(true);
    }

    public void ForgotPasswordPanel()
    {
        CloseAllPanels();
        forgotPasswordPanel.SetActive(true);    
    }

    public void Back()
    {
        CloseAllPanels();
        selectLogin.SetActive(true);
    }



}
