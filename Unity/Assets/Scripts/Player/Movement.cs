using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float jump = 200f;
    public int moveSpeed = 2;
    public bool grounded;
    private float gravity = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravity;
    }

    // Update is called once per frame
    void Update()
    {
        //allows the player to move horizontally
        float horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        //changes the way the fox is facing based on the way the player is moving
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        //the player can jump when space is pressed and they are touching the ground
        if (Input.GetButtonDown("Jump") && grounded == true)
        {
            rb.AddForce(new Vector2(rb.velocity.x, jump));
        }
        //when the player presses jump and is in the air, gravity is changed to make the player glide
        //GLIDING SYSTEM COULD DEFINATELY USE SOME IMPROVEMENTS
        if (Input.GetButtonDown("Jump") && grounded == false)
        {
            rb.gravityScale = gravity;
        }
        //when the player touches the ground, gravity is reset back to 2
        if (grounded == true)
        {
            rb.gravityScale = 1;
        }
    }

    //When the player is touching an object with a Ground tag, grounded = true
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
    }

    //When the player stops touching the object tagged with the ground tag, grounded = false
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Movement : MonoBehaviour
//{
//    private Rigidbody2D rb;
//    public float jumpForce = 5f; // Set a suitable jump force
//    public float moveSpeed = 2f; // Set the horizontal move speed
//    private bool grounded;
//    public float gravityScale = 2f; // Normal gravity scale
//    public float glideGravityScale = 0.5f; // Gravity scale while gliding

//    void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        rb.gravityScale = gravityScale; // Initialize gravity scale
//    }

//    void Update()
//    {
//        // Handle horizontal movement
//        float horizontalInput = Input.GetAxis("Horizontal");
//        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

//        // Flip the character based on direction
//        if (horizontalInput < 0)
//        {
//            transform.localScale = new Vector3(-1f, 1f, 1f);
//        }
//        else if (horizontalInput > 0)
//        {
//            transform.localScale = new Vector3(1f, 1f, 1f);
//        }

//        // Jumping
//        if (Input.GetButtonDown("Jump") && grounded)
//        {
//            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
//        }

//        // Change gravity scale for gliding
//        if (Input.GetButtonDown("Jump") && !grounded)
//        {
//            rb.gravityScale = glideGravityScale;
//        }
//        else
//        {
//            rb.gravityScale = gravityScale;
//        }
//    }

//    // When the player is touching an object with a Ground tag, grounded = true
//    void OnCollisionEnter2D(Collision2D other)
//    {
//        if (other.gameObject.CompareTag("Ground"))
//        {
//            grounded = true;
//        }
//    }

//    // When the player stops touching the object tagged with the Ground tag, grounded = false
//    void OnCollisionExit2D(Collision2D other)
//    {
//        if (other.gameObject.CompareTag("Ground"))
//        {
//            grounded = false;
//        }
//    }
//}
