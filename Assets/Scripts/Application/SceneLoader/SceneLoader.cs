using UnityEngine.SceneManagement;

namespace Application
{
    public class SceneLoader
    {
        public static void LoadScene(string path)
        {
            UnloadAllScenes();

            SceneManager.LoadScene(path,LoadSceneMode.Additive);
        }

        private static void UnloadAllScenes()
        {
            for(int i=1;i<SceneManager.sceneCount;i++)
            {
                SceneManager.UnloadSceneAsync(i);
            }
        }
    }

}