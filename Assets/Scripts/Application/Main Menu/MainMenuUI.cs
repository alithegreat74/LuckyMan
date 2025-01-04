using Network;
using Sfs2X.Entities;
using TMPro;
using UnityEngine;

namespace Application
{
    namespace MainMenu
    {
        public class MainMenuUI : MonoBehaviour
        {
            [SerializeField] private TextMeshProUGUI _username;
            private void Start()
            {
                _username.text = NetworkAPI.GetMyself().Name;
            }
        }

    }
}