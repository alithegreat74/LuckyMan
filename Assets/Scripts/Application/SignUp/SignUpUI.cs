using Model;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Application
{
    public class SignUpUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _usernameInput;
        [SerializeField] private TMP_InputField _passwordInput;
        [SerializeField] private TMP_InputField _confirmPasswordInput;
        [SerializeField] private Button _signupButton;
        [SerializeField] private Button _loginButton;


        private void Awake()
        {
            _signupButton.onClick.AddListener(SignUpButtonClicked);
            _loginButton.onClick.AddListener(LoginButtonClicked);
        }

        private void SignUpButtonClicked()
        {
            //Check regex
            if (_confirmPasswordInput.text != _passwordInput.text)
                throw new System.Exception("The passwords don't match");

            var info = new UserInfo() { Username=_usernameInput.text, Password=_passwordInput.text };

            GetComponent<SignUpManager>().SendRequest(info);
        }
        private void LoginButtonClicked()
        {
            SceneManager.LoadScene("Login");
        }
    }

}