using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProps : DestructibleProps
{
    public float ExplosionRadius = .05f;
    public float ExplosionForce = 10f; 
    public float ExplosionDmg = 150f;

    public override void Die()
    {
        base.Die();
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        sphere.transform.localScale = new Vector3(ExplosionRadius, ExplosionRadius, ExplosionRadius);

        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius);

        //foreach (Collider c in colliders)
        //{
        //    if (TryGetComponent(out DestructibleProps _destructibleProps))
        //    {
        //        float Distance = Vector3.Distance(transform.position, _destructibleProps.transform.position);
        //        float DistRatio = ExplosionRadius / Distance;
        //        _destructibleProps.ApplyDmg((int)(ExplosionDmg * DistRatio));
        //        if(_destructibleProps._lifePoint > 0)
        //        {
        //            Vector3 direction = (transform.position - _destructibleProps.transform.position).normalized;
        //            _destructibleProps.AddImpact(direction, ExplosionForce * DistRatio);
        //        }
        //    }
        //}
    }
}
