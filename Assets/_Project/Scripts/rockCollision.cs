using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCollision : MonoBehaviour
{
    private Rigidbody RockRgbd;
    private AudioSource AudioSource;
    [SerializeField]
    private float _hitForce = 50f;
    public LayerMask DestructiblePropsLayer; 

    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        RockRgbd = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Hit with " + collision.collider.name);
        //Debug.Log("Layer : " + collision.gameObject.layer + " , Search : " + DestructiblePropsLayer.)
        //if (collision.gameObject.layer.Equals(DestructiblePropsLayer))
        //{
        //    Debug.Log("Hit Layer is ARWorld");
            DestructibleProps props = collision.gameObject.GetComponent<DestructibleProps>();
            if (props)
            {
                Debug.Log("He is destructible");
                //Direction between collider collided 
                var forceDirection2 = collision.collider.bounds.center - transform.position;
                props.ApplyDmg(RockRgbd.mass, RockRgbd.velocity.magnitude);
                //props.ApplyDmg();
                props.AddImpact(forceDirection2, _hitForce);
            }
        //}

        if (collision.relativeVelocity.magnitude > 2)
            AudioSource.Play();
    }


}
