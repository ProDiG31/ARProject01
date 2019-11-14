using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPlaneManager : MonoBehaviour
{

    private Rigidbody[] childenRigid;

    private Rigidbody PlaneRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        PlaneRigidbody = GetComponent<Rigidbody>();
        childenRigid = GetComponentsInChildren<Rigidbody>();
        EnableKinematicRigidBody();
    }

    public void EnableKinematicRigidBody()
    {
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
    }
}
