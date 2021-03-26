using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Boomr : MonoBehaviour
{
    // 구현해볼 것

    bool gansub;

    // 부메랑 모델을 받아올 게임 오브젝트 (따로 회전시키기 위해)
    public GameObject model;

    // 플레이어 컨트롤러를 받을 변수
    public GameObject controller;

    // 베지어 곡선 포인트 
    private Vector3 Point1;
    private Vector3 Point2;
    private Vector3 Point3;
    private Vector3 Point4;

    // 부메랑의 Forward를 설정하기 위한 변수
    private Vector3 prePos;


    // 던져진 상태인지
    public bool isThrow;

    // 회전을 완료하는데 걸리는 시간
    private float MoveSpeed;

    // 베지오 곡선 진행값을 담을 변수
    private float bezierCounter;


    void Start()
    {
        // 부메랑 초기화
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        // 던진 상태면 움직임
        if (isThrow)
        {
            // 긴급회귀 버튼을 누르면 회귀
            if (Input.GetKeyDown(KeyCode.H))
            {
                Return();
            }

            // 간섭 구현 (베이저 카운터가 1 이상이면 정상적 이동중이지 않은 떄 간섭을 이미 사용하지 않았다면
            if (Input.GetKeyDown(KeyCode.Z) && bezierCounter < 1 && !gansub)
            {
                gansub = true;
            }
            // 간섭 구현 (베이저 카운터가 1 이상이면 정상적 이동중이지 않은 떄 // 나중에 업다운으로 바꾸면 && gansub 뺴도 됨
            if (Input.GetKeyDown(KeyCode.X) && gansub)
            {
                gansub = false;


                bezierCounter = 2;

                StartCoroutine("Gansub");
            }

            if (!gansub)
            {
                Move();
                RotateModel(5f);
            }
        }
    }

    public void SetBezierPoint(Vector3 _p1, Vector3 _p2, Vector3 _p3, Vector3 _p4)
    {
        Point1 = _p1;
        Point2 = _p2;
        Point3 = _p3;
        Point4 = _p4;
    }

    public void Throw()
    {
        // 던짐 여부를 True로 해서 부메랑이 움직이도록
        isThrow = true;

        MoveSpeed = 5f;
        bezierCounter = 0;
        prePos = this.transform.position;
    }

    // 부메랑이 잡혔을떄 호출되는 함수
    public void Init()
    {
        gansub = false;
        // 던짐 여부 비 활성화

        isThrow = false;

        StopCoroutine("Gansub");

    }

    // 움직이는 함수
    void Move()
    {
        if (bezierCounter < 1)
        {

            // RotTime값만큼의 초가 되면 _time값이 1이 됨
            bezierCounter += Time.deltaTime / MoveSpeed;

            // 부메랑의 목표지는 플레이어의 컨트롤러를 향하도록
            //Point4 = controller.transform.position;


            this.transform.position = MathCS.Bezier(Point1, Point2, Point3, Point4, bezierCounter);

            this.transform.forward = this.transform.position - prePos;

            prePos = this.transform.position;
        }
        else
        {
            if (bezierCounter != 2)
            {
                // 컨트롤러가 널이 아니고 만약 플레이어와 부메랑의 거리가 50 멀어졌다면
                if (controller && Vector3.Distance(this.transform.position, Point4) > 50f)
                {
                    controller.GetComponent<BoomrContrlCS>().CreateBoomr();
                    Invoke("DestroyBoomr", 2.0f);

                    // 컨트롤러에 널값을 넣어줌 (생성코드 1번만 실행되기 위해서
                    controller = null;
                }
            }

            if(bezierCounter == 2)
            {
                // lerpSet이 마지막 분기점이면 도착했는지 검사해줌
                if (Vector3.Distance(this.transform.position, Point4) < 0.1f)
                {
                    // 도착했을시 쭉 직진하기위해 bezierCounter값을 1로 설정
                    bezierCounter = 1;
                }
            }

            // 진행방향으로 쭉 날라감
            this.transform.position += this.transform.forward * (MoveSpeed * 2) * Time.deltaTime;
        }

    }

    void DestroyBoomr()
    {
        ObjectPoolManager.inst.ReturnObjectToPool("Boomerang", this.gameObject);
    }

    void RotateModel(float _speed)
    {
        // 부메랑 회전
        model.transform.Rotate(0, _speed, 0);
    }
    // 긴급 회귀
    void Return()
    {
        // 궤적을 따라가지 않도록 설정
        bezierCounter = 1;

        this.transform.forward = controller.transform.position - this.transform.position;
    }


    // 간섭
    IEnumerator Gansub()
    {
        // 출발지를 받아둠 | 도착지는 Point4
        Transform originPos = this.transform;

        // 구간별 러프 출발지
        Transform prePos = this.transform;

        // 현재 부메랑과 도착지의 거리값을 넣고
        float distance = Vector3.Distance(this.transform.position, Point4);

        // 거리값만큼 쪼개줌 (거리값이 2라면 dis에 0.5가 들어감)
        distance = 1 / distance;

        // 러프를 카운트해줄 변수
        float lerpCount = 0;

        // 다음 분기점 값을 담을 변수 변수 ex) 1. ( 0 ~ lerpSet(0.3))- lerpSet + distance - ( 0.3 ~ lerpSet (0.6 ) ~
        float lerpSet = distance;

        // 무한 루프 돌면서
        while (true)
        {
            
            // 일정거리 가까워 졌다면 코루틴 종료
            if(Vector3.Distance(this.transform.position, Point4) < 0.5f)
            {
                this.transform.forward = Point4 - this.transform.position;
                bezierCounter = 1;
                StopCoroutine("Gansub");
            }

            // Lerp의 첫번째 인자 값은 구간별 출발점이고 
            //                      두번째 인자값의 러프는 첫번재 인자값은 맨처음 출발점의 Forward와 출발점에서 도착치로 향하는 벡터를 러프 연산해서 분기별 도착지를 리턴해줌
            // 결과적으로는 처음이라면 Vector3.Slerp(0, 0.3, lerpCount) 두번째에는 Vector3.Slerp(0.3, 0.6, lerpCount)식으로 진행됨
            this.transform.forward = Vector3.Slerp(prePos.forward, Vector3.Slerp(originPos.forward, Point4 - originPos.position, lerpSet), lerpCount);

            // 러프카운트 누적합
            lerpCount += 0.05f;

            // 만약 분기 회전이 끝났다면 다음 분기로 값을 세팅해줌
            if (lerpCount >= 1)
            {
                lerpCount = 0;

                // 분기별 시작 Forward에 현재의 Forward 넣어줌
                prePos.forward = this.transform.forward;
                
                // 둘을 더하면 1을 넘는지
                if(lerpSet + distance < 1)
                {
                    // 도착값을 다음 분기 값으로 세팅
                    lerpSet += distance;
                }
                else
                {
                    // 넘으면 1로
                    lerpSet = 1;
                }
            }

            yield return new WaitForSeconds(0.01f);
        }
    }

    // 베지어 점과 점 Lerp 연결

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.tag == "Shield")
        {
           // 간섭이 아닐때 이렇게 실행해주고
            bezierCounter = 1 - bezierCounter;

            Vector3 temp = Point1;
            Point1 = Point4;
            Point4 = temp;

            temp = Point2;
            Point2 = Point3;
            Point3 = temp;
            // 간섭중이면 다르게 구현해야함
        }
    }
}
