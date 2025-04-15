

[System.Serializable]
public class AudioSaveData {

    public float masterVolume;
    public float musicVolume;
    public float effectsVolume;

    public AudioSaveData() {
        masterVolume = musicVolume = effectsVolume = 1.0f;
    }

    public AudioSaveData(float masterVolume, float musicVolume, float effectsVolume ) {
        //max value should be 1.0f

        this.masterVolume = masterVolume;
        this.musicVolume = musicVolume;
        this.effectsVolume = effectsVolume;

    }

}
