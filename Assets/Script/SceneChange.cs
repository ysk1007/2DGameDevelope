using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    Scene scene;    //�� ��ä�� ���� ����
    public GameObject Camera;   //����ī�޶� ������Ʈ�� ���� Camera ����
    void Start()
    {
        scene = SceneManager.GetActiveScene();  //���� ���� �ҷ��� ������ ����
        Debug.Log(scene.buildIndex);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var camera = Camera.GetComponent<CameraFollow>();   //����ī�޶��� CameraFollow ��ũ��Ʈ�� ������ �� camera ������ ����
            camera.StartCoroutine("FadeOut");   //CameraFollow�� FadeOut �ڷ�ƾ ����
            Invoke("NextScene", 2.5f);  //2.5�ʵ� NextScene �Լ� ����
        }
    }

    public void ChangeScene()
    {
        
    }

    public void NextScene(int CurrentIndex) //�ٷ� �Ű����� ���� ���� ������ ++ �Ͽ� �̵�
    {
        SceneManager.LoadScene(++CurrentIndex);
    }

    public void NextScene() //�Ű� ���� ������ ���� �� �ε����� �ҷ��� ++ �Ͽ� �̵�
    {
        int CurrentIndex = scene.buildIndex;
        SceneManager.LoadScene(++CurrentIndex);
    }
}
