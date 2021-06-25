using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public int playerLifeCount;
    private int minLifeCount = 0;
    [SerializeField] int moneyCount;
    float playerOldMoveSpeed;
    float playerOldFireRate;

    private Vector2 moveDir;
    private Vector2 playerInitialPos = new Vector2(0.5f, 0);

    public static bool isPlayerAlive = true;

    private Rigidbody2D rb;
    private Animator animator;
    private GameManager gameManagerScript;
    private Shooting playerShootingScript;

    public static event Action PlayerDeathHandler;

    public static event Action NukeExplosionHandler;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        playerShootingScript = GetComponent<Shooting>();

        gameManagerScript.UpdatePlayerLifeCount(playerLifeCount);
        playerOldMoveSpeed = moveSpeed;
        playerOldFireRate = playerShootingScript.fireRate;
    }

    // Update is called once per frame
    void Update()
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
        if ( Shooting.isShooting == false)
        {
            animator.SetFloat("Horizontal", moveX);
            animator.SetFloat("Vertical", moveY);
            animator.SetFloat("Speed", moveDir.sqrMagnitude);
        }
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

    void OnDisable()
    {
        isPlayerAlive = false;

        if (playerLifeCount > minLifeCount)
        {
            playerLifeCount -= 1;
            gameManagerScript.UpdatePlayerLifeCount(playerLifeCount);
        }
        else if (playerLifeCount == minLifeCount) // Sets actual life count under 0 to prevent player spawning on death
        {
            playerLifeCount -= 1;
        }
        
        if (playerLifeCount >= -1)
        {
            if (PlayerDeathHandler != null)
                    PlayerDeathHandler();
        }
    }

    void OnEnable()
    {
        isPlayerAlive = true;
        transform.position = playerInitialPos;
        Shooting.isShooting = false;
    }


    #region Item's functionality

    public void CoinX1Function()
    {
        moneyCount += 1;
        Debug.Log(moneyCount);
    }

    public void CoinX5Function()
    {
        moneyCount += 5;
    }

    public void LifeItemFunction()
    {
        playerLifeCount += 1;
        gameManagerScript.UpdatePlayerLifeCount(playerLifeCount);
    }

    public IEnumerator TeaItemFunction()
    {
        moveSpeed = 5.5f;
        yield return new WaitForSeconds(16);
        moveSpeed = playerOldMoveSpeed;
        yield return null;
    }

    public IEnumerator MachineGunItemFunction()
    {
        playerShootingScript.fireRate = 0.09f;
        yield return new WaitForSeconds(12);
        playerShootingScript.fireRate = playerOldFireRate;
        yield return null;
    }

    public void NukeItemFunction()
    {
        if (NukeExplosionHandler != null)
            NukeExplosionHandler();
    }

    public IEnumerator WagonWheelItemFunction()
    {
        playerShootingScript.isWagonWheelActive = true;
        yield return new WaitForSeconds(12);
        playerShootingScript.isWagonWheelActive = false;
        yield return null;
    }

# endregion
}
