using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int cleanEnemyBodyTime = 6;

    private AIDestinationSetter aiDestinationSetterScript;
    private AIPath enemyAIPath;
    private GameObject player;
    private PlayerController playerControllerScript;
    private BoxCollider2D enemyCollider;
    private GameManager gameManagerScript;
    
    // Enemy Child Fields
    private GameObject enemyChild;
    private Animator enemyChildAnimator;
    private SpriteRenderer enemyChildRenderer;

    private enum EnemyState{
        Walk,
        Dead
    }
    EnemyState enemyState;

    /*
    Getting all Components in awake and start
    */
    void Awake()
    {       
        // Sets enemy AI destination on Player
        aiDestinationSetterScript = GetComponent<AIDestinationSetter>();
        player = GameObject.Find("Player");
        enemyAIPath = GetComponent<AIPath>();
        enemyCollider = GetComponent<BoxCollider2D>();

        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    void Start()
    {
        aiDestinationSetterScript.target = player.GetComponent<Transform>();
        enemyState = EnemyState.Walk;
        enemyChild = transform.GetChild(0).gameObject;
        enemyChildRenderer = enemyChild.GetComponent<SpriteRenderer>();
        enemyChildAnimator = enemyChild.GetComponent<Animator>();

        PlayerController.PlayerDeathHandler += PlayerController_PlayerDeathHandler;
        PlayerController.NukeExplosionHandler += PlayerController_NukeExplosionHandler;
    }

    private void PlayerController_NukeExplosionHandler()
    {
        StartCoroutine(OnEnemyDeath());
    }

    private void PlayerController_PlayerDeathHandler()
    {
        Destroy(gameObject);
    }

    private void Update()
    {

    }

    // Enemy destroys player on collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && enemyState == EnemyState.Walk)
        {
            player = collision.gameObject;
            player.SetActive(false);
        }
    }

    // On bullet hit do all importatant stuff and play enemy death anim
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            gameManagerScript.DropItemIfLucky(transform.position, transform.rotation);
            Destroy(collision.gameObject);
            StartCoroutine(OnEnemyDeath());
        }
    }

    private void OnDisable()
    {
        PlayerController.PlayerDeathHandler -= PlayerController_PlayerDeathHandler;
        PlayerController.NukeExplosionHandler -= PlayerController_NukeExplosionHandler;
    }

    private IEnumerator OnEnemyDeath()
    {
        enemyAIPath.enabled = false;
        enemyCollider.enabled = false;
        enemyChildAnimator.SetBool("isDead", true);
        enemyChildRenderer.sortingOrder = 8; //Set child sorting layer under enemie's sorting layer
        enemyState = EnemyState.Dead;

        yield return new WaitForSeconds(cleanEnemyBodyTime);
        Destroy(gameObject);
    }
}
