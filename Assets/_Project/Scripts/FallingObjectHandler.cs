using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObjectHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        DestructibleProps props = other.gameObject.GetComponent<DestructibleProps>();
        if (props)
        {
            props.Die();
        }
    }
}

