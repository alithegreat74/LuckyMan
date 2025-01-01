using Model;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Application
{
    public class LoginUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _usernameInput;
        [SerializeField] private TMP_InputField _paswordInput;
        [SerializeField] private Button _loginButton;
        [SerializeField] private Button _signUpButton;

        private void Awake()
        {
            _loginButton.onClick.AddListener(LoginButtonClicked);
            _signUpButton.onClick.AddListener(GotoSignUp);
        }

        private void LoginButtonClicked()
        {
            UserInfo loginInfo = new UserInfo() { Username = _usernameInput.text, Password = _paswordInput.text };
            GetComponent<LoginManager>().SendLoginRequest(loginInfo);
        }

        private void GotoSignUp()
        {
            SceneManager.LoadScene("Sign Up");
        }

    }

}