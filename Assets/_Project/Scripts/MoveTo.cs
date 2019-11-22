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
    private bool _isBackToHome = false;
    private int _choosenCarrotIndex = 0;

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
        transform.LookAt(_agent.destination);

        if (IsDestinationReached())
        {
            if (!_isBackToHome)
            {
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

    private bool IsDestinationReached() =>
        _agent.pathStatus == NavMeshPathStatus.PathComplete
        && _agent.remainingDistance < 0.25f;
}
