
# Development-journal
Simplified template 

# Group project

<br>Keisha Byrne</br>
<br>2401317</br>

<br>Callum Wade

<br>2404781

<br>Fundamentals Of Games Development</br>

# Research

## What research did you do?
<br> Our research process involved gathering various elements to shape our game's design and style. We started by sourcing images for our characters to establish their look and feel, ensuring they aligned with the aesthetic we envisioned. For the game's style, we decided on 2D pixel art, reflecting a classic, retro-inspired approach. We explored and selected sound effects to match the game's theme, including atmospheric background music to enhance the player's immersion. Additionally, we researched potential enemy types to create diverse and engaging challenges, as well as a variety of weapon types to provide players with exciting gameplay options.</br>

## How did it help?

<br>Our research proved valuable in shaping our project. It gave us a clear and solid vision of what we wanted to create, providing a secure foundation for the entire development process. With a well-defined idea in place, we were able to identify exactly what needed to be done and estimate the time required for each task. This clarity allowed us to approach the project with confidence and efficiency, ensuring that all elements aligned with our overall goals and vision.</br>

## What websites did I use?

The main website I used was the unity documentation, specifically the documentation on instantiating objects and on using ienumerators and coroutines(Technologies, s.d.).

The first website, on instantiating objects, helped me to create the bullets that the player shoots. The website taught me how to layout an instantiate script so that I can control the location of where the bullet spawns and the objects rotation. Also it showed me how to organise the objects so that all the bullets spawn inside of a parent to keep the hierarchy tidy instead of having it full of bullet clones.

I found this website to be very informative on instantiating objects as it tells you everything there is to instantiating from the basics to setting the clones a parent object and setting the clones velocity when it spawns.

The second website, on using ienumerators and coroutines, helped me to add reloading to the game as I needed to make a script that reloaded the player's bullets after a few seconds whenever they press R or when they run out of ammo. The website showed me how to use a coroutine to wait a specific amount of time by using yield return new WaitForSeconds(waitTime);. The website also shows how coroutines can be stopped using code, which I didn't need but I still found it interesting and good to know for future projects.

I found this website to be very full on, this is because it goes very in depth on the ins and outs of coroutines even though I only needed the basics. However this isn't a bad thing since I can come back to read the rest of the website in the future when I need to use coroutines again.


# How Did I Make It?

<br>We approached the development of the game by dividing tasks effectively between us. Callum primarily handled the coding, guiding me along the way by using comments in the code to explain functions and processes. Meanwhile, I focused on the level design, creating the sprites, and sourcing images(@UnityAssetStore, 2022) that inspired and brought our ideas to life. I also selected the background music and some sound effects (Uppbeat.io, 2024), which we both reviewed and agreed suited the game. This collaboration allowed us to combine our skills and creativity, ensuring a smooth development process and a cohesive final product.</br>

## Implementation
- First I started by making the movement script for the player.
<br>
```csharp
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
}
```
*Figure 1. Shows the script that was used to control the player's movement and jumping as well as rotate the player asset based on the direction they are moving.*

<br>

- Then I added a gliding mechanic so that when the player holds jump, they can fall slower in the air
<br>

```csharp
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
```
*Figure 2. Shows the movement script with the gliding added to it*

<br>

