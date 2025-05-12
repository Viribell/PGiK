using UnityEngine;

public enum EnemyType {
    Undefined,
    GreenSlime,
    BlueSlime,
    Goblin,
    Raccoon,
    Fairy,
    Skeleton,
    Wraith,
    Zombie,
    ThreeEyedRaven,
    Gravedigger,
    Scorpion,
    LiveCactus,
    Mummy,
    FennecFox,
    FlyingFish,
    BearMagician,
    RedBumblebee,
    Snowman,
    Medusa,
    Caveman,
    SpitterSpider,
    VenomousBat,

    GreenSlimeChampion,
    BlueSlimeChampion,
    GoblinChampion,
    RaccoonChampion,
    FairyChampion,
    SkeletonChampion,
    WraithChampion,
    ZombieChampion,
    ThreeEyedRavenChampion,
    GravediggerChampion,
    ScorpionChampion,
    LiveCactusChampion,
    MummyChampion,
    FennecFoxChampion,
    FlyingFishChampion,
    BearMagicianChampion,
    RedBumblebeeChampion,
    SnowmanChampion,
    MedusaChampion,
    CavemanChampion,
    SpitterSpiderChampion,
    VenomousBatChampion,

    Magbitu,
    Necapues,
    Dertem,
    Hierum,
    Demacr
}

[CreateAssetMenu( menuName = "Scriptable Objects/Entity/Enemy" )]
public class EnemySO : EntitySO {
    [Header( "Enemy Info" )]
    [field: SerializeField] public EnemyType enemyType;
    [field: SerializeField] public GameObject prefab;
    [field: SerializeField] public int spawnWeight;
}
