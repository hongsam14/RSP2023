using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_GameBehaviour : MonoBehaviour
{
    [SerializeField] private Movement _movement;
    [SerializeField] private CharacterAnimationController _animationController;
    [SerializeField] private Rigidbody2D _body;
    [Header("Weapon")]
    [SerializeField] private Weapon _weapon;

    private Attack _attack;

    private bool _hit = false;
    private bool _attacking = false;

    private void Start()
    {
        _weapon.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _attack = collision.GetComponent<Attack>();
        if (_attack.finger < 0 || !_weapon.gameObject.activeSelf)
        {
            Hit(collision.transform.position);
        }
        else if (_attack.finger >= 0 && _weapon.gameObject.activeSelf)
        {
            Attack(collision.transform);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Drop"))
        {
            if (!_weapon.gameObject.activeSelf && !_attacking)
            {
                activate_weapon();
                _animationController.isBring = true; 
            }
            else
            {
                _weapon.fingers += 1;
            }
        }
    }

    private void Attack(Transform collisionPos)
    {
        _animationController.Attack(collisionPos.position);
        StartCoroutine(attackAction());
        //ÆÇÁ¤
    }

    private void Hit(Vector2 collisionPos)
    {
        if (_hit)
            return;
        _animationController.TurnHead(((collisionPos.x - gameObject.transform.position.x) > 0));
        deactivate_weapon();
        _movement.Hit(collisionPos);
        StartCoroutine(hitAction());
    }

    private IEnumerator hitAction()
    {
        _hit = true;

        _animationController.Hit();
        yield return new WaitUntil(() => !_movement.hit);

        _hit = false;
    }

    private IEnumerator attackAction()
    {
        _attacking = true;
        yield return new WaitUntil(() => !_animationController.isAttacking);
        _attacking = false;
        _animationController.isBring = false;
        deactivate_weapon();
    }

    private void deactivate_weapon()
    {
        if (_weapon.gameObject.activeSelf)
        {
            _weapon.fingers = 0;
            _weapon.gameObject.SetActive(false);
        }
    }

    private void activate_weapon()
    {
        if (!_weapon.gameObject.activeSelf)
        {
            _weapon.gameObject.SetActive(true);
            _weapon.fingers = 0;
        }
    }
}
