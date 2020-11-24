using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        bool isPlayedBefore;

        private void Start() 
        {
            isPlayedBefore = false;
        }

        private void OnTriggerEnter(Collider other) 
        {
            if (!isPlayedBefore && other.gameObject.tag == "Player")
            {
                GetComponent<PlayableDirector>().Play();
                isPlayedBefore = true;
            }
        }
    }
}