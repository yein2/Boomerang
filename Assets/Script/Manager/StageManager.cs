using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    private void Awake()
    {
        if (!inst)
            inst = this;

        StageEnemyInfo = new EnemySpawn[5];

        stageNum = 0;
    }

    public void ClearStage()
    {
        stageNum++;

        Invoke("SetStage", 1.0f);
    }

    public void FailStage()
    {
        // 게임오버 Ui

        // 메인 화면으로
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
        StageEnemyInfo[0].enemySpawnPos = new Vector3(-1.2f, 0, 6.44f);
        StageEnemyInfo[0].enemySpawnRot = Quaternion.LookRotation(Vector3.zero);

    }
    void Stage1_2()
    {
        StageEnemyInfo = new EnemySpawn[1];

        StageEnemyInfo[0].enemyType = EnemySpawn.EnemyType.Normal;
        StageEnemyInfo[0].enemySpawnPos = new Vector3(-1.2f, 0, 6.44f);
        StageEnemyInfo[0].enemySpawnRot = Quaternion.LookRotation(Vector3.zero);

    }
    void Stage1_3()
    {
        StageEnemyInfo = new EnemySpawn[2];

        StageEnemyInfo[0].enemyType = EnemySpawn.EnemyType.Normal;
        StageEnemyInfo[0].enemySpawnPos = new Vector3(-1.2f, 2, 6.44f);
        StageEnemyInfo[0].enemySpawnRot = Quaternion.LookRotation(Vector3.zero);

        StageEnemyInfo[1].enemyType = EnemySpawn.EnemyType.Tutorial;
        StageEnemyInfo[1].enemySpawnPos = new Vector3(-1.2f, 0, 6.44f);
        StageEnemyInfo[1].enemySpawnRot = Quaternion.LookRotation(Vector3.zero);
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


    }
}
