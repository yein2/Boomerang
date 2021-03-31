﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Enemy의 Spawn관련 정보값을 담을 구조체
public struct EnemySpawn
{
    public enum EnemyType {Tutorial, Normal, Move, Shiled, ShiledChild, Ghost, Tank }
    public EnemyType enemyType;
    public Vector3 enemySpawnPos;
    public Quaternion enemySpawnRot;
}

public class StageManager : MonoBehaviour
{
    public static StageManager inst;

    

    // Stage 내부의 에너미 정보값을 받아올 배열
    EnemySpawn[] StageEnemyInfo;

    // 현재 진행중인 스테이지의 넘버를 기록할 변수
    private int stageNum;



    // ----- 타이머 ----- //
    public Text timerText; // 타이머를 표시할 텍스트
    
    [SerializeField]
    private float timer; // 타이머
    private float timer_Start = 60f; // 시작 타이머 값
    private float timer_End = 0; // 타이머 종료 값
    public bool isEnable_Timer; // 타이머 활성화 / 비활성화


    private void Awake()
    {
        if (!inst)
            inst = this;

        StageEnemyInfo = new EnemySpawn[5];

        stageNum = 0;
    }
    
    private void Start() {
        timer = timer_Start; // 시작 타이머 값 
    }
    private void Update() 
    {
        PlayTimer();   // isEnable_Timer 가 true 일때 타이머 감소
    }
    

    // ------ 타이머 ------ //
    public void SetTimer() // 타이머 초기화 함수 // 스테이지가 끝났을 때 발동
    {
        isEnable_Timer = false;
        timer = timer_Start;
    }

    public void StopTimer() // 타이머 멈춤
    {
        isEnable_Timer = false;
    }

    private void PlayTimer() // 타이머를 작동시키는 함수
    {
        if(isEnable_Timer)
        {
            if(timer > timer_End)
            {
                // 타이머 감소
                timer -= Time.deltaTime;
                // 타이머 텍스트 출력 
                timerText.text = Mathf.Ceil(timer).ToString();            
            }
            else if(timer <= timer_End)
            {
                timer = timer_End; // 타이머를 0으로 표기 (사실 안해도 상관없음)
                StopTimer(); // 타이머를 멈춤

                // FailStage() 함수 호출 / 게임오버 호출

                Debug.Log("게임 오버!");
            }
        }
        
    }
    


    // ------ 스테이지 ------ //
    public void ClearStage()
    {
        // UI 완성하면 지우고 스테이지 넘기는 부분 NextStage()로 바꾸세용
        stageNum++;

        Invoke("SetStage", 3.0f);

        // 클리어 UI 띄우는 곳

    }

    public void NextStage()
    {
        stageNum++;
    }

    public void FailStage()
    {
        // 게임오버 Ui

        // 메인 화면으로 ?

        
    }

    void SetStage()
    {
        switch (stageNum)
        {
            case 1:
                Stage1_1();
                break;
            case 2:
                Stage1_2();
                break;
            case 3:
                Stage1_3();
                break;
            case 4:
                Stage2();
                break;
            case 5:
                break;
        }

        // 메모리풀에 넘겨줌
        ObjectPoolManager.inst.ActiveStageEnemyInPool(StageEnemyInfo);
    }

    // 스테이지 작성 규칙
    // 1. 소스의 에너미 종류별 순서는 Normal - Move - Tank - Shield - ShieldChild - Ghost 순으로 한다.
    

