using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Thorn : Enemy
{
    [SerializeField] private Transform targetTransf;
    private bool isSetup = false;

    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, targetTransf.position) < 0.2f)
        {
            if (isSetup == false)
            {
                enemyChildAnimator.SetBool("isSetup", true);
                hitPoints = 3;
                isSetup = true;
            }
        }
    }

    protected override Transform GetTarget()
    {
        int RandPosX = Random.Range(-6, 7);
        int RandPosY = Random.Range(-6, 7);

        targetTransf.position = new Vector2(RandPosX, RandPosY);
        return targetTransf;
    }

    protected override void HandleAttack()
    {
        hitPoints--;
        if (hitPoints == 0)
        {
            if (isSetup == true)
            {
                StartCoroutine(OnEnemyDeath());
            }
            else if (isSetup == false)
            {
                StartCoroutine(OnEnemyDeath());
                gameManagerScript.DropItemIfLucky(transform.position, transform.rotation);
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject;
            player.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        Destroy(transform.parent.gameObject);
    }
}
