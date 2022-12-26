using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    Scene scene;    //씬 객채를 담을 변수
    public GameObject Camera;   //메인카메라 오브젝트를 담을 Camera 변수
    void Start()
    {
        scene = SceneManager.GetActiveScene();  //현재 씬을 불러와 변수에 담음
        Debug.Log(scene.buildIndex);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var camera = Camera.GetComponent<CameraFollow>();   //메인카메라의 CameraFollow 스크립트를 가져와 새 camera 변수에 담음
            camera.StartCoroutine("FadeOut");   //CameraFollow의 FadeOut 코루틴 실행
            Invoke("NextScene", 2.5f);  //2.5초뒤 NextScene 함수 실행
        }
    }

    public void ChangeScene()
    {
        
    }

    public void NextScene(int CurrentIndex) //바로 매개변수 다음 차례 씬으로 ++ 하여 이동
    {
        SceneManager.LoadScene(++CurrentIndex);
    }

    public void NextScene() //매개 변수 없으면 현재 씬 인데스를 불러와 ++ 하여 이동
    {
        int CurrentIndex = scene.buildIndex;
        SceneManager.LoadScene(++CurrentIndex);
    }
}
