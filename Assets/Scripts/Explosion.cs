using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    Collider2D[] _inExplosionRadius = null;
    [SerializeField] private float _explosionForceMulti = 5f;
    [SerializeField] private float _explosionRadius = 5f;

    public void Explode()
    {
        _inExplosionRadius = Physics2D.OverlapCircleAll(transform.position, _explosionRadius);
        foreach (Collider2D obj in _inExplosionRadius)
        {
            Rigidbody2D obj_rigidbody2D = obj.GetComponent<Rigidbody2D>();
            if (obj_rigidbody2D != null)
            {
                Vector2 distanceVector = obj.transform.position - transform.position;
                if (distanceVector.magnitude > 0)
                {
                    obj_rigidbody2D.AddForce(distanceVector.normalized * _explosionForceMulti / distanceVector);
                }
            }
        }
    }
}
