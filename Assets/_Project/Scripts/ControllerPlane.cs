using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using System;

public class ControllerPlane : MonoBehaviour
{
    //INSTANTIATE AT START OR IN EDITOR
    private ARRaycastManager raycastManager;
    private ARPlaneManager _planeManager;

    //DEBBUG / TEST
    public Text Logger;


    public GameObject LevelPrefab;
    public GameObject Catapult;
    public GameObject levelWrapper;
    public GameObject JokerNullPrefab;

    private bool isLevelCreated = false;
    private float _timeElapsed = 0;


    // Start is called before the first frame update
    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        _planeManager = GetComponent<ARPlaneManager>();
        Catapult.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //ResetLog();
        if (Input.touchCount == 1)
        {
            List<ARRaycastHit> hitResults = new List<ARRaycastHit>();
            raycastManager.Raycast(Input.GetTouch(0).position,hitResults, TrackableType.Planes);

            if (hitResults.Count > 0)
            {
                foreach (ARRaycastHit hit in hitResults)
                {
                    if (
                        !isLevelCreated
                        //&& hit.hitType == TrackableType.Planes
                    )
                    {
                        Log(hit.pose.position.ToString());
                        CreateLevel(hit.pose.position);
                        DisablePlanes();
                        break;
                    }
                }
            }
        }
    }

    private void ResetLog()
    {
        _timeElapsed += Time.deltaTime;
        if(_timeElapsed > 20)
        {
            _timeElapsed = 0;
            Logger.text = string.Empty;
        }
    }

    private void Log(string value)
    {
        Debug.Log(value);
        Logger.text = Logger.text + "\n" + value;
    }

    private void CreateLevel(Vector3 positionSpawn)
    {
        Instantiate(LevelPrefab, positionSpawn, Quaternion.identity, levelWrapper.transform);
        Catapult.SetActive(true);
        isLevelCreated = true;
        _planeManager.enabled = false;
        Log("Plane Manager disabled");
    }

    void DisablePlanes()
    {

        foreach (ARPlane plane in FindObjectsOfType(Type.GetType("ARPlane")))
        {
            //plane.gameObject.SetActive(false);
            plane.enabled = false;
        }
        Log("Planes should not be visibles");
        _planeManager.planePrefab = JokerNullPrefab;
        Log("JokerNullPrefab");

        //_planeManager.enabled = false; 
    }
}