using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceController
{
    private static ResourceController m_Instance;
    public static ResourceController Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new ResourceController();
                m_Instance.Init();
            }
            return m_Instance;
        }
    }

    private string ResourceControllerObjectName = "ResourcesController";
    private ResourceControllerObject m_ResourceControllerObject;

    private void Init()
    {
        m_ResourceControllerObject = GameObject.Find(ResourceControllerObjectName).GetComponent<ResourceControllerObject>();

    }

    //得到棋子数量
    public int GetChessmanCount()
    {
        return m_ResourceControllerObject.ChessmanList.Count;
    }

    //得到棋子资源
    public Image GetChessmanByIndex(int index)
    {
        return m_ResourceControllerObject.ChessmanList[index];
    }



}
