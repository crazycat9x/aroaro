using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.UI.Keyboard;

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

        public List<Action<PlayFabError>> errorActions = new List<Action<PlayFabError>>();
        public List<Action<LoginResult>> loginActions = new List<Action<LoginResult>>();
        public List<Action<RegisterPlayFabUserResult>> registerActions = new List<Action<RegisterPlayFabUserResult>>();

        public static PlayFabAuth instance = null;

        private void Start()
        {
            if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
                PlayFabSettings.TitleId = "4CBA"; // make sure the title is set to the aroaro ID

            loggedIn = PlayFabClientAPI.IsClientLoggedIn();
            if (loggedIn)
                transform.Find("ContinuePanel").gameObject.SetActive(true);
            else
            {
                transform.Find("AuthPanel").gameObject.SetActive(true);

                userInput = transform.Find("AuthPanel/Panel/UsernameInput").gameObject.GetComponent<InputField>();
                userInput.onEndEdit.AddListener((string txt) => {
                    username = txt;
                    keyboardState = PFAuthKeyboardState.NotActive;
                    Keyboard.Instance.OnTextUpdated -= UpdateUserInput;
                });

                passInput = transform.Find("AuthPanel/Panel/PasswordInput").gameObject.GetComponent<InputField>();
                passInput.onEndEdit.AddListener((string txt) => {
                    password = txt;
                    keyboardState = PFAuthKeyboardState.NotActive;
                    Keyboard.Instance.OnTextUpdated -= UpdatePassInput;
                });
            }

        }

        private void Update()
        {
            if (!loggedIn && userInput != null && passInput != null)
            {
                if (keyboardState == PFAuthKeyboardState.NotActive)
                {
                    if (userInput.isFocused)
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
                {
                    Keyboard.Instance.PresentKeyboard();
                    //keyboard = transform.Find("Keyboard").gameObject.GetComponent<Keyboard>();
                    //keyboard.InputField = userInput;
                    //keyboard.PresentKeyboard();
                }
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
                return; // display some error

            LoginWithPlayFabRequest req = new LoginWithPlayFabRequest
            {
                TitleId = PlayFabSettings.TitleId,
                Username = username,
                Password = password
            };

            Login(req);
        }

        public void DoRegister()
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return; // display some error

            RegisterPlayFabUserRequest req = new RegisterPlayFabUserRequest
            {
                TitleId = PlayFabSettings.TitleId,
                Username = username,
                Password = password,
                RequireBothUsernameAndEmail = false
            };

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
            foreach (Action<LoginResult> action in loginActions)
                action(res);
        }

        private void OnRegisterSuccess(RegisterPlayFabUserResult res)
        {
            Debug.Log("regstered successfully");
            foreach (Action<RegisterPlayFabUserResult> action in registerActions)
                action(res);
        }

        private void OnAuthFailure(PlayFabError error)
        {
            Debug.LogError("PlayFab authentication failed:");
            Debug.LogError(error.GenerateErrorReport());

            foreach (Action<PlayFabError> action in errorActions)
                action(error);
        }
    }
}
