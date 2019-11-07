using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class ControllerPlane : MonoBehaviour
{
    //INSTANTIATE AT START OR IN EDITOR
    ARRaycastManager raycastManager;

    //DEBBUG / TEST
    public Text TextDebugARRaycast;


    public GameObject LevelPrefab;
    public GameObject Catapult;
    public GameObject levelWrapper;

    private bool isLevelCreated = false; 

    // Start is called before the first frame update
    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        Catapult.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1)
        {
            List<ARRaycastHit> hitResults = new List<ARRaycastHit>();
            raycastManager.Raycast(Input.GetTouch(0).position,hitResults, TrackableType.Planes);

            if (hitResults.Count > 0)
            {
                foreach (ARRaycastHit hit in hitResults)
                {
                    Debug.Log(hit.trackableId);
                    TextDebugARRaycast.text = hit.pose.position.ToString();
                    if (!isLevelCreated) CreateLevel(hit.pose.position);
                }
            }
        }
    }

    private void CreateLevel(Vector3 positionSpawn)
    {
        Instantiate(LevelPrefab, positionSpawn, Quaternion.identity, levelWrapper.transform);
        Catapult.SetActive(true);
        isLevelCreated = true;
    }
}