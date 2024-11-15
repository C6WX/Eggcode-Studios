using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PouncingFox : MonoBehaviour
{
    public float detectionRange = 10f;  // Range at which the enemy detects the player
    public float pounceHeight = 5f;     // How high the enemy pounces into the air
    public float pounceSpeed = 10f;     // How fast the enemy moves toward the player after pouncing
    public float pounceCooldown = 3f;   // Cooldown before another pounce can happen
    public float pounceTimeToReachPeak = 0.5f; // Time it takes to reach the peak of the pounce

    private Transform player;           // Reference to the player's transform
    private bool isPouncing = false;    // Flag to prevent pounce spamming
    private Vector3 pounceTarget;       // Where the enemy will dive toward
    private float pounceStartTime;      // Time when the pounce started

    private void Start()
    {
        // Find the player's GameObject (make sure your player has a tag "Player")
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // Check distance to player
        if (Vector3.Distance(transform.position, player.position) <= detectionRange && !isPouncing)
        {
            // Start pounce if player is close and not already pouncing
            StartCoroutine(PounceAtPlayer());
        }
    }

    private IEnumerator PounceAtPlayer()
    {
        // Set pouncing flag to prevent multiple pounces in quick succession
        isPouncing = true;

        // Record the start time of the pounce
        pounceStartTime = Time.time;

        // Pounce upwards with a smooth curve (using a sine function to ease it)
        Vector3 startPosition = transform.position;
        Vector3 pounceUpPosition = new Vector3(transform.position.x, transform.position.y + pounceHeight, transform.position.z);

        // While the pounce is going upwards, use a smooth easing
        while (Time.time - pounceStartTime < pounceTimeToReachPeak)
        {
            float t = (Time.time - pounceStartTime) / pounceTimeToReachPeak; // Time factor
            float height = Mathf.Sin(t * Mathf.PI * 0.5f) * pounceHeight; // Smooth easing for upward motion
            transform.position = new Vector3(startPosition.x, startPosition.y + height, startPosition.z);
            yield return null;
        }

        // Now calculate the target position for the dive
        pounceTarget = player.position;
        pounceTarget.y = transform.position.y; // Keep y the same as the pounce height

        // Dive towards the player's position with smooth interpolation
        while (Vector3.Distance(transform.position, pounceTarget) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, pounceTarget, pounceSpeed * Time.deltaTime);
            yield return null;
        }

        // Ensure we end up exactly at the target position to avoid jittering or oscillation
        transform.position = pounceTarget;

        // After pouncing, add cooldown before pouncing again
        yield return new WaitForSeconds(pounceCooldown);
        isPouncing = false;
    }
}
