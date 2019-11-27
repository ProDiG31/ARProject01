using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{
    public GameObject Carrot;
    public Transform FinalDestination;

    private NavMeshAgent _agent;
    private Animator _animator;
    private bool _isBackToHome = false;
    private bool _isAgentBlocked = false;

    // Don't set this too high, or NavMesh.SamplePosition() may slow down
    private float _onMeshThreshold = 2;

    private LineRenderer _lineGizmo;
    private bool _isAnimated = false;

    //Use for Debug
    void OnDrawGizmos()
    {
        //Init _lineGizmo Debug
        _lineGizmo = GetComponent<LineRenderer>();
        if (_lineGizmo == null)
        {
            _lineGizmo = gameObject.AddComponent<LineRenderer>();
            _lineGizmo.material = new Material(Shader.Find("Sprites/Default")) { color = Color.green };
            _lineGizmo.startWidth = 0.1f;
            _lineGizmo.endWidth = 0.1f;

            _lineGizmo.startColor = Color.green;
            _lineGizmo.endColor = Color.yellow;
        }

        if (_agent == null || _agent.path == null)
            return;

        var path = _agent.path;

        _lineGizmo.positionCount = path.corners.Length;
        for (int i = 0; i < path.corners.Length; i++)
        {
            _lineGizmo.SetPosition(i, path.corners[i]);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.SetDestination(Carrot.transform.position);
        _animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        AdjustPosition();
        ManageAgent();
    }

    private void ManageAgent()
    {
        if (!_isAnimated && _agent.hasPath)
        {
            RefreshGoal();

            if (_agent.remainingDistance < _agent.stoppingDistance)
            {
                if (!_isBackToHome)
                {
                    Carrot.SetActive(false);
                    _agent.SetDestination(FinalDestination.position);
                    _isBackToHome = true;
                }
                else
                {
                    SetNavMeshAgentActive(false);
                    _isAnimated = true;
                    _animator.SetBool("isLeaving", true);
                    
                }
            }
        }
    }

    private IEnumerator WaitAnimation()
    {
        yield return new WaitForSeconds(2f);
    }


    private void AdjustPosition()
    {
        var isOnNavMesh = IsAgentOnNavMesh();
        var isGrounded = IsGrounded(transform.position);

        if (
            ( !_agent.hasPath 
            && !isGrounded )
            && !_isAgentBlocked
        )
        {
            _isAgentBlocked = true;
            SetNavMeshAgentActive(false);
        } 
        else if (_isAgentBlocked && isGrounded && isOnNavMesh && !_isAnimated)
        {
            _isAgentBlocked = false;
            SetNavMeshAgentActive(true);
        }
    }

    private void RefreshGoal()
    {
        if (Carrot.activeSelf)
        {
            _agent.SetDestination(Carrot.transform.position);
        }
        else
        {
            _agent.SetDestination(FinalDestination.position);
        }
        transform.LookAt(_agent.destination);
    }

    private void SetNavMeshAgentActive(bool isActive)
    {
        _agent.updatePosition = isActive;
        _agent.updateRotation = isActive;
        _agent.enabled = isActive;

    }

    public bool IsAgentOnNavMesh()
    {
        Vector3 agentPosition = _agent.gameObject.transform.position;
        NavMeshHit hit;

        // Check for nearest point on navmesh to agent, within onMeshThreshold
        if (NavMesh.SamplePosition(agentPosition, out hit, _onMeshThreshold, NavMesh.AllAreas))
        {
            // Check if the positions are vertically aligned
            if (
                Mathf.Approximately(agentPosition.x, hit.position.x)
                && Mathf.Approximately(agentPosition.z, hit.position.z)
            )
            {
                // Lastly, check if object is below navmesh
                var hitPosition = hit.position.y;
                return agentPosition.y < hitPosition + 0.2f;// && agentPosition.y > hitPosition - 0.05f;
            }
        }

        return false;
    }

    private bool IsGrounded(Vector3 position)
    {
        return Physics.Raycast(position, Vector3.down, 0.2f);
    }
}
