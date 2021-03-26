using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Boomr : MonoBehaviour
{

    // 간섭에 사용될 변수들의 구조체 선언
    public struct Twist
    {
        public Transform originPos;

        public Transform prePos;

        public float distance;

        public float lerpCount;

        public float lerpFinal;
    }

    // 구조체 변수 선언
    private Twist twist;

    public enum BoomrState { Normal, Twist, Return, Stop, WaitDestroy, OnDestroy };
    BoomrState boomrState;

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

    private Quaternion prerot;


    // 던져진 상태인지
    public bool isThrow;

    // 회전을 완료하는데 걸리는 시간
    private float MoveSpeed;

    // 베지오 곡선 진행값을 담을 변수 0 ~ 0.999.. : 기본 회전 운동, 1 = 간섭 운동, 2 = 회전 끝난후 Dstroy
    private float bezierCounter;

    private float timeSet;
    private float timeCounter;





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
                boomrState = BoomrState.Return;
                this.transform.forward = controller.transform.position - this.transform.position;
            }

            // 간섭 구현 (베이저 카운터가 1 이상이면 정상적 이동중이지 않은 떄 간섭을 이미 사용하지 않았다면
            if (Input.GetKeyDown(KeyCode.Z) && bezierCounter < 1 && boomrState != BoomrState.Twist)
            {
                // 움직임을 멈추기 위해
                boomrState = BoomrState.Stop;

                prerot = this.transform.rotation;
            }

            if (boomrState == BoomrState.Stop)
            {
                transform.rotation = controller.transform.rotation * prerot;
            }

            // 간섭 구현  나중에 업다운으로 바꾸면 && && bezierCounter 뺴도 됨
            if (Input.GetKeyDown(KeyCode.X) && boomrState == BoomrState.Stop)
            {
                boomrState = BoomrState.Twist;
                GansubInit(this.transform, Point4);
            }

            if (boomrState != BoomrState.Stop)
            {
                Move();

                // 부메랑  모델 회전
                model.transform.Rotate(10, 0, 0);
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
        boomrState = BoomrState.Normal;

        timeSet = 0.01f;
        timeCounter = 0;

        // 던짐 여부 비 활성화
        isThrow = false;
    }


    // 움직이는 함수
    void Move()
    {
        // Normal은 제외한 BoomrStage들은 모두 직선으로 이동
        if (boomrState != BoomrState.Normal)
        {
            this.transform.position += this.transform.forward * (MoveSpeed * 2) * Time.deltaTime;
        }

        // State별 기능 처리 부분
        switch (boomrState)
        {
            case BoomrState.Normal:
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
                break;

            case BoomrState.Twist:

                GansubRotate();

                break;

            case BoomrState.WaitDestroy:

                // 일정 거리 만큼 멀어지면
                if (Vector3.Distance(this.transform.position, Point4) > 10f)
                {
                    boomrState = BoomrState.OnDestroy;
                    controller.GetComponent<BoomrContrlCS>().Invoke("CreateBoomr", 2f);
                    Invoke("DestroyBoomr", 1.0f);
                }

                break;

        }


        // 노말의 진행도가 반 이상이거나 부메랑의 상태가 노말이 아니고 부메랑의 상태가 시한부가 아니라면 ( 밑에 구문 한번만 처리하기 위한 AND문)
        if ((bezierCounter >= 0.5f || boomrState != BoomrState.Normal) && boomrState != BoomrState.WaitDestroy)
        {
            // 컨트롤러가 널이 아니고 만약 플레이어와 부메랑의 거리가 10 멀어졌다면
            if (Vector3.Distance(this.transform.position, Point4) < 0.5f)
            {
                boomrState = BoomrState.WaitDestroy;

                this.transform.forward = Point4 - this.transform.position;
            }
        }



    }
    void DestroyBoomr()
    {
        ObjectPoolManager.inst.ReturnObjectToPool("Boomerang", this.gameObject);
    }


    void GansubInit(Transform _origin, Vector3 _target)
    {
        // 출발지를 받아둠 | 도착지는 Point4

        twist.originPos = _origin;



        // 구간별 러프 출발지

        twist.prePos = _origin;



        // 현재 부메랑과 도착지의 거리값을 넣고

        twist.distance = Vector3.Distance(_origin.position, _target);



        // 거리값만큼 쪼개줌 (거리값이 2라면 dis에 0.5가 들어감)

        twist.distance = 1 / twist.distance;



        // 러프를 카운트해줄 변수

        twist.lerpCount = 0;

        // 다음 분기점 값을 담을 변수 변수 ex) 1. ( 0 ~ lerpSet(0.3))- lerpSet + distance - ( 0.3 ~ lerpSet (0.6 ) ~

        twist.lerpFinal = twist.distance;

    }

    // 간섭
    void GansubRotate()
    {

        timeCounter += Time.deltaTime;

        if (timeCounter >= timeSet)
        {

            timeCounter = 0;

            // Lerp의 첫번째 인자 값은 구간별 출발점이고 

            //                      두번째 인자값의 러프는 첫번재 인자값은 맨처음 출발점의 Forward와 출발점에서 도착치로 향하는 벡터를 러프 연산해서 분기별 도착지를 리턴해줌

            // 결과적으로는 처음이라면 Vector3.Slerp(0, 0.3, lerpCount) 두번째에는 Vector3.Slerp(0.3, 0.6, lerpCount)식으로 진행됨

            this.transform.forward = Vector3.Slerp(twist.prePos.forward, Vector3.Slerp(twist.originPos.forward, Point4 - twist.originPos.position, twist.lerpFinal), twist.lerpCount);


            // 러프카운트 누적합

            twist.lerpCount += 0.05f;


            // 만약 분기 회전이 끝났다면 다음 분기로 값을 세팅해줌

            if (twist.lerpCount >= 1)

            {

                twist.lerpCount = 0;



                // 분기별 시작 Forward에 현재의 Forward 넣어줌

                twist.prePos.forward = this.transform.forward;



                // 둘을 더하면 1을 넘는지

                if (twist.lerpFinal + twist.distance < 1)

                {

                    // 도착값을 다음 분기 값으로 세팅

                    twist.lerpFinal += twist.distance;

                }

                else

                {

                    // 넘으면 1로

                    twist.lerpFinal = 1;

                }

            }
        }

    }


    // 베지어 점과 점 Lerp 연결

    private void OnTriggerEnter(Collider _other)

    {
        if (_other.tag == "Shield")
        {
            // 간섭중 충돌했다면
            if (boomrState == BoomrState.Twist)
            {
                this.transform.Rotate(0, 180, 0);
                GansubInit(this.transform, Point1);
                twist.distance = 0.05f;
                twist.lerpFinal = 0.05f;
            }
            else
            {
                bezierCounter = 1 - bezierCounter;


                Vector3 temp = Point1;

                Point1 = Point4;

                Point4 = temp;


                temp = Point2;

                Point2 = Point3;

                Point3 = temp;
            }
        }

        else if(_other.tag == "StartObject")
        {
            StageManager.inst.Invoke("ClearStage", 3f);
            _other.gameObject.SetActive(false);
        }

        else if (_other.GetComponent<TutorialEnemy>())
        {
            boomrState = BoomrState.Stop;
        }


    }
}
