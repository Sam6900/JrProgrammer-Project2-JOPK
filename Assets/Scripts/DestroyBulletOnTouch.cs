using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBulletOnTouch : MonoBehaviour
{
    //Destroys bullet on leaving area
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
        }
    }
}
