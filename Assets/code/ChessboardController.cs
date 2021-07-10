using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ChessboardControllerData;

public class ChessboardController
{
    private ResourceGameplayControllerObject m_ResourceGameplayControllerObject;
    private ResourceColorControllerObject m_ResourceColorControllerObject;

    private ChessboardControllerUtil ChessboarControllerUtil;
    private ChessboardFactory ChessboarFactory;

    private Chessboard Chessboard;

    Dictionary<Chessman, Vector3> Chessman2NewLogicPositionDict;    //棋子对应的新位置
    private List<Vector3> ArriveList;    //行走的路线

    public void Init()
    {
        Chessboard = new Chessboard();
        ArriveList = new List<Vector3>();
        Chessman2NewLogicPositionDict = new Dictionary<Chessman, Vector3>();

        ChessboarControllerUtil = new ChessboardControllerUtil();
        ChessboarControllerUtil.Init();

        ChessboarFactory = new ChessboardFactory();
        ChessboarFactory.Init();

        m_ResourceGameplayControllerObject = ResourceController.Instance.GetGamePlayDefine();
        m_ResourceColorControllerObject = ResourceController.Instance.GetColorDefine();
    }

    public void UnInit()
    {
        ChessboarControllerUtil.UnInit();
        ChessboarFactory.UnInit();
    }

    //创建棋盘
    public void CreateChessboard(ChessboardType chessboardType)
    {
        Chessboard.m_ChessboardType = chessboardType;

        Vector2 chessboardSize = ResourceController.Instance.GetGamePlayDefine().ChessboardSize;
        Chessboard.CurHandRowIndex = 0;
        Chessboard.CurLastRowIndex = (int)chessboardSize.x - 1;
        Chessboard.CurHandColumnIndex = 0;
        Chessboard.CurLastColumnIndex = (int)chessboardSize.y - 1;

        ChessboarFactory.CreateBaseChessboard(ref Chessboard);

        RefreshRoad(Vector3.zero,true);
    }

    //刷新棋盘表现
    public void RefreshRenderChessboard()
    {
        Vector3 logicPosition = new Vector3();
        for (int x = Chessboard.CurHandRowIndex; x <= Chessboard.CurLastRowIndex; x++)
        {
            for (int y = Chessboard.CurHandColumnIndex; y <= Chessboard.CurLastColumnIndex; y++)
            {
                logicPosition.x = x;
                logicPosition.y = y;

                Chessman chessman;
                if (!Chessboard.LogicPosition2ChessmanDict.TryGetValue(logicPosition, out chessman))
                {
                    Debug.LogError("RenderChessboard Fail. " + logicPosition + " can not find chessman!" +
                        "CurHandRowIndex:" + Chessboard.CurHandRowIndex + "CurLastRowIndex:" + Chessboard.CurLastRowIndex + "CurHandColumnIndex:" + Chessboard.CurHandColumnIndex + "CurLastColumnIndex:" + Chessboard.CurLastColumnIndex);
                    continue;
                }

              //  Debug.Log("RefreshRenderChessboard." + "Id:"+ chessman.Id + "EnableArrive:"+ chessman.EnableArrive);

                chessman.ArtInfo.ArtPosition = ChessboarControllerUtil.GetDynamicArtPosition(chessman.LogicPosition, m_ResourceGameplayControllerObject.SortChessmanSize, Chessboard.CurHandRowIndex, Chessboard.CurHandColumnIndex);
                chessman.ArtInfo.TexColor = chessman.EnableArrive ? m_ResourceColorControllerObject.ChessmanEnableArriveColor : m_ResourceColorControllerObject.ChessmanDisablearriveColor;
            }
        }
    }

