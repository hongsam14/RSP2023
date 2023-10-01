using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AirStatus
{
    UP,
    DOWN,
    GROUND
}

[RequireComponent(typeof(Rigidbody2D))]
public class Jump : MonoBehaviour
{
    public AirStatus airStatus
    {
        get
        {
            return _airStatus;
        }
        set
        {
            if (value == _airStatus)
                return;
            switch (value)
            {
                case AirStatus.UP:
                case AirStatus.DOWN:
                    //jumpAnime(value);
                    break;
                case AirStatus.GROUND:
                    //groundAnime(value);
                    break;
            }
            _airStatus = value;
        }
    }
    public delegate void AnimeFunc(AirStatus status);
    public AnimeFunc jumpAnime;
    public AnimeFunc groundAnime;

    [SerializeField] private InputController input = null;
    [SerializeField, Range(0f, 10f)] private float jumpHeight = 3f;
    [SerializeField, Range(0, 5)] private int maxAirJumps = 0;
    [SerializeField, Range(0f, 5f)] private float downwardMovementMultiplier = 3f;
    [SerializeField, Range(0f, 5f)] private float upwardMovementMultiplier = 1.7f;
    [SerializeField, Range(0f, 0.3f)] private float _coyoteTime = 0.2f;
    [SerializeField, Range(0f, 0.3f)] private float _jumpBufferTime = 0.2f;

    private Rigidbody2D _body;
    private Ground _ground;
    private Vector2 _velocity;

    private int _jumpPhase;
    private float _defaultGravityScale;
    private float _coyoteCounter;
    private float _jumpBufferCounter;

    private bool _desiredJump;
    private bool _onGround;
    
    private bool _onAir;
    private AirStatus _airStatus;

    private float _jumpSpeed;

    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _ground = GetComponent<Ground>();

        _defaultGravityScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        _desiredJump |= input.RetrieveJumpInput();
    }

    void FixedUpdate()
    {
        _onGround = _ground.onGround;
        _velocity = _body.velocity;

        if (_onGround) //landing
        {
            //initialize
            _jumpPhase = 0;
            _coyoteCounter = _coyoteTime;

            _onAir = false;
            airStatus = AirStatus.GROUND;
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
        _body.velocity = _velocity;
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
            //_isJumping = true;
            
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
}
