using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_GameBehaviour : MonoBehaviour
{
    [SerializeField] private CharacterAnimationController _animationController;
    [SerializeField] private Rigidbody2D _body;
    [Header("Weapon")]
    [SerializeField] private Weapon _weapon;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Attack(collision.transform);
    }

    private void Attack(Transform collisionPos)
    {
        //_animationController.Attack(collisionPos.position);
        _animationController.isBring = !_animationController.isBring;
    }
}
