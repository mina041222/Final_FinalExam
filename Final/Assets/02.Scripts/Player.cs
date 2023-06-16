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

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();        //anim�� GetComponenet�� �̿��� Animator�� �޾ƿ´�
        spriteRenderer = GetComponent<SpriteRenderer>();    //spriteRendere�� SpriteRenderer ������ �޾ƿ´� 
        startPosition = transform.position;            //��ŸƮ �Լ��� ��ŸƮ ������ ���� ���۽�ų ��ġ ����
        oldPosition = transform.localPosition;          //oldPosition�� ���� ���� ����

    }

    // Update is called once per frame
    void Update() 
    {
        if(Input.GetMouseButtonDown(0))
        {
            CharTurn();       //ĳ���� ���� �ۼ�
        }
        else if(Input.GetKeyDown(KeyCode.Space))     
        {
            CharMove();       //ĳ���� ������ �ۼ�
        }
    }

    private void CharTurn()
    {
        isTurn = isTurn == true? false : true;   //isTurn�� ture�� false�� �����ϰ� false�� true�� �����ϵ��� �Ѵ� 

        spriteRenderer.flipX = isTurn;          //spriteRenderer�� filpX�� isTrue�� �����Ѵ�
    }

    private void CharMove()
    {
        moveCnt++;               //ĳ���� move�Լ��� ȣ�� �ɶ����� moveCnt�� ���� �����ش�
        MoveDirection();

        if(isFailTurn())         //�߸��� �������ΰ��� ���
        {
            anim.SetBool("Die",true);
            Debug.Log("�׾���!");
            return;              //�Լ��� ���������� �ϴ°� 
        }

        if(moveCnt > 5)          //moveCnt�� 5 �̻��Ͻ� ��� ����
        {
            RespawnStair();
        }
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
}
