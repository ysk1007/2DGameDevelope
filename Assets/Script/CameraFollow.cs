using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour
{
    public Transform target;    //ī�޶� ����ٴ� target�� ��ġ������ ���� ����
    public Image Fade;  //�� ��ȯȿ���� ���� ���� �̹��� �ϳ��� �����Ͽ� ���� ����

    public float smoothSpeed = 3;   //ī�޶� target�� ���󰡴� �ӵ�
    public Vector2 offset;  //ī�޶� ��ġ
    public float limitMinX, limitMaxX, limitMinY, limitMaxY;    //ȭ���� ���� ������ �Ѿ�� �ʰ� �� �ִ�,�ּ� ���� ����
    float cameraHalfWidth, cameraHalfHeight;    //ī�޶��� �߽����κ��� �Կ� �������� x��,y������� ����

    private void Start()
    {
        StartCoroutine("FadeIn");   //�� ���۽� FadeIn �ڷ�ƾ ����
        cameraHalfWidth = Camera.main.aspect * Camera.main.orthographicSize; //Camera.main.aspect�� �ػ� width/height�� ����� ����
        cameraHalfHeight = Camera.main.orthographicSize; //Camera.main.orthographicSize�� ī�޶��� ������
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = new Vector3(
            //Mathf.Clamp(��, �ּڰ�, �ִ�)���� �ּڰ�, �ִ��� ���� �ʰ� ����
            Mathf.Clamp(target.position.x + offset.x, limitMinX + cameraHalfWidth, limitMaxX - cameraHalfWidth),   // X
            Mathf.Clamp(target.position.y + offset.y, limitMinY + cameraHalfHeight, limitMaxY - cameraHalfHeight), // Y
            -10);                                                                                                  // Z
        //Vector3.Lerp(���� ��ġ, ������ ��ġ, t)�� ����ؼ� �ε巴�� �̵�
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
    }

    IEnumerator FadeIn()    //ȭ�� ������� ȿ��
    {
        Fade.color = new Color32(29, 29, 29, 255);  //���� ȭ���� ��ο� ȭ������ ����
        Color color = Fade.color;   //color ������ �����Ͽ� Fade.color�� rgba ���� �����Ͽ� ����
        while (Fade.color.a !> 0)   //Fade�� ������ 0���� ũ�� �ʴµ��� ��� ����
        {
            Debug.Log(color.a);
            color.a -= Time.deltaTime * 0.45f;  //color�� ���� ���� Time.deltaTime�� ������ float ���� ���Ͽ� ����

            yield return new WaitForSeconds(0.001f);    //0.001f �� ��ٷȴ� ����

            Fade.color = color; //Fade�� ���� ���ο� color ������ ����
        }
        yield return null;                                        //�ڷ�ƾ ����
    }

    public IEnumerator FadeOut()    //ȭ�� ��ο����� ȿ��
    {
        Debug.Log("FadeOut");
        Fade.color = new Color32(29, 29, 29, 0); 
        Color color = Fade.color;   //color ������ �����Ͽ� Fade.color�� rgba ���� �����Ͽ� ����
        while (Fade.color.a !< 255) //Fade�� ������ 255���� ���� �ʴµ��� ��� ����
        {
            Debug.Log(color.a);
            color.a += Time.deltaTime * 0.45f;  //color�� ���� ���� Time.deltaTime�� ������ float ���� ���Ͽ� �ø�

            yield return new WaitForSeconds(0.001f);    //0.001f �� ��ٷȴ� ����

            Fade.color = color; //Fade�� ���� ���ο� color ������ ����
        }
        yield return null;                                        //�ڷ�ƾ ����
    }
}

