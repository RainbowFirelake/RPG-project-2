﻿using UnityEngine;
using UnityEngine.Playables;

using RPG.Core;
using RPG.Control;

public class CinematicControlRemover : MonoBehaviour
{
    private GameObject player;
    private void Start() 
    {
        player = GameObject.FindWithTag("Player");
        GetComponent<PlayableDirector>().played += DisableControl;
        GetComponent<PlayableDirector>().stopped += EnableControl;
    }

    private void DisableControl(PlayableDirector pb)
    {
        player.GetComponent<ActionScheduler>().CancelCurrentAction();
        player.GetComponent<PlayerController>().enabled = false;
    }

    private void EnableControl(PlayableDirector pb)
    {
        player.GetComponent<PlayerController>().enabled = true;
    }
}
