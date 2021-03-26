using UnityEngine;
using System.Collections.Generic;


// 개체 풀의 기본 속성
// 현재 개체 풀의 이름, 프리 팹 및 크기를 저장

[System.Serializable]
public class ObjectPool
{
    public string Name;
    public GameObject prefab;
    public int Size = 5;
}

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager inst;

    // ObjectPool 목록    
    public List<ObjectPool> ObjectPoolList;

    // 이름으로 모든 개체 풀을 저장
    public Dictionary<string, Queue<GameObject>> enemyObjectPool;

    // 스테이지에 생성된 오브젝트들을 담아둘 변수
    private List<GameObject> stageEnemyList;

    public GameObject playerControler;

    // 쉴드 오브젝트를 담아둘 변수 (쉴드 차일드에게 넣어주기 위함)
    private GameObject shieldEnemy;

    // 개체 풀을 초기화합니다.여기서 개체는 인스턴스화되고 각 개체 풀에 저장됨
    private void Awake()
    {

        if (!inst)
        {
            inst = this;
        }

        // 풀 초기화
        enemyObjectPool = new Dictionary<string, Queue<GameObject>>();
        stageEnemyList = new List<GameObject>();

        // Inspector창에서 받아온 정보를 풀에 대입시켜주는 작업
        foreach (ObjectPool pool in ObjectPoolList)
        {
            //  각 개체 풀에 대해 빈 부모 개체를 만듭니다. DontDestroyOnLad에 생성             
            GameObject poolParentObj = new GameObject();
            poolParentObj.name = pool.Name + "Pool";

            GameObject.DontDestroyOnLoad(poolParentObj);

            // 개체 풀을 만들고 여기에 개체 저장
            Queue<GameObject> poolQueue = new Queue<GameObject>();

            for (int i = 0; i < pool.Size; i++)
            {
                GameObject obj = GameObject.Instantiate(pool.prefab);
                obj.SetActive(false);
                obj.transform.parent = poolParentObj.transform;

                // 큐에 추가
                poolQueue.Enqueue(obj);
            }
            // 개체 풀 추가  
            enemyObjectPool.Add(pool.Name, poolQueue);
        }
    }


    // 개체 생성 함수 (꺼내 쓰기)
    // 개체 풀에서 개체 가져 오기 개체의 위치 및 회전을 설정.    

    // StageManager가 이 함수를 호출
    public void ActiveStageEnemyInPool(EnemySpawn[] _stageEnemyInfo)
    {

        for (int count = 0; count < _stageEnemyInfo.Length; count++)
        {
            switch (_stageEnemyInfo[count].enemyType)
            {

                case EnemySpawn.EnemyType.Normal:
                    GetObjectFromPool("Normal", _stageEnemyInfo[count].enemySpawnPos, _stageEnemyInfo[count].enemySpawnRot);
                    break;

                case EnemySpawn.EnemyType.Move:
                    GetObjectFromPool("Move", _stageEnemyInfo[count].enemySpawnPos, _stageEnemyInfo[count].enemySpawnRot);

                    break;

                case EnemySpawn.EnemyType.Tank:
                    GetObjectFromPool("Tank", _stageEnemyInfo[count].enemySpawnPos, _stageEnemyInfo[count].enemySpawnRot);

                    break;

                case EnemySpawn.EnemyType.Shiled:
                    GetObjectFromPool("Shield", _stageEnemyInfo[count].enemySpawnPos, _stageEnemyInfo[count].enemySpawnRot);

                    break;

                case EnemySpawn.EnemyType.ShiledChild:
                    GetObjectFromPool("ShieldChild", _stageEnemyInfo[count].enemySpawnPos, _stageEnemyInfo[count].enemySpawnRot);
                    break;

                case EnemySpawn.EnemyType.Ghost:
                    GetObjectFromPool("Ghost", _stageEnemyInfo[count].enemySpawnPos, _stageEnemyInfo[count].enemySpawnRot);

                    break;
            }
        }
    }

    // 생성 예제
    //ex). ObjectPoolManager.GetObjectFromPool("Name", Vector3.Position , Quaternion.identity);
    //                                                                  ("이름", 위치, 회전값);
    public void GetObjectFromPool(string _poolName, Vector3 _position, Quaternion _rotation) // 이름, 위치, 회전값
    {

        if (enemyObjectPool.ContainsKey(_poolName))
        {
            if (enemyObjectPool[_poolName].Count > 0)
            {
                GameObject obj = enemyObjectPool[_poolName].Dequeue();
                obj.SetActive(true);
                obj.transform.position = _position;
                obj.transform.rotation = _rotation;


                // 예외처리가 필요한 쉴드와 쉴드 차일드를 예외처리 해줌
                switch (_poolName)
                {
                    case "Shield":
                        shieldEnemy = obj;
                        break;
                    case "ShieldChild":
                        // 쉴드 자식에게 쉴드 에너미를 넣어줌
                        obj.GetComponent<ShieldChildEnemy>().ShieldObj = shieldEnemy;

                        // 쉴드 에너미의 자식 수를 올려줌
                        shieldEnemy.GetComponent<ShieldEnemy>().childCount++;
                        break;
                }

                // 부메랑이 아니면
                if(_poolName != "Boomerang")
                // 스테이지리스트에 스테이지에 생성된 에너미 추가
                    stageEnemyList.Add(obj);
            }
            else
            {
                Debug.Log(_poolName + "더 이상 없음");
            }
        }
        else
        {
            Debug.Log(_poolName + " 의 오브젝트 풀을 사용할 수 없음. (1# 이름이 다를 수 있음. 2#풀에 등록이 안돼 있음. 3# 생성 함수 확인");
        }
    }

    // 개체를 반환하는 함수.
    // 다시 오브젝트 풀로 돌려보냄.
    public void ReturnObjectToPool(string poolName, GameObject poolObject)
    {
        if (enemyObjectPool.ContainsKey(poolName))
        {
            enemyObjectPool[poolName].Enqueue(poolObject);
            poolObject.SetActive(false);
        }
        else
        {
            Debug.Log(poolName + "의 오브젝트 풀을 사용할 수 없음. (1# 이름이 다를 수 있음. 2# 반환 함수 확인");
        }

        // 에너미가 한마리씩 죽을때마다 체크
        CheckClearStage();
    }

    // 스테이지를 클리어 했는지 검사
    void CheckClearStage()
    {
        bool isClear = true;

        foreach (GameObject gCount in stageEnemyList)
        {
            if (gCount.activeSelf)
            {
                isClear = false;
            }
        }

        if (isClear)
        {
            // 스테이지 에너미들 다 삭제
            stageEnemyList.Clear();

            // 다음 스테이지로 
            StageManager.inst.ClearStage();
        }
    }
}
