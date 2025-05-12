
using System.Collections.Generic;

[System.Serializable]
public class StadiumData {
    public int stadium;
    public bool isBeaten;

    public StadiumData(int stadium, bool isBeaten) {
        this.stadium = stadium;
        this.isBeaten = isBeaten;
    }
}

[System.Serializable]
public class LevelSaveData {
    public string levelId;
    public int currentStadium;
    public List<StadiumData> beatenStadiums;

    public LevelSaveData(string levelId) {
        this.levelId = levelId;

        beatenStadiums = new List<StadiumData>();
        currentStadium = 1;

        beatenStadiums.Add( new StadiumData( 1, false ) );
        beatenStadiums.Add( new StadiumData( 2, false ) );
        beatenStadiums.Add( new StadiumData( 3, false ) );
        beatenStadiums.Add( new StadiumData( 4, false ) );
        beatenStadiums.Add( new StadiumData( 5, false ) );
    }
}
