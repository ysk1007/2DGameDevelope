using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    public float distance;
    public LayerMask isLayer;   //레이어 정보를 담아두는 변수
    private float force;

    // Start is called before the first frame update
    void Start()
    {
        force = transform.parent.gameObject.GetComponent<ChracterMove>().Damage;   //force 변수에 부모 게임오브젝트의 속성을 가져옴 ChracterMove 스크립트의 Damge 변수 
        Invoke("DestoryEffect", 0.248f);    //0.248f초 뒤 DestroyEffect 함수 호출
    }

    // Update is called once per frame
    void Update()
    {
        //레이케스트를 발생합니다(시작점, 방향, 거리, 레이어 종류)
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.right, distance , isLayer);
        if(ray.collider != null)    //레이캐스트에 담긴 콜라이더가 null이 아니라면
        {
            if(ray.collider.tag == "Enemy") //태그가 적 일시
            {
                var newEnemy = ray.collider.GetComponent<Enemy>();  //적중한 콜라이더의 Enemy 스크립트를 가져옴
                newEnemy.Hit(force);    //Enemy 스크립트의 Hit(force) 함수 를 실행 함
                Debug.Log("명중");
            }
        }
        if (transform.localScale.x > 0) //플레이어의 방향에 따라 Slash 프리팹의 스프라이트도 뒤집히게 설정함
            GetComponent<SpriteRenderer>().flipX = false;
        else
            GetComponent<SpriteRenderer>().flipX = true;
    }

    void DestoryEffect()
    {
        Destroy(gameObject);    //게임오브젝트를 파괴합니다
    }
}