    void Stage1_1()
    {
        StageEnemyInfo = new EnemySpawn[1];

        StageEnemyInfo[0].enemyType = EnemySpawn.EnemyType.Normal;
        StageEnemyInfo[0].enemySpawnPos = new Vector3(0, 0, 6.44f);
        StageEnemyInfo[0].enemySpawnRot = Quaternion.LookRotation(Vector3.zero);

        ScoreManager.inst.PresetClearScore(1);

        isEnable_Timer = true; // 타이머 상태 활성화
    }
    void Stage1_2() // 리턴 유도
    {
        StageEnemyInfo = new EnemySpawn[2];
    
        StageEnemyInfo[0].enemyType = EnemySpawn.EnemyType.Normal;
        StageEnemyInfo[0].enemySpawnPos = new Vector3(3.7f, 0, 8.34f);
        StageEnemyInfo[0].enemySpawnRot = Quaternion.LookRotation(Vector3.zero);

        StageEnemyInfo[1].enemyType = EnemySpawn.EnemyType.Tutorial;
        StageEnemyInfo[1].enemySpawnPos = new Vector3(6.3f, 0, 13.1f);
        StageEnemyInfo[1].enemySpawnRot = Quaternion.LookRotation(Vector3.zero);

        ScoreManager.inst.PresetClearScore(2);

        isEnable_Timer = true; // 타이머 상태 활성화
    }
    void Stage1_3() // 방향 회전(간섭) 유도 
    {
        StageEnemyInfo = new EnemySpawn[2];

        StageEnemyInfo[0].enemyType = EnemySpawn.EnemyType.Normal;
        StageEnemyInfo[0].enemySpawnPos = new Vector3(0, 3, 6.44f);
        StageEnemyInfo[0].enemySpawnRot = Quaternion.LookRotation(Vector3.zero);

        StageEnemyInfo[1].enemyType = EnemySpawn.EnemyType.Tutorial;
        StageEnemyInfo[1].enemySpawnPos = new Vector3(0, 0, 8.4f);
        StageEnemyInfo[1].enemySpawnRot = Quaternion.LookRotation(Vector3.zero);

        ScoreManager.inst.PresetClearScore(2);

        isEnable_Timer = true; // 타이머 상태 활성화
    }

    void Stage2() 
    {
        StageEnemyInfo = new EnemySpawn[6];

        StageEnemyInfo[0].enemyType = EnemySpawn.EnemyType.Normal;
        StageEnemyInfo[0].enemySpawnPos = new Vector3(-1.2f, 0, 6.44f);
        StageEnemyInfo[0].enemySpawnRot = Quaternion.LookRotation(Vector3.zero);

        StageEnemyInfo[1].enemyType = EnemySpawn.EnemyType.Move;
        StageEnemyInfo[1].enemySpawnPos = new Vector3(-0.05f, 0, -3.96f);
        StageEnemyInfo[1].enemySpawnRot = Quaternion.LookRotation(Vector3.zero);


        StageEnemyInfo[2].enemyType = EnemySpawn.EnemyType.Tank;
        StageEnemyInfo[2].enemySpawnPos = new Vector3(-3.75f, 0, 0.1f);
        StageEnemyInfo[2].enemySpawnRot = Quaternion.LookRotation(Vector3.zero);

        StageEnemyInfo[3].enemyType = EnemySpawn.EnemyType.Shiled;
        StageEnemyInfo[3].enemySpawnPos = new Vector3(3.62f, 0, -3.25f);
        StageEnemyInfo[3].enemySpawnRot = Quaternion.LookRotation(Vector3.zero);

        StageEnemyInfo[4].enemyType = EnemySpawn.EnemyType.ShiledChild;
        StageEnemyInfo[4].enemySpawnPos = new Vector3(7f, 0, -3.25f);
        StageEnemyInfo[4].enemySpawnRot = Quaternion.LookRotation(Vector3.zero);

        StageEnemyInfo[5].enemyType = EnemySpawn.EnemyType.Ghost;
        StageEnemyInfo[5].enemySpawnPos = new Vector3(6.39f, 0, 9.35f);
        StageEnemyInfo[5].enemySpawnRot = Quaternion.Euler(0, -138f, 0);

        ScoreManager.inst.PresetClearScore(6);
        ScoreManager.inst.ResetScore();
        isEnable_Timer = true; // 타이머 상태 활성화
    }
}
