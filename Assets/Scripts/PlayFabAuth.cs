using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.UI.Keyboard;
using TMPro;
using UnityEngine.SceneManagement;

namespace Aroaro
{
    public enum PFAuthKeyboardState
    {
        NotActive,
        ActiveUser,
        ActivePassword
    }

    public class PlayFabAuth : MonoBehaviour
    {
        bool loggedIn;

        Keyboard keyboard;
        PFAuthKeyboardState keyboardState = PFAuthKeyboardState.NotActive;

        string keyboardText;
        string username;
        string password;

        InputField userInput;
        InputField passInput;
        TextMeshProUGUI infoText;

        public List<Action<PlayFabError>> errorActions = new List<Action<PlayFabError>>();
        public List<Action<LoginResult>> loginActions = new List<Action<LoginResult>>();
        public List<Action<RegisterPlayFabUserResult>> registerActions = new List<Action<RegisterPlayFabUserResult>>();

        public static PlayFabAuth instance = null;

        private void Start()
        {
            if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
                PlayFabSettings.TitleId = "4CBA"; // make sure the title is set to the aroaro ID

            transform.Find("ContinuePanel/Panel/ContinueButton").gameObject.GetComponent<Button>()
                .onClick.AddListener(() =>
                {
                    transform.Find("ContinuePanel").gameObject.SetActive(false); // hide panel
                    StartCoroutine(Continue()); // transition scene
                });

            loggedIn = PlayFabClientAPI.IsClientLoggedIn();
            if (loggedIn)
                transform.Find("ContinuePanel").gameObject.SetActive(true);
            else
            {
                // show the auth panel
                transform.Find("AuthPanel").gameObject.SetActive(true);

                infoText = transform.Find("AuthPanel/Panel/InfoText").gameObject.GetComponent<TextMeshProUGUI>();

                // add the listeners for the buttons
                transform.Find("AuthPanel/Panel/LoginButton").gameObject.GetComponent<Button>()
                    .onClick.AddListener(DoLogin);
                transform.Find("AuthPanel/Panel/RegisterButton").gameObject.GetComponent<Button>()
                    .onClick.AddListener(DoRegister);

                // add the listeners for the inputs
                userInput = transform.Find("AuthPanel/Panel/UsernameInput").gameObject.GetComponent<InputField>();
                userInput.onEndEdit.AddListener((string txt) =>
                {
                    username = txt;
                    keyboardState = PFAuthKeyboardState.NotActive;
                    Keyboard.Instance.OnTextUpdated -= UpdateUserInput;
                    Keyboard.Instance.Close(); // make sure kbd closes
                });

                passInput = transform.Find("AuthPanel/Panel/PasswordInput").gameObject.GetComponent<InputField>();
                passInput.onEndEdit.AddListener((string txt) =>
                {
                    password = txt;
                    keyboardState = PFAuthKeyboardState.NotActive;
                    Keyboard.Instance.OnTextUpdated -= UpdatePassInput;
                    Keyboard.Instance.Close();
                });
            }

        }

        private void Update()
        {
            if (!loggedIn && userInput != null && passInput != null)
            {
                if (keyboardState == PFAuthKeyboardState.NotActive)
                {
                    if (userInput.isFocused) // input is the current target
                    {
                        keyboardState = PFAuthKeyboardState.ActiveUser;
                        Keyboard.Instance.OnTextUpdated += UpdateUserInput;
                    }
                    else if (passInput.isFocused)
                    {
                        keyboardState = PFAuthKeyboardState.ActivePassword;
                        Keyboard.Instance.OnTextUpdated += UpdatePassInput;
                    }

                }
                else
                    Keyboard.Instance.PresentKeyboard();
            }
        }

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
        }

        public void UpdateUserInput(string txt)
        {
            if (!string.IsNullOrEmpty(txt) && userInput != null)
                userInput.text = txt;
        }

        public void UpdatePassInput(string txt)
        {
            if (!string.IsNullOrEmpty(txt) && passInput != null)
                passInput.text = txt;
        }

