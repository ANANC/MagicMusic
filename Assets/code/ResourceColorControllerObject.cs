using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceColorControllerObject : MonoBehaviour
{
    [Header("������ɫ-�ɴ�")]
    public Color ChessmanEnableArriveColor;

    [Header("������ɫ-���ɴ�")]
    public Color ChessmanDisablearriveColor;

    [Header("������ɫ")]
    public List<Color> MusicColorList = new List<Color>();

    [Header("�������ͨ��ɫ")]
    public Color ArrivePointNormalColor;


    [Header("����㴥����ɫ")]
    public Color ArrivePointTouchColor;
}
