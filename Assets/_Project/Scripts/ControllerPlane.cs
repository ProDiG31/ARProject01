﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class ControllerPlane : MonoBehaviour
{
    //INSTANTIATE AT START OR IN EDITOR
    private ARRaycastManager raycastManager;
    private ARPlaneManager _planeManager;

    //DEBBUG / TEST
    public Text Logger;
    private static Text _Logger;

    public GameObject LevelPrefab;
    public GameObject levelWrapper;
    public Canvas UI;

    public static Canvas IGCanvas;
    public static GameObject levelCreated;
    public GameObject Catapult;

    public static bool isLevelCreated = false;

    // Start is called before the first frame update
    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        _planeManager = GetComponent<ARPlaneManager>();
        IGCanvas = UI;
        _Logger = Logger;
        Catapult.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1 && !isLevelCreated)
        {
            List<ARRaycastHit> hitResults = new List<ARRaycastHit>();
            
            raycastManager.Raycast(Input.GetTouch(0).position, hitResults, TrackableType.Planes);
            
            foreach (ARRaycastHit hit in hitResults)
            {
                if (!isLevelCreated)
                {
                    CreateLevel(hit.pose.position);
                    DisablePlanes();
                    break;
                }
            }
        }
    }

    private void CreateLevel(Vector3 positionSpawn)
    {
        isLevelCreated = true;
        Log(positionSpawn.ToString());
        levelCreated = Instantiate(LevelPrefab, positionSpawn, Quaternion.identity, levelWrapper.transform);
        Catapult.SetActive(true);
    }

    void DisablePlanes()
    {
        foreach (ARPlane plane in _planeManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
        _planeManager.enabled = false;
    }

    public static void Log(string value)
    {
        Debug.Log(value);
        if (_Logger.text.Length > 200) _Logger.text = "";
        _Logger.text = _Logger.text + "\n" + value;
    }
}