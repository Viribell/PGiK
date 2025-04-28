using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Pool;

#region AudioData

[System.Serializable]
public class AudioSoundData {
    [Header( "Sound Info" )]
    public string name;
    public SoundFXType soundType; //not really needed, but will see
    public AudioClip clip;
    public AudioMixerGroup mixerGroup;

    [Header( "Sound Behaviour Config" )]
    public bool playOnAwake = false;
    public bool loop = false;

    [Header( "Sound Parameters Config" )]
    [Range( 0.0f, 1.0f )] public float volume = 1.0f;
    [Range( -3.0f, 3.0f )] public float pitch = 1.0f;

    //This has to go outside somehow
    [Header( "Delay Info" )]
    public bool hasDelay = false;
    public bool useLengthAsDelay = false;
    [Min( 0.0f )] public float playDelay; //in seconds
    [field: SerializeField][MarkColor] private float lastPlayed = 0.0f; //in seconds

#if UNITY_EDITOR
    public float Editor_LastPlayed { set { lastPlayed = value; } }
#endif

    [HideInInspector]
    public float Length {
        get {
            if ( clip == null ) return 0.0f;
            else return clip.length / pitch;
        }
    }

    //This has to go outside somehow
    public bool CanPlay() {
        if ( !hasDelay ) return true;

        float currTime = Time.time;
        float delayAmount = useLengthAsDelay ? Length : playDelay;

        if ( lastPlayed + delayAmount >= currTime ) return false;

        lastPlayed = currTime;
        return true;
    }
}

[System.Serializable]
public class AudioMusicData {
    public MusicType musicType;
    public AudioClip clip;
}

#endregion

#region AudioEnums

public enum SoundFXType {
    Undefined,
    Entity,
    Ability
}

public enum MusicType {
    Undefined,
}

public enum EntitySoundType {
    Undefined,
    Walking,
    BasicAttack,
    Hurt,
    Death
}

public enum AudioPlayType {
    Basic,
    Delayed,
    OneShot,
    Scheduled
}

#endregion

public class AudioManager : MonoBehaviour, IPersistentData {
    public static AudioManager Instance { get; private set; }
    
    private IObjectPool<SoundEmitter> soundEmitterPool;
    private List<SoundEmitter> activeSoundEmitters = new List<SoundEmitter>();

    [Header("Basic Audio Config")]
    [field: SerializeField] private AudioMixer defaultAudioMixer;
    [field: SerializeField] private MusicSetSO audioAssets;
    [Range(0.0f, 1.0f)]
    [field: SerializeField] private float defaultAudioVolume;

    [Header( "Object Pool Config" )]
    [field: SerializeField] private bool collectionCheck = true;
    [field: SerializeField] private int defaultCapacity = 10;
    [field: SerializeField] private int maxPoolSize = 100;


    [Header( "Audio Objects Prefabs" )]
    [field: SerializeField] private AudioSource soundFXSource;
    [field: SerializeField] private SoundEmitter emitterPrefab;

    [Header( "Audio Objects References" )]
    [field: SerializeField] private AudioSource musicSource;
    [field: SerializeField] private AudioSource localOneShot;

