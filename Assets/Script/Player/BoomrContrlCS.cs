using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomrContrlCS : MonoBehaviour
{

    // 부메랑 프리팹 받아옴
    public GameObject boomrPrefab;


    // 베지어 곡선 구현에 필요한 좌표 2개 받아옴 (이 두 좌표는 플레이어 컨트롤에 자식으로 들어가있음).
    public GameObject bezierObj;

    // 충돌한 부메랑 오브젝트를 담을 변수
    private GameObject boomrObj;

    // 던지는 방향을 구하기 위해 컨트롤러의 전 좌표를 담을 변수
    private Vector3 prePos;

    // 부메랑을 던졌을때 던지는 방향을 구해줄 변수
    private Vector3 throwVec;

    // 오른쪽 컨트롤러인지?
    public bool isRightControler;
    // 현재 부메랑을 잡고 있는지 여부
    private bool isCatching;


    // Start is called before the first frame update
    void Start()
    {

        // 초기화
        boomrObj = null;
        isCatching = false;



        // 부메랑 프리팹 생성해줌
        CreateBoomr();
    }

    // Update is called once per frame
    void Update()
    {
        // 테스트용 컨트롤러 무브 소스
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.position += this.transform.forward * 5 * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            this.transform.position += -this.transform.right * 5 * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            this.transform.position += -this.transform.forward * 5 * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            this.transform.position += this.transform.right * 5 * Time.deltaTime;
        }

        // 변수가 null이 아니라면 (현재 컨트롤러와 부메랑이 충돌한 상태라면)
        if (boomrObj != null)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                // 잡았으니 True로
                isCatching = true;

                // 계산 변수들 값 초기화
                prePos = this.transform.position;
                throwVec = Vector3.zero;

                // 부메랑 소스 내부의 초기화 함수 호출 (날아온 부메랑을 잡은 경우 부메랑 이동 멈출수 있게)
                boomrObj.GetComponent<Boomr>().Init();
            }
            // KeyDown을 눌러 잡은 상태에서만 동장하도록 (미리 누르고 있어도 잡히는 걸 방지하기 위해)
            if (isCatching)
            {
                if (Input.GetKey(KeyCode.G) && isCatching)
                {
                    // 부메랑의 좌표를 컨트롤러와 동일하게 맞춰줌 (잡은것 처럼 보이도록)
                    boomrObj.transform.position = this.transform.position;
                    boomrObj.transform.rotation = this.transform.rotation;

                    // 현재 컨트롤러가 향하고 있는 방향을 구하는 식
                    throwVec = this.transform.position - prePos;

                    // 현재 좌표를 다음 계산을 위해 전 좌표 변수 값으로 넣어줌
                    prePos = this.transform.position;
                }
                else if (Input.GetKeyUp(KeyCode.G))
                {
                    isCatching = false;

                    // 방향값이 0.1보다 작다면 부메랑 오브젝트 리셋
                    if (throwVec.magnitude < 0.01f)
                    {
                        // 방향이 없어 밑으로 떨어지는 연출
                        boomrObj.GetComponent<Rigidbody>().useGravity = true;
                        boomrObj.GetComponent<Boomr>().Invoke("DestroyBoomr", 2.0f);
                        Invoke("CreateBoomr", 1.0f);
                    }
                    else
                    {
                        bezierObj.transform.position = this.transform.position;
                        bezierObj.transform.forward = throwVec;

                        if (isRightControler)
                        {

                            boomrObj.GetComponent<Boomr>().SetBezierPoint(bezierObj.transform.position,
                                                                          bezierObj.transform.GetChild(0).transform.position,
                                                                          bezierObj.transform.GetChild(2).transform.position,
                                                                          this.transform.position);
                        }
                        else
                        {
                            boomrObj.GetComponent<Boomr>().SetBezierPoint(bezierObj.transform.position,
                                                                          bezierObj.transform.GetChild(0).transform.position,
                                                                          bezierObj.transform.GetChild(1).transform.position,
                                                                          this.transform.position);
                        }

                        // 부메랑 소스에 방향값을 넘겨줌 (부메랑이 넣어준 방향대로 날라감)
                        boomrObj.GetComponent<Boomr>().Throw();
                    }
                }
            }
        }


    }

    public void CreateBoomr()
    {
        ObjectPoolManager.inst.GetObjectFromPool("Boomerang", this.transform.position, Quaternion.LookRotation(this.transform.forward));
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.tag == "Boomr")
        {
            boomrObj = _other.gameObject;
            _other.GetComponent<Boomr>().controller = this.gameObject;
        }
        
    }

    private void OnTriggerExit(Collider _other)
    {
        if (_other.tag == "Boomr")
        {
            boomrObj = null;
        }
    }

   

}
