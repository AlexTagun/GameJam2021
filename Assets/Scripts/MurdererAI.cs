using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MurdererAI : MonoBehaviour
{
    [SerializeField] private float timeToChangeGoalPosition = 3f;
    [SerializeField] private Transform playerTransform;

    private NavMeshAgent navMesh;

    private float curTime = 0f;

    private void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {

        if (curTime >= timeToChangeGoalPosition)
        {
            curTime = 0f;
            navMesh.SetDestination(playerTransform.position);
        }
        else curTime += Time.deltaTime;
    }
}
