using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Application
{
    public class LoginUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _usernameInput;
        [SerializeField] private TMP_InputField _paswordInput;
        [SerializeField] private Button _loginButton;

        private void Awake()
        {
            _loginButton.onClick.AddListener(LoginButtonClicked);
        }

        private void LoginButtonClicked()
        {
            LoginInfo loginInfo = new LoginInfo() { Username = _usernameInput.text, Password = _paswordInput.text };
            GetComponent<LoginManager>().SendRequest(loginInfo);
        }



    }

}