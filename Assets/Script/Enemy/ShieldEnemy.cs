using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy : EnemyCS
{
    /*
     * 풀에 정보 넘길떄 보호막자식들은 보호막 바로 뒤의 순서로 배치해서 넣어줌
그럼 반복문 돌리면서 폴링하다가 쉴드 오브젝트를 발견
쉴드 오브젝트를 발견하면 쉴드오브젝트 다움 인덱스부터 shieldChild가 아닐때까지 반복문을 돌면서
shieldChild수를 카운트하고
shieldChild에게는 Shield오브젝트 넘겨줌
연산 끝나면 카운트한거 Shield오브젝트에 넘겨주고
마지막 차일드 다음 인덱스 부터 폴링 반복문 실행

     * */

    public GameObject shield;

    public int childCount;

    float childsDeadCount;

    private void Awake()
    {
        childCount = 0;
    }

    public void AddChildDead()
    {
        childsDeadCount++;

        // 사망한 자식의 갯수가 자식 전체의 갯수와 같다면
        if (childsDeadCount == childCount)
        {
            // 쉴드 파괴
            shield.SetActive(false);
        }
    }


    protected override void Hit()
    {
        // 쉴드가 비활성화 된 상태이면 사망 처리
        if(!shield.activeSelf)
            ObjectPoolManager.inst.ReturnObjectToPool("Shield", this.gameObject);
    }

}