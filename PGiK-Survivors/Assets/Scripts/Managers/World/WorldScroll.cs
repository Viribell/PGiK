using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class WorldScroll : MonoBehaviour {
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float tileSize = 20.0f;

    [SerializeField] private Vector2 lastPlayerPos;
    [SerializeField] private Vector2 lastTilePos;
    [SerializeField] private float tileUpdateDistance;

    private List<GameObject> tiles;

    private void Awake() {
        tiles = new List<GameObject>();


        lastPlayerPos = playerTransform.position;
        lastTilePos = GetTileCoords( playerTransform.position );

        if( tileUpdateDistance == default ) tileUpdateDistance = 5;
    }

    private void Update() {
        if ( PauseControl.IsGamePaused ) return;

        if ( playerTransform == null ) return;

        CheckForUpdate();
    }

    private void CheckForUpdate() {
        if ( Vector2.Distance( playerTransform.position, lastPlayerPos ) > tileUpdateDistance ) {
            lastPlayerPos = playerTransform.position;
            Vector2 currTilePos = GetTileCoords( playerTransform.position );

            if ( currTilePos != lastTilePos ) {
                UpdateTiles( currTilePos );
                lastTilePos = currTilePos;
            }
        }
    }

    private void UpdateTiles(Vector2 playerTile) {
        List<GameObject> activeTiles = new List<GameObject>();
        List<GameObject> freeTiles = new List<GameObject>();

        for ( int i = -2; i <= 1; i++ ) {
            for ( int j = -2; j <= 1; j++ ) {
                Vector2 tileCoords = new Vector2( playerTile.x + i, playerTile.y + j );
                GameObject tile = TileExistsAt( tileCoords );

                if ( tile != null ) activeTiles.Add( tile );

            }
        }

        foreach(GameObject tile in tiles) {
            if ( activeTiles.IndexOf( tile ) >= 0 ) continue;
            freeTiles.Add( tile );
        }

        for ( int i = -2; i <= 1; i++ ) {
            for ( int j = -2; j <= 1; j++ ) {
                Vector2 tileCoords = new Vector2( playerTile.x + i, playerTile.y + j );

                if( !TileExistsAt(tileCoords) ) {
                    Vector2 newPos = new Vector2( tileCoords.x * tileSize, tileCoords.y * tileSize );
                    if( freeTiles.Count > 0 ) {
                        freeTiles[0].transform.position = newPos;
                        freeTiles.RemoveAt( 0 );
                    }
                }
            }
        }
    }
    
    private Vector2 GetTileCoords(Vector2 pos) {
        int tileX =  (int)(pos.x / tileSize );
        int tileY = (int)(pos.y / tileSize );

        return new Vector2( tileX, tileY );
    }

    private GameObject TileExistsAt(Vector2 tileCoords) {
        foreach(GameObject tile in tiles) {
            Vector2 pos = tile.transform.position;
            if ( pos.x == tileCoords.x * tileSize && pos.y == tileCoords.y * tileSize ) return tile;
        }
        
        return null;
    }

    public void Add( GameObject tileObject ) {
        tiles.Add( tileObject );
    }
}
