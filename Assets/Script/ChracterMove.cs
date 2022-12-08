using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChracterMove : MonoBehaviour
{
    public float moveSpeed = 1f; //�÷��̾� ������ �ӵ�
    public float jumpPower = 3f; //�÷��̾� ������

    Rigidbody2D rigid; //������ٵ�(�߷�) ���� ����
    Animator animator; //�ִϸ����� ���� ����

    Vector3 movement;
    public bool isJumping = false; //���� ���� ������ ����
    public int JumpingCount = 2;    //���� ��밡���� ���� Ƚ��
    // Start is called before the first frame update
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>(); //rigid ������ ���� ������Ʈ(���� ��ũ��Ʈ�� �ִ�)�� ������Ʈ Rigidbody2D�� ������
        animator = gameObject.GetComponentInChildren<Animator>();   //animato ������ ���� ������Ʈ ���� ������Ʈ�� �ִϸ����� ������Ʈ�� ������
    }



    // Update is called once per frame
    void Update()
    {
        if (animator.GetBool("isRunning") != true)  //���� �ٴ� ����� �ƴ϶��
        {
            if (Input.GetKey(KeyCode.LeftShift))    // ���� SHIFT Ű�� ��������
            {
                animator.SetBool("isRunning", true);    //�ٴ� ���
                moveSpeed = 2f;                         //�÷��̾� �ӵ� ����
            }
        }
        
        
        else if (Input.GetKeyUp(KeyCode.LeftShift))     //���� �ٴ� ����̶��, ���� SHIFT Ű���� ���� ������
        {
            animator.SetBool("isRunning", false);       //�ٴ� ��� �ߴ�
            moveSpeed = 1f;                             //�÷��̾� �ӵ� �϶�
        }

        if (Input.GetAxisRaw ("Horizontal") == 0)       //���� �� 0�� �޾��� �� (��, ���� �Է��� ����)
        {
            animator.SetBool("isRunning", false);       //�ٴ� ��� �ߴ�
            animator.SetBool("isMoving", false);        //�ȴ� ��� �ߴ�
            moveSpeed = 1f;                             //�÷��̾� �⺻ �̵� �ӵ�
        }


        if (Input.GetButtonDown("Jump"))    //���� Ű �Է��� �޾��� ��
        {
            isJumping = true;               //���� bool �� true
            animator.SetBool("isJumping", true);    //�ִϸ����� ���� bool �� true
            animator.SetTrigger("doJumping");       //�ִϸ����� ������
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

        float h = Input.GetAxisRaw("Horizontal");   // �Է� ���� ���� ���� h�� ����
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse); // ForceMode2D.Impulse => ��ݷ��̶� ���� ũ��� �ִ� �ð��� ���� ��ġ
        if (rigid.velocity.x > moveSpeed)//��� ������
        {
            rigid.velocity = new Vector2(moveSpeed, rigid.velocity.y);  //y���� 0���� ������ ���߿��� �������
            transform.localScale = new Vector3(1, 1, 1);    //ĳ���� ������ �������� �ø�
            animator.SetBool("isMoving", true);     //�ȴ� ���
            animator.SetInteger("Direction", 1);    //�ִϸ������� ���� �Ķ���� ���� 1 (���=������)
        }
        else if (rigid.velocity.x < moveSpeed * (-1))//���� ����
        {
            rigid.velocity = new Vector2(moveSpeed * (-1), rigid.velocity.y);
            transform.localScale = new Vector3(-1, 1, 1);   //ĳ���� ���� �������� �ø�
            animator.SetBool("isMoving", true);     //�ȴ� ���
            animator.SetInteger("Direction", -1);   //�ִϸ������� ���� �Ķ���� ���� -1 (����=����)
        }

        /*transform.position += moveVelocity * moveSpeed * Time.deltaTime; //���� ��ü�� ������ ���� �������� ����*/
    }

    void Jump()
    {
        if (!isJumping) //���� ���̶�� return
            return;

        if (JumpingCount == 0)  //���� ���� Ƚ���� 0 �̶�� return
            return;

        rigid.velocity = Vector2.zero;  //rigid �ӵ� 0,0

        Vector2 jumpVelocity = new Vector2(0, jumpPower);   //jumpVelocity�� 0, ������
        rigid.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse); //rigid�� ���� ���� up����

        isJumping = false;  //���� �� false

        JumpingCount -= 1;  //���� ī��Ʈ -1
    }


    //Attach Event
    void OnTriggerEnter2D(Collider2D other) //���� Ʈ����(������ ���)
    {
        Debug.Log("Attach : " + other.gameObject.layer);    //���˽� ������Ʈ�� ���̾� �ε��� ����׿� ���

        if (other.gameObject.layer == 6 && rigid.velocity.y < 0)    //���̾� �ε����� ��ġ�ϸ�, �Ǵ� rigid�� y�� �ӵ��� 0���� ���� ��
            animator.SetBool("isJumping", false);   //���� ��� �ߴ�
        JumpingCount = 2;   //���� ���� ȸ�� �ʱ�ȭ
        
    }

    //Detach Event
    void OnTriggerExit2D(Collider2D other)  //�и� Ʈ����
    {
        Debug.Log("Detach : " + other.gameObject.layer);    //�и��� ������Ʈ�� ���̾� �ε��� ����׿� ���
    }

}
