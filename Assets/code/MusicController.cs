using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ChessboardControllerData;

public class MusicController 
{
    public void Init()
    {

    }

    public void UnInit()
    {

    }

    public void CreateChessboardMusic(ref Chessboard chessboard )
    {
        for(int index = 0;index< chessboard.AllChessmanList.Count;index++)
        {
            Chessman chessman = chessboard.AllChessmanList[index];

            if(Random.value < 0.6f)
            {
                continue;
            }

            MusicalNote musicalNote = CreateMusicalNote();
            chessman.MusicalNote = musicalNote;
        }
    }

    public MusicalNote CreateMusicalNote()
    {
        MusicalNote musicalNote = new MusicalNote();
        MusicalNoteArt musicalNoteArt = new MusicalNoteArt();

        musicalNote.ArtInfo = musicalNoteArt;

        int musicCount = ResourceController.Instance.GetMusicCount();
        musicalNote.MusicIndex = Random.Range(0, musicCount);
        musicalNote.MusicTime = Random.Range(2, 10);

        int musicUICount = ResourceController.Instance.GetMusicUICount();
        musicalNoteArt.ArtPosition = Vector3.zero;
        musicalNoteArt.ResourceIndex = Random.Range(0, musicUICount);
        musicalNoteArt.TexColor = ResourceController.Instance.GetColorDefine().MusicColorList[musicalNote.MusicIndex];

        return musicalNote;
    }

}
