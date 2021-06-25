using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollisionWithEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Ignores collision between enemies and entry points
        Physics2D.IgnoreLayerCollision(8, 9);
    }
}
