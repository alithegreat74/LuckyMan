using Model;
using System;
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
        [SerializeField] private PopUp _popUp;

        private void Awake()
        {
            _loginButton.onClick.AddListener(LoginButtonClicked);
        }

        private async void LoginButtonClicked()
        {
            UserInfo loginInfo = new UserInfo() { Username = _usernameInput.text, Password = _paswordInput.text };
            Tuple<bool,string> result = await GetComponent<LoginManager>().LoginSequence(loginInfo);
            if (!result.Item1)
            {
                _popUp.ShowPopUp("Unable to complete the login sequence");
                return;
            }
            SceneManager.LoadScene("Main Menu");
        }
    }

}