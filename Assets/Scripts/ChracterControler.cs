using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

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
    //spine animation skeleton
    public Spine.AnimationState spineAnimationState { get; private set; }
    public Spine.Skeleton skeleton { get; private set; }
    
    private Rigidbody2D rigidbody;
    private SkeletonAnimation skeletonAnimation;
    
    //status
    private bool _isMoving = false;
    private float _isHeading = 1f;
    public bool isMoving
    {
        get => _isMoving;
        set
        {
            if (value == _isMoving)
                return ;
            switch (value)
            {
                case true:
                    spineAnimationState?.SetAnimation(0, move, true);
                    break;
                case false:
                    spineAnimationState?.SetAnimation(0, stand, true);
                    break;
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
    private bool isJumping = false;

    //input
    private Vector2 movementInput = Vector2.zero;
    private bool jumpInput = false;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
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

        //set status
        setStatus(movementInput.x);
        
        //jump
        if (!isJumping && Input.GetKeyDown(KeyCode.Space))
        {
            jumpInput = true;
        }
    }

    void FixedUpdate()
    {
        moveHorizontal();
    }

    void moveHorizontal()
    {
        rigidbody.velocity += movementInput * speed *  Time.deltaTime;
        movementInput.x = 0;
    }

    void setStatus(float movement)
    {
        //set isMoving
        isMoving = !(movement == 0);
        
        //set isHeading
        if (movement > 0f)
            isHeading = 1f;
        else if (movement < 0f)
            isHeading = -1f;
    }
}