- Afterwards, I created a fox ai script for the basic fox enemy. This script makes the fox patrol left and right until the player comes close to it which causes it to chase the player.

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public int basicFoxHealth = 30;
    Shooting shootingScript;

    public float moveSpeed = 2f;           // Enemy's movement speed
    public float patrolRange = 5f;         // Distance for patrolling
    public float chaseSpeed = 4f;          // Speed when chasing the player
    public float detectionRange = 3f;      // Range within which enemy detects the player

    private bool movingRight = true;       // Direction of patrolling
    private float patrolStartX;            // Starting X position for patrolling
    private Rigidbody2D rb;                // Enemy's Rigidbody2D component
    private Transform player;              // Reference to the player's Transform

    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        patrolStartX = transform.position.x;  // Set patrol starting point
        player = GameObject.FindGameObjectWithTag("Player").transform;  // Find the player using tag
        shootingScript = GameObject.FindObjectOfType<Shooting>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Calculate distance between the enemy and the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // If the player is within the detection range, chase the player
        if (distanceToPlayer <= detectionRange)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        if (basicFoxHealth <= 0) 
        {
            Destroy(gameObject);
        }
    }

    // Patrolling movement
    void Patrol()
    {
        // Check if the enemy is moving beyond patrol range and reverse direction
        if (movingRight && transform.position.x >= patrolStartX + patrolRange)
        {
            Flip();
        }
        else if (!movingRight && transform.position.x <= patrolStartX - patrolRange)
        {
            Flip();
        }

        // Move the enemy
        rb.velocity = new Vector2((movingRight ? moveSpeed : -moveSpeed), rb.velocity.y);
    }

    // Chase the player when they are within detection range
    void ChasePlayer()
    {
        // Move towards the player
        float direction = player.position.x > transform.position.x ? 1 : -1;
        rb.velocity = new Vector2(direction * chaseSpeed, rb.velocity.y);

        // Flip enemy direction to face the player
        if ((direction > 0 && !movingRight) || (direction < 0 && movingRight))
        {
            Flip();
        }
    }

    // Flip the enemy's direction (left or right)
    void Flip()
    {
        movingRight = !movingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    // Optional: Visualize patrol and detection range in Unity editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector2(patrolStartX - patrolRange, transform.position.y), new Vector2(patrolStartX + patrolRange, transform.position.y));

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            basicFoxHealth = basicFoxHealth - shootingScript.bulletDamage;
            audioSource.Play();
        }


    }
}
```
*Figure 3. This script controls the basic fox, causing it to patrol an area and chase the player. Also this script allows the fox to take damage when they are shot, which was implemented after*

<br>

- After making the fox, I worked on the shooting. I first made the pistol.

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject pistolBulletPrefab;
    public Transform firePoint;
    [SerializeField] private LevelSelect levelSelectScript;
    public float bulletSpeed;
    public float fireRate;
    private float nextFireTime;
    public string currentGun;
    public float pistolAmmoCount = 6f;
    public float bulletLifetime;
    public float reloadTime;
    public int bulletDamage;
    public float maxAmmo = 6;  // Max ammo for pistol (you can adjust for other guns)
    public bool isReloading = false;

    [SerializeField] private Transform PistolFirePoint;
    private Transform SpawnPoint;

    private AudioSource[] audioSources;
    private int biteAudioIndex = 0;
    private int gunShotAudioIndex = 1;
    private int jumpAudioIndex = 2;
    private int landAudioIndex = 3;
    private void Start()
    {
        levelSelectScript = GameObject.Find("1").GetComponent<LevelSelect>();
        audioSources = GetComponents<AudioSource>();
        currentGun = "Pistol";
        if (LevelSelect.levelUnlocked >= 3)
        {
            shotgunUnlocked = true;
        }        
    }

    void Update()
    {
        // Check if the player can shoot (enough ammo, fire rate limit, and not reloading)
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime && !isReloading)
        {
            if (currentGun == "Pistol" && pistolAmmoCount > 0)
            {
                Shoot();
                pistolAmmoCount--;  // Deduct ammo for pistol
                if (!audioSources[gunShotAudioIndex].isPlaying)
                {
                    audioSources[gunShotAudioIndex].Play();       
                }
            }
            nextFireTime = Time.time + fireRate;
        }

        // Switch between guns
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentGun = "Pistol";
        }

        // Set weapon stats based on the current gun
        if (currentGun == "Pistol")
        {
            bulletSpeed = 20f;
            fireRate = 0.5f;
            nextFireTime = 0f;
            maxAmmo = 6;
            reloadTime = 2f;
            bulletLifetime = 2f;
            bulletDamage = 10;
        }

        // Reload script (start reloading if ammo is 0 or player presses 'r')
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && (pistolAmmoCount == 0))
        {
            StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        // Calculate the direction from the player to the mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - firePoint.position).normalized;

        if (currentGun == "Pistol")
        {
            // Instantiate the pistol bullet at the fire point position
            GameObject bullet = Instantiate(pistolBulletPrefab, PistolFirePoint.position, firePoint.rotation);

            // Get the Rigidbody2D component from the bullet and apply force in the direction of the mouse
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = direction * bulletSpeed;

            // Rotate the fire point to face the mouse direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // Destroy the bullet after a certain amount of time
            Destroy(bullet, bulletLifetime);
        }
    }



    IEnumerator Reload()
    {
        isReloading = true; // Set reloading to true to prevent shooting while reloading
        Debug.Log("Reloading...");

        // Wait for the reload time
        yield return new WaitForSeconds(reloadTime);

        // Refill ammo based on the current gun
        if (currentGun == "Pistol")
        {
            pistolAmmoCount = maxAmmo;
        }
        isReloading = false;
        Debug.Log("Reload complete. Ammo refilled.");
    }
}
```
*Figure 4. This is the shooting script with the pistol added. When the player presses 1, they equip the pistol which changes the guns stats to the pistol stats. When R is pressed or the pistol runs out of ammo, the pistol reloads. When left click is pressed, a bullet is instantiated at the firing point.*

