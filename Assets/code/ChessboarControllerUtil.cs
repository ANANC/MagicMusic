using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessboarControllerUtil
{
    public void Init()
    {

    }

    public void UnInit()
    {

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
