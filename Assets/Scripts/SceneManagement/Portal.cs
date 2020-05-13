using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using RPG.Control;
using UnityEngine.AI;

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
            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);
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
                player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
                player.transform.rotation = otherPortal.spawnPoint.rotation;
            }
            else
            {
                print("PORTAL: UpdatePlayer: no otherPortal");
            }
        }
    }
}