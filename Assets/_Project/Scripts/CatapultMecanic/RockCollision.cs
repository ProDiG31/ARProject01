using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCollision : MonoBehaviour
{
    public LayerMask DestructiblePropsLayer;

    private Rigidbody _RockRgbd;
    private AudioSource _AudioSource;
    private ParticleSystem _SmokeSystem;

    void Start()
    {
        _AudioSource = GetComponent<AudioSource>();
        _RockRgbd = GetComponent<Rigidbody>();
        _SmokeSystem = GetComponent<ParticleSystem>();
        Destroy(gameObject, 5f);
    }

    void OnCollisionEnter(Collision collision)
    {
        DestructibleProps props = collision.gameObject.GetComponent<DestructibleProps>();
        if (props)
        {
            Debug.Log("He is destructible");
            //Direction between collider collided 
            var forceDirection2 = collision.collider.bounds.center - transform.position;
            props.ApplyPhysicsDmg(_RockRgbd.mass, _RockRgbd.velocity.magnitude);
            props.AddImpact(forceDirection2, _RockRgbd.mass);
        }

        if (collision.relativeVelocity.magnitude > 2)
        {
            _AudioSource.Play();
            _SmokeSystem.Emit(1);
        }
    }
}
