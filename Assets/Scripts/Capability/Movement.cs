using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public enum AirStatus
{
    UP,
    DOWN,
    LAND
}

public enum MoveStatus
{
    MOVE,
    STAND
}

[RequireComponent(typeof(Rigidbody2D), typeof(Ground))]
public class Movement : MonoBehaviour
{
    public MoveStatus moveStatus
    {
        get => _moveStatus;
        set
        {
            if (value == _moveStatus)
                return;
            //anime control
            switch (value)
            {
                case MoveStatus.MOVE:
                    if (!hit)
                        _animationController.Move();
                    break;
                case MoveStatus.STAND:
                    if (!hit)
                        _animationController.Stand();
                    break;
            }
            _moveStatus = value;
        }
    }

    public AirStatus airStatus
    {
        get => _airStatus;
        set
        {
            if (value == _airStatus)
                return;
            //anime control
            switch (value)
            {
                case AirStatus.UP:
                case AirStatus.DOWN:
                    if (!hit)
                        _animationController.Jump(value);
                    break;
                case AirStatus.LAND:
                    if (hit)
                        hit = false;
                    _animationController.Land(_moveStatus);
                    break;
            }
            _airStatus = value;
        }
    }

    public bool headingRight
    {
        get => _headingRight;
        set
        {
            if (value == _headingRight)
                return;
            //anime control
            _animationController.heading = value;
            _headingRight = value;
        }
    }

    public bool hit
    {
        get => _hit;
        set
        {
            if (value == _hit)
                return;
            _hit = value;
        }
    }

    [Header("Animation Controller")]
    [SerializeField] private ObjectAnimationController _animationController;
    [Header("Input Controller")]
    [SerializeField] private InputController input = null;
    [Header("Movement")]
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 20f;
    [Header("Jump")]
    [SerializeField, Range(0f, 10f)] private float jumpHeight = 3f;
    [SerializeField, Range(0, 5)] private int maxAirJumps = 0;
    [SerializeField, Range(0f, 5f)] private float downwardMovementMultiplier = 3f;
    [SerializeField, Range(0f, 5f)] private float upwardMovementMultiplier = 1.7f;
    [SerializeField, Range(0f, 0.3f)] private float _coyoteTime = 0.2f;
    [SerializeField, Range(0f, 0.3f)] private float _jumpBufferTime = 0.2f;
    [Header("hit knockback")]
    [SerializeField] private Transform _hitbox;
    [SerializeField, Range(0f, 0.3f)] private float _knockBackBufferTime = 0.2f;

    private Vector2 _dir;
    private Vector2 _desiredVelocity;
    private Vector2 _velocity;

    private Rigidbody2D _body;
    private Ground _ground;
    private bool _onGround;

    private float _maxSpeedChange;
    private float _acceleration;

    private int _jumpPhase;
    private float _defaultGravityScale;
    private float _coyoteCounter;
    private float _jumpBufferCounter;
    private float _jumpSpeed;

    private bool _desiredJump;
    private bool _onAir;
    private bool _desiredHit;

    private MoveStatus _moveStatus;
    private AirStatus _airStatus;
    private bool _headingRight = true;
    private bool _hit = false;

    private float _hitSpeed;
    private float _hitBufferCounter;
    private float _knockbackForce;

    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _ground = GetComponent<Ground>();

        _desiredVelocity = Vector2.zero;
        _defaultGravityScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_hit)
        {
            //hit
        }
        else
        {
            _dir.x = input.RetrieveMoveInput();
            //move Anime control
            MoveAnimeControl(_dir.x);

            _desiredVelocity.x = _dir.x * Mathf.Max(maxSpeed - _ground.friction, 0f);
            _desiredJump |= input.RetrieveJumpInput();
        }
    }

    void FixedUpdate()
    {
        _onGround = _ground.onGround;
        _velocity = _body.velocity;

        GroundStatus();

        if (!hit)
        {
            MoveHorizontal();
            JumpVertical();
        }
        else
        {
            HitMovement();
        }

        GravityScale();

        _body.velocity = _velocity;

    }

    private void MoveAnimeControl(float control)
    {
        if (control == 0)
            moveStatus = MoveStatus.STAND;
        else
        {
            moveStatus = MoveStatus.MOVE;
            headingRight = (control > 0);
        }
    }

    private void MoveHorizontal()
    {
        _acceleration = _onGround ? maxAcceleration : maxAirAcceleration;
        _maxSpeedChange = _acceleration * Time.deltaTime;
        _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);
    }

    private void GroundStatus()
    {
        if (_onGround) //landing
        {
            //initialize
            _jumpPhase = 0;
            _coyoteCounter = _coyoteTime;

            _onAir = false;
            airStatus = AirStatus.LAND;
        }
        else if (_body.velocity.y != 0)
        {
            _coyoteCounter -= Time.deltaTime;
            _onAir = true;
            if (_body.velocity.y > 0)
            {
                airStatus = AirStatus.UP;
            }
            else
            {
                airStatus = AirStatus.DOWN;
            }
        }
    }

    private void JumpVertical()
    {
        if (_desiredJump)
        {
            _desiredJump = false;
            _jumpBufferCounter = _jumpBufferTime;
        }
        else if (!_desiredJump && _jumpBufferCounter > 0)
        {
            _jumpBufferCounter -= Time.deltaTime;
        }

        if (_jumpBufferCounter > 0)
        {
            JumpAction();
        }
    }

    private void JumpAction()
    {
        if (_coyoteCounter > 0f || (_jumpPhase < maxAirJumps && _onAir))
        {
            if (_onAir)
            {
                _jumpPhase += 1;
            }

            _jumpBufferCounter = 0;
            _coyoteCounter = 0;

            _jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);

            if (_velocity.y > 0f)
            {
                _jumpSpeed = Mathf.Max(_jumpSpeed - _velocity.y, 0f);
            }
            else if (_velocity.y < 0f)
            {
                _jumpSpeed += Mathf.Abs(_body.velocity.y);
            }
            _velocity.y += _jumpSpeed;
        }
    }

    private void HitMovement()
    {
        if (_desiredHit)
        {
            _desiredHit = false;
            _hitBufferCounter = _knockBackBufferTime;
        }
        else if (!_desiredHit && hit)
        {
            _hitBufferCounter -= Time.deltaTime;
        }

        if (_hitBufferCounter > 0)
        {
            HitAction();
        }
    }

    private void HitAction()
    {
        _hitBufferCounter = 0;

        _hitSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);

        if (_velocity.y > 0f)
        {
            _hitSpeed = Mathf.Max(_hitSpeed - _velocity.y, 0f);
        }
        else if (_velocity.y < 0f)
        {
            _hitSpeed += Mathf.Abs(_body.velocity.y);
        }
        _velocity.y += _hitSpeed;
        _velocity.x += Mathf.Lerp(_knockbackForce, 0f, 0.01f);
    }

    private void GravityScale()
    {
        if (input.RetrieveJumpHoldInput() && _body.velocity.y > 0)
        {
            _body.gravityScale = upwardMovementMultiplier;
        }
        else if (!input.RetrieveJumpHoldInput() || _body.velocity.y < 0)
        {
            _body.gravityScale = downwardMovementMultiplier;
        }
        else if (_body.velocity.y == 0)
        {
            _body.gravityScale = _defaultGravityScale;
        }
    }

    public void Hit(Vector2 colPos)
    {
        hit = true;
        _desiredHit = true;
        _knockbackForce = transform.position.x - colPos.x; 
    }
}
