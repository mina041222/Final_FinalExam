using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string sceneName = "Game";                 //���� ������ �Ѿ�� ���� ����

    public void ClickStart()
    {
        Debug.Log("�ε�");
        SceneManager.LoadScene(sceneName);   
    }

    public void ClickShop()
    {
        Debug.Log("����");
    }

    public void ClickExit()
    {
        Debug.Log("��������");
        Application.Quit();       //Application�� ���� ū Ŭ����
    }
}
