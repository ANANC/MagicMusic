using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessboardControllerData
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
        public int Index;
        public Vector3 LogicPosition;
        public int RowIndex;            //��index
        public int ColumnIndex;         //��index
        public bool EnableArrive;       //�ܷ�ͨ��

        public ChessmanArt ArtInfo;     //��������

        public MusicalNote MusicalNote;
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
        public Dictionary<int, List<Chessman>> Row2ChessmanDict = new Dictionary<int, List<Chessman>>();   //��-������Ϣ
        public Dictionary<int, List<Chessman>> Column2ChessmanDict = new Dictionary<int, List<Chessman>>();//��-������Ϣ

        public Dictionary<int, Chessman> Id2ChessmanDict = new Dictionary<int, Chessman>();          //id-ȫ��������Ϣ
        public Dictionary<Vector3, Chessman> LogicPosition2ChessmanDict = new Dictionary<Vector3, Chessman>();    //logicPosition-ȫ��������Ϣ
        public List<Chessman> AllChessmanList = new List<Chessman>();                     //ȫ��������Ϣ

        /*                      CurHandColumnIndex
         *    CurHandRowIndex   0 --------------------> CurLastRowIndex
         *                      |
         *                      |
         *                      |
         *                      v
         *                      CurLastColumnIndex
         */

        //��ǰ��Χ
        public int CurHandRowIndex;        //ͷ-�� x С
        public int CurLastRowIndex;        //β-�� x ��
        public int CurHandColumnIndex;     //ͷ-�� y С
        public int CurLastColumnIndex;     //β-�� y ��

        //��һ�η�Χ
        public int LastUpdateHandRowIndex;        //ͷ-�� x С
        public int LastUpdateLastRowIndex;        //β-�� x ��
        public int LastUpdateHandColumnIndex;     //ͷ-�� y ��
        public int LastUpdateLastColumnIndex;     //β-�� y С

        public ChessboardType m_ChessboardType;
        public bool IsCreateChessman;
        public bool IsCreateActiveChessman;
        public bool IsCreateMusic;

        public bool IsMove;
    }
}
