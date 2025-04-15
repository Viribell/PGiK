using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "Scriptable Objects/Assets/Audio Set" )]
public class EntityAudioSetSO : ScriptableObject {
    [field: SerializeField] public SerializableDictionary<EntitySoundType, Wrapper> entitySounds;

    [System.Serializable]
    public class Wrapper {
        public List<AudioSoundData> sounds;
    }

    public AudioSoundData GetSoundData( EntitySoundType soundType ) {
        if ( HasKey( soundType ) && HasValue( soundType ) ) return entitySounds[soundType].sounds[0];

        return null;
    }

    public List<AudioSoundData> GetSoundsData( EntitySoundType soundType ) {
        if ( HasKey( soundType ) && HasValue( soundType ) ) return entitySounds[soundType].sounds;

        return null;
    }

    public AudioClip GetSoundClip( EntitySoundType soundType ) {
        if ( HasKey( soundType ) && HasValue( soundType ) ) return entitySounds[soundType].sounds[0].clip;

        return null;
    }

    private bool HasKey( EntitySoundType key ) {
        return entitySounds.ContainsKey( key );
    }

    private bool HasValue( EntitySoundType key ) {
        return entitySounds[key] != null && entitySounds[key].sounds != null && entitySounds[key].sounds.Count > 0;
    }
}
