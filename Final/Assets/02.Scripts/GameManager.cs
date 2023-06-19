using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement;

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

    [Header("UI")]
    [Space(10)]         //10��ŭ ���� �ְ� �ϱ�
    public GameObject UI_GameOver;   //���� ����â�� �����Ҽ� ���� �ְ� GameObject�� �ۼ�
    public TextMeshProUGUI textNowScore;
    public TextMeshProUGUI textMaxScore;
    public TextMeshProUGUI textShowScore;
    private int maxScore = 0;          //������ ����ϰ� ǥ�� �ϱ� ���� int�����ϰ� �ʱ�ȭ
    private int nowScore = 0;

    [Header("Audio")]
    [Space(10)]
    private AudioSource sound;
    public AudioClip bgmSound;
    public AudioClip dieSound;

    public string sceneName = "Main";       //Main������ ����

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;                    //Instance�� �ڱ� �ڽ����� ���Խ�Ű��

        sound = GetComponent<AudioSource>();

        Init();                             //Init�Լ��� �̵�
        InitStairs();

    }



    public void Init()
    {
        state = State.Start;
        oldPosition = Vector3.zero;          //start ���� 0���� �ʱ�ȭ�Ѵ�

        isTurn = new bool[Stairs.Length];    //stairs�� �迭�� ���� ��ŭ �ʱ�ȭ ���ش�

        for(int i = 0; i < Stairs.Length; i++)     //����� �迭�� ���̸�ŭ �����ش�
        {
            Stairs[i].transform.position = Vector3.zero;    //����� ��ġ���� 0���� �ʱ�ȭ ���ش�
            isTurn[i] = false;
        }

        nowScore = 0;           //NowScore�� 0���� �ʱ�ȭ ���ش�

        textShowScore.text = nowScore.ToString();     //���̴� ������ ToString(���ڿ��� ǥ��

        UI_GameOver.SetActive(false);

        sound.clip = bgmSound;
        sound.Play();
        sound.loop = true;   //�ݺ��ؼ� ��� �ǰ� ��

    }

    //ó�� ���۽� ����� �ʱ�ȭ �ϱ� ����
    public void InitStairs()
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


    public void GameOver()
    {
        sound.loop = false;
        sound.Stop();
        sound.clip = dieSound;
        sound.Play();

        StartCoroutine(ShowGameOver());

    }

    IEnumerator ShowGameOver()     //�ڷ�ƾ�� ����ϱ� ���� ���
    {
        yield return new WaitForSeconds(1f);

        UI_GameOver.SetActive(true);

        if(nowScore > maxScore)     //MaxScore ����
        {
            maxScore = nowScore;
        }

        textMaxScore.text = maxScore.ToString();
        textNowScore.text = nowScore.ToString();


    }

    public void AddScore()     //����� ���������� ���� �߰�
    {
        nowScore++;
        textShowScore.text = nowScore.ToString();

    }

    public void ClickMain()
    {
        Debug.Log("�������� �����!");
        SceneManager.LoadScene(sceneName);       //main���� ���� ����
    }
}
