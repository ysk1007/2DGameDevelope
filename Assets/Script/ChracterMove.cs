using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChracterMove : MonoBehaviour
{
    public float moveSpeed = 1f; //플레이어 움직임 속도
    public float jumpPower = 4f; //플레이어 점프력
    public float rollforce = 1f;
    public float Damage = 1f;
    public GameObject slash;
    public Transform slash_pos;

    private Sensor_Chracter c_groundSensor;
    private Sensor_Chracter c_wallSensorR1;
    private Sensor_Chracter c_wallSensorR2;
    private Sensor_Chracter c_wallSensorL1;
    private Sensor_Chracter c_wallSensorL2;

    public float c_rollDuration = 0.53f;
    public float c_rollCurrentTime;
    public bool c_rolling = false;

    public float c_attackDuration = 0.24f;
    public float c_attackCurrentTime;
    public bool c_attacking = false;

    Rigidbody2D rigid; //리지드바디(중력) 변수 설정
    Animator animator; //애니메이터 변수 설정

    Vector3 movement;
    public bool isJumping = false; //현재 점프 유무의 상태
    public int JumpingCount = 2;    //현재 사용가능한 점프 횟수

    // Start is called before the first frame update
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>(); //rigid 변수에 게임 오브젝트(현재 스크립트가 있는)에 컴포넌트 Rigidbody2D를 가져옴
        animator = gameObject.GetComponentInChildren<Animator>();   //animato 변수에 게임 오브젝트 하위 오브젝트의 애니메이터 컴포넌트를 가져옴
        
        c_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Chracter>(); //센서 캐릭터 스크립트가 있는 컴포넌트를 찾음
        c_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_Chracter>();
        c_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_Chracter>();
        c_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_Chracter>();
        c_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_Chracter>();
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //스페이스 키가 눌렸을 때
            Roll(); //구르는 함수 호출

        if (animator.GetBool("isRunning") != true)  //현재 뛰는 모션이 아니라면
        {
            if (Input.GetKey(KeyCode.LeftShift))    // 좌측 SHIFT 키를 눌렀을때
            {
                animator.SetBool("isRunning", true);    //뛰는 모션
                moveSpeed = 2f;                         //플레이어 속도 증가
            }
        }
        
        
        else if (Input.GetKeyUp(KeyCode.LeftShift))     //현재 뛰는 모션이라면, 좌측 SHIFT 키에서 손을 땠을때
        {
            animator.SetBool("isRunning", false);       //뛰는 모션 중단
            moveSpeed = 1f;                             //플레이어 속도 하락
        }

        if (Input.GetAxisRaw ("Horizontal") == 0)       //수평 값 0을 받았을 때 (즉, 수평 입력이 없음)
        {
            animator.SetBool("isRunning", false);       //뛰는 모션 중단
            animator.SetBool("isMoving", false);        //걷는 모션 중단
            moveSpeed = 1f;                             //플레이어 기본 이동 속도
        }


        if (Input.GetKeyDown(KeyCode.W))    //점프 키 입력을 받았을 때
        {
            if(JumpingCount == 0)
            {
                animator.ResetTrigger("doJumping"); //선입력된 트리거를 리셋 함
            }
            else
            {
                isJumping = true;               //점프 bool 값 true
                animator.SetBool("isJumping", true);    //애니메이터 점프 bool 값 true
                animator.SetTrigger("doJumping");       //애니메이터 점프중
            }

        }

        if (Input.GetMouseButtonDown(0))    //마우스 좌클릭 했을때
        {
            Attack();
        }
    }

    //Physics engine Updates
    void FixedUpdate()
    {
        Move();
        Jump();
        Grabing();
        Fall();
    }

    void Move()
    {

        float h = Input.GetAxisRaw("Horizontal");   // 입력 받은 수평 값을 h에 저장
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse); // ForceMode2D.Impulse => 충격량이랑 힘의 크기와 주는 시간을 곱한 수치
        if (rigid.velocity.x > moveSpeed)//양수 오른쪽
        {
            rigid.velocity = new Vector2(moveSpeed, rigid.velocity.y);  //y값을 0으로 잡으면 공중에서 멈춰버림
            transform.localScale = new Vector3(1, 1, 1);    //캐릭터 오른쪽 방향으로 플립
            animator.SetBool("isMoving", true);     //걷는 모션
            animator.SetInteger("Direction", 1);    //애니메이터의 방향 파라미터 값을 1 (양수=오른쪽)
        }
        else if (rigid.velocity.x < moveSpeed * (-1))//음수 왼쪽
        {
            rigid.velocity = new Vector2(moveSpeed * (-1), rigid.velocity.y);
            transform.localScale = new Vector3(-1, 1, 1);   //캐릭터 왼쪽 방향으로 플립
            animator.SetBool("isMoving", true);     //걷는 모션
            animator.SetInteger("Direction", -1);   //애니메이터의 방향 파라미터 값을 -1 (음수=왼쪽)
        }

        /*transform.position += moveVelocity * moveSpeed * Time.deltaTime; //현재 물체의 포지션 값에 움직임을 더함*/
    }

    void Jump()
    {
        if (!isJumping) //점프 중이라면 return
            return;

        if (JumpingCount == 0)  //남은 점프 횟수가 0 이라면 return
        {
            return;
        }   

        rigid.velocity = Vector2.zero;  //rigid 속도 0,0

        Vector2 jumpVelocity = new Vector2(0, jumpPower);   //jumpVelocity에 0, 점프력
        rigid.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse); //rigid에 힘을 가함 up방향

        isJumping = false;  //점프 중 false
        JumpingCount -= 1;  //점프 카운트 -1
    }

    void Grabing()
    {
        
        if(animator.GetBool("wallAttach") == true && animator.GetBool("isJumping") == true) //벽을 감지했고, 공중에 있다면
        {
            float Direction = rigid.velocity.x; //x축 속도를 Dircetion 변수에 저장
            if (Direction > 0 || Direction < 0)   // Dircetion 1이상 or 1이하 (속도가 있다면)
            {
                //sensor_Chracter 스크립트에서 State 함수를 실행함. 벽을 감지 했다면 true, 아니라면 false 를 리턴 하는 함수임
                if((c_wallSensorR1.State() && c_wallSensorR2.State()) || (c_wallSensorL1.State() && c_wallSensorL2.State())) //우 센서 둘 다 감지 or 좌 센서 둘 다 감지
                {
                    if (Direction > 0)
                        transform.localScale = new Vector3(1, 1, 1);
                    else
                        transform.localScale = new Vector3(-1, 1, 1);
                    animator.SetBool("doGrabing",true); // 벽 잡는 모션
                }
            }
            else if (rigid.velocity.x == 0) // x축 속도가 없으면 (입력 x)
                animator.SetBool("doGrabing", false); // 벽 잡는 모션 중지
        }
        else if (animator.GetBool("wallAttach") == false)   //만약 벽을 감지하지 않았다면
        {
            animator.SetBool("doGrabing", false);   //벽 잡는 모션 중지
            animator.SetBool("wallAttach", false);  //벽 감지 중지
        }

    }

    void Roll()
    {
        //구르는 중이 아니고, 벽을 잡는 중도 아니며, 공중에 떠 있지 않음
        if (!c_rolling && !animator.GetBool("doGrabing") && !animator.GetBool("isJumping"))
        {
            animator.ResetTrigger("isRolling");
            c_rolling = true; // 구르는 중
            animator.SetTrigger("isRolling");   //구르는 모션
            rigid.velocity = new Vector2(animator.GetInteger("Direction") * rollforce, rigid.velocity.y);   //방향에 구르는 속도 곱 하여 힘을 줌
            moveSpeed += rollforce;
            Invoke("RollStop", c_rollDuration); //c_rollDuration(0.53초) 뒤 RollStop 함수 실행
        }
    }

    void RollStop()
    {
        if (animator.GetBool("isRunning"))  //달리고 있다면
            moveSpeed = 2f; //달리는 속도 유지
        else
            moveSpeed = 1f; //아니라면 기본 속도
        c_rolling = false;  //구르는 중 아님
    }

    void Fall()
    {
        if (rigid.velocity.y < 0 && !animator.GetBool("falling")) //y축 속도가 0이하 (떨어지는중) 이고 낙하 모션 중이 아니라면
        {
            animator.SetTrigger("fall");    //낙하
            animator.SetBool("falling", true);  //낙하 모션
        }
        else if (rigid.velocity.y >= 0) //y축 속도가 0보다 크거나 같다면 (상승, 지면 상태일 때)
            animator.SetBool("falling", false); //낙하 모션 중지
    }

    void Attack()
    {
        if (!c_attacking && !animator.GetBool("doGrabing")) //공격중이 아니며, 벽 잡는중이 아닐때
        {
            c_attacking = true; //공격중
            animator.SetBool("isAttacking", true);  //애니메이션 공격 모션 true
            animator.SetTrigger("doAttack");    //공격 트리거 작동

            var newSlash = Instantiate(slash,slash_pos.position, transform.rotation);   //새로운 공격을 생성 Instantiate(오브젝트, 오브젝트 포지션, 각도);
            newSlash.transform.parent = gameObject.transform;   //새로운 공격의 좌표는 게임오브젝트의 좌표

            Invoke("AttackStop", c_attackDuration); //c_attackDuration(0.12초) 뒤 AttackStop 함수 실행
        }

        else
            return;
    }

    void AttackStop()
    {
        c_attacking = false;    //공격중 아님
        animator.SetBool("isAttacking", false); //애니메이션 공격모션 중지
    }

    //Attach Event
    void OnTriggerEnter2D(Collider2D other) //접촉 트리거(착지로 사용)
    {
        Debug.Log("Attach : " + other.gameObject.layer);    //접촉시 오브젝트의 레이어 인덱스 디버그에 출력

        if (other.gameObject.layer == 6 && rigid.velocity.y < 0)    //레이어 인덱스가 일치하고igid의 y축 속도가 0보다 작을 때
        {
            animator.SetBool("isJumping", false);   //점프 모션 중단
            JumpingCount = 2;   //남은 점프 회수 초기화
        }

        if (other.gameObject.layer == 7) //레이어 인덱스가 일치하면, 또는 rigid의 y축 속도가 0보다 작을 때
        {
            animator.SetBool("wallAttach", true);   //벽 감지
        }
            

    }

    //Detach Event
    void OnTriggerExit2D(Collider2D other)  //분리 트리거
    {
        Debug.Log("Detach : " + other.gameObject.layer);    //분리시 오브젝트의 레이어 인덱스 디버그에 출력
        if (other.gameObject.layer == 7)
            animator.SetBool("wallAttach", false);

    }

}
