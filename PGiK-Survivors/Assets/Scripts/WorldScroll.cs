using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldScroll : MonoBehaviour {
    [SerializeField] private Transform playerTransform;
    [SerializeField] private int tileHorizontalCount;
    [SerializeField] private int tileVerticalCount;
    [SerializeField] private float tileSize = 20.0f;
    [SerializeField] private Vector2Int playerTilePos;
    [SerializeField] private int fovHeight = 3;
    [SerializeField] private int fovWidth = 3;

    private Vector2Int currentTilePos;
    private Vector2Int tileGridPos;

    private GameObject[,] tiles;


    private void Awake() {
        tiles = new GameObject[tileHorizontalCount, tileVerticalCount];
        currentTilePos = new Vector2Int( 0, 0 );
    }

    private void Update() {
        playerTilePos.x = ( int )( playerTransform.position.x / (tileSize / 2) );
        playerTilePos.y = ( int )( playerTransform.position.y / (tileSize / 2 ) );

        playerTilePos.x -= playerTransform.position.x < 0 ? 1 : 0;
        playerTilePos.y -= playerTransform.position.y < 0 ? 1 : 0;

        if ( currentTilePos != playerTilePos ) {
            currentTilePos = playerTilePos;

            tileGridPos.x = CalculateWrapPos( tileGridPos.x, true );
            tileGridPos.y = CalculateWrapPos( tileGridPos.y, false );

            UpdateTiles();
        }
    }

    private int CalculateWrapPos(float currVal, bool horizontal) {
        if( horizontal ) {
            if( currVal >= 0 ) {
                currVal = currVal % tileHorizontalCount;

            } else {
                currVal++;
                currVal = tileHorizontalCount + currVal % tileHorizontalCount;
            }

        } else {
            if ( currVal >= 0 ) {
                currVal = currVal % tileVerticalCount;

            } else {
                currVal++;
                currVal = tileVerticalCount + currVal % tileVerticalCount;
            }
        }

        return (int)currVal;
    }

    private void UpdateTiles() {
        for(int i = -(fovWidth/2); i <= fovWidth/2; i++ ) {
            for(int j = -(fovHeight/2); j <= fovHeight/2; j++ ) {
                int tileUpdateX = CalculateWrapPos( playerTilePos.x + i, true );
                int tileUpdateY = CalculateWrapPos( playerTilePos.y + j, false );

                GameObject tile = tiles[tileUpdateX, tileUpdateY];
                tile.transform.position = CalculateTile(playerTilePos.x + i, playerTilePos.y + j);
            }
        }
    }

    private Vector2 CalculateTile(int x, int y) {
        return new Vector2(x * tileSize, y * tileSize);
    }


    public void Add(GameObject tileObject, Vector2Int tilePos) {
        tiles[tilePos.x, tilePos.y] = tileObject;
    }
}
