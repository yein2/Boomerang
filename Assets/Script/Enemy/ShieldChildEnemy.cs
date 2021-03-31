using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldChildEnemy : EnemyCS
{
    // Start is called before the first frame update
    // 쉴드 에너미 오브젝트를 받아옴
    public GameObject shieldObj;
    public GameObject shieldLazer;

    private void Update()
    {
        shieldLazer.transform.forward = shieldObj.transform.position - this.transform.position;
        shieldLazer.transform.localScale = new Vector3(1,  1, Vector3.Distance(this.transform.position, shieldObj.transform.position));
    }

    protected override void Hit()
    {
        // 쉴드 에너미의 자식 사망처리 함수를 호출
        shieldObj.GetComponent<ShieldEnemy>().AddChildDead();

        // 본인 게임 오브젝트 비활성화
        ObjectPoolManager.inst.ReturnObjectToPool("ShieldChild", this.gameObject);
    }
}
