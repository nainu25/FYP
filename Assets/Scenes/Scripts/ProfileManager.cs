using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase;
using System.Collections;
using TMPro;
using Firebase.Extensions;
using System;

public class ProfileManager : MonoBehaviour
{
    [Header("Firebase Variables")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;

    [Space]
    [Header("Login Variables")]
    public TMPro.TMP_InputField loginEmailField;
    public TMPro.TMP_InputField loginPasswordField;
    public TMP_Text loginWarningText;

    [Space]
    [Header("Sign Up Variables")]
    public TMPro.TMP_InputField SignUpNameField;
    public TMPro.TMP_InputField SignUpEmailField;
    public TMPro.TMP_InputField SignUpPasswordField;
    public TMPro.TMP_InputField ageField;
    public TMP_Text signUpWarningText;

    [Space]
    [Header("Forgot Password")]
    public TMPro.TMP_InputField forgotMailField;
    public TMP_Text forgotPText;

    public static ProfileManager instance;

    private void Awake()
    {
        // Singleton pattern to ensure a single instance of AudioController
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // Make this object persistent across scenes
    }

    private void Start()
    {
        StartCoroutine(CheckAndFixDependenciesAsync());
        if(LoginPageManager.instance == null)
        {
            LoginPageManager.instance = FindFirstObjectByType<LoginPageManager>();
        }
    }

    private IEnumerator CheckAndFixDependenciesAsync()
    {
        var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();
        yield return new WaitUntil(() => dependencyTask.IsCompleted);

        dependencyStatus = dependencyTask.Result;
        if (dependencyStatus == DependencyStatus.Available)
        {
            InitializeFirebase();
            yield return new WaitForEndOfFrame();
            StartCoroutine(CheckforAutoLogin());
        }
        else
        {
            Debug.LogError("Firebase dependency is not available. Check your connection and try again." + dependencyStatus);
        }
    }

    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    private IEnumerator CheckforAutoLogin()
    {
        if(user != null)
        {
            var reloadUserTask = user.ReloadAsync();

            yield return new WaitUntil(() => reloadUserTask.IsCompleted);

            AutoLogin();
        }
        else
        {
            LoginPageManager.instance.LoginPanel();
        }
    }

    private void AutoLogin()
    {
        if(user!=null)
        {
            References.userName = user.DisplayName;
            SceneManager.LoadScene("Main Menu");
        }
        else
        {
            LoginPageManager.instance.LoginPanel();
        }
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if(auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if(!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
                SceneManager.LoadScene("Select Login");
            }
            user = auth.CurrentUser;
            if(signedIn)
            {
                Debug.Log("Signed in " + user.UserId + user.DisplayName);
                References.userID = user.UserId;
                References.userName = user.DisplayName;

            }
        }
    }

    public void SignOut()
    {
        if(auth !=null && user != null)
        {
            auth.SignOut();
            SceneManager.LoadScene("Splash Screen");
        }
    }

    public void Login()
    {
        StartCoroutine(LoginAsync(loginEmailField.text, loginPasswordField.text));
    }

    private IEnumerator LoginAsync(string email, string password)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(predicate: () => loginTask.IsCompleted);

        if(loginTask.Exception != null)
        {
            Debug.LogError(loginTask.Exception);

            FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException; // FirebaseException is the base class for all Firebase exceptionsoginTask
            AuthError authError = (AuthError)firebaseException.ErrorCode;

            string failedMessage = "Failed to sign in: " + authError.ToString();

            switch(authError)
            {
                case AuthError.MissingEmail:
                    failedMessage += "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    failedMessage += "Missing Password";
                    break;
                case AuthError.InvalidEmail:
                    failedMessage += "Invalid Email";
                    break;
                case AuthError.WrongPassword:
                    failedMessage += "Wrong Password";
                    break;
                default:
                    failedMessage += "Login Failed";
                    break;
            }
            Debug.Log(failedMessage);
            loginWarningText.text = failedMessage;
        }
        else
        {
            user = loginTask.Result.User;
            References.userName = user.DisplayName;
            SceneManager.LoadScene("Main Menu");
        }
    }

