using UnityEngine;

namespace Application
{
    public class InitialSceneLoader : MonoBehaviour
    {
        [SerializeField] private string _initialScene; 
        void Start()
        {
            //TODO: Save User's info if you had the time
            SceneLoader.LoadScene(_initialScene);
        }

    }

}