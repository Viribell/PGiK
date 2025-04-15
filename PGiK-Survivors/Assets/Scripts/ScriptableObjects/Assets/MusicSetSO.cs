using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Assets/Music Set")]
public class MusicSetSO : ScriptableObject {
    [field: SerializeField] public List<AudioMusicData> musicList;

    public AudioMusicData GetMusicData( MusicType musicType ) {
        foreach ( AudioMusicData data in musicList ) {
            if ( data.musicType == musicType ) return data;
        }

        return null;
    }

    public AudioClip GetMusicClip( MusicType musicType ) {
        foreach ( AudioMusicData data in musicList ) {
            if ( data.musicType == musicType ) return data.clip;
        }

        return null;
    }

    public List<AudioClip> GetMusicClips( MusicType musicType ) {
        List<AudioClip> audioClips = new List<AudioClip>();

        foreach ( AudioMusicData data in musicList ) {
            if ( data.musicType == musicType ) audioClips.Add( data.clip );
        }

        return audioClips;
    }
}