    public void SignUp()
    {
        StartCoroutine(SignUpAsync(SignUpNameField.text, SignUpEmailField.text, SignUpPasswordField.text));
    }

    private IEnumerator SignUpAsync(string name, string email, string password)
    {
        if(name=="")
        {
            Debug.LogError("Username is Empty");
        }
        else if(email=="")
        {
            Debug.LogError("Email is Empty");
        }
        else if(password=="")
        {
            Debug.LogError("Password is Empty");
        }
        else
        {
            var signUpTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
            yield return new WaitUntil(() => signUpTask.IsCompleted);

            if(signUpTask.Exception!=null)
            {
                Debug.LogError(signUpTask.Exception);

                FirebaseException firebaseException = signUpTask.Exception.GetBaseException() as FirebaseException;
                AuthError authError = (AuthError)firebaseException.ErrorCode;

                string failedmessage = "Sign Up failed because ";
                switch(authError)
                {
                    case AuthError.MissingEmail:
                        failedmessage += "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        failedmessage += "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        failedmessage += "Weak Password";
                        break;
                    case AuthError.InvalidEmail:
                        failedmessage += "Invalid Email";
                        break;
                    case AuthError.WrongPassword:
                        failedmessage += "Wrong Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        failedmessage += "Email Already In Use";
                        break;
                    default:
                        failedmessage += "Sign Up Failed";
                        break;
                }
                Debug.Log(failedmessage);
                signUpWarningText.text = failedmessage;
            }
            else
            {
                user = signUpTask.Result.User;
                References.age = ageField.text;
                Debug.Log("Age: " + References.age);
                PlayerPrefs.SetInt("Age", int.Parse(References.age));
                
                UserProfile userProfile = new UserProfile{DisplayName = name};
                var updateProfileTask = user.UpdateUserProfileAsync(userProfile);
                yield return new WaitUntil(() => updateProfileTask.IsCompleted);

                if(updateProfileTask.Exception != null)
                {
                    user.DeleteAsync();
                    Debug.LogError(updateProfileTask.Exception);

                    FirebaseException firebaseException = updateProfileTask.Exception.GetBaseException() as FirebaseException;
                    AuthError authError = (AuthError)firebaseException.ErrorCode;

                    string failedMessage = "Profile update failed because ";
                    switch(authError)
                    {
                        case AuthError.InvalidEmail:
                            failedMessage += "Invalid Email";
                            break;
                        case AuthError.WrongPassword:
                            failedMessage += "Wrong Password";
                            break;
                        case AuthError.MissingEmail:
                            failedMessage += "Missing Email";
                            break;
                        case AuthError.MissingPassword:
                            failedMessage += "Missing Password";
                            break;
                        default:
                            failedMessage += "Profile Update Failed";
                            break;
                    }
                    Debug.Log(failedMessage);
                    signUpWarningText.text = failedMessage;
                }
                else
                {
                    References.userName = user.DisplayName;
                    SceneManager.LoadScene("Main Menu");
                    
                }
            }

        }
        
    }


    void ForgetPasswordSubmit(string email)
    {
        auth.SendPasswordResetEmailAsync(email).ContinueWithOnMainThread(task =>
        {
            if(task.IsCanceled)
            {
                Debug.Log("SendPasswordResetEmailAsync was cancelled");
            }

            if (task.IsFaulted)
            {
                foreach (Exception ex in task.Exception.Flatten().InnerExceptions)
                {
                    Firebase.FirebaseException firebaseException = ex as FirebaseException;
                    if (firebaseException != null)
                    {
                        var error = (AuthError)firebaseException.ErrorCode;
                        Debug.Log("Firebase error: " + error);
                    }
                }
            }

            forgotPText.color = Color.black;
            forgotPText.text = "Reset mail sent";
        });
    }

    public void ResetPassword()
    {
        if(string.IsNullOrEmpty(forgotMailField.text))
        {
            forgotPText.color = Color.red;
            forgotPText.text = "Enter Mail";
        }
        else
        {
            ForgetPasswordSubmit(forgotMailField.text);
        }   

    }

}