<br>

- Then I added a bullet script to go on the bullet prefab

```csharp
public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //stops the bullet and player from interacting
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }
}
```
*Figure 5. This script stops the bullets from interacting with the physics of the player.*

<br>

- After, I added the shotgun to the shooting script

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject pistolBulletPrefab;
    public GameObject shotgunBulletPrefab;
    public Transform firePoint;
    [SerializeField] private LevelSelect levelSelectScript;
    public float bulletSpeed;
    public float fireRate;
    private float nextFireTime;
    public string currentGun;
    public bool shotgunUnlocked = false;
    public float pistolAmmoCount = 6f;
    public float shotgunAmmoCount = 4f;
    public float bulletLifetime;
    public float reloadTime;
    public int bulletDamage;
    public float maxAmmo = 6;  // Max ammo for pistol (you can adjust for other guns)
    public bool isReloading = false;

    [SerializeField] private Transform ShotGunFirePoint;
    [SerializeField] private Transform PistolFirePoint;
    private Transform SpawnPoint;

    private AudioSource[] audioSources;
    private int biteAudioIndex = 0;
    private int gunShotAudioIndex = 1;
    private int jumpAudioIndex = 2;
    private int landAudioIndex = 3;
    private void Start()
    {
        levelSelectScript = GameObject.Find("1").GetComponent<LevelSelect>();
        audioSources = GetComponents<AudioSource>();
        currentGun = "Pistol";
        if (LevelSelect.levelUnlocked >= 3)
        {
            shotgunUnlocked = true;
        }        
    }

    void Update()
    {
        // Check if the player can shoot (enough ammo, fire rate limit, and not reloading)
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime && !isReloading)
        {
            if (currentGun == "Pistol" && pistolAmmoCount > 0)
            {
                Shoot();
                pistolAmmoCount--;  // Deduct ammo for pistol
                if (!audioSources[gunShotAudioIndex].isPlaying)
                {
                    audioSources[gunShotAudioIndex].Play();       
                }
            }
            else if (currentGun == "Shotgun" && shotgunAmmoCount > 0)
            {
                Shoot();
                shotgunAmmoCount--;  // Deduct ammo for shotgun
            }

            nextFireTime = Time.time + fireRate;
        }

        // Switch between guns
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentGun = "Pistol";
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && shotgunUnlocked)
        {
            currentGun = "Shotgun";
        }

        // Set weapon stats based on the current gun
        if (currentGun == "Pistol")
        {
            bulletSpeed = 20f;
            fireRate = 0.5f;
            nextFireTime = 0f;
            maxAmmo = 6;
            reloadTime = 2f;
            bulletLifetime = 2f;
            bulletDamage = 10;
        }
        if (currentGun == "Shotgun")
        {
            bulletSpeed = 20f;
            fireRate = 1f;
            nextFireTime = 0.5f;
            maxAmmo = 4;
            reloadTime = 3f;
            bulletLifetime = 1f;
            bulletDamage = 20;
        }

        // Reload script (start reloading if ammo is 0 or player presses 'r')
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && (pistolAmmoCount == 0 || shotgunAmmoCount == 0))
        {
            StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        // Calculate the direction from the player to the mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - firePoint.position).normalized;

        if (currentGun == "Pistol")
        {
            // Instantiate the pistol bullet at the fire point position
            GameObject bullet = Instantiate(pistolBulletPrefab, PistolFirePoint.position, firePoint.rotation);

            // Get the Rigidbody2D component from the bullet and apply force in the direction of the mouse
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = direction * bulletSpeed;

            // Rotate the fire point to face the mouse direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // Destroy the bullet after a certain amount of time
            Destroy(bullet, bulletLifetime);
        }
        else if (currentGun == "Shotgun")
        {   
            // Move the spawn position in front of the player
            // firePoint.up will give us the direction the player is facing
            //Vector3 spawnPosition = firePoint.position + firePoint.up * 0.5f;  // Adjust 0.5f to control how far in front of the player the bullet spawns

            // Ensure the spawn position is in front of the player, accounting for player facing direction
            GameObject bullet = Instantiate(shotgunBulletPrefab, ShotGunFirePoint.position, firePoint.rotation);

            // Get the Rigidbody2D component from the bullet and apply force in the direction of the mouse
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = direction * bulletSpeed;

            // Rotate the fire point to face the mouse direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // Destroy the bullet after a certain amount of time
            Destroy(bullet, bulletLifetime);
        }
    }



    IEnumerator Reload()
    {
        isReloading = true; // Set reloading to true to prevent shooting while reloading
        Debug.Log("Reloading...");

        // Wait for the reload time
        yield return new WaitForSeconds(reloadTime);

        // Refill ammo based on the current gun
        if (currentGun == "Pistol")
        {
            pistolAmmoCount = maxAmmo;
        }
        else if (currentGun == "Shotgun")
        {
            shotgunAmmoCount = maxAmmo;
        }

        isReloading = false;
        Debug.Log("Reload complete. Ammo refilled.");
    }
}

