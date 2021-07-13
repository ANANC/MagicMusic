using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ChessboardControllerData;

public class UIArtController : MonoBehaviour
{
    [Header("���̸��ڵ�")]
    public Transform ChessboardRoot;

    [Header("�����")]
    public Transform ArrivePoint;

    [Header("����Ƶ��")]
    public float UpdateInterval;

    [Header("��Ч���ڵ�")]
    public Transform AudioSourceRoot;

    [Header("����UI")]
    public Transform Finish;

    private ChessboardController m_ChessboardController;
    private List<AudioSource> ActiveAudioSourceList = new List<AudioSource>();
    private List<AudioSource> SleepAudioSourceList = new List<AudioSource>();

    private ResourceColorControllerObject ResourceColorControllerObject;

    public class UIInfo
    {
        public int ChessmanId;
        public GameObject GameObject;
        public Transform Transform;
        public Image Image;

        public Image MusicImage;
    }

    private float CurUpdateInterval;
    private float m_CurInterval;
    private Vector3 MoveDirection;
    private int TimeIndex;
    private Vector3 CurArrivePoint;
    private bool IsTouch;

    private List<UIInfo> UIInfoList = new List<UIInfo>();
    private Dictionary<int, AudioSource> PlayMusicDict = new Dictionary<int, AudioSource>();

    private Image ArrivePointImage;
    private float FinishRotationValue;

    // Start is called before the first frame update
    void Start()
    {
        CurUpdateInterval = UpdateInterval;
        m_CurInterval = 0;
        TimeIndex = 0;
        IsTouch = false;

        FinishRotationValue = Random.value;

        ResourceColorControllerObject = ResourceController.Instance.GetColorDefine();

        ArrivePointImage = ArrivePoint.GetComponent<Image>();

        CreateChessboard();

        MoveDirection = Vector3.zero;

        //�������̱���
        UpdateChessboard();

        //���µ�������
        UpdateArrive();
    }

    // Update is called once per frame
    void Update()
    {
        Tweens();

        InputControl();

        m_CurInterval += Time.deltaTime;
        if (m_CurInterval < CurUpdateInterval)
        {
            return;
        }

        TimeIndex += 1;
        m_CurInterval = 0;

        UpdateGame();
    }

    public void UpdateGame()
    {
        if (m_ChessboardController.EnableMove())
        {
            // �ƶ�
            m_ChessboardController.MoveChessboard(MoveDirection);

            //����·�� //TimeIndex % 30 == 0
            m_ChessboardController.RefreshRoad(MoveDirection, false);

            //�������̱���
            UpdateChessboard();
        }

        //���µ�������
        UpdateArrive();

    }


    //��������
    public void CreateChessboard()
    {
        //�����߼�
        m_ChessboardController = new ChessboardController();
        m_ChessboardController.Init();
        m_ChessboardController.CreateChessboard(ChessboardControllerData.ChessboardType.Random);

        //����UI
        __CreateUIChessboard();
    }

    //����UI����
    private void __CreateUIChessboard()
    {
        GameObject chessmanGameobject = ResourceController.Instance.GetChessmanUIGameObject();

        //����UI
        List<Chessman> chessmenList = m_ChessboardController.GetAllChessman();
        for (int index = 0; index < chessmenList.Count; index++)
        {
            Chessman chessman = chessmenList[index];

            UIInfo uiInfo = new UIInfo();
            uiInfo.ChessmanId = chessman.Id;
            uiInfo.GameObject = GameObject.Instantiate(chessmanGameobject);
            uiInfo.Transform = uiInfo.GameObject.transform;
            uiInfo.Transform.SetParent(ChessboardRoot);
            uiInfo.Image = uiInfo.GameObject.GetComponent<Image>();
            uiInfo.Image.sprite = ResourceController.Instance.GetChessmanByIndex(chessman.ArtInfo.ResourceIndex);

           // uiInfo.Transform.Rotate(new Vector3(0, 0, Random.Range(0, 180)));

            uiInfo.GameObject.name = uiInfo.ChessmanId.ToString();

            if(chessman.MusicalNote!=null)
            {
                uiInfo.MusicImage = uiInfo.Transform.Find("music").gameObject.GetComponent<Image>();
                uiInfo.MusicImage.sprite = ResourceController.Instance.GetMusicUIByIndex(chessman.MusicalNote.ArtInfo.ResourceIndex);
                uiInfo.MusicImage.color = chessman.MusicalNote.ArtInfo.TexColor;
                uiInfo.MusicImage.gameObject.SetActive(false);
            }

            UIInfoList.Add(uiInfo);
        }
    }


    //��������
    public void UpdateChessboard()
    {
        //ˢ�±���
        m_ChessboardController.RefreshRenderChessboard();

        for (int index = 0; index < UIInfoList.Count; index++)
        {
            UIInfo uiInfo = UIInfoList[index];
            Chessman chessman = m_ChessboardController.GetChessmanById(uiInfo.ChessmanId);
            ChessmanArt chessmanArt = chessman.ArtInfo;

            uiInfo.Transform.localPosition = chessmanArt.ArtPosition;
            uiInfo.Image.color = chessmanArt.TexColor;
        }
    }

