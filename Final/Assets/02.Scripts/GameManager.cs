using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;     //다른 곳에서 접근이 가능하게 만듬

    [Header("계단")]
    [Space(10)]                             //Space를 이용해서 10정도 거리 떨어 뜨리기
    public GameObject[] stairs;             //Stairs라는 변수 선언

    private enum State {Start, Left, Right}; //게임 내의 설정을 만들기
    private State state;                     //state라는 변수 선언
    private Vector3 oldPosition;              //Vector3에 위치값을 넣어 대입



    // Start is called before the first frame update
    void Start()
    {
        Instance = this;                    //Instance를 자기 자신으로 대입시키기
        Init();                             //Init함수로 이동
        InitStairs();
    }

    private void Init()
    {
        state = State.Start;
        oldPosition = Vector3.zero;          //start 값을 0으로 초기화한다
    }
    //처음 시작시 계단을 초기화 하기 위함
    private void InitStairs()
    {
        for (int i = 0; i < Stairs.Length; i++)
        {
            switch(State)
            {
                case State.Start:
                {
                    Stairs[i].transform.position = new Vector3(0.75f, -0.1f, 0);  //첫번째 시작할경우에 계단의 위치값을 Vector3로 정함 
                    state State.Right;            //주인공이 오른쪽으로 가야하기 때문에 state에 right값을 더한다
                    break;
                }

                case State.Left:
                {
                    Stairs[i].transform.position = oldPosition + new Vector3(-0.75f, -0.5f, 0); //계단 1칸을 가야하기 때문에 0.1f을 0.5f로 바꿔준다
                    break;
                }

                case State.Right:
                {
                    Stairs[i].transform.position = oldPosition + new Vector3(0.75f, -0.5f, 0);  //oldPosition(원래 위치값)에 vector3값을 더해준다
                    break;
                }

                oldPosition = Stairs[i].transform.position;                     //switch문이 끝날 시 실행
            }
        }
    }
}