```
*Figure 6. Shows the Shooting script with the shotgun implemented. When 2 is pressed, the player switches to the shotgun*

<br>

- I then created a player script to manage the player's health.

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static float playerHealth = 100;

    // Update is called once per frame
    void Update()
    {
        //Stops the health from going below 0
        if (playerHealth <= 0)
        {
            playerHealth = 0;
        }
        if (playerHealth <= 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            playerHealth = playerHealth - 30f;
        }
    }
}

```
*Figure 7. A script to manage the player's health so that when the player collides with an enemy, they take damage. Also when the player dies, the level resets.*

<br>

- After, I worked on multiple UI scripts to display information for the player on the UI.

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    TMP_Text playerHealthText;

    // Start is called before the first frame update
    void Start()
    {
        playerHealthText = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        playerHealthText.text = ("Health: ") + Player.playerHealth;
    }
}
```
*Figure 8. The script that displays the player's current health on the UI*

<br>

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gunstoselect : MonoBehaviour
{
    TMP_Text gunsToSelectText;
    Shooting shootingScript;
    string pistolText = "1 - Pistol";
    string shotgunText = "2 - Shotgun";
    
    // Start is called before the first frame update
    private void Start()
    {
        gunsToSelectText = GetComponent<TMP_Text>();
        shootingScript = GameObject.FindObjectOfType<Shooting>();
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (shootingScript.shotgunUnlocked == false)
        {
            gunsToSelectText.text = pistolText;
        }
        else if (shootingScript.shotgunUnlocked == true)
        {
            gunsToSelectText.text = pistolText + "\n" + shotgunText;
        }
    }
}
```
*Figure 9. Displays the GunsToSelect script which displays the guns that are useable.*

<br>

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoCount : MonoBehaviour
{
    TMP_Text ammoCountText;
    Shooting shootingScript;

    // Start is called before the first frame update
    void Start()
    {
        ammoCountText = GetComponent<TMP_Text>();
        shootingScript = GameObject.FindObjectOfType<Shooting>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shootingScript.isReloading == false)
        {
            if (shootingScript.currentGun == "Pistol")
            {
                ammoCountText.text = (shootingScript.pistolAmmoCount.ToString() + "/" + shootingScript.maxAmmo.ToString());
            }
            if (shootingScript.currentGun == "Shotgun")
            {
                ammoCountText.text = (shootingScript.shotgunAmmoCount.ToString() + "/" + shootingScript.maxAmmo.ToString());
            }
        }
        if (shootingScript.isReloading == true)
        {
            ammoCountText.text = ("Reloading...");
        }       
    }
}

```
*Figure 10. The ammo count script displays the players current ammo count. When the player is reloading it shows "Reloading..."*

<br>

```csharp
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrentGun : MonoBehaviour
{
    TMP_Text currentGunText;
    Shooting shootingScript;

    // Start is called before the first frame update
    void Start()
    {
        currentGunText = GetComponent<TMP_Text>();
        shootingScript = GameObject.FindObjectOfType<Shooting>();
    }

    // Update is called once per frame
    void Update()
    {
        currentGunText.text = ("Gun: " + shootingScript.currentGun);
    }
}
```
*Figure 11. The current gun script displays the current gun that the player is using.*

<br>

- After completing the UI, I added an end level script

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    [SerializeField] private LevelSelect levelSelectScript;
    private void Start()
    {
        levelSelectScript = GameObject.Find("1").GetComponent<LevelSelect>();
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("hi");
        if (other.gameObject.CompareTag("Player"))
        {
            LoadLevelSelect();
        }
    }
    public void LoadLevelSelect()
    {
        //levelSelectScript.levelUnlocked++;
        LevelSelect.levelUnlocked++;
        SceneManager.LoadScene("Level Select");
    }
}

```
*Figure 12. EndLevel script makes it so that when the player collides with the end coop, the level select scene is loaded and unlocks the next level using an int variable call levelUnlocked.*

