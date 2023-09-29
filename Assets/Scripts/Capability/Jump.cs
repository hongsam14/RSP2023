using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private InputController input = null;
    [SerializeField, Range(0f, 10f)] private float jumpHeight = 3f;
    [SerializeField, Range(0, 5)] private int maxAirJumps = 0;
    [SerializeField, Range(0f, 5f)] private float downwardMovementMultiplier = 3f;
    [SerializeField, Range(0f, 5f)] private float upwardMovementMultiplier = 1.7f;

    private Rigidbody2D _body;
    private Ground _ground;
    private Vector2 _velocity;

    private int _jumpPhase;
    private float _defaultGravityScale;

    private bool _desiredJump;
    private bool _onGround;

    private float _jumpSpeed;

    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _ground = GetComponent<Ground>();

        _defaultGravityScale = 1f;
    }
    // Start is called before the first frame update
    void Start()
    {
        
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

        if (_onGround)
        {
            _jumpPhase = 0;
        }
        if (_desiredJump)
        {
            _desiredJump = false;
            JumpAction();
        }
        if (_body.velocity.y > 0)
        {
            _body.gravityScale = upwardMovementMultiplier;
        }
        if (_body.velocity.y < 0)
        {
            _body.gravityScale = downwardMovementMultiplier;
        }
        if (_body.velocity.y == 0)
        {
            _body.gravityScale = _defaultGravityScale;
        }
        _body.velocity = _velocity;
    }

    private void JumpAction()
    {
        if (_onGround || _jumpPhase < maxAirJumps)
        {
            _jumpPhase++;
            _jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
            if (_velocity.y > 0f)
            {
                _jumpSpeed = Mathf.Max(_jumpSpeed - _velocity.y, 0f);
            }
            _velocity.y += _jumpSpeed;
        }
    }
}
