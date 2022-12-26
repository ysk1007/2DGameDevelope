using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Player;   //�÷��̾� ������Ʈ�� ���� ����

    void Start()
    {
        
    }

    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision) //���� Ʈ����
    {
        //�� ��Ż ���� �ڵ�
        if (collision.gameObject.tag == "Player")   //������ ���� ������Ʈ�� �±װ� �÷��̾���
        {
            Invoke("PlayerReset", 2f);  //�÷��̾� ���� �Լ� 2f �ʵ� ����
        }
    }

    void PlayerReset()
    {
        Player.transform.position = new Vector3(0, 0, 0);   //�÷��̾��� ��ġ�� 0,0,0 ���� �̵�
    }
}
