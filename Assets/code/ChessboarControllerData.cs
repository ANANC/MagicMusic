using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessboarControllerData
{

    public enum ChessboardType
    {
        Random = 1,
        Fixed = 2,
    }

    public enum MusicalNoteMusicType
    {

    }


    public class Chessman
    {
        public int Id;
        public Vector3 LogicPosition;
        public int RowIndex;            //行index
        public int ColumnIndex;         //列index
        public bool EnableArrive;       //能否通过

        public ChessmanArt ArtInfo;     //美术表现

        public MusicalNote MusicalNote;

        public bool Acitve;
    }

    public class ChessmanArt
    {
        public Vector3 ArtPosition;
        public int ResourceIndex;
        public Color TexColor;
    }


    public class MusicalNote
    {
        public string MusicName;
        public float MusicTime;

        public MusicalNoteArt ArtInfo;
    }

    public class MusicalNoteArt
    {
        public Vector3 ArtPosition;
        public string ResourceName;
        public Color TexColor;
    }

    public class Chessboard
    {
        public Dictionary<int, List<Chessman>> Row2ChessmanDict = new Dictionary<int, List<Chessman>>();   //行-棋子信息
        public Dictionary<int, List<Chessman>> Column2ChessmanDict = new Dictionary<int, List<Chessman>>();//列-棋子信息

        public Dictionary<int, Chessman> Id2ChessmanDict = new Dictionary<int, Chessman>();          //id-全部棋子信息
        public Dictionary<Vector3, Chessman> LogicPosition2ChessmanDict = new Dictionary<Vector3, Chessman>();    //logicPosition-全部棋子信息
        public List<Chessman> AllChessmanList = new List<Chessman>();                     //全部棋子信息

        public int CurHandRowIndex;        //头-行
        public int CurLastRowIndex;        //尾-行
        public int CurHandColumnIndex;     //头-列
        public int CurLastColumnIndex;     //尾-列

        public ChessboardType m_ChessboardType;
        public bool IsCreateChessman;
        public bool IsCreateActiveChessman;
        public bool IsCreateMusic;

        public bool IsMove;
    }
}
