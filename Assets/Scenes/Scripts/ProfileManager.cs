using UnityEngine;
using UnityEngine.SceneManagement;

public class ProfileManager : MonoBehaviour
{
    public TMPro.TMP_InputField emailField;
    public TMPro.TMP_InputField passwordField;

    public void Login()
    {
        if(PlayerPrefs.GetString("email") == emailField.text && PlayerPrefs.GetString("password") == passwordField.text)
        {
            SceneManager.LoadScene("Main Menu");
        }
        else
        {
            passwordField.text = "";
        }
    }

}
