using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPlace : MonoBehaviour
{
    public Transform Place;

    [SerializeField] private GameObject road;
    [SerializeField] private GameObject blickPrefab;
    [SerializeField] private Transform blickPosition;

    [SerializeField] private AudioSource audioSiren;

    private Transform blickTransform = null;
    private Transform playerTransform = null;

    private bool isActivate = false;

    private void Start()
    {
        road.SetActive(false);
    }

    private void Update()
    {
        if (!isActivate) return;
        MoveBlickToPlayer();

    }
    public void Activate()
    {
        var blick = Instantiate(blickPrefab, blickPosition.position, Quaternion.identity);
        blick.SetActive(true);
        blickTransform = blick.transform;
        playerTransform = GameController.Instance.Player.transform;
        road.SetActive(true);
        audioSiren.Play();
        isActivate = true;
    }

    public void MoveBlickToPlayer()
    {
        var distance = Vector3.Distance(playerTransform.position, blickPosition.position);
        if (distance > 100)
        {
            var direction = blickPosition.position - playerTransform.position;
            blickTransform.position = playerTransform.position + direction.normalized * 100;
        }
        else blickTransform.position = blickPosition.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameController.Instance.HandleGameOver(true);
        }
    }
}
