using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rockCollision : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField]
    private float _hitForce = 50f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.layer == LayerMask.GetMask("AR_World"))
        {
            //Direction between collider collided 
            var forceDirection2 = collision.collider.bounds.center - transform.position;
            AddImpact(collision.gameObject, forceDirection2, _hitForce);
        }

        if (collision.relativeVelocity.magnitude > 2)
            audioSource.Play();
    }

    public void AddImpact(GameObject ImpactedObject, Vector3 impactDirection, float force)
    {
        impactDirection.Normalize();
        float mass = ImpactedObject.GetComponent<Rigidbody>().mass;
        // reflect down force on the ground
        if (impactDirection.y < 0) impactDirection.y = -impactDirection.y;
        Vector3 impact = impactDirection.normalized * force / mass;
        StartCoroutine(ApplyImpactOnTime(impact, ImpactedObject));
    }

    IEnumerator ApplyImpactOnTime(Vector3 impact, GameObject impactObject)
    {
        Rigidbody impactedRigidbody = impactObject.GetComponent<Rigidbody>();
        while (impact.magnitude > 0.2)
        {
            //if (!impactPreview.emitting) impactPreview.emitting = true;
            impactedRigidbody.AddForce(impact * Time.deltaTime, ForceMode.Force);
            // consumes the impact energy each cycle:
            impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

}
