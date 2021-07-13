using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceColorControllerObject : MonoBehaviour
{
    [Header("棋子颜色-可达")]
    public Color ChessmanEnableArriveColor;

    [Header("棋子颜色-不可达")]
    public Color ChessmanDisablearriveColor;

    [Header("音符颜色")]
    public List<Color> MusicColorList = new List<Color>();

    [Header("到达点普通颜色")]
    public Color ArrivePointNormalColor;


    [Header("到达点触碰颜色")]
    public Color ArrivePointTouchColor;
}
