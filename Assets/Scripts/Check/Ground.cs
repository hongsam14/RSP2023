using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public bool onGround { get; private set; }
    public float friction { get; private set; }

    private PhysicsMaterial2D _physicalMat = null;
    private Vector2 _normal;

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
        friction = 0;
        onGround = false;
    }

    private void EvaluateCollision(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            _normal = collision.GetContact(i).normal;
            onGround |= (_normal.y >= 0.9f);
        }
    }

    private void RetrieveFriction(Collision2D collision)
    {
        _physicalMat = collision.rigidbody.sharedMaterial;
        friction = 0;
        if (_physicalMat != null)
        {
            friction = _physicalMat.friction;
        }
    }
}
