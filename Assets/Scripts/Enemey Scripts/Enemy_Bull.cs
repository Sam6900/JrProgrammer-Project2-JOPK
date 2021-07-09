using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bull : Enemy
{
    protected override Transform GetTarget()
    {
        return player.GetComponent<Transform>();
    }
}
