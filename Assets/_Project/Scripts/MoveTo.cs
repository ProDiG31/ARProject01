﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{
    public GameObject[] Carrots;
    public Transform FinalDestination;

    private NavMeshAgent _agent;
    private Animator _animator;
    private bool _isBackToHome = false;
    private bool _isAgentBlocked = false;
    private int _choosenCarrotIndex = 0;
    // Don't set this too high, or NavMesh.SamplePosition() may slow down
    private float _onMeshThreshold = 3;

    //private LineRenderer _lineGizmo;
    // Use for Debug
    //void OnDrawGizmos()
    //{
    //    //Init _lineGizmo Debug
    //    _lineGizmo = GetComponent<LineRenderer>();
    //    if (_lineGizmo == null)
    //    {
    //        _lineGizmo = gameObject.AddComponent<LineRenderer>();
    //        _lineGizmo.material = new Material(Shader.Find("Sprites/Default")) { color = Color.green };
    //        _lineGizmo.startWidth = 0.1f;
    //        _lineGizmo.endWidth = 0.1f;

    //        _lineGizmo.startColor = Color.green;
    //        _lineGizmo.endColor = Color.yellow;
    //    }

    //    if (_agent == null || _agent.path == null)
    //        return;

    //    var path = _agent.path;

    //    _lineGizmo.positionCount = path.corners.Length;
    //    for (int i = 0; i < path.corners.Length; i++)
    //    {
    //        _lineGizmo.SetPosition(i, path.corners[i]);
    //    }
    //}

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
        if (!_isAgentBlocked)
        {
            ManageAgent();
        }

        UnblockAgent();
    }

    private void ManageAgent()
    {
        transform.position = _agent.nextPosition;
        transform.LookAt(_agent.destination);

        if (_agent.remainingDistance < _agent.stoppingDistance)
        {
            if (!_isBackToHome)
            {
                Debug.Log("Path status : " + _agent.pathStatus);
                Debug.Log("Path calculated ? : " + _agent.pathPending);
                Debug.Log("A carrot wil disappear !");
                Carrots[_choosenCarrotIndex].SetActive(false);
                _choosenCarrotIndex++;

                if (_choosenCarrotIndex == Carrots.Length)
                {
                    _agent.SetDestination(FinalDestination.position);
                    _isBackToHome = true;
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

    private void UnblockAgent()
    {
        if (
            !_isAgentBlocked
            && _agent.pathStatus == NavMeshPathStatus.PathInvalid
        )
        {
            _isAgentBlocked = true;
            _agent.enabled = false;
        }
        else if (_isAgentBlocked && IsAgentOnNavMesh(_agent.gameObject))
        {
            _isAgentBlocked = false;
            //transform.rotation = Quaternion.Euler(Vector3.zero);
            _agent.nextPosition = transform.position;
            _agent.enabled = true;
        }
    }

    public bool IsAgentOnNavMesh(GameObject agentObject)
    {
        Vector3 agentPosition = agentObject.transform.position;
        NavMeshHit hit;

        // Check for nearest point on navmesh to agent, within onMeshThreshold
        if (NavMesh.SamplePosition(agentPosition, out hit, _onMeshThreshold, NavMesh.AllAreas))
        {
            // Check if the positions are vertically aligned
            if (Mathf.Approximately(agentPosition.x, hit.position.x)
                && Mathf.Approximately(agentPosition.z, hit.position.z))
            {
                // Lastly, check if object is below navmesh
                var hitPosition = hit.position.y; 
                return agentPosition.y < hitPosition + 0.01f && agentPosition.y >= hitPosition;
            }
        }

        return false;
    }
}
