using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public enum LevelType {
    Forest,
    Cemetery,
    Desert,
    Tundra,
    Cave
}

public class Timer {
    public float remainingTime;

    private float currentTime;
    private bool isStarted = false;
    private bool hasFinished = false;

    public Timer() {
        isStarted = false;
        hasFinished = false;
    }

    public Timer(float time) {
        remainingTime = time;
        isStarted = false;
        hasFinished = false;
    }

    public void Update() {
        if ( isStarted ) Countdown();
    }

    public void Restart(bool startAgain = false) {
        currentTime = remainingTime;
        isStarted = startAgain;
        hasFinished = false;
    }

    public void UpdateTime(float time) {
        remainingTime = time;
        currentTime = remainingTime;
    }

    public void StartCountdown() { isStarted = true; }
    public void StopCountdown() { isStarted = false; }

    public bool IsStarted() { return isStarted; }
    public bool HasFinished() { return hasFinished; }

    public float GetCurrentTime() { return currentTime; }
    public float GetElapsedTime() { return remainingTime - currentTime; }

    private void Countdown() {
        if ( currentTime > 0 ) { currentTime -= Time.deltaTime; } 
        else { currentTime = 0; hasFinished = true; isStarted = false; }
    }

}

public class LevelControl : MonoBehaviour, IPersistentData {
    public static LevelControl Instance { get; private set; }

    [Header( "Level Info" )]
    [field: SerializeField] public string levelId;
    [field: SerializeField] public LevelType level;
    [field: SerializeField] public int levelStadium;
    [field: SerializeField] public int baseGameMinutes;

    [Header( "Level State" )]
    [field: SerializeField] private List<EnemySO> currentWave;

    [Header( "Stadium Info" )]
    [field: SerializeField] public List<StatModifier> stadiumMods;
    [Range( 0.0f, 1.0f )] [field: SerializeField] private float modValueIncrease = 0.1f;

    [Header( "Materials Info" )]
    [field: SerializeField] private ResourceSO[] materials;
    [field: SerializeField] private float materialSpawnDistance;

    [Header( "NPC Rescuce Info" )]
    [field: SerializeField] private NPCRescueSO npcRescuce;
    [field: SerializeField] private float rescueSpawnDistance;

    [Header( "Wave Info" )]
    [field: SerializeField] private EnemySO[] levelEnemies;
    [field: SerializeField] private float waveTime;
    [field: SerializeField] private bool spawnMulitpleOnTick = false;
    [Range( 0.0f, 0.09f )] [field: SerializeField] private float spawnChancePerStadium = 0.05f;
    [Range( 1, 5 )] [field: SerializeField] private int maxEnemyTypes;
    [Range(0.1f, 1.0f)] [field: SerializeField] private float baseSpawnChance = 0.5f;

    [Header( "Enemy Info" )]
    [field: SerializeField] private float enemySpawnTime;
    [field: SerializeField] private float enemySpawnDistance;
    [field: SerializeField] private float enemySpawnDistanceMin;

    [Header( "Champion Info" )]
    [field: SerializeField] private EnemySO[] possibleChampions;
    [field: SerializeField] private EnemySO lastSpawnedChampion;
    [field: SerializeField] private float championSpawnTime;

    [Header( "Boss Info" )]
    [field: SerializeField] private EnemySO boss;
    [field: SerializeField] private EnemyController spawnedBoss;

    [field: Header( "Misc Info" )]
    [field: SerializeField] private int timeBonusBase = 15;
    [field: SerializeField] private int gameBonusBase = 250;
    [field: SerializeField] private ResourceSO gold;
    [field: SerializeField] private GameObject portalPrefab;

    private Timer enemySpawnTimer;
    private Timer championSpawnTimer;
    private Timer waveTimer;
    private Timer gameTimer;

    private bool finalStage = false;

    private float timeBonus;
    private float gameBonus;

