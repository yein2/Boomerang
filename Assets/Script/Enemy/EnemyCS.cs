using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCS : MonoBehaviour
{


    // 가상 함수 (자식들이 오버라이드 할수 있도록)
    protected virtual void Hit()
    {
        ObjectPoolManager.inst.ReturnObjectToPool("Normal", this.gameObject);
    }

    
    private void OnTriggerEnter(Collider _other)
    {
        if (_other.tag == "Boomr")
        {
            Hit();
        }
    }
}
