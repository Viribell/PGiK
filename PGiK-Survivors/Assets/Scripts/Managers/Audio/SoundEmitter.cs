using System.Collections;
using UnityEngine;

public class SoundEmitter : MonoBehaviour {
    [field: SerializeField] private AudioSoundData soundData;
    [field: SerializeField] private AudioSource audioSource;

    private Coroutine PlayingRoutine;
    private WaitWhile waitUntilSoundEnd;

    private AudioManager audioManager;

    private void Awake() {
        waitUntilSoundEnd = new WaitWhile( () => audioSource.isPlaying );
    }

    public void Play() {
        if ( PlayingRoutine != null ) StopCoroutine( PlayingRoutine );

        audioSource.Play();
        PlayingRoutine = StartCoroutine( WaitForSoundEnd() );
    }

    public void Stop() {
        if ( PlayingRoutine != null ) { StopCoroutine( PlayingRoutine ); PlayingRoutine = null; }

        audioSource.Stop();
        audioManager.ReturnEmitter( this );
    }

    private IEnumerator WaitForSoundEnd()  {
        yield return waitUntilSoundEnd;

        audioManager.ReturnEmitter( this );
    }

    public void Init(AudioSoundData data) {
        soundData = data;

        audioSource.clip = data.clip;
        audioSource.outputAudioMixerGroup = data.mixerGroup;

        audioSource.loop = data.loop;
        audioSource.playOnAwake = data.playOnAwake;
    }

    public void Init( AudioSoundData data, Vector2 pos ) {
        SetPosition( pos );
        
        soundData = data;

        audioSource.clip = data.clip;
        audioSource.outputAudioMixerGroup = data.mixerGroup;
        audioSource.volume = data.volume;
        audioSource.pitch = data.pitch;

        audioSource.loop = data.loop;
        audioSource.playOnAwake = data.playOnAwake;
    }

    public void SetPosition( Vector2 pos ) {
        transform.position = pos;
    }

    public void SetAudioManager(AudioManager manager) {
        if ( audioManager == manager ) return;
        
        audioManager = manager;
    }

    public void SetParent(Transform parent) {
        transform.parent = parent;
    }

    public void RandomizePitch(float min = -0.1f, float max = 0.1f) {
        audioSource.pitch += Random.Range( min, max );
    }

    public void SetRandomPitch( float min = -1f, float max = 1.2f ) {
        min = Mathf.Max(-3, min);
        max = Mathf.Min( max, 3 );

        audioSource.pitch += Random.Range( min, max );
    }
}
