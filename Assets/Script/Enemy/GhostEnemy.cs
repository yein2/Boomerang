using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEnemy : EnemyCS
{
    // Start is called before the first frame update

    // 고스트 에너미의 자식들을 받아올 배열
    public GameObject[] ghostChild;

    // 고스트 에너미의 차일드들이 순차적으로 맞았는지 검사해줄 변수
    private int childHitCount;

    // 첫번째 오브젝트가 맞았는지
    private bool isHIt;


    // 고스트를 맞췄는지 몇초후인지 시간값을 가지고 있는 변수
    private float checkTime;

    // 시간 누적값을 저장해줄 변수
    private float timeCount;

    private void Start()
    {
        //초기화
        Init();
    }

    private void Update()
    {

        // 첫번째 고스트가 맞은 상태면
        if (isHIt)
        {
            // 시간값 더해줌
            timeCount += Time.deltaTime;

            // 지정한 시간이 되면
            if (timeCount > checkTime)
            {
                // Reset
                Init();
                ResetGhostEnemy();
            }
        }
    }

    // 초기화 변수
    public void Init()
    {

        isHIt = false;

        childHitCount = 0;

        // 시간 변수 초기화
        timeCount = 0f;
        checkTime = 5f;
    }

    // 자식이 맞았을때 호출하는 함수
    public void HitChild(GameObject _childObj)
    {
        // 첫번쨰 에너미가 맞은 상태면
        if (isHIt)
        {
            // 지금 맞았다고 호출한 자식의 순서가 문제없다면
            if (ghostChild[childHitCount] == _childObj)
            {
                // 자식을 죽이고
                _childObj.SetActive(false);

                // 검사 인덱스 변수 증가
                childHitCount++;

                // 검사 인덱스의 값이 자식 배열의 길이와 같을 경우 (더 이상 계산할 자식이 없다. 모든 자식이 죽었다.)
                if (ghostChild.Length == childHitCount)
                {
                    ObjectPoolManager.inst.ReturnObjectToPool("Ghost", this.gameObject);
                }
            }
        }
    }

    // 못 맞췄을경우 리셋해줌
    public void ResetGhostEnemy()
    {
        // 반복문 돌면서 죽은 자식들 다시 살려줌
        for (int i = 0; i < ghostChild.Length; i++)
        {
            // 자식이 비활성화 되어있으면
            if (!ghostChild[i].activeSelf)
                ghostChild[i].SetActive(true);
        }
    }


    protected override void Hit()
    {
        isHIt = true;
    }
}