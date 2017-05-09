using UnityEngine;
using UnityEngine.SceneManagement;

namespace Zulu
{

    class LevelLoader : MonoBehaviour
    {
        public static string sceneName;

        public void Load(string scene)
        {
            SceneManager.LoadScene(scene);
        }

        public void SetLevelName(string name)
        {
            sceneName = name;
        }

        public void Exit()
        {
            Application.Quit();
        }
    }

}