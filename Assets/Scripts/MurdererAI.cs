using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MurdererAI : MonoBehaviour
{
    [SerializeField] private float timeToChangeGoalPosition = 3f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float ScanFrequency;
    [SerializeField] private float ScanRadius;
    [SerializeField] private float AttackRadius;
    [SerializeField] private Player player;

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

    private void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();

        _curState = MurdererStates.Roaming;
    }

    private void Update()
    {

        // if (curTime >= timeToChangeGoalPosition)
        // {
        //     curTime = 0f;
        //     navMesh.SetDestination(playerTransform.position);
        // }
        // else curTime += Time.deltaTime;

        switch (_curState) {
            case MurdererStates.Roaming:
                if(_curCoroutine == null) _curCoroutine = StartCoroutine(ScanCoroutine());
                // StartCoroutine(ScanCoroutine());
                break;
            case MurdererStates.Following:
                navMesh.SetDestination(playerTransform.position);
                if (Vector3.Distance(transform.position, player.transform.position) < AttackRadius) {
                    Debug.Log("Kill");
                }
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
}
