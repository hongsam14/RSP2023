using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChracterControler : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 0.0f;
    [SerializeField] private float maxSpeed = 0.0f;
    [SerializeField] private float jumpForce = 0.0f;

    private Rigidbody2D rigidbody;
    //status
    private bool isMoving = false;
    private bool isJumping = false;

    //input
    private Vector2 movementInput = Vector2.zero;
    private bool jumpInput = false;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
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
        moveHorizontal();
    }

    void moveHorizontal()
    {
        rigidbody.velocity += movementInput * speed *  Time.deltaTime;
        movementInput.x = 0;
    }
}
