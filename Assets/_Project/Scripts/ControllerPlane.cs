using System;
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
    public Text Score;

    public GameObject LevelPrefab;
    public GameObject levelWrapper;
    public Canvas UI;
    public GameObject ParticleSystem;

    private static Text _Logger;
    private static Text _Score;


    public static int maxRewardPoint = 0;
    public static int actualScore = 0;
    public static GameObject ExplosionSystem;
    public static CanvasController IGCanvas;
    public static GameObject levelCreated;
    public static bool isLevelCreated = false;

    // Start is called before the first frame update
    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        _planeManager = GetComponent<ARPlaneManager>();
        IGCanvas = UI.GetComponent<CanvasController>();
        _Logger = Logger;
        _Score = Score;
        ExplosionSystem = ParticleSystem;
        IGCanvas.DisactiveGameUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1 && !isLevelCreated) {
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
        IGCanvas.initialLevelPosition = levelCreated.transform.position;
        IGCanvas.ActivateGameUI();
        IGCanvas.DisableAllStar();
    }

    void DisablePlanes()
    {
        foreach (ARPlane plane in _planeManager.trackables) plane.gameObject.SetActive(false);
        _planeManager.enabled = false;
    }

    public static void Log(string value)
    {
        Debug.Log(value);
        if (_Logger.text.Length > 200) _Logger.text = "";
        _Logger.text = _Logger.text + "\n" + value;
    }

    public static void AddRewardPoint(int rwd)
    {
        actualScore += rwd;
        _Score.text = (actualScore).ToString() + " / " + maxRewardPoint.ToString(); 

        if(maxRewardPoint != 0)
        {
            if(actualScore > (IGCanvas.ActivatedStar * .25) * maxRewardPoint)
            {
                IGCanvas.ActivateOneStar();
            }
        }
    }
}