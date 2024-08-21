using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera vcama;
    public CinemachineVirtualCamera vcamb;

    public GameObject start;
    public GameObject end;

    public GameObject presenter;

    private float timer;
    private bool timerOn;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) // 스페이스바를 누를 때 전환
        {
            // vcamA에서 vcamB로 전환
            vcama.Priority = 9; // vcamA의 우선순위 낮춤
            vcamb.Priority = 10; // vcamB의 우선순위 높임
            
            presenter.SetActive(true);

            timerOn = true;
        }

        if (timerOn)
            timer += Time.deltaTime;

        if (timer >= 2.4f)
        {
            start.SetActive(true);
            end.SetActive(true);
            timerOn = false;
        }
    }
}
