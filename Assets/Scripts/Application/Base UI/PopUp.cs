using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Application
{
    public class PopUp : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _popUpText;
        [SerializeField] private Button _okButton;
        public void ShowPopUp(string text)
        {
            gameObject.SetActive(true);
            _popUpText.text = text;
            _okButton.onClick.AddListener(HidePopUp);
        }
        private void HidePopUp()
        {
            _okButton.onClick.RemoveAllListeners();
            gameObject.SetActive(false);
        }
    }

}