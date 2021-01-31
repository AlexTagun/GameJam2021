using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Player : MonoBehaviour {

    public float NoiseLevel = 1;
    public float VisibilityLevel { get; private set; }

    public void SetVisibilityLevel(bool isOn)
    {
        VisibilityLevel = (isOn) ? visibilityLevelOn : visibilityLevelOff;
    }

    public int NumberAttempts = 3;

    [SerializeField] private float visibilityLevelOn = 5;
    [SerializeField] private float visibilityLevelOff = 1;
    [SerializeField] private PlayableDirector playableDirectorFalling;

    private PlayerMovement playerMovement = null;
    private FlashlightController flashlightController = null;

    private bool isAlive = true;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        flashlightController = GetComponent<FlashlightController>();

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
        flashlightController.SetHasFlashlight(true);
        flashlightController.TurnOnFlashlight(true);
        Destroy(flashlight.gameObject);
    }
}
