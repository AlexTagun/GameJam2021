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
    [SerializeField] private float FollowingSpeed;
    [SerializeField] private float FleeSpeed;
    [SerializeField] private Player player;
    [SerializeField] private NavMeshAgent navMeshAgent;

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
    }

    private void Update() {
        if (IsSeenByPlayer()) _curState = MurdererStates.Flee;

        switch (_curState) {
            case MurdererStates.Roaming:
                if (_curCoroutine == null) _curCoroutine = StartCoroutine(ScanCoroutine());
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
                navMeshAgent.speed = FleeSpeed;
                if (_curCoroutine == null) _curCoroutine = StartCoroutine(RunFromCor());
                break;
        }
    }

    private IEnumerator ScanCoroutine() {
        yield return new WaitForSeconds(ScanFrequency);
        if ((ScanRadius + player.NoiseLevel * player.VisibilityLevel) >
            Vector3.Distance(transform.position, player.transform.position)) {
            Debug.Log("Player has found");
            _curState = MurdererStates.Following;
        } else {
            Debug.Log("Player not found");
        }

        _curCoroutine = null;
    }

    private IEnumerator RunFromCor() {
        while (IsSeenByPlayer()) {
            
            LeanTween.value(gameObject, 0f, -100f, 0.01f)
                .setOnUpdate(f => {
                    _colorAdjustments.contrast.value = f;
                }).setOnComplete(() => {
                    LeanTween.value(gameObject, -100f, 0, 0.8f)
                        .setOnUpdate(f => {
                            _colorAdjustments.contrast.value = f;
                        });
                });
            RunFrom();
            yield return new WaitForSeconds(1);
        }

        _curState = MurdererStates.Following;
        _curCoroutine = null;
    }

    private void RunFrom() {
        // Vector3 runTo = (transform.position - player.transform.position).normalized;
        //
        // var angle = Random.Range(-45, 45);
        // runTo.x = Mathf.Acos(Mathf.Deg2Rad * angle);
        // runTo.z = Mathf.Asin(Mathf.Deg2Rad * angle);
        // runTo *= 10;
        // Debug.Log(runTo);
        // navMesh.SetDestination(transform.position + runTo);
        
        Vector3 randomVector = Random.onUnitSphere;
        randomVector.y = 0;

        randomVector = randomVector.normalized * 40;
        Debug.Log(randomVector);
        transform.position = transform.position + randomVector;

    }

    [SerializeField] private Renderer renderer;
    private Camera _camera;

    private bool IsSeenByPlayer() {
        if (!renderer.isVisible) return false;
        if (DistanceToFlee < Vector3.Distance(transform.position, player.transform.position)) return false;
        Vector2 pos = _camera.WorldToViewportPoint(transform.position);
        if (Vector2.Distance(pos, new Vector2(0.5f, 0.5f)) < 0.5f) {
            return true;
        }

        return false;
    }
}