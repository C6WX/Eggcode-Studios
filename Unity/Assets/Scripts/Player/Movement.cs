using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float jump = 200f;
    public int moveSpeed = 2;
    public bool grounded = true;
    private bool wasInAir = false;
    private float gravity = 0.5f;
    private SpriteRenderer spriteRenderer;

    private AudioSource[] audioSources;
    private int biteAudioIndex = 0;
    private int gunShotAudioIndex = 1;
    private int jumpAudioIndex = 2;
    private int landAudioIndex = 3;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravity;
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSources = GetComponents<AudioSource>();
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
            // spriteRenderer.flipX = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            //// spriteRenderer.flipX = false;
        }

        //the player can jump when space is pressed and they are touching the ground
        if (Input.GetButtonDown("Jump") && grounded == true)
        {
            rb.AddForce(new Vector2(rb.velocity.x, jump));
            wasInAir = true;
            audioSources[jumpAudioIndex].Play();
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
        if (wasInAir == true && grounded == true)
        {
            wasInAir = false;
            audioSources[landAudioIndex].Play();
        }
    }

    //When the player is touching an object with a Ground tag, grounded = true
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            audioSources[biteAudioIndex].Play();
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
