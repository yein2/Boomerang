using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveEnemy : EnemyCS
{
    private float speed;
    private float speedWithTime;

    private float length;

    private Vector3 centerPos;



    private void Start()
    {
        // 위아래로 이동하는 속도
        speed = 2f;

        // 위 아래로 이동하는 폭
        length = 1f;

        // 속도와 시간값을 누적시켜줄 변수
        speedWithTime = 0;

        // 사인 함수의 중심이 될 좌표
        centerPos = this.transform.position;

    }

    private void Update()
    {

        // 시간값과 스피드값을 곱해서 누적시켜줌
        speedWithTime += speed * Time.deltaTime;

        // 중심 좌표를 기준으로 length만큼의 폭을 speedwithTime만큼의 속도로 이동
        this.transform.position = centerPos + new Vector3(0, Mathf.Sin(speedWithTime) * length, 0);
    }
    protected override void Hit()
    {
        ObjectPoolManager.inst.ReturnObjectToPool("Move", this.gameObject);
    }

}