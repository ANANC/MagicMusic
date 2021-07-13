using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceControllerObject : MonoBehaviour
{
    [Header("棋子资源")]
    public List<Sprite> ChessmanList = new List<Sprite>();

    [Header("音符ui资源")]
    public List<Sprite> MusicArtList = new List<Sprite>();

    [Header("音乐资源")]
    public List<AudioClip> AudioClipList = new List<AudioClip>();



}
