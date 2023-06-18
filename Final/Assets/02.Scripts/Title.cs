using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string sceneName = "Game";                 //다음 씬으로 넘어가는 변수 선언

    public void ClickStart()
    {
        Debug.Log("로딩");
        SceneManager.LoadScene(sceneName);   
    }

    public void ClickShop()
    {
        Debug.Log("상점");
    }

    public void ClickExit()
    {
        Debug.Log("게임종료");
        Application.Quit();       //Application이 가장 큰 클래스
    }
}
