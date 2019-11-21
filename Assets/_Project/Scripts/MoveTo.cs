using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{
    public GameObject[] Carrots;
    public Transform FinalDestination;

    private NavMeshAgent _agent;
    private Animator _animator;
    private bool finalStarted = false;
    private int _choosenCarrotIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.destination = Carrots[_choosenCarrotIndex].transform.position;
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(_agent.destination);

        if (IsDestinationReached())
        {
            if (!finalStarted)
            {
                Carrots[_choosenCarrotIndex].SetActive(false);
                _choosenCarrotIndex++;

                if (_choosenCarrotIndex == Carrots.Length)
                {
                    Debug.Log("Go to warren");
                    _agent.SetDestination(FinalDestination.position);
                    finalStarted = true;
                }
                else
                {
                    _agent.SetDestination(Carrots[_choosenCarrotIndex].transform.position);
                }
            }
            else
            {
                _agent.isStopped = true;
                _animator.SetBool("isLeaving", true);
            }
        }
    }

    private bool IsDestinationReached() =>
        _agent.pathStatus == NavMeshPathStatus.PathComplete
        && _agent.remainingDistance < 0.25f;
}