    private void Awake() {
        if( Instance == null ) { Instance = this; }
        else {
            Debug.Log( "Instance of LevelControl already exists!" );
        }

        enemySpawnTimer = new Timer();
        championSpawnTimer = new Timer();
        waveTimer = new Timer();
        gameTimer = new Timer();

        currentWave = new List<EnemySO>();
        lastSpawnedChampion = null;

        levelStadium = GameState.Instance.chosenLevelStadium;

        UpdateStadiumMods();
    }

    private void Start() {
        enemySpawnTimer.UpdateTime( enemySpawnTime );
        championSpawnTimer.UpdateTime( championSpawnTime );
        waveTimer.UpdateTime( waveTime );
        gameTimer.UpdateTime( ( baseGameMinutes * 60 ) + ( ( levelStadium - 1 ) * ( 5 * 60 ) ) ); //10 minut + stadium * 5 minut

        enemySpawnTimer.StartCountdown();
        championSpawnTimer.StartCountdown();
        waveTimer.StartCountdown();
        gameTimer.StartCountdown();

        SpawnMaterials();
        SpawnRescue();

        UpdateWave();
    }

    private void Update() {
        if( PauseControl.IsGamePaused ) { return; }

        if ( !finalStage ) {
            HandleSpawnLogic();

        } 

    }

    private void UpdateStadiumMods() {
        if ( levelStadium == 1 ) { stadiumMods = null; return; }

        foreach(StatModifier mod in stadiumMods ) {
            mod.value += mod.value * ( (levelStadium - 1) * modValueIncrease );
        }
    }

    private void HandleSpawnLogic() {
        enemySpawnTimer.Update();
        championSpawnTimer.Update();
        waveTimer.Update();
        gameTimer.Update();

        if ( enemySpawnTimer.HasFinished() && !gameTimer.HasFinished() ) {
            SpawnEnemies();
            enemySpawnTimer.Restart( true );
        }

        if ( championSpawnTimer.HasFinished() && !gameTimer.HasFinished() ) {
            SpawnChampion();
            championSpawnTimer.Restart( true );
        }

        if( waveTimer.HasFinished() && !gameTimer.HasFinished() ) {
            UpdateWave();
            waveTimer.Restart( true );
        }

        if ( gameTimer.HasFinished() ) {
            SpawnBoss();
            finalStage = true;
            //gameTimer.Restart();
        }
    }

    #region Materials
    private void SpawnMaterials() {
        foreach(ResourceSO material in materials) {
            SpawnControl.Instance.SpawnReadyResource( material, GeneratePosition( materialSpawnDistance ) );
        }
    }
    #endregion

    #region Rescue
    private void SpawnRescue() {
        if ( npcRescuce == null || GameState.Instance.IsAvailable( npcRescuce.npc ) ) return;

        SpawnControl.Instance.SpawnReadyRescue( npcRescuce,  GeneratePosition( rescueSpawnDistance ) );
    }
    #endregion

    #region Enemies

    private void SpawnEnemies() {
        float chanceRoll = 0.0f;
        float spawnChance = baseSpawnChance + ( (levelStadium - 1) * spawnChancePerStadium );

        foreach( EnemySO enemy in currentWave ) {
            chanceRoll = Random.Range( 0.0f, 1.0f );

            if ( chanceRoll <= ( spawnChance / enemy.spawnWeight ) ) { 
                SpawnEnemy( enemy ); 
                if( !spawnMulitpleOnTick ) return; 
            } 

        }
    }

    private void SpawnEnemy( EnemySO enemy ) {
        SpawnControl.Instance.SpawnReadyEnemy( enemy, GeneratePosition( enemySpawnDistance, enemySpawnDistanceMin ) );
    }

    private void SpawnChampion() {
        int championNumber = 0;
        EnemySO champion = null;

        do {
            championNumber = Random.Range( 0, possibleChampions.Length );
            champion = possibleChampions[ championNumber ];

        } while ( champion == lastSpawnedChampion );


        SpawnControl.Instance.SpawnReadyEnemy( champion, GeneratePosition( enemySpawnDistance, enemySpawnDistanceMin ) );
    }

