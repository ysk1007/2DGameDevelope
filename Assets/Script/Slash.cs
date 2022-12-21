using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    public float distance;
    public LayerMask isLayer;   //���̾� ������ ��Ƶδ� ����
    private float force;

    // Start is called before the first frame update
    void Start()
    {
        force = transform.parent.gameObject.GetComponent<ChracterMove>().Damage;   //force ������ �θ� ���ӿ�����Ʈ�� �Ӽ��� ������ ChracterMove ��ũ��Ʈ�� Damge ���� 
        Invoke("DestoryEffect", 0.248f);    //0.248f�� �� DestroyEffect �Լ� ȣ��
    }

    // Update is called once per frame
    void Update()
    {
        //�����ɽ�Ʈ�� �߻��մϴ�(������, ����, �Ÿ�, ���̾� ����)
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.right, distance , isLayer);
        if(ray.collider != null)    //����ĳ��Ʈ�� ��� �ݶ��̴��� null�� �ƴ϶��
        {
            if(ray.collider.tag == "Enemy") //�±װ� �� �Ͻ�
            {
                var newEnemy = ray.collider.GetComponent<Enemy>();  //������ �ݶ��̴��� Enemy ��ũ��Ʈ�� ������
                newEnemy.Hit(force);    //Enemy ��ũ��Ʈ�� Hit(force) �Լ� �� ���� ��
                Debug.Log("����");
            }
        }
        if (transform.localScale.x > 0) //�÷��̾��� ���⿡ ���� Slash �������� ��������Ʈ�� �������� ������
            GetComponent<SpriteRenderer>().flipX = false;
        else
            GetComponent<SpriteRenderer>().flipX = true;
    }

    void DestoryEffect()
    {
        Destroy(gameObject);    //���ӿ�����Ʈ�� �ı��մϴ�
    }
}
