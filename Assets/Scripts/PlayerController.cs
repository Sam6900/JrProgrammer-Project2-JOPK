using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
//  ENCAPSULATION

    [SerializeField] private float moveSpeed;
    [HideInInspector] public int playerLifeCount;
    [HideInInspector] public int moneyCount;
    private int minLifeCount = 0;
    public int MinLifeCount
    {
        get
        {
            return minLifeCount;
        }

        set
        {
            if (value < 0)
            {
                Debug.LogError("Player minimum life count cannot be negative");
            }

            else
            {
                minLifeCount = value;
            }
        }
    }

    private float playerOldMoveSpeed;
    private float playerOldFireRate;
    private Vector2 moveDir;
    private Vector2 playerInitialPos = new Vector2(0.5f, 0);
    public static bool isPlayerAlive = true;

    private Rigidbody2D rb;
    private Animator animator;
    private Shooting playerShootingScript;
    [SerializeField] private HUDController hudControllerScript;

    public static event Action PlayerDeathHandler;
    public static event Action NukeExplosionHandler;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerShootingScript = GetComponent<Shooting>();

        playerOldMoveSpeed = moveSpeed;
        playerOldFireRate = playerShootingScript.fireRate;
        hudControllerScript.UpdatePlayerLifeCount(playerLifeCount);
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    private void FixedUpdate()
    {
        // Moves the player rigidbody
        rb.MovePosition(rb.position + moveDir.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CoinX1"))
        {
            CoinX1Function();
        }
        else if (collision.CompareTag("CoinX5"))
        {
            CoinX5Function();
        }
        else if (collision.CompareTag("LifeItem"))
        {
            LifeItemFunction();
        }
        else if (collision.CompareTag("Tea"))
        {
            StartCoroutine(TeaItemFunction());
        }
        else if (collision.CompareTag("MachineGun"))
        {
            StartCoroutine(MachineGunItemFunction());
        }
        else if (collision.CompareTag("Nuke"))
        {
            NukeItemFunction();
        }
        else if (collision.CompareTag("WagonWheel"))
        {
            StartCoroutine(WagonWheelItemFunction());
        }
    }

 // ABSTRACTION

    private void HandleMovement()
    {
        float moveX = 0f;
        float moveY = 0f;

        // Get Input for movement
        if (Input.GetKey(KeyCode.W))
        {
            moveY = +1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveY = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveX = +1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1f;
        }
        moveDir = new Vector2(moveX, moveY);

        // If player is not shooting change move animation states
        if (Shooting.isShooting == false)
        {
            animator.SetFloat("Horizontal", moveX);
            animator.SetFloat("Vertical", moveY);
            animator.SetFloat("Speed", moveDir.sqrMagnitude);
        }
    }

    private void HandlePlayerDeath()
    {
        if (playerLifeCount > minLifeCount)
        {
            playerLifeCount -= 1;
            hudControllerScript.UpdatePlayerLifeCount(playerLifeCount);

            if (PlayerDeathHandler != null)
                PlayerDeathHandler();
        }
        else if (playerLifeCount == minLifeCount) // Sets actual life count under 0 to prevent player spawning on death
        {
            playerLifeCount -= 1;
            if (PlayerDeathHandler != null)
                PlayerDeathHandler();
        }
    }

    void OnDisable()
    {
        isPlayerAlive = false;
        HandlePlayerDeath();
    }

    void OnEnable()
    {
        isPlayerAlive = true;
        transform.position = playerInitialPos;
        moveSpeed = playerOldMoveSpeed;
        playerShootingScript.fireRate = playerOldFireRate;
        playerShootingScript.isWagonWheelActive = false;
    }

    #region Item's functionality

    private void CoinX1Function()
    {
        moneyCount += 1;
        Debug.Log(moneyCount);
    }

    private void CoinX5Function()
    {
        moneyCount += 5;
    }

    private void LifeItemFunction()
    {
        playerLifeCount += 1;
        hudControllerScript.UpdatePlayerLifeCount(playerLifeCount);
    }

    private IEnumerator TeaItemFunction()
    {
        moveSpeed = 5.5f;
        yield return new WaitForSeconds(16);
        moveSpeed = playerOldMoveSpeed;
        yield return null;
    }

    private IEnumerator MachineGunItemFunction()
    {
        playerShootingScript.fireRate = 0.09f;
        yield return new WaitForSeconds(12);
        playerShootingScript.fireRate = playerOldFireRate;
        yield return null;
    }

    private void NukeItemFunction()
    {
        if (NukeExplosionHandler != null)
            NukeExplosionHandler();
    }

    private IEnumerator WagonWheelItemFunction()
    {
        playerShootingScript.isWagonWheelActive = true;
        yield return new WaitForSeconds(12);
        playerShootingScript.isWagonWheelActive = false;
        yield return null;
    }

    #endregion
}
