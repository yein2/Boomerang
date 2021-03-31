using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemy : EnemyCS
{
    protected override void Hit()
    {
        ObjectPoolManager.inst.ReturnObjectToPool("Tutorial", this.gameObject);
    }
}
