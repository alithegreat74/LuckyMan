using Model;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Application
{
    public class SignUpUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _usernameInput;
        [SerializeField] private TMP_InputField _passwordInput;
        [SerializeField] private TMP_InputField _confirmPasswordInput;
        [SerializeField] private Button _signupButton;
        [SerializeField] private PopUp _popUp;
        private void Awake()
        {
            _signupButton.onClick.AddListener(SignUpButtonClicked);
        }

        private async void SignUpButtonClicked()
        {
            // Check regex
            if (_confirmPasswordInput.text != _passwordInput.text)
                throw new System.Exception("The passwords don't match");

            var info = new UserInfo()
            {
                Username = _usernameInput.text,
                Password = _passwordInput.text,
            };

            Tuple<bool,string> result = await GetComponent<SignUpManager>().SendRequest(info);
            if(!result.Item1)
            {
                _popUp.ShowPopUp(result.Item2);
                return;
            }
            SceneLoader.LoadScene("Login");
        }
    }
}
