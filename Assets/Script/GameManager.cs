using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Player;   //플레이어 오브젝트를 담을 변수

    void Start()
    {
        
    }

    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision) //접촉 트리거
    {
        //맵 이탈 방지 코드
        if (collision.gameObject.tag == "Player")   //접촉한 게임 오브젝트의 태그가 플레이어라면
        {
            Invoke("PlayerReset", 2f);  //플레이어 리셋 함수 2f 초뒤 실행
        }
    }

    void PlayerReset()
    {
        Player.transform.position = new Vector3(0, 0, 0);   //플레이어의 위치를 0,0,0 으로 이동
    }
}
