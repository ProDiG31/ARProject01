using eDmitriyAssets.NavmeshLinksGenerator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelController : MonoBehaviour
{
    public GameObject NavMeshSurface;

    private NavMeshSurface _navMeshSurface;
    private NavMeshLinks_AutoPlacer _navMeshLinks;
    private float _elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        _elapsedTime = 0f;
        _navMeshSurface = NavMeshSurface.GetComponent<NavMeshSurface>();
        _navMeshLinks = NavMeshSurface.GetComponent<NavMeshLinks_AutoPlacer>();
        GenerateNavigation();
    }

    // Update is called once per frame
    void Update()
    {
        _elapsedTime += Time.deltaTime;
        if(_elapsedTime >= 1)
        {
            _elapsedTime = 0;
            GenerateNavigation();
        }
    }

    private void GenerateNavigation()
    {
        _navMeshSurface.BuildNavMesh();
        _navMeshLinks.ClearLinks();
        _navMeshLinks.Generate();
    }
}
