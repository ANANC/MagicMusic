using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ChessboarControllerData;

public class ChessboarController
{
    private ChessboarControllerUtil ChessboarControllerUtil;

    private Chessboard Chessboard;
    private Vector2 ChessboardSize;

    private List<Vector3> ArriveList;    //行走的路线

    private void Init()
    {
        Chessboard = new Chessboard();
        ArriveList = new List<Vector3>();

        ChessboarControllerUtil = new ChessboarControllerUtil();
        ChessboarControllerUtil.Init();

    }

    private void UnInit()
    {
        ChessboarControllerUtil.UnInit();
    }

    public void CreateChessboard(ChessboardType chessboardType, Vector2 size)
    {
        ChessboardSize = size;
        Chessboard.m_ChessboardType = chessboardType;

    }

    private void CreateBaseChessboard()
    {
        for(int x = 0;x < ChessboardSize.x;x++)
        {
            for(int y = 0;y <ChessboardSize.y;y++)
            {

            }
        }
    }

    private void CreateBaseChessman(Vector3 logicPosition)
    {
        Chessman chessman = new Chessman();

        chessman.LogicPosition = logicPosition;
        chessman.Id = ChessboarControllerUtil.GetIdByLogicPosition(logicPosition);
        chessman.RowIndex = (int)logicPosition.x;
        chessman.ColumnIndex = (int)logicPosition.y;
        chessman.EnableArrive = false;

        ChessmanArt chessmanArt = new ChessmanArt();
        chessmanArt.ResourceIndex = ResourceController.Instance.GetChessmanCount();
    }
}
