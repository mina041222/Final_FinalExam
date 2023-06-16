using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;     //�ٸ� ������ ������ �����ϰ� ����

    [Header("���")]
    [Space(10)]                             //Space�� �̿��ؼ� 10���� �Ÿ� ���� �߸���
    public GameObject[] Stairs;             //Stairs��� ���� ����
    public bool[] isTurn;                    //�迭 ������ֱ� ���� isTurn ���� �̸� �ۼ�

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

        isTurn = new bool[Stairs.Length];    //stairs�� �迭�� ���� ��ŭ �ʱ�ȭ ���ش�

        for(int i = 0; i < Stairs.Length; i++)     //����� �迭�� ���̸�ŭ �����ش�
        {
            Stairs[i].transform.position = Vector3.zero;    //����� ��ġ���� 0���� �ʱ�ȭ ���ش�
            isTurn[i] = false;
        }
    }

    //ó�� ���۽� ����� �ʱ�ȭ �ϱ� ����
    private void InitStairs()
    {
        for (int i = 0; i < Stairs.Length; i++)
        {
            switch(state)
            {
                case State.Start:

                    Stairs[i].transform.position = new Vector3(0.75f, -0.1f, 0);  //ù��° �����Ұ�쿡 ����� ��ġ���� Vector3�� ���� 
                    state = State.Right;            //���ΰ��� ���������� �����ϱ� ������ state�� right���� ���Ѵ�
                    break;
                

                case State.Left:

                    Stairs[i].transform.position = oldPosition + new Vector3(-0.75f, 0.5f, 0); //��� 1ĭ�� �����ϱ� ������ 0.1f�� 0.5f�� �ٲ��ش�
                    isTurn[i] = true;              //�����϶� isTurn�� true�� �������϶� isTurn�� false�� �ۼ�
                    break;


                case State.Right:

                    Stairs[i].transform.position = oldPosition + new Vector3(0.75f, 0.5f, 0);  //oldPosition(���� ��ġ��)�� vector3���� �����ش�
                    isTurn[i] = false;
                    break;

            }
            oldPosition = Stairs[i].transform.position;                     //switch���� ���� �� ����

            if(i != 0)
            {
                int ran = Random.Range(0,5);         //5���� �������� �Ѱ��� ���� ���������� ����

                //2���� ���� ���� ���� ������ ����
                if(ran < 2 && i < Stairs.Length - 1)       // i ���� ����� �迭���� 1�۰� �� (�迭�� ���̰� 20 ������ i���� 0���� 19�����̱� ������)
                {
                    state = state == State.Left? State.Right : State.Left; //���ǿ� ������ �Ǹ� state���� �����̸� ����, �������̸� ���������� �ǰ� �ۼ�
                }
            }
        }
    }

    //����� �����ɶ� InitStairs ����
    public void SpawnStair(int cnt) 
    {
         int ran = Random.Range(0,5);       //�������� �߰����� ����� �����ؾ��Ѵ�

         if(ran < 2)
         {
             state = state == State.Left? State.Right : State.Left;
         }

         switch(state)
         {
             case State.Left:
             
                 Stairs[cnt].transform.position = oldPosition + new Vector3(-0.75f, 0.5f, 0);
                 isTurn[cnt] = true;
                 break;
             
             case State.Right:
             {
                 Stairs[cnt].transform.position = oldPosition + new Vector3(0.75f, 0.5f, 0);
                 isTurn[cnt] = false;
                 break;
             }

         }
         oldPosition = Stairs[cnt].transform.position;
    }
}
