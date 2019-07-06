using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour {


    [Tooltip("In seconds")]
    [SerializeField] float levelLoadDelay = 1f;

    [Tooltip("FXs prefabbed on the player")]
    [SerializeField] GameObject deathFX;



    void OnTriggerEnter(Collider collider)
    {
        StartDeathSequence();
        deathFX.SetActive(true);
        Invoke("ReloadScene", levelLoadDelay);
    }

    private void StartDeathSequence()
    {
        SendMessage("OnPlayerDeath");
    }

    private void ReloadScene() // string ref
    {
        SceneManager.LoadScene(1);
    }
}
