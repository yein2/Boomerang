using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEnemy : EnemyCS
{
    private float Hp = 2;

    protected override void Hit()
    {
        Hp--;
        if (Hp == 0)
        {
            ObjectPoolManager.inst.ReturnObjectToPool("Tank", this.gameObject);
        }
    }
}