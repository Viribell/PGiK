using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBehaviour : MonoBehaviour {
    [Header("Normal drop based on chance")]
    [SerializeField] public List<ItemSO> dropList = new List<ItemSO>();

    [Header( "Gold Drop Config" )]
    [SerializeField] public bool dropGoldFromRange = false;
    [SerializeField] public int goldRangeMin;
    [SerializeField] public int goldRangeMax;
    [SerializeField] public int bulkGoldAmount;

    [Header( "Map Phase Influence" )]
    [SerializeField] public bool useMapPhaseFactor = false;

    [Header( "Drop Limit Config" )]
    [SerializeField] public bool limitNumberOfDrops = false;
    [Range(1, 10)]
    [SerializeField] public int maxNumberOfDrops = 2;

    private List<ItemSO> GetDrops() {
        float chanceRoll = 0;
        List<ItemSO> possibleDrops = new List<ItemSO>();
        int mapFactor = useMapPhaseFactor ? LevelControl.Instance.levelStadium : 1;

        foreach( ItemSO item in dropList) {
            chanceRoll = Random.Range( 0.0f, 1.0f );

            if ( !item.usesDropChance ) { Debug.Log("Item doesn't use drop chance!"); continue; }

            if ( chanceRoll <= NormalizeChance( item.dropChance * mapFactor ) ) possibleDrops.Add( item );

        }

        if ( possibleDrops.Count > 0 ) return possibleDrops;
        else return null;
    }

    private List<ItemSO> GetDrops(float dropChance) {
        dropChance = NormalizeChance( dropChance );

        float chanceRoll = 0;
        List<ItemSO> possibleDrops = new List<ItemSO>();

        foreach ( ItemSO item in dropList ) {
            chanceRoll = Random.Range( 0.0f, 1.0f );

            if ( chanceRoll <= dropChance ) possibleDrops.Add( item );

        }

        if ( possibleDrops.Count > 0 ) return possibleDrops;
        else return null;
    }

    public void Drop( Vector2 pos ) {
        List<ItemSO> drops = GetDrops();

        if ( drops == null ) return;

        if ( limitNumberOfDrops ) LimitedSpawn( drops, pos );
        else Spawn( drops, pos );
    }

    public void Drop( Vector2 pos, float dropChance ) {
        List<ItemSO> drops = GetDrops( dropChance );

        if ( drops == null ) return;

        if ( limitNumberOfDrops ) LimitedSpawn( drops, pos );
        else Spawn( drops, pos );
    }

    private void SpawnGold( ItemSO item, Vector2 pos ) {
        if ( dropGoldFromRange ) SpawnControl.Instance.SpawnGold( item, goldRangeMin, goldRangeMax, pos );
        else SpawnControl.Instance.SpawnGold( item, bulkGoldAmount, pos ); 
    }

    private void Spawn( List<ItemSO> drops, Vector2 pos ) {
        foreach ( ItemSO item in drops ) {
            if ( item.itemType == ItemType.Gold ) { SpawnGold( item, pos ); continue; }
            SpawnControl.Instance.SpawnReadyItem( item, pos );
        }
    }

    private void LimitedSpawn( List<ItemSO> drops, Vector2 pos ) {
        int count = drops.Count;

        if ( count > maxNumberOfDrops ) count = maxNumberOfDrops;

        for( int i = 0; i < count; i++ ) {
            ItemSO item = drops[i];

            if ( item.itemType == ItemType.Gold ) { SpawnGold( item, pos ); continue; }
            SpawnControl.Instance.SpawnReadyItem( item, pos );
        }

    }

    private float NormalizeChance( float dropChance ) {
        if ( dropChance < 0.0f ) dropChance = 0;
        else if ( dropChance > 1.0f ) dropChance = 1.0f;

        return dropChance;
    }
}
