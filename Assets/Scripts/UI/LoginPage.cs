using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace UI{
    
    public class LoginPage : MonoBehaviour
    {
        [SerializeField] private TMP_InputField m_UsernameInput;
        [SerializeField] private Button m_LoginButton;


        private void Start()
        {
            m_LoginButton.onClick.AddListener(OnLoginButtonPressed);
        }

        private void OnLoginButtonPressed()
        {
            Core.ApplicationLogin.TryLogin(m_UsernameInput.text);
        }

    }
}
