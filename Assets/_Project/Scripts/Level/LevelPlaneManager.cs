using eDmitriyAssets.NavmeshLinksGenerator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelPlaneManager : MonoBehaviour
{
    public bool IsKinematic { get; private set; }
    
    private Rigidbody[] childenRigid;
    private Rigidbody PlaneRigidbody;
    
    private NavMeshSurface[] _navMeshSurfaces;
    private NavMeshLinks_AutoPlacer _navMeshLinks;

    private float _elapsedTime;
        
    // Start is called before the first frame update
    void Start()
    {
        _elapsedTime = 0f;
        _navMeshSurfaces = GetComponentsInChildren<NavMeshSurface>();
        _navMeshLinks = GetComponentInChildren<NavMeshLinks_AutoPlacer>();
        _navMeshSurfaces[1].BuildNavMesh();
        GenerateNavigation();

        PlaneRigidbody = GetComponent<Rigidbody>();
        childenRigid = GetComponentsInChildren<Rigidbody>();
        EnableKinematicRigidBody();
    }

    private void Update()
    {
        if(!IsKinematic)
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= 1f)
            {
                _elapsedTime = 0;
                GenerateNavigation();
            }
        }
    }

    public void EnableKinematicRigidBody()
    {
        IsKinematic = true;
        foreach(Rigidbody rgd in childenRigid)
        {
            if(rgd != PlaneRigidbody) rgd.isKinematic = true;
        }
    }

    public void DisableKinematicRigidBody()
    {
        foreach (Rigidbody rgd in childenRigid)
        {
            if (rgd != PlaneRigidbody) rgd.isKinematic = false;
        }

        IsKinematic = false;
    }

    private void GenerateNavigation()
    {
        foreach( var surface in _navMeshSurfaces)
        {
            if(!surface.CompareTag("LevelGround"))
            {
                surface.BuildNavMesh();
                Debug.Log("Surface baked");
            }
        }
        _navMeshLinks.Generate();
    }
}
