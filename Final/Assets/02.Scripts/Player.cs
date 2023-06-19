using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer spriteRenderer;     //sptriteRenderer에 접근해야함
    private Vector3 startPosition;            //캐릭터가 이동하게 되므로 이동하게 되면 위치값을 알아야하니 startPosition과 oldPosition 선언 
    private Vector3 oldPosition;
    private bool isTurn = false;           //캐릭터가 왼쪽인지 오른쪽인지 체크해주기 위해 변수 선언

    private int moveCnt = 0;               //moveCount 0으로 초기화
    private int turnCnt = 0;               //turnCount 0으로 초기화
    private int spawnCnt = 0;              //spawnCount 0으로 초기화 

    private bool isDie = false; 

    private AudioSource sound;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();        //anim에 GetComponeet를 이용해 Animator을 받아온다
        spriteRenderer = GetComponent<SpriteRenderer>();    //spriteRendere에 SpriteRenderer 정보를 받아온다
        startPosition = transform.position;                 //스타트 함수에 스타트 포지션 값에 시작시킬 위치 대입
        sound = GetComponent<AudioSource>();

        Init();

    }

    // Update is called once per frame
    //void Update() 
    //{
        //if(Input.GetMouseButtonDown(0))
        //{
            //CharTurn();       //캐릭터 돌기 작성
        //}

        //else if(Input.GetKeyDown(KeyCode.Space))
        //{
            //CharMove();       //캐릭터 움직임 작성
        //}
    //}

    private void Init()
    {
        anim.SetBool("Die",false);
        transform.position = startPosition;     // 케릭터 위치에 startPosition 값을  대입
        oldPosition = startPosition;            //oldPosition에 startposition 값을 대입
        moveCnt = 0;
        spawnCnt = 0;
        turnCnt = 0;
        isTurn = false;
        spriteRenderer.flipX = isTurn;
        isDie = false;
    }

    public void CharTurn()
    {
        isTurn = isTurn == true? false : true;   //isTurn이 ture면 false로 번경하고 false면 true로 번경하도록 한다 

        spriteRenderer.flipX = isTurn;          //spriteRenderer의 filpX에 isTrue를 대입한다
    }

    public void CharMove()
    {
        if(isDie)     //isDie가 true라면 return을 이용해서 빠져나가기 
        {
            return; 
        }
       
        sound.Play();

        moveCnt++;               //캐릭터 move함수가 호출 될때마다 moveCnt는 증가 시켜준다
        MoveDirection();

        if(isFailTurn())         //잘못된 방향으로가면 사망
        {
            CharDie();
            return;              //함수를 빠져나가게 하는것 
        }

        if(moveCnt > 5)          //moveCnt가 5 이상일시 계단 스폰
        {
            RespawnStair();
        }

        GameManager.Instance.AddScore();
    }

    private void MoveDirection()          //방향에 대한 함수 작성
    {
        if(isTurn)  //왼쪽으로 바뀐다
        {
            oldPosition += new Vector3(-0.75f, 0.5f,0);            //왼쪽이기 때문에 x값을 -로 작성
            //더해준 값을 대입
        }
        else
        {
             oldPosition += new Vector3(0.75f, 0.5f,0);      //오른쪽이기 때문에 x값 양수
        }

        transform.position = oldPosition;
        anim.SetTrigger("Move");             //실행될시 애니메이션 발생

    }

    private bool isFailTurn()
    {
        bool resurt = false;

        //이 배열값과 isTurn과 같지 않다면 resurt값이 true로 바뀜
        if(GameManager.Instance.isTurn[turnCnt] != isTurn) //조건이 맞지 않으면 turnCnt를 증가 시킴
        {
            resurt = true;
        }

        turnCnt++;
        
        if(turnCnt > GameManager.Instance.Stairs.Length - 1) //0 ~ 19 length == 20  -1 뺀 값보다 크게 되면 turnCnt는 0으로 초기화 해준다
        {
            turnCnt = 0;
        }
        
        return resurt;
    }

    private void RespawnStair()
    {
        GameManager.Instance.SpawnStair(spawnCnt);    //스폰 스테어 함수 작성
        //스폰 되어지는 계단을 카운트 해야함

        spawnCnt++;

        if(spawnCnt > GameManager.Instance.Stairs.Length - 1)
        {
            spawnCnt = 0;
        }
    }

    private void CharDie()
    {
        GameManager.Instance.GameOver();     //게임 오버 함수 호출
        anim.SetBool("Die",true);
        isDie = true;
    }

    //버튼 Restart를 누르면 다시 시작
    public void ButtonRestart()
    {
        Init();
        GameManager.Instance.Init();
        GameManager.Instance.InitStairs();
    }
}
