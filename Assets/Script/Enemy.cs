using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Enemy : MonoBehaviour
{
    public float EnemyHP = 1000;    //몬스터 피통
    public float CurrentHp; //몬스터 현재 hp
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
        sprite = gameObject.GetComponent<SpriteRenderer>(); //스프라이트(이미지) 속성을 가져옴
        particle_1 = transform.Find("flare_1").GetComponent<ParticleSystem>();  //파티클효과 flare_1 속성을 가져옴
        particle_2 = transform.Find("flare_dark_1").GetComponent<ParticleSystem>(); //파티클효과 flare_dark_1 속성을 가져옴
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit(float damage)
    {
        CurrentHp -= damage;  //체력에서 damage 만큼 깎음
        StartCoroutine(FadeTo());   //코루틴 FadeTo 실행
        particle_1.Play();  //파티클 1 실행
        particle_2.Play();  //파티클 2 실행
        Debug.Log("남은 HP : " + CurrentHp);
        hpBarFiled_1.fillAmount = CurrentHp / EnemyHP;
        hpBarFiled_2.fillAmount = CurrentHp / EnemyHP;
        hpBarBackground.SetActive(true);
    }

    IEnumerator FadeTo()    //피격시 적이 껌뻑껌뻑 거리는 효과
    {
        int countTime = 0;  //카운트

        while (countTime < 1)   //카운트가 1이 넘으면
        {
            if (countTime % 2 == 0)
                sprite.color = new Color32(255, 255, 255, 90);  //스프라이트의 투명도를 90/255 설정
            else
                sprite.color = new Color32(255, 255, 255, 180); //투명도 180/255

            yield return new WaitForSeconds(0.2f);  //0.2초 기다렸다 리턴

            countTime++;    //카운트 +1
        }

        sprite.color = new Color32(255, 255, 255, 255); //투명도 255/255 (기본)
    }
}
