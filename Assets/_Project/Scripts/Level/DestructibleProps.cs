using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleProps : MonoBehaviour
{

    public int LifePointMax;
    public int Reward;
    public GameObject explosionPrefab;

    public int _lifePoint;
    private Rigidbody _propsRgbd;

    // Start is called before the first frame update
    void Start()
    {
        _lifePoint = LifePointMax;
        _propsRgbd = GetComponent<Rigidbody>();
    }

    public virtual void Die()
    {
        ControllerPlane.AddRewardPoint(Reward);
        GameObject Explosion = Instantiate(explosionPrefab, transform.position, transform.rotation, transform);
        Explosion.transform.localPosition = Vector3.zero;
        Explosion.GetComponent<ParticleSystem>().Emit(1);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        Destroy(gameObject, 2f);
    }

    //Rigidbody impactedRigidbody
    public void ApplyPhysicsDmg(float mass, float velocity)
    {
        //e =  1/2 mv²
        float energy = .5f * (mass * (velocity * velocity));
        Debug.Log("Energy received = " + energy , this);
        ApplyDmg((int)energy);
    }

    public void ApplyDmg(int Dmg)
    {
        _lifePoint = _lifePoint - Mathf.Abs(Dmg);
        if (_lifePoint < 0) Die();
    }

    public void AddImpact(Vector3 impactDirection, float ImpactMass)
    {
        impactDirection.Normalize();
        float mass = _propsRgbd.mass;
        // reflect down force on the ground
        if (impactDirection.y < 0) impactDirection.y = -impactDirection.y;
        Vector3 impact = impactDirection.normalized * ImpactMass / mass;
        StartCoroutine(ApplyImpactOnTime(impact));
    }

    IEnumerator ApplyImpactOnTime(Vector3 impact)
    {
        while (impact.magnitude > 0.2)
        {
            //if (!impactPreview.emitting) impactPreview.emitting = true;
            _propsRgbd.AddForce(impact * Time.deltaTime, ForceMode.Force);
            // consumes the impact energy each cycle:
            impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
