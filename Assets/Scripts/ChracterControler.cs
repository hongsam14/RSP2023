using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public enum GroundType
{
    None,
    Soft,
    Hard
}

public class ChracterControler : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 0.0f;
    [SerializeField] private float maxSpeed = 0.0f;
    [SerializeField] private float jumpForce = 0.0f;
    [Header("Spine")]
    [SerializeField] private string stand;
    [SerializeField] private string move;
    [SerializeField] private string jump;
    [SerializeField] private string fall;

    [SerializeField] private string bring_stand;
    [SerializeField] private string bring_move;
    [SerializeField] private string bring_jump;
    [SerializeField] private string bring_fall;
    [Header("Weapon")]
    [SerializeField] private GameObject weapon_obj;

    //spine animation skeleton
    public Spine.AnimationState spineAnimationState { get; private set; }
    public Spine.Skeleton skeleton { get; private set; }
    private Spine.TrackEntry currentTrack = null;
    
    private Rigidbody2D rigidbody;
    
    private Collider2D collider;
    private LayerMask softGroundMask;
    private LayerMask hardGroundMask;

    private SkeletonAnimation skeletonAnimation;
    
    //status
    private bool _isMoving = false;
    private float _isHeading = 1f;
    private bool _isJumping = false;
    private bool _isFalling = false;
    private bool _bring = false;
    private GroundType groundType;
    
    private bool _isMoving_past = false;
    private bool _isFalling_past = false;
    //weapon
    private Weapon weapon;
    
    /**
     * Define Animation State 
     * 
     */
    public bool isMoving
    {
        get => _isMoving;
        set
        {
            if (!isJumping)
            {
                switch (_bring)
                {
                    case true:
                        switch (value)
                        {
                            case true:
                                currentTrack = spineAnimationState?.SetAnimation(0, bring_move, true);
                                break;
                            case false:
                                currentTrack = spineAnimationState?.SetAnimation(0, bring_stand, true);
                                break;
                        }
                        break;
                    case false:
                        switch (value)
                        {
                            case true:
                                currentTrack = spineAnimationState?.SetAnimation(0, move, true);
                                break;
                            case false:
                                currentTrack = spineAnimationState?.SetAnimation(0, stand, true);
                                break;
                        }
                        break;
                }
            }
            _isMoving = value;
        }
    }
    public float isHeading
    {
        get => _isHeading;
        set
        {
            if (value == _isHeading)
                return ;
            _isHeading = value;
            skeleton.ScaleX = value;
        }
    }
    public bool isJumping
    {
        get => _isJumping;
        set
        {
            switch (_bring)
            {
                case true:
                    switch (value)
                    {
                        case true:
                            currentTrack = spineAnimationState?.SetAnimation(0, bring_jump, false);
                            break;
                        case false:
                            if (_isMoving)
                                currentTrack = spineAnimationState?.SetAnimation(0, bring_move, true);
                            else
                                currentTrack = spineAnimationState?.SetAnimation(0, bring_stand, true);
                            break;
                    }
                    break;
                case false:
                    switch (value)
                    {
                        case true:
                            currentTrack = spineAnimationState?.SetAnimation(0, jump, false);
                            break;
                        case false:
                            if (_isMoving)
                                currentTrack = spineAnimationState?.SetAnimation(0, move, true);
                            else
                                currentTrack = spineAnimationState?.SetAnimation(0, stand, true);
                            break;
                    }
                    break;
            }
            _isJumping = value;
        }
    }
    public bool isFalling
    {
        get => _isFalling;
        set
        {
            switch (_bring)
            {
                case true:
                    switch (value)
                    {
                        case true:
                            currentTrack = spineAnimationState?.SetAnimation(0, bring_fall, true);
                            break;
                    }
                    break;
                case false:
                    switch (value)
                    {
                        case true:
                            currentTrack = spineAnimationState?.SetAnimation(0, fall, true);
                            break;
                    }
                    break;
            }
            _isFalling = value;
        }
    }

    public bool isBring
    {
        get => _bring;
        set
        {
            _bring = value;
            isMoving = _isMoving;
            isJumping = _isJumping;
            isFalling = _isFalling;
            //weapon
            weapon_obj.SetActive(value);
        }
    }

    //input
    private Vector2 movementInput = Vector2.zero;
    private bool jumpInput = false;
    private Vector2 velocity;
    private Vector2 prevVelocity;
    private Vector2 jumpVec;

    void Awake()
    {
        jumpVec = new Vector2(0, jumpForce);
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        softGroundMask = LayerMask.GetMask("Ground Soft");
        hardGroundMask = LayerMask.GetMask("Ground Hard");
        //spine animation
        skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;
        //weapon object
        weapon = weapon_obj.GetComponent<Weapon>();
        weapon_obj.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        //update
        //horizontal
        movementInput.x = Input.GetAxis("Horizontal");
        
        //jump
        if (!isJumping && Input.GetKeyDown(KeyCode.Space))
        {
            jumpInput = true;
        }
    }

    void FixedUpdate()
    {
        UpdateGround();
        //set status
        SetStatus(movementInput.x);
        
        MoveHorizontal();
        Jump();
        prevVelocity = rigidbody.velocity;
    }

    void MoveHorizontal()
    {
        velocity = rigidbody.velocity;
        
        velocity += movementInput * speed * Time.deltaTime;
        movementInput.x = 0;

        //clamp hor speed
        velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);
        //assign back to body
        rigidbody.velocity = velocity;
    }

    void Jump()
    {
        //jump
        if (jumpInput && groundType != GroundType.None)
        {
            //impulse force
            rigidbody.AddForce(jumpVec, ForceMode2D.Impulse);
            jumpInput = false;
            isJumping = true;
            Debug.Log("jump");
        }
        //landing
        else if (isJumping && isFalling && groundType != GroundType.None)
        {
            prevVelocity.y = rigidbody.velocity.y;
            rigidbody.velocity = prevVelocity;

            if (_isFalling_past)
            {
                isFalling = false;
                _isFalling_past = isFalling;
            }
            isJumping = false;
            Debug.Log("landing");
        }
    }

    void SetStatus(float movement)
    {
        //set isMoving
        if (!(movement == 0) != _isMoving_past)
        {
            isMoving = !(movement == 0);
            _isMoving_past = isMoving;
        }
        
        //set isHeading
        if (movement > 0f)
            isHeading = 1f;
        else if (movement < 0f)
            isHeading = -1f;

        //falling
        if (isJumping && rigidbody.velocity.y < 0)
        {
            if (!_isFalling_past)
            {
                isFalling = true;
                _isFalling_past = isFalling;
            }
            Debug.Log("on air");
        }
    }

    void UpdateGround()
    {
        if (collider.IsTouchingLayers(softGroundMask))
        {
            groundType = GroundType.Soft;
        }
        else if (collider.IsTouchingLayers(hardGroundMask))
        {
            groundType = GroundType.Hard;
        }
        else
        {
            groundType = GroundType.None;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Drop"))
        {
            //get orb
            if (!isBring)
            {
                isBring = true;
            }
            else
            {
                weapon.fingers += 1;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Attack"))
        {
            Debug.Log("attacked");
            weapon.fingers = 0;
            isBring = false;
        }
    }
}