    #region SetupFunctions
    private void Awake() {
        if ( Instance == null ) Instance = this;
        else {
            Debug.LogWarning( "There is more than on AudioManager. Destroying the new one." );
            Destroy( gameObject );
            return;
        }

        DontDestroyOnLoad( gameObject );
        InitPool();
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded( Scene scene, LoadSceneMode mode ) {
        PlayMusicSource( MusicType.Undefined ); //TEMP_TEST
    }

    #endregion


    #region PlayMusicFromSource
    public void PlayMusicSource( AudioClip audioClip ) {
        SimpleStopIfPlaying();

        musicSource.clip = audioClip;
        musicSource.volume = defaultAudioVolume;

        musicSource.Play();
    }

    public void PlayMusicSource( MusicType musicType ) {
        SimpleStopIfPlaying();

        musicSource.clip = audioAssets.GetMusicClip( musicType );
        musicSource.volume = defaultAudioVolume;

        musicSource.Play();
    }
    #endregion

    #region PlaySoundFromNewObject

    public void PlaySound( AudioClip audioClip, Vector2 position ) {
        GameObject soundObject = new GameObject( "SoundObject_Code" );
        soundObject.transform.position = position;

        AudioSource audioSource = soundObject.AddComponent<AudioSource>();

        audioSource.clip = audioClip;
        audioSource.volume = defaultAudioVolume;

        audioSource.Play();

        Destroy( soundObject, audioClip.length );
    }

    public void PlaySound( AudioSoundData soundData, Vector2 position ) {
        if ( soundData == null ) { Debug.Log( $"Sound data not found." ); return; }
        if ( !soundData.CanPlay() ) { Debug.Log( $"Sound {soundData.name} cannot play." ); return; }

        GameObject soundObject = new GameObject( "SoundObject_Code" );
        soundObject.transform.position = position;

        AudioSource audioSource = soundObject.AddComponent<AudioSource>();

        audioSource.clip = soundData.clip;
        audioSource.volume = soundData.volume;

        audioSource.Play();

        Destroy( soundObject, soundData.Length );
    }

    #endregion

    #region PlaySoundFromOneShot

    public void PlaySound( AudioClip audioClip ) {
        localOneShot.volume = defaultAudioVolume;
        localOneShot.PlayOneShot( audioClip );

    }

    public void PlaySound( AudioSoundData soundData ) {
        if ( soundData == null ) { Debug.Log( $"Sound data not found." ); return; }
        if ( !soundData.CanPlay() ) { Debug.Log($"Sound {soundData.name} cannot play."); return; }

        localOneShot.volume = soundData.volume;
        localOneShot.PlayOneShot( soundData.clip );

    }

    public void PlayRandomSound( AudioClip[] sounds ) {
        if ( sounds == null || sounds.Length == 0 ) return;

        localOneShot.volume = defaultAudioVolume;
        localOneShot.PlayOneShot( sounds[ Random.Range(0, sounds.Length) ] );

    }

    public void PlayRandomSound( List<AudioSoundData> sounds ) {
        if ( sounds == null || sounds.Count == 0 ) return;

        AudioSoundData randSound = sounds[Random.Range( 0, sounds.Count )];

        localOneShot.volume = randSound.volume;
        localOneShot.PlayOneShot( randSound.clip );

    }

    #endregion

    #region PlaySoundFromPrefab

    public void PlaySoundFXClip( AudioClip audioClip, Transform spawn, float volume ) {
        AudioSource audioSource = Instantiate( soundFXSource, spawn.position, Quaternion.identity );

        audioSource.clip = audioClip;
        audioSource.volume = Mathf.Clamp01( volume );
        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy( audioSource.gameObject, clipLength );

    }

    public void PlaySoundFXClip( AudioSoundData soundData, Transform spawn, float volume ) {
        if ( soundData == null ) { Debug.Log( $"Sound data not found." ); return; }
        if ( !soundData.CanPlay() ) { Debug.Log( $"Sound {soundData.name} cannot play." ); return; }

        AudioSource audioSource = Instantiate( soundFXSource, spawn.position, Quaternion.identity );

        audioSource.clip = soundData.clip;
        audioSource.volume = Mathf.Clamp01( volume );
        audioSource.Play();

        Destroy( audioSource.gameObject, soundData.Length );

    }

    public void PlaySoundFXClip( AudioSoundData soundData, Transform spawn ) {
        if ( soundData == null ) { Debug.Log( $"Sound data not found." ); return; }
        if ( !soundData.CanPlay() ) { Debug.Log( $"Sound {soundData.name} cannot play." ); return; }

        AudioSource audioSource = Instantiate( soundFXSource, spawn.position, Quaternion.identity );

        audioSource.clip = soundData.clip;
        audioSource.volume = Mathf.Clamp01( soundData.volume );
        audioSource.Play();

        Destroy( audioSource.gameObject, soundData.Length );

    }

    public void PlayRandomSoundFXClip( AudioClip[] audioClips, Transform spawn, float volume ) {
        int rand = Random.Range( 0, audioClips.Length );
        
        AudioSource audioSource = Instantiate( soundFXSource, spawn.position, Quaternion.identity );

        audioSource.clip = audioClips[rand];
        audioSource.volume = Mathf.Clamp01( volume );
        audioSource.Play();

        //if pitch varation will be added, then the length needs to be modified to include pitch change: clip.length / pitch
        float clipLength = audioSource.clip.length;

        Destroy( audioSource.gameObject, clipLength );

    }

    public void PlayRandomSoundFXClip( List<AudioSoundData> sounds, Transform spawn ) {
        if ( sounds == null || sounds.Count == 0 ) return;

        AudioSoundData randSound = sounds[Random.Range( 0, sounds.Count )];

        localOneShot.volume = randSound.volume;
        localOneShot.PlayOneShot( randSound.clip );

        AudioSource audioSource = Instantiate( soundFXSource, spawn.position, Quaternion.identity );

        audioSource.clip = randSound.clip;
        audioSource.volume = Mathf.Clamp01( randSound.volume );
        audioSource.Play();

        Destroy( audioSource.gameObject, randSound.Length );

    }

    #endregion


    #region PlaySoundFromPool

    public void PlayPoolSound( AudioSoundData soundData, Vector2 position ) {
        if ( soundData == null ) { Debug.Log( $"Sound data not found." ); return; }

        BuildSound( soundData, position );
    }

    public void PlayPoolSound( AudioSoundData soundData, Vector2 position, bool randomPitch ) {
        if ( soundData == null ) { Debug.Log( $"Sound data not found." ); return; }

        BuildSound( soundData, position, randomPitch );
    }

    #endregion

    #region PoolFunctions

    public SoundEmitter GetEmitter() { return soundEmitterPool.Get(); }
    public void ReturnEmitter(SoundEmitter emitter) { soundEmitterPool.Release( emitter ); }


    private void BuildSound(AudioSoundData data, Vector2 pos, bool randomPitch = false) {
        SoundEmitter emitter = GetEmitter();
        emitter.Init( data, pos );

        if ( randomPitch ) emitter.RandomizePitch();

        emitter.Play();
    }

    private void InitPool() {
        soundEmitterPool = new ObjectPool<SoundEmitter>( CreatePoolObject, OnTakeFromPool, OnReturnToPool, OnDestroyPool, 
            collectionCheck, defaultCapacity, maxPoolSize );
    }

    private SoundEmitter CreatePoolObject() {
        SoundEmitter emitter = Instantiate( emitterPrefab );
        emitter.gameObject.SetActive( false );
        emitter.SetAudioManager( this );
        emitter.SetParent( transform );

        return emitter;
    }

    private void OnTakeFromPool( SoundEmitter emitter ) {
        emitter.gameObject.SetActive( true );
        activeSoundEmitters.Add( emitter );
    }

    private void OnReturnToPool( SoundEmitter emitter ) {
        emitter.gameObject.SetActive( false );
        activeSoundEmitters.Remove( emitter );
    }

    private void OnDestroyPool( SoundEmitter emitter ) {
        Destroy( emitter.gameObject );
    }

    #endregion


    #region MiscFunctions

    private void SimpleStopIfPlaying() {
        if ( musicSource.isPlaying ) musicSource.Stop();
    }

    public void LoopMusicSource(bool loop) { musicSource.loop = loop; }
    public void StopMusicSource() { musicSource.Stop(); }

    #endregion


    #region MixerVolumeFunctions
    //to be used with Slider with range (0.00001f, 1.0f)
    //optionally make range (0, 100) but add functions for normalization of Volume To Value and reverse
    public void SetMasterVolume(float level) {
        defaultAudioMixer.SetFloat( "masterVolume", Mathf.Log10( level ) * 20.0f );
    }

    public void SetSoundFXVolume( float level ) {
        defaultAudioMixer.SetFloat( "soundFXVolume", Mathf.Log10( level ) * 20.0f );
    }

    public void SetMusicVolume( float level ) {
        defaultAudioMixer.SetFloat( "musicVolume", Mathf.Log10( level ) * 20.0f );
    }

    public float GetMasterVolume() {
        float volume = 1.0f;

        defaultAudioMixer.GetFloat( "masterVolume", out volume );
        volume = Mathf.Pow( 10, volume / 20 );

        return volume;
    }

    public float GetMusicVolume() {
        float volume = 0.0f;

        defaultAudioMixer.GetFloat( "musicVolume", out volume );
        volume = Mathf.Pow( 10, volume / 20 );

        return volume;
    }

    public float GetEffectsVolume() {
        float volume = 1.0f;

        defaultAudioMixer.GetFloat( "soundFXVolume", out volume );
        volume = Mathf.Pow( 10, volume / 20 );

        return volume;
    }

    #endregion

    #region IPersistentData_Functions

    public void LoadData( SaveData data ) {
        AudioSaveData audio = data.audioData;

        if ( audio == null ) { Debug.LogError("Audio Data Object in SaveData is null!"); return; }

        SetMasterVolume( audio.masterVolume );
        SetMusicVolume( audio.musicVolume );
        SetSoundFXVolume( audio.effectsVolume );
    }

    public void SaveData( SaveData data ) {
        data.audioData.masterVolume = GetMasterVolume();
        data.audioData.musicVolume = GetMusicVolume();
        data.audioData.effectsVolume = GetEffectsVolume();
    }

    #endregion
}
