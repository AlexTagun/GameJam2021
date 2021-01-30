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

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();


    }

    public void DiedFromTraps()
    {
        NumberAttempts--;
        if (NumberAttempts > 0) return;
        playableDirectorFalling.Play();
        playerMovement.SetCanMove(false);
        RememberFlashlight.Instance.RememberLastPosition(transform.position);
        GameController.Instance.Restart();
    }

    public void RaiseFlashlight(Flashlight flashlight)
    {
        playerMovement.SetHasFlashlight(true);
        playerMovement.TurnOnFlashlight(true);
        Destroy(flashlight.gameObject);
    }
}