    //��������
    public void Tweens()
    {
        if (!m_ChessboardController.EnableMove())
        {
            Finish.Rotate(Vector3.forward * FinishRotationValue);
            FinishRotationValue = -FinishRotationValue;
        }

        //ˢ�±���
        m_ChessboardController.RefreshRenderChessboard();

        for (int index = 0; index < UIInfoList.Count; index++)
        {
            UIInfo uiInfo = UIInfoList[index];
            Chessman chessman = m_ChessboardController.GetChessmanById(uiInfo.ChessmanId);

            //����
            uiInfo.Transform.Rotate(Vector3.forward * Random.Range(-0.3f, 0.3f));

            MusicalNote musicalNote = chessman.MusicalNote;
            if (musicalNote != null)
            {
                bool show = chessman.EnableArrive;
                uiInfo.MusicImage.gameObject.SetActive(show);

                if (show)
                {
                    uiInfo.MusicImage.transform.Rotate(Vector3.forward * Random.Range(-0.3f, 0.3f));


                }

            }
        }

        Color arrivePointColor = IsTouch ? ResourceColorControllerObject.ArrivePointTouchColor : ResourceColorControllerObject.ArrivePointNormalColor;

        ArrivePoint.Rotate(Vector3.forward * Random.Range(-50f, 50f));
        ArrivePointImage.color = arrivePointColor;
    }

    //���µ����
    public void UpdateArrive()
    {
        Vector3 curArriveLogicPosition = m_ChessboardController.GetCurArrivePoint();
        Chessman chessman = m_ChessboardController.GetChessmanByLogicPosition(curArriveLogicPosition);
        if (chessman == null || !chessman.EnableArrive)
        {
            if (chessman == null)
            {
                //Debug.LogError("UpdateArrive Fail."+ curArriveLogicPosition + " can not find chessman!");
            }

            //��Ѫ����ֹͣ�ƶ�
            StopMove();
            return;
        }

        ArrivePoint.transform.localPosition = chessman.ArtInfo.ArtPosition + ChessboardRoot.localPosition;

        IsTouch = false;

        if (chessman.MusicalNote != null)
        {
            IsTouch = true;

            bool playSounce = CurArrivePoint != curArriveLogicPosition;
            CurArrivePoint = curArriveLogicPosition;

            if (playSounce)
            {
                PlayMusic(chessman.MusicalNote);
            }
        }
    }

    //�ƶ������
    public void MoveArrive()
    {
        Vector3 curArriveLogicPosition = m_ChessboardController.GetCurArrivePoint();
        curArriveLogicPosition.x += MoveDirection.x;
        curArriveLogicPosition.y += -MoveDirection.y;

        Chessman chessman = m_ChessboardController.GetChessmanByLogicPosition(curArriveLogicPosition);
        if (chessman == null || !chessman.EnableArrive)
        {
            //��Ѫ����
            StopMove();
            return;
        }

        m_ChessboardController.AddArrivePoint(curArriveLogicPosition);

        //���µ�������
        UpdateArrive();
    }


    private void InputControl()
    {
        Vector3 newDirction = Vector3.zero;

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            newDirction = Vector3.up;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            newDirction = Vector3.down;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            newDirction = Vector3.right;
        }

        if (newDirction != Vector3.zero)
        {
            if (MoveDirection == Vector3.zero)
            {
                m_ChessboardController.StartMove();
            }
            else
            {
                if (!m_ChessboardController.EnableMove())
                {
                    return;
                }
            }
            MoveDirection = newDirction;
            MoveArrive();
        }
    }

    private void StopMove()
    {
        m_ChessboardController.StopMove();
        Finish.gameObject.SetActive(true);
    }

    private void PlayMusic(MusicalNote musicalNote)
    {
        AudioSource audioSource;

        bool isPlay = true;
        if(!PlayMusicDict.TryGetValue(musicalNote.MusicIndex,out audioSource))
        {
            if (SleepAudioSourceList.Count == 0)
            {
                GameObject gameObject = new GameObject();
                gameObject.transform.SetParent(AudioSourceRoot);
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            else
            {
                audioSource = SleepAudioSourceList[0];
                SleepAudioSourceList.RemoveAt(0);
            }

            ActiveAudioSourceList.Add(audioSource);
            PlayMusicDict.Add(musicalNote.MusicIndex, audioSource);

            audioSource.clip = ResourceController.Instance.GetMusicByIndex(musicalNote.MusicIndex);

            isPlay = false;
        }

        StartCoroutine(PlaySounce(audioSource, isPlay, musicalNote.MusicTime));

    }

    private IEnumerator PlaySounce(AudioSource audioSource,bool isPlay,float time)
    {
        
        if(isPlay)
        {
            audioSource.UnPause();
        }
        else
        {
            audioSource.Play();
        }
        yield return new WaitForSeconds(time);

        audioSource.Pause();

        //ActiveAudioSourceList.Remove(audioSource);
        //SleepAudioSourceList.Add(audioSource);
    }
}
