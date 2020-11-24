using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B, C, D, E, F
        }

        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destinationIdentifier;
        [SerializeField] float fadeOutTime = 0.5f;
        [SerializeField] float fadeInTime = 0.5f;
        [SerializeField] float fadeWaitTime = 1f;

        private void Start() 
        {
            
        }

        private void OnTriggerEnter(Collider other) 
        {
            if(other.gameObject.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }
        private IEnumerator Transition()
        {
            if (sceneToLoad < 0) 
            {
                Debug.LogError("Scene load not found");
                yield break;
            }
            DontDestroyOnLoad(gameObject);
            Fader fader = FindObjectOfType<Fader>();

            yield return fader.FadeOut(fadeOutTime);

           SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            
          //  wrapper.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            //wrapper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            //wrapper.Save();

            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);

            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {

            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.transform.position = otherPortal.spawnPoint.position;
            //player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
        }

        private Portal GetOtherPortal()
        {
            foreach(Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.destinationIdentifier != destinationIdentifier) continue;
                return portal;
            }
            return null;
        }
    }
}
