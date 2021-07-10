using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessboardControllerUtil
{
    public void Init()
    {

    }

    public void UnInit()
    {

    }

    //得到动态的美术未知
    public Vector3 GetDynamicArtPosition(Vector3 logicPosition,Vector3 sortChessmanSize, int curHandRowIndex, int curHandColumnIndex)
    {
        Vector3 artPoisition = new Vector3();

        int xShifting = (int)logicPosition.x - curHandRowIndex;
        int yShifting = curHandColumnIndex - (int)logicPosition.y;

        artPoisition.x = xShifting * sortChessmanSize.x;
        artPoisition.y = yShifting * sortChessmanSize.y;

        return artPoisition;
    }

}
