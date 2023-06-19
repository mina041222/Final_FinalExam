using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer spriteRenderer;     //sptriteRenderer�� �����ؾ���
    private Vector3 startPosition;            //ĳ���Ͱ� �̵��ϰ� �ǹǷ� �̵��ϰ� �Ǹ� ��ġ���� �˾ƾ��ϴ� startPosition�� oldPosition ���� 
    private Vector3 oldPosition;
    private bool isTurn = false;           //ĳ���Ͱ� �������� ���������� üũ���ֱ� ���� ���� ����

    private int moveCnt = 0;               //moveCount 0���� �ʱ�ȭ
    private int turnCnt = 0;               //turnCount 0���� �ʱ�ȭ
    private int spawnCnt = 0;              //spawnCount 0���� �ʱ�ȭ 

    private bool isDie = false; 

    private AudioSource sound;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();        //anim�� GetComponeet�� �̿��� Animator�� �޾ƿ´�
        spriteRenderer = GetComponent<SpriteRenderer>();    //spriteRendere�� SpriteRenderer ������ �޾ƿ´�
        startPosition = transform.position;                 //��ŸƮ �Լ��� ��ŸƮ ������ ���� ���۽�ų ��ġ ����
        sound = GetComponent<AudioSource>();

        Init();

    }

    // Update is called once per frame
    //void Update() 
    //{
        //if(Input.GetMouseButtonDown(0))
        //{
            //CharTurn();       //ĳ���� ���� �ۼ�
        //}

        //else if(Input.GetKeyDown(KeyCode.Space))
        //{
            //CharMove();       //ĳ���� ������ �ۼ�
        //}
    //}

    private void Init()
    {
        anim.SetBool("Die",false);
        transform.position = startPosition;     // �ɸ��� ��ġ�� startPosition ����  ����
        oldPosition = startPosition;            //oldPosition�� startposition ���� ����
        moveCnt = 0;
        spawnCnt = 0;
        turnCnt = 0;
        isTurn = false;
        spriteRenderer.flipX = isTurn;
        isDie = false;
    }

    public void CharTurn()
    {
        isTurn = isTurn == true? false : true;   //isTurn�� ture�� false�� �����ϰ� false�� true�� �����ϵ��� �Ѵ� 

        spriteRenderer.flipX = isTurn;          //spriteRenderer�� filpX�� isTrue�� �����Ѵ�
    }

    public void CharMove()
    {
        if(isDie)     //isDie�� true��� return�� �̿��ؼ� ���������� 
        {
            return; 
        }
       
        sound.Play();

        moveCnt++;               //ĳ���� move�Լ��� ȣ�� �ɶ����� moveCnt�� ���� �����ش�
        MoveDirection();

        if(isFailTurn())         //�߸��� �������ΰ��� ���
        {
            CharDie();
            return;              //�Լ��� ���������� �ϴ°� 
        }

        if(moveCnt > 5)          //moveCnt�� 5 �̻��Ͻ� ��� ����
        {
            RespawnStair();
        }

        GameManager.Instance.AddScore();
    }

    private void MoveDirection()          //���⿡ ���� �Լ� �ۼ�
    {
        if(isTurn)  //�������� �ٲ��
        {
            oldPosition += new Vector3(-0.75f, 0.5f,0);            //�����̱� ������ x���� -�� �ۼ�
            //������ ���� ����
        }
        else
        {
             oldPosition += new Vector3(0.75f, 0.5f,0);      //�������̱� ������ x�� ���
        }

        transform.position = oldPosition;
        anim.SetTrigger("Move");             //����ɽ� �ִϸ��̼� �߻�

    }

    private bool isFailTurn()
    {
        bool resurt = false;

        //�� �迭���� isTurn�� ���� �ʴٸ� resurt���� true�� �ٲ�
        if(GameManager.Instance.isTurn[turnCnt] != isTurn) //������ ���� ������ turnCnt�� ���� ��Ŵ
        {
            resurt = true;
        }

        turnCnt++;
        
        if(turnCnt > GameManager.Instance.Stairs.Length - 1) //0 ~ 19 length == 20  -1 �� ������ ũ�� �Ǹ� turnCnt�� 0���� �ʱ�ȭ ���ش�
        {
            turnCnt = 0;
        }
        
        return resurt;
    }

    private void RespawnStair()
    {
        GameManager.Instance.SpawnStair(spawnCnt);    //���� ���׾� �Լ� �ۼ�
        //���� �Ǿ����� ����� ī��Ʈ �ؾ���

        spawnCnt++;

        if(spawnCnt > GameManager.Instance.Stairs.Length - 1)
        {
            spawnCnt = 0;
        }
    }

    private void CharDie()
    {
        GameManager.Instance.GameOver();     //���� ���� �Լ� ȣ��
        anim.SetBool("Die",true);
        isDie = true;
    }

    //��ư Restart�� ������ �ٽ� ����
    public void ButtonRestart()
    {
        Init();
        GameManager.Instance.Init();
        GameManager.Instance.InitStairs();
    }
}
