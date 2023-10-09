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
    [Header("hit time")]
    [SerializeField] private float _hitTime = 1f;

    private Attack _attack;

    private WaitForSecondsRealtime _hitWaitTime;

    private bool _hit = false;

    private void Start()
    {
        _hitWaitTime = new WaitForSecondsRealtime(_hitTime);

        _weapon.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("trigger hit");
        _attack = collision.GetComponent<Attack>();
        if (_attack.finger < 0)
        {
            Hit(collision.transform);
        }
        else if (_weapon.gameObject.activeSelf)
        {
            Attack(collision.transform);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("drop hit");
        if (collision.gameObject.CompareTag("Drop"))
        {
            if (!_weapon.gameObject.activeSelf)
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
        _animationController.isBring = false;
        deactivate_weapon();
        //ÆÇÁ¤
    }

    private void Hit(Transform collisionPos)
    {
        if (_hit)
            return;
        _animationController.TurnHead(((collisionPos.position.x - gameObject.transform.position.x) > 0));
        deactivate_weapon();
        StartCoroutine(hitAction());
    }

    private IEnumerator hitAction()
    {
        _hit = true;
        _movement.hit = true;
        Debug.Log("hit start");

        _animationController.Hit();
        yield return _hitWaitTime;

        Debug.Log("hit end");
        _movement.hit = false;
        _hit = false;
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
