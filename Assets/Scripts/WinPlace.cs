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
    [SerializeField] private List<AudioClip> sirenVoices = new List<AudioClip>();

    private Blink blink;

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
        var blick = Instantiate(blickPrefab, blickPosition.position, Quaternion.identity);
        blink = blick.GetComponent<Blink>();
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
        if (distance >= 600)
        {
            if (curTimeToBlick >= 15) coroutineBlick = StartCoroutine(ShowBlick(10));
        }
        else if (distance >= 300)
        {
            if (curTimeToBlick >= 20) coroutineBlick = StartCoroutine(ShowBlick(5));
        }
        else if (distance >= 100)
        {
            if (curTimeToBlick >= 30) coroutineBlick = StartCoroutine(ShowBlick(3));
        }
        curTimeToBlick += Time.deltaTime;
    }

    private bool playVoice = false;
    private IEnumerator ShowBlick(float timeShowing)
    {
        Debug.Log("Сирена");

        if (!playVoice)
        {
            GameController.Instance.Player.PlayVoice(sirenVoices[Random.Range(0, sirenVoices.Count)]);
            playVoice = true;
        }

        audioSiren.Play();
        blink.ChangeLightIntensity(200);
        isShowindBlink = true;
        yield return blink.StartBlink(timeShowing);
        isShowindBlink = false;
        blink.ChangeLightIntensity(0);
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
