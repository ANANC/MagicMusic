using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ChessboardControllerData;

public class ChessboardFactory 
{
    private int AutoChessmanId; //�Զ�����id

    public void Init()
    {
        AutoChessmanId = 1;
    }

    public void UnInit()
    {

    }


    //������������
    public void CreateBaseChessboard(ref Chessboard chessboard)
    {
        Vector2 chessboardSize = ResourceController.Instance.GetGamePlayDefine().ChessboardSize;

        for (int x = 0; x < chessboardSize.x; x++)
        {
            for (int y = 0; y < chessboardSize.y; y++)
            {
                Chessman chessman = CreateBaseChessman(new Vector3(x, y, 0));

                chessboard.AllChessmanList.Add(chessman);

                //����
                chessboard.Id2ChessmanDict.Add(chessman.Id, chessman);
                chessboard.LogicPosition2ChessmanDict.Add(chessman.LogicPosition, chessman);

                //����
                List<Chessman> chessmanList;
                if (!chessboard.Row2ChessmanDict.TryGetValue(x, out chessmanList))
                {
                    chessmanList = new List<Chessman>();
                }
                chessmanList.Add(chessman);

                if (!chessboard.Column2ChessmanDict.TryGetValue(y, out chessmanList))
                {
                    chessmanList = new List<Chessman>();
                }
                chessmanList.Add(chessman);
            }
        }
    }


    //������������
    public Chessman CreateBaseChessman(Vector3 logicPosition)
    {
        Chessman chessman = new Chessman();
        ChessmanArt chessmanArt = new ChessmanArt();

        chessman.Id = AutoChessmanId++;
        chessman.ArtInfo = chessmanArt;

        UpdateChessman(ref chessman, logicPosition);

        return chessman;
    }

    //�������ӻ�����Ϣ
    public void UpdateChessman(ref Chessman chessman, Vector3 logicPosition)
    {
        ChessmanArt chessmanArt = chessman.ArtInfo;

        //�߼�
        chessman.LogicPosition = logicPosition;
        chessman.Index = GetIdByLogicPosition(logicPosition);
        chessman.RowIndex = (int)logicPosition.x;
        chessman.ColumnIndex = (int)logicPosition.y;
        chessman.EnableArrive = false;

        //����
        int chessmanArtCount = ResourceController.Instance.GetChessmanCount();
        chessmanArt.ResourceIndex = Random.Range(0, chessmanArtCount);
    }

    //ͨ��logicposition�õ�id
    public int GetIdByLogicPosition(Vector3 logicPosition)
    {
        int x = (int)logicPosition.x * 1000;
        int y = (int)logicPosition.y;

        int id = x + y;
        return id;
    }
}
