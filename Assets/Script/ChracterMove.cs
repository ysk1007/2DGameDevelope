using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChracterMove : MonoBehaviour
{
    public float moveSpeed = 1f; //플레이어 움직임 속도
    public float jumpPower = 3f; //플레이어 점프력

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
    }



    // Update is called once per frame
    void Update()
    {
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


        if (Input.GetButtonDown("Jump"))    //점프 키 입력을 받았을 때
        {
            isJumping = true;               //점프 bool 값 true
            animator.SetBool("isJumping", true);    //애니메이터 점프 bool 값 true
            animator.SetTrigger("doJumping");       //애니메이터 점프중
        }
    }

    //Physics engine Updates
    void FixedUpdate()
    {
        Move();
        Jump();
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
            return;

        rigid.velocity = Vector2.zero;  //rigid 속도 0,0

        Vector2 jumpVelocity = new Vector2(0, jumpPower);   //jumpVelocity에 0, 점프력
        rigid.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse); //rigid에 힘을 가함 up방향

        isJumping = false;  //점프 중 false

        JumpingCount -= 1;  //점프 카운트 -1
    }


    //Attach Event
    void OnTriggerEnter2D(Collider2D other) //접촉 트리거(착지로 사용)
    {
        Debug.Log("Attach : " + other.gameObject.layer);    //접촉시 오브젝트의 레이어 인덱스 디버그에 출력

        if (other.gameObject.layer == 6 && rigid.velocity.y < 0)    //레이어 인덱스가 일치하면, 또는 rigid의 y축 속도가 0보다 작을 때
            animator.SetBool("isJumping", false);   //점프 모션 중단
        JumpingCount = 2;   //남은 점프 회수 초기화
        
    }

    //Detach Event
    void OnTriggerExit2D(Collider2D other)  //분리 트리거
    {
        Debug.Log("Detach : " + other.gameObject.layer);    //분리시 오브젝트의 레이어 인덱스 디버그에 출력
    }

}
