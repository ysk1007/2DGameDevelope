using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor_Chracter : MonoBehaviour
{
    private int c_ColCount = 0;

    private float c_DisableTimer;

    private void OnEnable()
    {
        c_ColCount = 0;
    }

    public bool State()
    {
        if (c_DisableTimer > 0)
            return false;
        return c_ColCount > 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        c_ColCount++;
    }

    void OnTriggerExit2D(Collider2D other)
    {
       c_ColCount--;
    }

    void Update()
    {
        c_DisableTimer -= Time.deltaTime;
    }

    public void Disable(float duration)
    {
        c_DisableTimer = duration;
    }
}

