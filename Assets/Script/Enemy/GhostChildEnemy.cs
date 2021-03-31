using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostChildEnemy : EnemyCS
{
    public GameObject lazerObj;
    public GameObject lazertargetObj;

    private void Update()
    {

        lazerObj.transform.forward = lazertargetObj.transform.position - this.transform.position;
        lazerObj.transform.localScale = new Vector3(1, 1, ( Vector3.Distance(lazertargetObj.transform.position, this.transform.position) / 2));
    }
    protected override void Hit()
    {
        // 고스트에게 맞았다고 알림
        this.transform.parent.GetComponent<GhostEnemy>().HitChild(this.gameObject);
    }
}