    //移动棋盘
    public void MoveChessboard(Vector3 speed)
    {
        //不可向右移
        if (speed.x < 0)
        {
            return;
        }

        Chessman2NewLogicPositionDict.Clear();

        Vector2 chessboardSize = m_ResourceGameplayControllerObject.ChessboardSize;

        int xShifting = (int)speed.x;
        int yShifting = Mathf.Abs((int)speed.y);

        Vector3 logicPosition = new Vector3();
        Vector3 newLogixPosition = new Vector3();

        //x方向移动 移动列 （x > 0 == 向左移 == 将最左行移动到最右行）
        for (int xIndex = 0; xIndex < xShifting; xIndex++)
        {
            logicPosition.x = Chessboard.CurHandRowIndex + xIndex;
            for (int yIndex = 0; yIndex < chessboardSize.y; yIndex++)
            {
                logicPosition.y = Chessboard.CurHandColumnIndex + yIndex;

                Chessman chessman;
                if (!Chessboard.LogicPosition2ChessmanDict.TryGetValue(logicPosition, out chessman))
                {
                    Debug.LogError("MoveChessboard Fail. " + logicPosition + " can not find chessman!");
                    continue;
                }

                //如果是新的，就使用原本的y位置
                if (!Chessman2NewLogicPositionDict.TryGetValue(chessman, out newLogixPosition))
                {
                    Chessman2NewLogicPositionDict.Add(chessman, newLogixPosition);

                    newLogixPosition.y = logicPosition.y;
                }
                //x改变，移动到最前面的列
                newLogixPosition.x = Chessboard.CurLastRowIndex + xIndex + 1;

                Chessman2NewLogicPositionDict[chessman] = newLogixPosition;
            }
        }

        //y方向移动 移动行 (speed.y > 0 == 往上 == 将最后一行行移到第一行去)
        for (int yIndex = 0; yIndex < yShifting; yIndex++)
        {
            logicPosition.y = speed.y > 0 ? (Chessboard.CurLastColumnIndex + yIndex) : (Chessboard.CurHandColumnIndex + yIndex);
            for (int xIndex = 0; xIndex < chessboardSize.x; xIndex++)
            {
                logicPosition.x = Chessboard.CurHandRowIndex + xIndex;

                Chessman chessman;
                if (!Chessboard.LogicPosition2ChessmanDict.TryGetValue(logicPosition, out chessman))
                {
                    Debug.LogError("MoveChessboard Fail. " + logicPosition + " can not find chessman!");
                    continue;
                }

                //如果是新的，就使用原本的x位置
                if (!Chessman2NewLogicPositionDict.TryGetValue(chessman, out newLogixPosition))
                {
                    Chessman2NewLogicPositionDict.Add(chessman, newLogixPosition);

                    newLogixPosition.x = logicPosition.x;
                }
                //y改变，移动到最前/最后的列
                newLogixPosition.y = speed.y > 0 ? (Chessboard.CurHandColumnIndex - yIndex - 1) : (Chessboard.CurLastColumnIndex + yIndex + 1);

                Chessman2NewLogicPositionDict[chessman] = newLogixPosition;
            }
        }

        Dictionary<Chessman, Vector3>.Enumerator enumerator = Chessman2NewLogicPositionDict.GetEnumerator();
        while (enumerator.MoveNext())
        {
            Chessman chessman = enumerator.Current.Key;
            Vector3 newLogicPosition = enumerator.Current.Value;

            Debug.Log("Change." + chessman.LogicPosition + "->" + newLogicPosition);

            //旧数据
            int chessmanRowIndex = (int)chessman.LogicPosition.x;
            int chessmanColumnIndex = (int)chessman.LogicPosition.y;

            // 清理
            Chessboard.LogicPosition2ChessmanDict.Remove(chessman.LogicPosition);

            List<Chessman> chessmanList;
            if (Chessboard.Row2ChessmanDict.TryGetValue(chessmanRowIndex, out chessmanList))
            {
                chessmanList.Remove(chessman);
                if (chessmanList.Count == 0)
                {
                    Chessboard.Row2ChessmanDict.Remove(chessmanColumnIndex);
                }
            }
            if (Chessboard.Column2ChessmanDict.TryGetValue(chessmanColumnIndex, out chessmanList))
            {
                chessmanList.Remove(chessman);
                if (chessmanList.Count == 0)
                {
                    Chessboard.Column2ChessmanDict.Remove(chessmanColumnIndex);
                }
            }

            //更新新数据
            ChessboarFactory.UpdateChessman(ref chessman, newLogicPosition);

            //添加
            Chessboard.LogicPosition2ChessmanDict.Add(chessman.LogicPosition, chessman);

            if (!Chessboard.Row2ChessmanDict.TryGetValue((int)chessman.LogicPosition.x, out chessmanList))
            {
                chessmanList = new List<Chessman>();
            }
            chessmanList.Add(chessman);
            if (!Chessboard.Column2ChessmanDict.TryGetValue((int)chessman.LogicPosition.y, out chessmanList))
            {
                chessmanList = new List<Chessman>();
            }
            chessmanList.Add(chessman);
        }

        Chessman2NewLogicPositionDict.Clear();

        //记录当前范围
        __UpdateLastRange();

        //更新当前范围
        Chessboard.CurHandRowIndex += (int)speed.x;
        Chessboard.CurLastRowIndex += (int)speed.x;
        Chessboard.CurHandColumnIndex += -(int)speed.y;
        Chessboard.CurLastColumnIndex += -(int)speed.y;

       Debug.Log( "ChangeCurRange."+ " speed:"+ speed + " CurHandRowIndex:" + Chessboard.CurHandRowIndex + " CurLastRowIndex:" + Chessboard.CurLastRowIndex + " CurHandColumnIndex:" + Chessboard.CurHandColumnIndex + " CurLastColumnIndex:" + Chessboard.CurLastColumnIndex);

    }