<br>

- After finishing this script, I added the level select scene and programmed it.

```csharp
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    [HideInInspector] public static int levelUnlocked = 1;
    private int levelClickedInt;

    // Start is called before the first frame update
    public void Start()
    {
        //gets the name of the object the script is on
        string levelClicked = gameObject.name;
        //changes the levelClicked variable to an int variable
        levelClickedInt = int.Parse(levelClicked);
    }

    public void LevelClicked()
    {
       if (levelClickedInt <= levelUnlocked)
        {
            //loads the level based on levelClickedInt
            SceneManager.LoadScene(levelClickedInt);
        }
    }
}
```
*Figure 13. The LevelSelect script tracks the levels completed and unlocked with the levelUnlocked. When a level is completed, the levelUnlocked variable is increased by 1. This script goes on each level button and the script gets the buttons name, which is the same as the number of the level, then it changes it into an int variable which is compared to the levelUnlocked variable to determine if the button will take the player to the level. *


# Outcome (The Final Product)

- [Video Demonstration Part 1](https://youtu.be/KOPqNBwS5-k) 
- [Video Demonstration Part 2](https://youtu.be/LpnZCovlkdE )
- [Game Build](https://c6wx.itch.io/eggs-of-vengeance)
- [Github](https://github.com/C6WX/Eggcode-Studios)

# Game Images
<img width="200" height="200" src="https://raw.githubusercontent.com/C6WX/Eggcode-Studios/refs/heads/main/chicken.jpg">
<br>
<img width="200" height="200" src="https://raw.githubusercontent.com/C6WX/Eggcode-Studios/refs/heads/main/egg.png">  
<br>
<img width="200" height="200" src="https://raw.githubusercontent.com/C6WX/Eggcode-Studios/refs/heads/main/fox.png">  
<br>
<img width="200" height="200" src="https://raw.githubusercontent.com/C6WX/Eggcode-Studios/refs/heads/main/pistol.png"> 
<br>
<img width="200" height="200" src="https://raw.githubusercontent.com/C6WX/Eggcode-Studios/refs/heads/main/shotgun.png"> 
<br>
<img width="400" height="250" src="https://raw.githubusercontent.com/C6WX/Eggcode-Studios/refs/heads/main/Level1Start%20.png"> 
<br>
<br>
<img width="400" height="250" src="https://raw.githubusercontent.com/C6WX/Eggcode-Studios/refs/heads/main/Level3Fox.png"> 
<br>
<br>
<img width="400" height="250" src="https://raw.githubusercontent.com/C6WX/Eggcode-Studios/refs/heads/main/Level4Foxes.png"> 
<br>

# Reflection

## How did it go?
<br>Overall, I think the project went quite well. We were able to work together effectively, maintaining strong communication throughout the process. This allowed us to share ideas openly and brainstorm creative solutions, ensuring that each aspect of the game aligned with our shared vision. Our collaboration was productive, and the division of tasks played to our strengths, making the development process both efficient and enjoyable.</br>

## What would you do differently?
<br>Upon reflection, there are a few things I would do differently. Firstly, I would aim to stay more consistent with the project timeline, as I fell behind in the first few weeks due to personal issues. Addressing this earlier would have helped maintain steady progress. Secondly, I would focus on improving my coding skills so that I could contribute more effectively to the technical aspects of the project, in addition to my work on the art and design. These changes would not only enhance my contribution but also provide a more balanced workload and a deeper understanding of the development process.</br>

## Bibliography
-Technologies, U. (s.d.) Unity - Scripting API: Object.Instantiate. At: https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Object.Instantiate.html (Accessed  05/12/2024).

-Technologies, U. (s.d.) Unity - Scripting API: MonoBehaviour.StartCoroutine. At: https://docs.unity3d.com/6000.0/Documentation/ScriptReference/MonoBehaviour.StartCoroutine.html (Accessed  05/12/2024).


## Declared Assets 

@UnityAssetStore. (2022). Superposition Principle - Asset Store. [online] Available at: https://assetstore.unity.com/publishers/67934 [Accessed 4 Dec. 2024]. (@UnityAssetStore, 2022)

‌ - tile map we used for the level 

Uppbeat.io. (2024). Free Music For Creators. [online] Available at: https://uppbeat.io/browse/search?query=pistol%20gun&type=sfx [Accessed 4 Dec. 2024].(Uppbeat.io, 2024) 

‌ - website we used for our sound effects 