using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shambler : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform target;
    // Start is called before the first frame update
    void Start() {
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update() {
        agent.SetDestination(target.position);
        var pos = transform.position;
        float distance = Vector3.Distance(pos, pos);
    }
}
