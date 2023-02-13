using System;
using System.Linq;
using DataStructs;
using Enums;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance
    {
        get => _instance;
    }

    private static SoundManager _instance;

    [SerializeField]
    private SoundItem[] sfx;
    [SerializeField]
    private SoundItem[] music;
    [SerializeField]
    private SoundItem[] others;

    [SerializeField]
    private GameObject sfxRoot;
    [SerializeField]
    private GameObject musicRoot;
    [SerializeField]
    private GameObject otherRoot;

    private void Start()
    {
        if ( Instance != null && Instance != this )
            Destroy(gameObject);

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound() => Debug.Log("Playing sound...");

    public void PlaySound( SoundType type, string id , bool isLoop = false, bool forceFirst = false)
    {
        AudioClip clip;
        switch ( type )
        {
            case SoundType.Sfx:
                GetClip(sfx,id,out clip);
                break;
            case SoundType.Music:
                GetClip(music,id,out clip);
                break;
            case SoundType.Other:
                GetClip(others,id,out clip);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof( type ), type, null);
        }

        if ( clip == null )
            throw new Exception("Can't find the item");

        GetSourceArray(type, out AudioSource[] sources);

        if ( forceFirst )
        {
            sources[0].clip = clip;
            sources[0].loop = isLoop;
            sources[0].Play();
            return;
        }

        foreach ( AudioSource s in sources )
        {
            if(s.isPlaying) continue;
            s.clip = clip;
            s.loop = isLoop;
            s.Play();
            return;
        }

        AudioSource newSource;
        switch ( type )
        {
            case SoundType.Sfx:
                CreateNewSourceForPool( sfxRoot.transform, out newSource);
                return;
            case SoundType.Music:
                CreateNewSourceForPool( musicRoot.transform, out newSource);
                break;
            case SoundType.Other:
                CreateNewSourceForPool( otherRoot.transform, out newSource);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof( type ), type, null);
        }

        newSource.clip = clip;
        newSource.loop = isLoop;
        newSource.Play();
    }

    private void GetClip(in SoundItem[] soundArray, string id, out AudioClip clip)
    {
        clip = ( from soundItem in soundArray where soundItem.name == id select soundItem.audioClip ).FirstOrDefault();
    }

    private void GetSourceArray( SoundType type, out AudioSource[] sources )
    {
        sources = null;
        GameObject root = type switch
        {
            SoundType.Sfx => Instance.sfxRoot,
            SoundType.Music => Instance.musicRoot,
            SoundType.Other => Instance.otherRoot,
            _ => throw new ArgumentOutOfRangeException(nameof( type ), type, null),
        };
        sources = root.GetComponentsInChildren<AudioSource>();
    }

    private void CreateNewSourceForPool( Transform parent, out AudioSource newSource )
    {
        newSource = new GameObject("AudioSource").AddComponent<AudioSource>();
        newSource.transform.SetParent(parent);
        newSource.playOnAwake = false;
    }
}