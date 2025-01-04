using UnityEngine;
using UnityEngine.UI;

namespace Application
{
    namespace BaseUI
    {
        [RequireComponent(typeof(Button))]
        public class SceneLoaderButton : MonoBehaviour
        {
            [SerializeField] private string _sceneName;
            private Button _button;
            
            private void OnEnable()
            {
                _button = GetComponent<Button>();
                _button.onClick.AddListener(LoadScene);
            }

            private void LoadScene()
            {
                SceneLoader.LoadScene(_sceneName);
            }
        }
    }
}
