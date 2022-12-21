using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChracterMove : MonoBehaviour
{
    public float moveSpeed = 1f; //�÷��̾� ������ �ӵ�
    public float jumpPower = 4f; //�÷��̾� ������
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
        
        c_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Chracter>(); //���� ĳ���� ��ũ��Ʈ�� �ִ� ������Ʈ�� ã��
        c_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_Chracter>();
        c_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_Chracter>();
        c_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_Chracter>();
        c_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_Chracter>();
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //�����̽� Ű�� ������ ��
            Roll(); //������ �Լ� ȣ��

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


        if (Input.GetKeyDown(KeyCode.W))    //���� Ű �Է��� �޾��� ��
        {
            if(JumpingCount == 0)
            {
                animator.ResetTrigger("doJumping"); //���Էµ� Ʈ���Ÿ� ���� ��
            }
            else
            {
                isJumping = true;               //���� bool �� true
                animator.SetBool("isJumping", true);    //�ִϸ����� ���� bool �� true
                animator.SetTrigger("doJumping");       //�ִϸ����� ������
            }

        }

        if (Input.GetMouseButtonDown(0))    //���콺 ��Ŭ�� ������
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
        {
            return;
        }   

        rigid.velocity = Vector2.zero;  //rigid �ӵ� 0,0

        Vector2 jumpVelocity = new Vector2(0, jumpPower);   //jumpVelocity�� 0, ������
        rigid.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse); //rigid�� ���� ���� up����

        isJumping = false;  //���� �� false
        JumpingCount -= 1;  //���� ī��Ʈ -1
    }

    void Grabing()
    {
        
        if(animator.GetBool("wallAttach") == true && animator.GetBool("isJumping") == true) //���� �����߰�, ���߿� �ִٸ�
        {
            float Direction = rigid.velocity.x; //x�� �ӵ��� Dircetion ������ ����
            if (Direction > 0 || Direction < 0)   // Dircetion 1�̻� or 1���� (�ӵ��� �ִٸ�)
            {
                //sensor_Chracter ��ũ��Ʈ���� State �Լ��� ������. ���� ���� �ߴٸ� true, �ƴ϶�� false �� ���� �ϴ� �Լ���
                if((c_wallSensorR1.State() && c_wallSensorR2.State()) || (c_wallSensorL1.State() && c_wallSensorL2.State())) //�� ���� �� �� ���� or �� ���� �� �� ����
                {
                    if (Direction > 0)
                        transform.localScale = new Vector3(1, 1, 1);
                    else
                        transform.localScale = new Vector3(-1, 1, 1);
                    animator.SetBool("doGrabing",true); // �� ��� ���
                }
            }
            else if (rigid.velocity.x == 0) // x�� �ӵ��� ������ (�Է� x)
                animator.SetBool("doGrabing", false); // �� ��� ��� ����
        }
        else if (animator.GetBool("wallAttach") == false)   //���� ���� �������� �ʾҴٸ�
        {
            animator.SetBool("doGrabing", false);   //�� ��� ��� ����
            animator.SetBool("wallAttach", false);  //�� ���� ����
        }

    }

    void Roll()
    {
        //������ ���� �ƴϰ�, ���� ��� �ߵ� �ƴϸ�, ���߿� �� ���� ����
        if (!c_rolling && !animator.GetBool("doGrabing") && !animator.GetBool("isJumping"))
        {
            animator.ResetTrigger("isRolling");
            c_rolling = true; // ������ ��
            animator.SetTrigger("isRolling");   //������ ���
            rigid.velocity = new Vector2(animator.GetInteger("Direction") * rollforce, rigid.velocity.y);   //���⿡ ������ �ӵ� �� �Ͽ� ���� ��
            moveSpeed += rollforce;
            Invoke("RollStop", c_rollDuration); //c_rollDuration(0.53��) �� RollStop �Լ� ����
        }
    }

    void RollStop()
    {
        if (animator.GetBool("isRunning"))  //�޸��� �ִٸ�
            moveSpeed = 2f; //�޸��� �ӵ� ����
        else
            moveSpeed = 1f; //�ƴ϶�� �⺻ �ӵ�
        c_rolling = false;  //������ �� �ƴ�
    }

    void Fall()
    {
        if (rigid.velocity.y < 0 && !animator.GetBool("falling")) //y�� �ӵ��� 0���� (����������) �̰� ���� ��� ���� �ƴ϶��
        {
            animator.SetTrigger("fall");    //����
            animator.SetBool("falling", true);  //���� ���
        }
        else if (rigid.velocity.y >= 0) //y�� �ӵ��� 0���� ũ�ų� ���ٸ� (���, ���� ������ ��)
            animator.SetBool("falling", false); //���� ��� ����
    }

    void Attack()
    {
        if (!c_attacking && !animator.GetBool("doGrabing")) //�������� �ƴϸ�, �� ������� �ƴҶ�
        {
            c_attacking = true; //������
            animator.SetBool("isAttacking", true);  //�ִϸ��̼� ���� ��� true
            animator.SetTrigger("doAttack");    //���� Ʈ���� �۵�

            var newSlash = Instantiate(slash,slash_pos.position, transform.rotation);   //���ο� ������ ���� Instantiate(������Ʈ, ������Ʈ ������, ����);
            newSlash.transform.parent = gameObject.transform;   //���ο� ������ ��ǥ�� ���ӿ�����Ʈ�� ��ǥ

            Invoke("AttackStop", c_attackDuration); //c_attackDuration(0.12��) �� AttackStop �Լ� ����
        }

        else
            return;
    }

    void AttackStop()
    {
        c_attacking = false;    //������ �ƴ�
        animator.SetBool("isAttacking", false); //�ִϸ��̼� ���ݸ�� ����
    }

    //Attach Event
    void OnTriggerEnter2D(Collider2D other) //���� Ʈ����(������ ���)
    {
        Debug.Log("Attach : " + other.gameObject.layer);    //���˽� ������Ʈ�� ���̾� �ε��� ����׿� ���

        if (other.gameObject.layer == 6 && rigid.velocity.y < 0)    //���̾� �ε����� ��ġ�ϰ�igid�� y�� �ӵ��� 0���� ���� ��
        {
            animator.SetBool("isJumping", false);   //���� ��� �ߴ�
            JumpingCount = 2;   //���� ���� ȸ�� �ʱ�ȭ
        }

        if (other.gameObject.layer == 7) //���̾� �ε����� ��ġ�ϸ�, �Ǵ� rigid�� y�� �ӵ��� 0���� ���� ��
        {
            animator.SetBool("wallAttach", true);   //�� ����
        }
            

    }

    //Detach Event
    void OnTriggerExit2D(Collider2D other)  //�и� Ʈ����
    {
        Debug.Log("Detach : " + other.gameObject.layer);    //�и��� ������Ʈ�� ���̾� �ε��� ����׿� ���
        if (other.gameObject.layer == 7)
            animator.SetBool("wallAttach", false);

    }

}
