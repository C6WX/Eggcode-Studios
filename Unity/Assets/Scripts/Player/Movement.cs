using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float jump;
    public int moveSpeed = 2;
    bool grounded;
    public float gravity = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //allows the player to move horizontally
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * moveSpeed * Time.deltaTime);

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
            rb.gravityScale = 2;
        }
    }

    //When the player is touching an object with a Ground tag, grounded = true
    void OnCollisionEnter2D (Collision2D other)
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
