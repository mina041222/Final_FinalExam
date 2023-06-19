using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;     //다른 곳에서 접근이 가능하게 만듬

    [Header("계단")]
    [Space(10)]                             //Space를 이용해서 10정도 거리 떨어 뜨리기
    public GameObject[] Stairs;             //Stairs라는 변수 선언
    public bool[] isTurn;                    //배열 만들어주기 위해 isTurn 변수 이름 작성

    private enum State {Start, Left, Right}; //게임 내의 설정을 만들기
    private State state;                     //state라는 변수 선언
    private Vector3 oldPosition;              //Vector3에 위치값을 넣어 대입

    [Header("UI")]
    [Space(10)]         //10만큼 간격 있게 하기
    public GameObject UI_GameOver;   //게임 오버창을 시작할수 끌수 있게 GameObject로 작성
    public TextMeshProUGUI textNowScore;
    public TextMeshProUGUI textMaxScore;
    public TextMeshProUGUI textShowScore;
    private int maxScore = 0;          //점수를 등록하고 표시 하기 위해 int선언하고 초기화
    private int nowScore = 0;

    [Header("Audio")]
    [Space(10)]
    private AudioSource sound;
    public AudioClip bgmSound;
    public AudioClip dieSound;

    public string sceneName = "Main";       //Main씬으로 복구

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;                    //Instance를 자기 자신으로 대입시키기

        sound = GetComponent<AudioSource>();

        Init();                             //Init함수로 이동
        InitStairs();

    }



    public void Init()
    {
        state = State.Start;
        oldPosition = Vector3.zero;          //start 값을 0으로 초기화한다

        isTurn = new bool[Stairs.Length];    //stairs의 배열의 길이 만큼 초기화 해준다

        for(int i = 0; i < Stairs.Length; i++)     //계단의 배열의 길이만큼 돌아준다
        {
            Stairs[i].transform.position = Vector3.zero;    //계단의 위치값을 0으로 초기화 해준다
            isTurn[i] = false;
        }

        nowScore = 0;           //NowScore을 0으로 초기화 해준다

        textShowScore.text = nowScore.ToString();     //보이는 점수를 ToString(문자열로 표기

        UI_GameOver.SetActive(false);

        sound.clip = bgmSound;
        sound.Play();
        sound.loop = true;   //반복해서 재생 되게 함

    }

    //처음 시작시 계단을 초기화 하기 위함
    public void InitStairs()
    {
        for (int i = 0; i < Stairs.Length; i++)
        {
            switch(state)
            {
                case State.Start:

                    Stairs[i].transform.position = new Vector3(0.75f, -0.1f, 0);  //첫번째 시작할경우에 계단의 위치값을 Vector3로 정함 
                    state = State.Right;            //주인공이 오른쪽으로 가야하기 때문에 state에 right값을 더한다
                    break;
                

                case State.Left:

                    Stairs[i].transform.position = oldPosition + new Vector3(-0.75f, 0.5f, 0); //계단 1칸을 가야하기 때문에 0.1f을 0.5f로 바꿔준다
                    isTurn[i] = true;              //왼쪽일때 isTurn은 true로 오른쪽일때 isTurn은 false로 작성
                    break;


                case State.Right:

                    Stairs[i].transform.position = oldPosition + new Vector3(0.75f, 0.5f, 0);  //oldPosition(원래 위치값)에 vector3값을 더해준다
                    isTurn[i] = false;
                    break;

            }
            oldPosition = Stairs[i].transform.position;                     //switch문이 끝날 시 실행

            if(i != 0)
            {
                int ran = Random.Range(0,5);         //5개중 랜덤으로 한개의 값을 지역변수로 대입

                //2보다 작은 값이 오면 조건이 성립
                if(ran < 2 && i < Stairs.Length - 1)       // i 값이 계단의 배열보다 1작게 함 (배열의 길이가 20 이지만 i값은 0부터 19까지이기 때문에)
                {
                    state = state == State.Left? State.Right : State.Left; //저건에 들어오게 되면 state값이 왼쪽이면 왼쪽, 오른쪽이면 오른쪽으로 되게 작성
                }
            }
        }
    }

    //계단이 생성될때 InitStairs 생성
    public void SpawnStair(int cnt) 
    {
         int ran = Random.Range(0,5);       //렌덤으로 추가적인 계단을 생성해야한다

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

    IEnumerator ShowGameOver()     //코루틴을 사용하기 위해 사용
    {
        yield return new WaitForSeconds(1f);

        UI_GameOver.SetActive(true);

        if(nowScore > maxScore)     //MaxScore 설정
        {
            maxScore = nowScore;
        }

        textMaxScore.text = maxScore.ToString();
        textNowScore.text = nowScore.ToString();


    }

    public void AddScore()     //계단을 오를때마다 점수 추가
    {
        nowScore++;
        textShowScore.text = nowScore.ToString();

    }

    public void ClickMain()
    {
        Debug.Log("메인으로 가즈아!");
        SceneManager.LoadScene(sceneName);       //main으로 가게 해줌
    }
}
