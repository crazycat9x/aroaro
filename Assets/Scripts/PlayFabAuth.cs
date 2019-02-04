using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

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

        TouchScreenKeyboard keyboard;
        PFAuthKeyboardState keyboardState = PFAuthKeyboardState.NotActive;

        string keyboardText;
        string user;
        string password;

        public List<Action<PlayFabError>> errorActions = new List<Action<PlayFabError>>();
        public List<Action<LoginResult>> loginActions = new List<Action<LoginResult>>();
        public List<Action<RegisterPlayFabUserResult>> registerActions = new List<Action<RegisterPlayFabUserResult>>();

        public static PlayFabAuth instance = null;

        private void Start()
        {
            if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
                PlayFabSettings.TitleId = "4CBA"; // make sure the title is set to the aroaro ID

            loggedIn = PlayFabClientAPI.IsClientLoggedIn();
            if (!loggedIn)
            {
                // display the auth window if the client is not logged in
                transform.Find("Container").gameObject.SetActive(true);
            }
        }

        private void Update()
        {
            if (TouchScreenKeyboard.visible == false && keyboard != null)
            {
                if (keyboard.status == TouchScreenKeyboard.Status.Done)
                {
                    if (keyboardState == PFAuthKeyboardState.ActiveUser)
                        user = keyboard.text;
                    else if (keyboardState == PFAuthKeyboardState.ActivePassword)
                        password = keyboard.text;

                    keyboard.text = null;
                    keyboardState = PFAuthKeyboardState.NotActive;
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

        public void ActivateEntryUser()
        {
            keyboardState = PFAuthKeyboardState.ActiveUser;
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false, "Enter username");
        }

        public void ActivateEntryPassword()
        {
            keyboardState = PFAuthKeyboardState.ActivePassword;
            keyboard = keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, true, false, "Enter password");
        }

        public void DoLogin()
        {
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
                return; // display some error

            LoginWithPlayFabRequest req = new LoginWithPlayFabRequest {
                TitleId = PlayFabSettings.TitleId,
                Username = user,
                Password = password
            };

            Login(req);
        }

        public void DoRegister()
        {
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
                return; // display some error

            RegisterPlayFabUserRequest req = new RegisterPlayFabUserRequest {
                TitleId = PlayFabSettings.TitleId,
                Username = user,
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
            foreach (Action<LoginResult> action in loginActions)
                action(res);
        }

        private void OnRegisterSuccess(RegisterPlayFabUserResult res)
        {
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
