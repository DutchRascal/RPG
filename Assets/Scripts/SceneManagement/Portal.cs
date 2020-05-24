using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Saving;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {

        enum DestinationIdentifier
        {
            A, B, C, D, E
        }

        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeInTime = 2f;
        [SerializeField] float fadeWaitTime = 0.5f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.name != "Player") { return; }
            StartCoroutine(Transition());
        }

        IEnumerator Transition()
        {
            if (sceneToLoad < 0)
            {
                Debug.LogError("PORTAL: Transition: sceneToLoad not set!");
                yield break;
            }
            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();

            yield return fader.FadeOut(fadeOutTime);
            savingWrapper.Save();
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            yield return new WaitForSeconds(0.5f);
            savingWrapper.Load();
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);
            savingWrapper.Save();
            Destroy(gameObject);
        }

        private Portal GetOtherPortal()
        {
            Portal[] portals = FindObjectsOfType<Portal>();
            foreach (Portal portal in portals)
            {
                if (portal == this) { continue; }
                if (portal.destination != destination) { continue; }
                return portal;
            }
            return null;
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            if (otherPortal)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<NavMeshAgent>().enabled = false;
                player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
                player.transform.rotation = otherPortal.spawnPoint.rotation;
                player.GetComponent<NavMeshAgent>().enabled = true;
            }
            else
            {
                Debug.LogError("PORTAL: UpdatePlayer: no otherPortal");
            }
        }
    }
}