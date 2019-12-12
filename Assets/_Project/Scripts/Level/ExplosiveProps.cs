using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExplosiveProps : DestructibleProps
{
    private float _ExplosionRadius = .5f;
    private float _ExplosionForce = 10f; 
    private static float _ExplosionDmg = 150f;
    public ParticleSystem explosionParticleSytem;

    public override void Die()
    {
        base.Die();
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = transform.position;
        SphereCollider collider = sphere.AddComponent<SphereCollider>();
        StartCoroutine(ExplodeCoroutine(sphere, 1f));
        explosionParticleSytem.Emit(1);
    }

    private IEnumerator ExplodeCoroutine(GameObject sphere, float time)
    {
        Vector3 fromScale = Vector3.zero;
        Vector3 toScale = new Vector3(_ExplosionRadius, _ExplosionRadius, _ExplosionRadius);
        Material SphereMaterial = sphere.GetComponent<MeshRenderer>().material; 
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            float progress = (elapsedTime / time);
            sphere.transform.localScale = Vector3.Lerp(fromScale, toScale, progress);
            SphereMaterial.color = Color.Lerp(Color.red, Color.black, progress);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        elapsedTime = 0;
        while (elapsedTime < (time / 2))
        {
            float progress = (elapsedTime / (time / 2));
            sphere.transform.localScale = Vector3.Lerp(toScale, fromScale,  progress);
            SphereMaterial.color = Color.Lerp(Color.black, Color.clear, progress);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Destroy(sphere);
    }
}

