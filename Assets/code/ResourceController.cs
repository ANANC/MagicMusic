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

    private string ResourceColorControllerObjectName = "ColorController";
    private ResourceColorControllerObject m_ResourceColorControllerObject;

    private string ResourceGameObjectControllerObjectName = "GameObjectController";
    private ResourceGameObjectControllerObject m_ResourceGameObjectControllerObject;

    private string ResourceGameplayControllerObjectName = "GamePlayController";
    private ResourceGameplayControllerObject m_ResourceGameplayControllerObject;
    private void Init()
    {
        m_ResourceControllerObject = GameObject.Find(ResourceControllerObjectName).GetComponent<ResourceControllerObject>();
        m_ResourceColorControllerObject = GameObject.Find(ResourceColorControllerObjectName).GetComponent<ResourceColorControllerObject>();
        m_ResourceGameObjectControllerObject = GameObject.Find(ResourceGameObjectControllerObjectName).GetComponent<ResourceGameObjectControllerObject>();
        m_ResourceGameplayControllerObject = GameObject.Find(ResourceGameplayControllerObjectName).GetComponent<ResourceGameplayControllerObject>();
    }

    //得到玩法配置
    public ResourceGameplayControllerObject GetGamePlayDefine()
    {
        return m_ResourceGameplayControllerObject;
    }

    //得到颜色配置
    public ResourceColorControllerObject GetColorDefine()
    {
        return m_ResourceColorControllerObject;
    }

    //得到棋子数量
    public int GetChessmanCount()
    {
        return m_ResourceControllerObject.ChessmanList.Count;
    }

    //得到棋子资源
    public Sprite GetChessmanByIndex(int index)
    {
        return m_ResourceControllerObject.ChessmanList[index];
    }

    //得到棋子UIGameObject
    public GameObject GetChessmanUIGameObject()
    {
        return m_ResourceGameObjectControllerObject.ChessmanGameObject;
    }

    //得到音乐数量
    public int GetMusicCount()
    {
        return m_ResourceControllerObject.AudioClipList.Count;
    }

    //得到音乐
    public AudioClip GetMusicByIndex(int index)
    {
        return m_ResourceControllerObject.AudioClipList[index];
    }

    //得到音乐UI数量
    public int GetMusicUICount()
    {
        return m_ResourceControllerObject.MusicArtList.Count;
    }

    //得到音乐UI资源
    public Sprite GetMusicUIByIndex(int index)
    {
        return m_ResourceControllerObject.MusicArtList[index];
    }
}
