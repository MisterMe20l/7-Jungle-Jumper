using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class SceneLoadRequester  : MonoBehaviour
    {
        [SerializeField] string m_sceneName;
        [SerializeField] bool m_loadingScreen = false;
        [SerializeField] LoadSceneMode m_loadSceneMode = LoadSceneMode.Single;

        public void RequestScene()
        {
            SceneLoader.instance.LoadSceneAsync(m_loadingScreen, m_sceneName, m_loadSceneMode);
        }
    }
}
