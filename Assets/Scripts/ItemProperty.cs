using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemProperty : MonoBehaviour
{
    [SerializeField] int itemLifeTime;
    private float itemStayTime = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyItselfAfterLifeTime());
    }

    void Update()
    {
        if (!PlayerController.isPlayerAlive)
        {
            Destroy(gameObject);
        }    
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(DestroyOnTouch());
        } 
    }

    IEnumerator DestroyItselfAfterLifeTime()
    {
        yield return new WaitForSeconds(itemLifeTime);
        Destroy(gameObject);
    }

    IEnumerator DestroyOnTouch()
    {
        yield return new WaitForSeconds(itemStayTime);
        Destroy(gameObject);
    }
}
