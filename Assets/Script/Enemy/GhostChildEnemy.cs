using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostChildEnemy : EnemyCS
{
    
    protected override void Hit()
    {
        // 고스트에게 맞았다고 알림
        this.transform.parent.GetComponent<GhostEnemy>().HitChild(this.gameObject);
    }
}
