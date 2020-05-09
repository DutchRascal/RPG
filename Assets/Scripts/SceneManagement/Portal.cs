using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField]
        int sceneToLoad = -1;

        private void OnTriggerEnter(Collider other)
        {
            if (other.name != "Player") { return; }
            StartCoroutine(Transition());
        }

        IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            print("Scene Loaded");
            Destroy(gameObject);
        }
    }
}