    private void SpawnBoss() {
        //TO BE CHANGED WHEN BOSS CLASS IS CREATED
        spawnedBoss = SpawnControl.Instance.SpawnReadyEnemy( boss, GeneratePosition( enemySpawnDistance, enemySpawnDistanceMin ) ).GetComponent<EnemyController>();

        spawnedBoss.SetDeathAction( () => { EndLevel(); } );
    }

    #endregion


    #region LevelEnd

    private void EndLevel() {
        StopTimers();

        MarkLevelBeaten();

        SpawnPortal();
    }

    private void SpawnPortal() {
        Transform player = RefCacheControl.Instance.Player.transform;
        Vector2 pos = new Vector2( player.position.x + 8, player.position.y + 8 );

        Instantiate( portalPrefab, pos, Quaternion.identity );
    }

    public void CalculateBonuses( EndScreenType screen ) {
        float minutes = Mathf.Floor( GetElapsedGameTime() / 60 );

        timeBonus = minutes * ( timeBonusBase * levelStadium );
        gameBonus = screen == EndScreenType.WinScreen ? ( gameBonusBase * levelStadium ) : 0;

    }

    private void GiveBonuses() {
        GameResources.Instance.AddToResource( gold, (int)timeBonus );
        GameResources.Instance.AddToResource( gold, (int)gameBonus );
    }

    public void EndScreen( EndScreenType screen ) {
        StopTimers();
        CalculateBonuses( screen );
        GiveBonuses();
        EndScreenControl.Instance.InitBonuses( timeBonus, gameBonus );
        EndScreenControl.Instance.Activate( screen, GetElapsedGameTime() );
    }

    private void StopTimers() {
        enemySpawnTimer.StopCountdown();
        championSpawnTimer.StopCountdown();
        waveTimer.StopCountdown();
        gameTimer.StopCountdown();
    }

    public float GetGameTime() { return gameTimer.GetCurrentTime(); }
    public float GetElapsedGameTime() { return gameTimer.GetElapsedTime(); }

    #endregion

    #region Misc

    private void UpdateWave() {
        float enemyTypes = Random.Range( 1, maxEnemyTypes + 1 );

        currentWave?.Clear();

        HashSet<int> enemyNumbers = new HashSet<int>();

        while ( enemyNumbers.Count < enemyTypes ) {
            enemyNumbers.Add( Random.Range( 0, levelEnemies.Length ) );
        }

        foreach ( int number in enemyNumbers ) {
            currentWave?.Add( levelEnemies[number] );
        }

    }

    private Vector2 GeneratePosition(float distance, float minDistance = 100.0f) {
        float negateRand = Random.Range( 0.0f, 1.0f );
        float directionRand = Random.Range( 0.0f, 1.0f );
        Vector2 pos = new Vector2();

        negateRand = negateRand > 0.5f ? -1.0f : 1.0f;

        if ( directionRand > 0.5f ) {
            pos.x = Random.Range( -distance, distance );
            pos.y = Random.Range( minDistance, distance ) * negateRand;

        } else {
            pos.y = Random.Range( -distance, distance );
            pos.x = Random.Range( minDistance, distance ) * negateRand;
        }

        pos += (Vector2)RefCacheControl.Instance.Player.transform.position;

        return pos;
    }

    #endregion

    #region SaveLoad
    public void MarkLevelBeaten() {
        SaveData data = SaveControl.Instance.GetSaveData();

        LevelSaveData levelData = data.levels.Find( item => string.Equals( item.levelId, levelId ) );

        levelData.beatenStadiums.Find( item => string.Equals( item.stadium, levelStadium ) ).isBeaten = true;
    }

    public void LoadData( SaveData data ) {
        if ( data.levels == null || data.levels.Count == 0 ) { Debug.LogWarning("Create new levels data."); data.CreateLevels(); }

        LevelSaveData levelData = data.levels.Find( item => string.Equals( item.levelId, levelId ) );

        if( levelData == null ) {
            levelData = new LevelSaveData( levelId );
            data.levels.Add( levelData );
        }
    }

    public void SaveData( SaveData data ) {
        LevelSaveData levelData = data.levels.Find( item => string.Equals( item.levelId, levelId ) );

        levelData.currentStadium = levelStadium;
    }
    #endregion
}
