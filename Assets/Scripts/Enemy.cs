using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private readonly int cleanEnemyBodyTime = 6;
    [SerializeField] protected int hitPoints;
    private AIDestinationSetter aiDestinationSetterScript;
    protected AIPath enemyAIPath;
    protected GameObject player;
    private BoxCollider2D enemyCollider;
    protected GameManager gameManagerScript;
    
    // Enemy Child Fields
    private GameObject enemyChild;
    protected Animator enemyChildAnimator;
    private SpriteRenderer enemyChildRenderer;

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
        aiDestinationSetterScript.target = GetTarget();
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

    protected abstract Transform GetTarget();

    // Enemy destroys player on collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject;
            player.SetActive(false);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            HandleAttack();
        }
    }

    private void OnDisable()
    {
        PlayerController.PlayerDeathHandler -= PlayerController_PlayerDeathHandler;
        PlayerController.NukeExplosionHandler -= PlayerController_NukeExplosionHandler;
    }

    protected virtual void HandleAttack()
    {
        hitPoints--;
        if (hitPoints == 0)
        {
            StartCoroutine(OnEnemyDeath());
            gameManagerScript.DropItemIfLucky(transform.position, transform.rotation);
        }
    }

    protected IEnumerator OnEnemyDeath()
    {
        enemyAIPath.enabled = false;
        enemyCollider.enabled = false;
        enemyChildAnimator.SetBool("isDead", true);
        enemyChildRenderer.sortingOrder = 8; //Set child sorting layer under enemie's sorting layer

        yield return new WaitForSeconds(cleanEnemyBodyTime);
        Destroy(gameObject);
    }
}
