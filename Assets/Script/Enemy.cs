using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Enemy : MonoBehaviour
{
    public float EnemyHP = 1000;    //���� ����
    public float CurrentHp; //���� ���� hp
    SpriteRenderer sprite;
    ParticleSystem particle_1;
    ParticleSystem particle_2;

    public GameObject hpBarBackground;
    public Image hpBarFiled_1;
    public Image hpBarFiled_2;

    // Start is called before the first frame update
    void Start()
    {
        CurrentHp = EnemyHP;
        hpBarFiled_1.fillAmount = 1f;
        hpBarFiled_2.fillAmount = 1f;
        sprite = gameObject.GetComponent<SpriteRenderer>(); //��������Ʈ(�̹���) �Ӽ��� ������
        particle_1 = transform.Find("flare_1").GetComponent<ParticleSystem>();  //��ƼŬȿ�� flare_1 �Ӽ��� ������
        particle_2 = transform.Find("flare_dark_1").GetComponent<ParticleSystem>(); //��ƼŬȿ�� flare_dark_1 �Ӽ��� ������
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit(float damage)
    {
        CurrentHp -= damage;  //ü�¿��� damage ��ŭ ����
        StartCoroutine(FadeTo());   //�ڷ�ƾ FadeTo ����
        particle_1.Play();  //��ƼŬ 1 ����
        particle_2.Play();  //��ƼŬ 2 ����
        Debug.Log("���� HP : " + CurrentHp);
        hpBarFiled_1.fillAmount = CurrentHp / EnemyHP;
        hpBarFiled_2.fillAmount = CurrentHp / EnemyHP;
        hpBarBackground.SetActive(true);
    }

    IEnumerator FadeTo()    //�ǰݽ� ���� �������� �Ÿ��� ȿ��
    {
        int countTime = 0;  //ī��Ʈ

        while (countTime < 1)   //ī��Ʈ�� 1�� ������
        {
            if (countTime % 2 == 0)
                sprite.color = new Color32(255, 255, 255, 90);  //��������Ʈ�� ������ 90/255 ����
            else
                sprite.color = new Color32(255, 255, 255, 180); //���� 180/255

            yield return new WaitForSeconds(0.2f);  //0.2�� ��ٷȴ� ����

            countTime++;    //ī��Ʈ +1
        }

        sprite.color = new Color32(255, 255, 255, 255); //���� 255/255 (�⺻)
    }
}
