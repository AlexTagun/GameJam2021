using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPlace : MonoBehaviour
{
    public Transform Place;

    [SerializeField] private GameObject road;
    [SerializeField] private GameObject invisibleWall;
    [SerializeField] private GameObject blickPrefab;
    [SerializeField] private Transform blickPosition;

    [SerializeField] private AudioSource audioSiren;

    private GameObject blick;

    private Transform blickTransform = null;
    private Transform playerTransform = null;

    private bool isActivate = false;
    private bool isShowindBlink = false;

    private float curTimeToBlick = 10000f;

    private Coroutine coroutineBlick = null;

    private void Start()
    {
        road.SetActive(false);
    }

    private void Update()
    {
        if (!isActivate) return;
        UpdateBlick();
        if (isShowindBlink) MoveBlickToPlayer();

    }
    public void Activate()
    {
        blick = Instantiate(blickPrefab, blickPosition.position, Quaternion.identity);
       
        blickTransform = blick.transform;
        playerTransform = GameController.Instance.Player.transform;
        road.SetActive(true);
        invisibleWall.SetActive(false);
        curTimeToBlick = (RememberFlashlight.Instance.NeedActivateRoad) ? 0 : 1000;

        isActivate = true;
    }

    private void UpdateBlick()
    {
        if (coroutineBlick != null) return;
        var distance = Vector3.Distance(playerTransform.position, blickPosition.position);
        if (distance >= 800)
        {
            if (curTimeToBlick >= 45) coroutineBlick = StartCoroutine(ShowBlick(10));
        }
        else if (distance >= 600)
        {
            if (curTimeToBlick >= 60) coroutineBlick = StartCoroutine(ShowBlick(5));
        }
        else if (distance >= 300)
        {
            if (curTimeToBlick >= 120) coroutineBlick = StartCoroutine(ShowBlick(3));
        }
        curTimeToBlick += Time.deltaTime;
    }

    private IEnumerator ShowBlick(float timeShowing)
    {
        Debug.Log("Сирена");
        audioSiren.Play();
        blick.SetActive(true);
        isShowindBlink = true;
        yield return new WaitForSeconds(timeShowing);
        isShowindBlink = false;
        blick.SetActive(false);
        audioSiren.Stop();
        curTimeToBlick = 0f;
        coroutineBlick = null;
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
