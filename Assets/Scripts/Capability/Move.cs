using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveStatus
{
    MOVE,
    STAND
}

[RequireComponent(typeof(Rigidbody2D))]
public class Move : MonoBehaviour
{
    public MoveStatus moveStatus
    {
        get
        {
            return _moveStatus;
        }
        set
        {
            if (value == _moveStatus)
                return;
            switch (value)
            {
                case MoveStatus.MOVE:
                    //moveAnime(value);
                    break ;
                case MoveStatus.STAND:
                    //standAnime(value);
                    break ;
            }
            _moveStatus = value;
        }
    }

    public delegate void AnimeFunc(MoveStatus status);
    public AnimeFunc standAnime;
    public AnimeFunc moveAnime;
    
    [SerializeField] private InputController input = null;
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 20f;

    private Vector2 _dir;
    private Vector2 _desiredVelocity;
    private Vector2 _velocity;

    private Rigidbody2D _body;
    private Ground _ground;

    private float _maxSpeedChange;
    private float _acceleration;
    private bool _onGround;

    private MoveStatus _moveStatus;

    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _ground = GetComponent<Ground>();
        
        _desiredVelocity = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        _dir.x = input.RetrieveMoveInput();
        _desiredVelocity.x = _dir.x * Mathf.Max(maxSpeed - _ground.friction, 0f);
    }

    void FixedUpdate()
    {
        _onGround = _ground.onGround;
        _velocity = _body.velocity;

        _acceleration = _onGround ? maxAcceleration : maxAirAcceleration;
        _maxSpeedChange = _acceleration * Time.deltaTime;
        _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);

        _body.velocity = _velocity;

    }
}
