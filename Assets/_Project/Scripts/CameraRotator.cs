using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    public GameObject Camera;
    public float Speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Camera.transform.Rotate(0, Speed * Time.deltaTime, 0);
    }
}
