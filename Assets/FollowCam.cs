using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [Range(0,20)]
    [SerializeField]
    private float dist = 1;
    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0.5f, 0.5f)) + (Camera.main.transform.forward * dist);
    }
}
