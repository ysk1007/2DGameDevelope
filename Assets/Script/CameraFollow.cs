using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour
{
    public Transform target;    //카메라를 따라다닐 target의 위치정보를 담을 변수
    public Image Fade;  //씬 전환효과를 위해 투명 이미지 하나를 생성하여 담을 변수

    public float smoothSpeed = 3;   //카메라가 target을 따라가는 속도
    public Vector2 offset;  //카메라 위치
    public float limitMinX, limitMaxX, limitMinY, limitMaxY;    //화면이 일정 범위를 넘어가지 않게 할 최대,최소 범위 변수
    float cameraHalfWidth, cameraHalfHeight;    //카메라의 중심으로부터 촬영 범위까지 x축,y축까지의 길이

    private void Start()
    {
        StartCoroutine("FadeIn");   //씬 시작시 FadeIn 코루틴 실행
        cameraHalfWidth = Camera.main.aspect * Camera.main.orthographicSize; //Camera.main.aspect는 해상도 width/height를 계산한 비율
        cameraHalfHeight = Camera.main.orthographicSize; //Camera.main.orthographicSize는 카메라의 사이즈
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = new Vector3(
            //Mathf.Clamp(값, 최솟값, 최댓값)으로 최솟값, 최댓값을 넘지 않게 지정
            Mathf.Clamp(target.position.x + offset.x, limitMinX + cameraHalfWidth, limitMaxX - cameraHalfWidth),   // X
            Mathf.Clamp(target.position.y + offset.y, limitMinY + cameraHalfHeight, limitMaxY - cameraHalfHeight), // Y
            -10);                                                                                                  // Z
        //Vector3.Lerp(시작 위치, 도착할 위치, t)를 사용해서 부드럽게 이동
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
    }

    IEnumerator FadeIn()    //화면 밝아지는 효과
    {
        Fade.color = new Color32(29, 29, 29, 255);  //투명 화면을 어두운 화면으로 변경
        Color color = Fade.color;   //color 변수를 생성하여 Fade.color의 rgba 값을 복사하여 넣음
        while (Fade.color.a !> 0)   //Fade의 투명도가 0보다 크지 않는동안 계속 실행
        {
            Debug.Log(color.a);
            color.a -= Time.deltaTime * 0.45f;  //color의 투명도 값을 Time.deltaTime에 임의의 float 값을 곱하여 내림

            yield return new WaitForSeconds(0.001f);    //0.001f 초 기다렸다 실행

            Fade.color = color; //Fade의 값을 새로운 color 값으로 설정
        }
        yield return null;                                        //코루틴 종료
    }

    public IEnumerator FadeOut()    //화면 어두워지는 효과
    {
        Debug.Log("FadeOut");
        Fade.color = new Color32(29, 29, 29, 0); 
        Color color = Fade.color;   //color 변수를 생성하여 Fade.color의 rgba 값을 복사하여 넣음
        while (Fade.color.a !< 255) //Fade의 투명도가 255보다 작지 않는동안 계속 실행
        {
            Debug.Log(color.a);
            color.a += Time.deltaTime * 0.45f;  //color의 투명도 값을 Time.deltaTime에 임의의 float 값을 곱하여 올림

            yield return new WaitForSeconds(0.001f);    //0.001f 초 기다렸다 실행

            Fade.color = color; //Fade의 값을 새로운 color 값으로 설정
        }
        yield return null;                                        //코루틴 종료
    }
}

