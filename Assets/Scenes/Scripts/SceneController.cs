using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{
    int sceneIndex;
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            StartCoroutine(EndSplashScreen());
        }
    }
    
    IEnumerator EndSplashScreen()
    {
        yield return new WaitForSeconds(1.9f);
        SceneManager.LoadScene("Select Login");
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
        Debug.Log("Quitted");
    }

    public void Play()
    {
        SceneManager.LoadScene("Game Selector");
    }

    public void Back()
    {
        if(SceneManager.GetActiveScene().name == "Sign In" || SceneManager.GetActiveScene().name == "Sign Up") 
        { 
            SceneManager.LoadScene("Select Login");
        }
        else
        {
            SceneManager.LoadScene("Main Menu");
        }
        
    }

    public void SignIn()
    {
        SceneManager.LoadScene("Sign In");
    }

    public void SignUp()
    {
        SceneManager.LoadScene("Sign Up");
    }
}