        public void DoLogin()
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                infoText.text = "Please enter a username and password";
                return; // display some error
            }

            infoText.text = "Logging in, please wait...";

            LoginWithPlayFabRequest req = new LoginWithPlayFabRequest
            {
                TitleId = PlayFabSettings.TitleId,
                Username = username,
                Password = password
            };

            password = null;
            passInput.text = null; // clear auth vars

            Login(req);
        }

        public void DoRegister()
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                infoText.text = "Please enter a username and password";
                return; // display some error
            }

            infoText.text = "Registering, please wait...";

            RegisterPlayFabUserRequest req = new RegisterPlayFabUserRequest
            {
                TitleId = PlayFabSettings.TitleId,
                Username = username,
                Password = password,
                RequireBothUsernameAndEmail = false
            };

            password = null;
            passInput.text = null; // clear auth vars

            Register(req);
        }

        private void Login(LoginWithPlayFabRequest req)
        {
            PlayFabClientAPI.LoginWithPlayFab(req, OnLoginSuccess, OnAuthFailure);
        }

        private void Register(RegisterPlayFabUserRequest req)
        {
            PlayFabClientAPI.RegisterPlayFabUser(req, OnRegisterSuccess, OnAuthFailure);
        }

        // action callbacks
        private void OnLoginSuccess(LoginResult res)
        {
            Debug.Log("logged in successfully");
            transform.Find("AuthPanel").gameObject.SetActive(false);
            transform.Find("ContinuePanel").gameObject.SetActive(true);

            username = null;
            userInput.text = null;
            loggedIn = true;

            UpdateWelcomeText();

            foreach (Action<LoginResult> action in loginActions)
                action(res);
        }

        private void OnRegisterSuccess(RegisterPlayFabUserResult res)
        {
            Debug.Log("regstered successfully");
            transform.Find("AuthPanel").gameObject.SetActive(false);
            transform.Find("ContinuePanel").gameObject.SetActive(true);

            username = null;
            userInput.text = null;
            loggedIn = true;

            UpdateWelcomeText();

            foreach (Action<RegisterPlayFabUserResult> action in registerActions)
                action(res);
        }

        private void UpdateWelcomeText()
        {
            if (loggedIn)
                PlayFabClientAPI.GetAccountInfo(null, (GetAccountInfoResult data) =>
                {
                    transform.Find("ContinuePanel/Panel/WelcomeText").gameObject
                        .GetComponent<TextMeshProUGUI>().text = string.Format("Welcome, {0}!", data.AccountInfo.Username);
                }, (PlayFabError err) => Debug.LogError(err));
        }

        private void OnAuthFailure(PlayFabError error)
        {
            Debug.LogError("PlayFab authentication failed:");
            Debug.LogError(error.GenerateErrorReport());

            switch (error.Error)
            {
                case PlayFabErrorCode.AccountNotFound:
                    infoText.text = "Account not found (have you registered?)";
                    break;
                case PlayFabErrorCode.InvalidUsernameOrPassword:
                    infoText.text = "Invalid username or password";
                    break;
                case PlayFabErrorCode.EmailAddressNotAvailable:
                    infoText.text = "Email address not available";
                    break;
                case PlayFabErrorCode.InvalidEmailAddress:
                    infoText.text = "Invalid email address";
                    break;
                case PlayFabErrorCode.InvalidPassword:
                    infoText.text = "Password must be 6-100 characters";
                    break;
                case PlayFabErrorCode.InvalidUsername:
                    infoText.text = "Username must be 3-20 characters";
                    break;
                case PlayFabErrorCode.UsernameNotAvailable:
                case PlayFabErrorCode.NameNotAvailable:
                    infoText.text = "Username not available";
                    break;
                case PlayFabErrorCode.ProfaneDisplayName:
                    infoText.text = "Please choose another username";
                    break;
                default:
                    infoText.text = "Unknown error, please try again later";
                    break;
            }

            foreach (Action<PlayFabError> action in errorActions)
                action(error);
        }

        private IEnumerator Continue()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("WhiteboardTestScene", LoadSceneMode.Single);
            asyncLoad.allowSceneActivation = false;
            while (!asyncLoad.isDone)
            {
                if (asyncLoad.progress >= 0.9f)
                    asyncLoad.allowSceneActivation = true;
                yield return null;
            }
        }
    }
}
