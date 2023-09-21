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
    [SerializeField] private string onAir;
    [SerializeField] private string fall;
    [Header("Attack Mode")]
    [SerializeField] private string a_stand;
    [SerializeField] private string a_move;
    [SerializeField] private string a_jump;

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
    /**
     * Define Animation State 
     * 
     */
    public bool isMoving
    {
        get => _isMoving;
        set
        {
            if (value == _isMoving)
                return ;
            if (!isJumping)
            {
                switch (value)
                {
                    case true:
                        currentTrack = spineAnimationState?.SetAnimation(0, move, true);
                        break;
                    case false:
                        currentTrack = spineAnimationState?.SetAnimation(0, stand, true);
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
            if (value == _isJumping)
                return ;
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
            _isJumping = value;
        }
    }
    public bool isFalling
    {
        get => _isFalling;
        set
        {
            if (value == _isFalling)
                return;
            switch (value)
            {
                case true:
                    currentTrack = spineAnimationState?.SetAnimation(0, onAir, true);
                    break;
                /*
                case false:
                    currentTrack = spineAnimationState?.SetAnimation(0, fall, false);
                    break;
                */
            }
            _isFalling = value;
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

            isFalling = false;
            isJumping = false;
            Debug.Log("landing");
        }
    }

    void SetStatus(float movement)
    {
        //set isMoving
        isMoving = !(movement == 0);
        
        //set isHeading
        if (movement > 0f)
            isHeading = 1f;
        else if (movement < 0f)
            isHeading = -1f;

        //falling
        if (isJumping && rigidbody.velocity.y < 0)
        {
            isFalling = true;
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
        }
    }
}
