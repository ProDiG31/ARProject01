﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObjectHandler : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        DestructibleProps props = other.gameObject.GetComponent<DestructibleProps>();
        if (props)
        {
            props.Die();
        }
    }
}

