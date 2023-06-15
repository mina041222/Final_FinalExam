using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;     //�ٸ� ������ ������ �����ϰ� ����

    [Header("���")]
    [Space(10)]                             //Space�� �̿��ؼ� 10���� �Ÿ� ���� �߸���
    public GameObject[] stairs;             //Stairs��� ���� ����

    private enum State {Start, Left, Right}; //���� ���� ������ �����
    private State state;                     //state��� ���� ����
    private Vector3 oldPosition;              //Vector3�� ��ġ���� �־� ����



    // Start is called before the first frame update
    void Start()
    {
        Instance = this;                    //Instance�� �ڱ� �ڽ����� ���Խ�Ű��
        Init();                             //Init�Լ��� �̵�
        InitStairs();
    }

    private void Init()
    {
        state = State.Start;
        oldPosition = Vector3.zero;          //start ���� 0���� �ʱ�ȭ�Ѵ�
    }
    //ó�� ���۽� ����� �ʱ�ȭ �ϱ� ����
    private void InitStairs()
    {
        for (int i = 0; i < Stairs.Length; i++)
        {
            switch(State)
            {
                case State.Start:
                {
                    Stairs[i].transform.position = new Vector3(0.75f, -0.1f, 0);  //ù��° �����Ұ�쿡 ����� ��ġ���� Vector3�� ���� 
                    state State.Right;            //���ΰ��� ���������� �����ϱ� ������ state�� right���� ���Ѵ�
                    break;
                }

                case State.Left:
                {
                    Stairs[i].transform.position = oldPosition + new Vector3(-0.75f, -0.5f, 0); //��� 1ĭ�� �����ϱ� ������ 0.1f�� 0.5f�� �ٲ��ش�
                    break;
                }

                case State.Right:
                {
                    Stairs[i].transform.position = oldPosition + new Vector3(0.75f, -0.5f, 0);  //oldPosition(���� ��ġ��)�� vector3���� �����ش�
                    break;
                }

                oldPosition = Stairs[i].transform.position;                     //switch���� ���� �� ����
            }
        }
    }
}
