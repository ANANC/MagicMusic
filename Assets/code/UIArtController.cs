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

    private ChessboardController m_ChessboardController;

    public class UIInfo
    {
        public int ChessmanId;
        public GameObject GameObject;
        public Transform Transform;
        public Image Image;
    }

    private float CurUpdateInterval;
    private float m_CurInterval;
    private Vector3 MoveDirection;

    private List<UIInfo> UIInfoList = new List<UIInfo>();

    // Start is called before the first frame update
    void Start()
    {
        CurUpdateInterval = UpdateInterval;
        m_CurInterval = 0;

        CreateChessboard();

        MoveDirection = Vector3.right;

        //�������̱���
        UpdateChessboard();

        //���µ�������
        UpdateArrive();
    }

    // Update is called once per frame
    void Update()
    {
        InputControl();

        m_CurInterval += Time.deltaTime;
        if (m_CurInterval < CurUpdateInterval)
        {
            return;
        }

        m_CurInterval = 0;

        UpdateGame();
    }

    public void UpdateGame()
    {
        // �ƶ�
        m_ChessboardController.MoveChessboard(MoveDirection);

        //����·��
        m_ChessboardController.RefreshRoad(MoveDirection);

        //�������̱���
        UpdateChessboard();

        MoveArrive();
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

            uiInfo.GameObject.name = uiInfo.ChessmanId.ToString();

            UIInfoList.Add(uiInfo);
        }
    }

    //���µ����
    public void UpdateArrive()
    {
        Vector3 curArriveLogicPosition = m_ChessboardController.GetCurArrivePoint();
        Chessman chessman = m_ChessboardController.GetChessmanByLogicPosition(curArriveLogicPosition);
        if(chessman == null)
        {
            Debug.LogError("UpdateArrive Fail."+ curArriveLogicPosition + " can not find chessman!");
        }
        ArrivePoint.transform.localPosition = chessman.ArtInfo.ArtPosition + ChessboardRoot.localPosition;
    }

    public void MoveArrive()
    {
        Vector3 curArriveLogicPosition = m_ChessboardController.GetCurArrivePoint();
        curArriveLogicPosition.x += MoveDirection.x;
        curArriveLogicPosition.y += -MoveDirection.y;
        m_ChessboardController.AddArrivePoint(curArriveLogicPosition);

        //���µ�������
        UpdateArrive();
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

            //����
            uiInfo.Transform.Rotate(Vector3.forward * Random.Range(-0.2f, 0.2f));
        }
    }


    private void InputControl()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            MoveDirection = Vector3.up;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            MoveDirection = Vector3.down;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            MoveDirection = Vector3.right;
        }
    }

}
