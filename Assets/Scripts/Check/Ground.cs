using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public bool onGround { get; private set; }
    public float friction { get; private set; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
    }

    private void EvaluateCollision(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector2 normal = collision.GetContact(i).normal;
            onGround |= (normal.y >= 0.9f);
        }
    }

    private void RetrieveFriction(Collision2D collision)
    {
        PhysicsMaterial2D mat = collision.rigidbody.sharedMaterial;
        friction = 0;
        if (mat != null)
        {
            friction = mat.friction;
        }
    }
}