    public void RefreshRoad(Vector3 moveDirection, bool isCreate = false)
    {
        if (isCreate)
        {
            Vector3 firstRoad = GetCurArrivePoint();

            Chessman chessman;
            if (!Chessboard.LogicPosition2ChessmanDict.TryGetValue(firstRoad, out chessman))
            {
                Debug.LogError("CreateRoad Fail. " + firstRoad + " can not find chessman!");
                return;
            }

            chessman.EnableArrive = true;
            __CreateNewRoad(firstRoad);

        }
        else
        {
            Vector3 logicPosition = new Vector3();
            if (moveDirection.x != 0)
            {
                int xShifting = Chessboard.CurLastRowIndex - Chessboard.LastUpdateLastRowIndex;
                for (int x = 0; x < xShifting; x++)
                {
                    logicPosition.x = Chessboard.LastUpdateLastRowIndex + x;
                    for (int y = Chessboard.CurHandColumnIndex; y <= Chessboard.CurLastColumnIndex; y++)
                    {
                        logicPosition.y = y;

                        Chessman chessman;
                        if (!Chessboard.LogicPosition2ChessmanDict.TryGetValue(logicPosition, out chessman))
                        {
                            Debug.LogError("CreateRoad Fail. " + logicPosition + " can not find chessman!");
                            continue;
                        }

                        if (chessman.EnableArrive)
                        {
                            __CreateNewRoad(logicPosition);
                            break;
                        }
                    }
                }
                __UpdateLastRange();
            }
            if (moveDirection.y != 0)
            {
                int yShifting = Mathf.Abs(Chessboard.CurLastColumnIndex - Chessboard.LastUpdateLastColumnIndex);
                for (int y = 0; y < yShifting; y++)
                {
                    logicPosition.y = moveDirection.y > 0 ? Chessboard.LastUpdateHandColumnIndex - y : Chessboard.LastUpdateLastColumnIndex + y;
                    for (int x = Chessboard.CurHandRowIndex; x <= Chessboard.CurLastRowIndex; x++)
                    {
                        logicPosition.x = x;

                        Chessman chessman;
                        if (!Chessboard.LogicPosition2ChessmanDict.TryGetValue(logicPosition, out chessman))
                        {
                            Debug.LogError("CreateRoad Fail. " + logicPosition + " can not find chessman!");
                            continue;
                        }

                        if (chessman.EnableArrive)
                        {
                            __CreateNewRoad(logicPosition);
                            break;
                        }
                    }
                }
                __UpdateLastRange();
            }
        }

    }

    private Vector3 __CreateNewRoad(Vector3 current)
    {
        Chessman chessman;
        Vector3 next;

        do
        {
            next = current;
            

            bool isX = Random.value > 0.5f;
            if (isX)
            {
                next += Vector3.right;
            }
            else
            {
                int yDirection = Random.value > 0.5f ? 1 : -1;
                next += Vector3.up * yDirection;
            }

          //  Debug.Log("__CreateNewRoad." + current + "->" + next);

            if (!Chessboard.LogicPosition2ChessmanDict.TryGetValue(next, out chessman))
            {
                break;
            }
            chessman.EnableArrive = true;

            current = next;

        } while (true);

        return current;
    }

    //记录当前范围
    private void __UpdateLastRange()
    {
        Chessboard.LastUpdateHandRowIndex = Chessboard.CurHandRowIndex;
        Chessboard.LastUpdateLastRowIndex = Chessboard.CurLastRowIndex;
        Chessboard.LastUpdateHandColumnIndex = Chessboard.CurHandColumnIndex;
        Chessboard.LastUpdateLastColumnIndex = Chessboard.CurLastColumnIndex;
    }

    public void AddArrivePoint(Vector3 point)
    {
        ArriveList.Insert(0, point);
    }

    public Vector3 GetCurArrivePoint()
    {
        if(ArriveList.Count == 0)
        {
            Vector2 chessboardSize = ResourceController.Instance.GetGamePlayDefine().ChessboardSize;

            return new Vector3((int)(chessboardSize.x / 2), (int)(chessboardSize.y/2),0);
        }
        else
        {
            return ArriveList[0];
        }
    }

    public List<Chessman> GetAllChessman()
    {
        return Chessboard.AllChessmanList;
    }

    public Chessman GetChessmanById(int id)
    {
        Chessman chessman;
        if (Chessboard.Id2ChessmanDict.TryGetValue(id, out chessman))
        {
            return chessman;
        }
        return null;
    }

    public Chessman GetChessmanByLogicPosition(Vector3 logicPosition)
    {
        Chessman chessman;
        if (Chessboard.LogicPosition2ChessmanDict.TryGetValue(logicPosition, out chessman))
        {
            return chessman;
        }
        return null;
    }
}
