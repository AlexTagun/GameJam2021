using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MurdererAI : MonoBehaviour {
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float ScanFrequency;
    [SerializeField] private float ScanRadius;
    [SerializeField] private float AttackRadius;
    [SerializeField] private float DistanceToFlee;
    [SerializeField] private float DistanceToSpot;
    [SerializeField] private float FollowingSpeed;
    [SerializeField] private float RoamingRadius = 40f;
    [SerializeField] private float RoamingSpeed = 3.5f;
    [SerializeField] private float TpRadius = 65f;
    [SerializeField] private float ShowTime = 0.5f;
    [SerializeField] private Player player;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Sprite[] sprites;
    // [SerializeField] private AudioClip PlayerFleeVoice;
    [SerializeField] private AudioClip[] PlayerVoices;

    private NavMeshAgent navMesh;

    private float curTime = 0f;

    private enum MurdererStates {
        Roaming,
        Following,
        ReadyToAttack,
        Attack,
        Spotted,
        Flee
    }

    private MurdererStates _curState;
    private Coroutine _curCoroutine;
    private ColorAdjustments _colorAdjustments;

    private void Start() {
        navMesh = GetComponent<NavMeshAgent>();
        _camera = Camera.main;
        _curState = MurdererStates.Roaming;

        var volume = GameObject.Find("Global Volume").GetComponent<Volume>();
        volume.profile.TryGet(out _colorAdjustments);
        StartCoroutine(ScanCoroutine());
        renderer.color = new Color(0,0,0,0.01f);
    }

    private void Update() {
        IsSeenByPlayer();
        renderer.transform.LookAt(playerTransform);
        renderer.transform.eulerAngles = new Vector3(0, renderer.transform.eulerAngles.y, 0);

        switch (_curState) {
            case MurdererStates.Roaming:
                navMeshAgent.speed = RoamingSpeed;
                break;
            case MurdererStates.Following:
                navMeshAgent.speed = FollowingSpeed;
                navMesh.SetDestination(playerTransform.position);
                if (Vector3.Distance(transform.position, player.transform.position) < AttackRadius) {
                    GameController.Instance.Player.Died();
                    Debug.Log("Kill");
                }

                break;
            case MurdererStates.Flee:
                // if (_curCoroutine == null) _curCoroutine = StartCoroutine(RunFromCor());
                _curState = MurdererStates.Roaming;
                break;
            case MurdererStates.Spotted:
                _curState = MurdererStates.Roaming;
                break;
        }
    }

    private IEnumerator ScanCoroutine() {
        while (true) {
            yield return new WaitForSeconds(ScanFrequency);
            if ((ScanRadius + player.NoiseLevel * player.VisibilityLevel) >
                Vector3.Distance(transform.position, player.transform.position)) {
                Debug.Log("Player has found");
                if (_curState == MurdererStates.Roaming) _curState = MurdererStates.Following;
            } else {
                Debug.Log("Player not found");
                Vector3 randomVector = Random.onUnitSphere;
                randomVector.y = 0;

                randomVector = randomVector.normalized * RoamingRadius;
                navMesh.SetDestination(player.transform.position + randomVector);
                _curState = MurdererStates.Roaming;
            }

            _curCoroutine = null;
        }
    }

    private bool _runCoroutine;

    private IEnumerator RunFrom(bool showFlash) {
        _runCoroutine = true;
        // Vector3 runTo = (transform.position - player.transform.position).normalized;
        //
        // var angle = Random.Range(-45, 45);
        // runTo.x = Mathf.Acos(Mathf.Deg2Rad * angle);
        // runTo.z = Mathf.Asin(Mathf.Deg2Rad * angle);
        // runTo *= 10;
        // Debug.Log(runTo);
        // navMesh.SetDestination(transform.position + runTo);
        
        renderer.color = Color.white;
        yield return new WaitForSeconds(ShowTime);
        renderer.color = new Color(0,0,0,0.01f);
        if (showFlash) {
            LeanTween.value(gameObject, 0f, -100f, 0.01f)
                .setOnUpdate(f => { _colorAdjustments.contrast.value = f; }).setOnComplete(() => {
                    LeanTween.value(gameObject, -100f, 0, 0.8f)
                        .setOnUpdate(f => { _colorAdjustments.contrast.value = f; });
                });
        }


        Vector3 randomVector = Random.onUnitSphere;
        randomVector.y = 0;

        randomVector = randomVector.normalized * TpRadius;
        transform.position = transform.position + randomVector;
        renderer.sprite = sprites[Random.Range(0, sprites.Length)];
        _runCoroutine = false;
    }

    [SerializeField] private SpriteRenderer renderer;
    private Camera _camera;

    private bool IsSeenByPlayer() {
        if (_runCoroutine) return false;
        if (!renderer.isVisible) return false;
        if (!player.flashlightController.IsBurn) return false;
        Vector2 pos = _camera.WorldToViewportPoint(renderer.transform.position);
        var camDist = Vector2.Distance(pos, new Vector2(0.5f, 0.5f));

        if (camDist > 0.5f) return false;
        var dist = Vector3.Distance(transform.position, player.transform.position);
        if (DistanceToFlee > dist) {
            _curState = MurdererStates.Flee;
            player.PlayVoice(PlayerVoices[Random.Range(0, PlayerVoices.Length)]);
            StartCoroutine(RunFrom(true));
            return true;
        }

        if (DistanceToSpot > dist) {
            _curState = MurdererStates.Spotted;
            player.PlayVoice(PlayerVoices[Random.Range(0, PlayerVoices.Length)]);
            StartCoroutine(RunFrom(false));
            return true;
        }

        return false;
    }
}