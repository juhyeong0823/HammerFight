using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject target;

    bool canSeePlayer = false;
    public float sight;

    public LayerMask traceLayer;
    void Start()
    {
        target = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();        
    }

    void Update()
    {

        if (Physics.CheckSphere(this.transform.position, sight, traceLayer))
        {
            agent.SetDestination(target.transform.position);
        }
        else
        {
            agent.SetDestination(this.transform.position);
        }
        //else agent.SetDestination() ���� ���⼭ �׳� ���� �������� ��Ʈ�Ѹ� ���Ѿ���
    }
}
