using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Player : MonoBehaviour {

    public float NoiseLevel = 1;
    public float VisibilityLevel = 20;

    public int NumberAttempts = 3;

    [SerializeField] private PlayableDirector playableDirectorFalling;

    private PlayerMovement playerMovement = null;

    private bool isAlive = true;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();


    }

    public void DiedFromTraps()
    {
        NumberAttempts--;
        if (NumberAttempts > 0) return;
        Died();
    }

    public void Died()
    {
        if (!isAlive) return;
        isAlive = false;
        playableDirectorFalling.Play();
        playerMovement.SetCanMove(false);
        RememberFlashlight.Instance.RememberLastPosition(transform.position);
        GameController.Instance.HandleGameOver(false);
    }

    public void RaiseFlashlight(Flashlight flashlight)
    {     
        playerMovement.SetHasFlashlight(true);
        playerMovement.TurnOnFlashlight(true);
        Destroy(flashlight.gameObject);
    }
}
