using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnControl : MonoBehaviour {
    public static SpawnControl Instance { get; private set; }

    [Header("Spawn Prefabs")]
    [SerializeField] public ResourceDropPickup resourceDropPrefab;
    [SerializeField] public ResourceDropPickup magneticResourceDropPrefab;
    [SerializeField] public MaterialInteractable materialPrefab;
    [SerializeField] public ExpPointPickup expPointPrefab;
    [SerializeField] public ConsumablePickup consumablePrefab;
    [SerializeField] public BasicDesctructible basicDestructPrefab;

    [Header( "Spawns Hierarchy Target" )]
    [SerializeField] private Transform itemsParent;
    [SerializeField] private Transform enemiesParent; 

    [Header( "Spawn Force Config" )]
    [SerializeField] public bool useForceOnDrop = false;
    [SerializeField] public float dropForce;
    [SerializeField] public ForceMode2D forceMode;

    [Header( "RigidBody Normalization Config" )]
    [Range(0.0f, 200.0f)]
    [SerializeField] public float rbMass = 60.0f;
    [Range( 0.0f, 10.0f )]
    [SerializeField] public float rbLiniearDrag = 2.0f;
    [Range( 0.0f, 10.0f )]
    [SerializeField] public float rbAngularDrag = 0.0f;

    private void Awake() {
        if ( Instance == null ) { Instance = this; }
    }

    public void TestRes( ResourceSO resource ) { SpawnMagneticResourceDrop( resource, 1, new Vector2( 10.0f, 0.0f ) ); }
    public void TestSpawn( GameObject prefab ) { SpawnPrefab( prefab, new Vector2( 10.0f, 0.0f ) ); }
    public void TestMat( ResourceSO material ) { SpawnMaterial( material, new Vector2( 10.0f, 0.0f ) ); }
    public void TestReadyRescue( NPCRescueSO rescueData ) { SpawnReadyRescue( rescueData, new Vector2( 10.0f, 0.0f ) ); }
    public void TestReadyBasicDesctruct( DestructibleSO destruct ) { SpawnReadyBasicDestruct( destruct, new Vector2( 10.0f, 0.0f ) ); }
    public void TestReadyItem( ItemSO item ) { SpawnReadyItem( item, new Vector2( 10.0f, 0.0f ) ); }

    public void SpawnResourceDrop( ResourceSO resource, int amount, Vector2 pos ) {
        if ( resource == null ) { Debug.Log( "Given resource is null!" ); return; }

        ResourceDropPickup spawnedObject = Instantiate( resourceDropPrefab, pos, Quaternion.identity );
        spawnedObject.Init( resource, amount );
        Propel( spawnedObject.gameObject );
    }

    public void SpawnMagneticResourceDrop( ResourceSO resource, int amount, Vector2 pos ) {
        if ( resource == null ) { Debug.Log( "Given resource is null!" ); return; }

        ResourceDropPickup spawnedObject = Instantiate( magneticResourceDropPrefab, pos, Quaternion.identity );
        spawnedObject.Init( resource, amount );
        Propel( spawnedObject.gameObject );
    }

    public void SpawnExpPoint( ExpPointSO expPoint, Vector2 pos ) {
        if ( expPoint == null ) { Debug.Log( "Given expPoint is null!" ); return; }

        ExpPointPickup spawnedObject = Instantiate( expPointPrefab, pos, Quaternion.identity );
        spawnedObject.Init( expPoint );
        Propel( spawnedObject.gameObject );
    }

    public void SpawnConsumable( ConsumableSO consumable, Vector2 pos ) {
        if ( consumable == null ) { Debug.Log( "Given consumable is null!" ); return; }

        ConsumablePickup spawnedObject = Instantiate( consumablePrefab, pos, Quaternion.identity );
        spawnedObject.Init( consumable );
        Propel( spawnedObject.gameObject );
    }

    public void SpawnMaterial( ResourceSO material, Vector2 pos ) {
        if ( material == null ) { Debug.Log( "Given resource material is null!" ); return; }
        if ( !material.isMaterial ) { Debug.Log( "Given resource is not a material!" ); return; }

        MaterialInteractable spawnedObject = Instantiate( materialPrefab, pos, Quaternion.identity );
        spawnedObject.Init( material );
    }

    public void SpawnBasicDestruct( DestructibleSO destruct, Vector2 pos ) {
        if ( destruct == null ) { Debug.Log( "Given destructible is null!" ); return; }

        BasicDesctructible spawnedObject = Instantiate( basicDestructPrefab, pos, Quaternion.identity );
        spawnedObject.Init( destruct );
    }

    public void SpawnGold(ItemSO gold, int amount, Vector2 pos) {
        if ( gold.itemType != ItemType.Gold ) return;
        int mapFactor = 1; //map difficulty factor, implement later

        int goldAmount = amount * mapFactor;

        SpawnMagneticResourceDrop( (ResourceSO)gold, goldAmount, pos );
    }

    public void SpawnGold( ItemSO gold, int rangeMin, int rangeMax, Vector2 pos ) {
        if ( gold.itemType != ItemType.Gold ) return;
        int mapFactor = 1; //map difficulty factor, implement later

        int goldAmount = Random.Range( rangeMin, rangeMax ) * mapFactor;

        SpawnMagneticResourceDrop( ( ResourceSO )gold, goldAmount, pos );
    }


    public GameObject SpawnPrefab( GameObject prefab, Vector2 pos ) {
        if ( prefab == null ) { Debug.Log( "Given prefab is null!" ); return null; }

        return Propel( Instantiate( prefab, pos, Quaternion.identity ) );
    }

    public Transform SpawnPrefab( Transform prefab, Vector2 pos ) {
        if ( prefab == null ) { Debug.Log( "Given prefab is null!" ); return null; }

        return Propel( Instantiate( prefab, pos, Quaternion.identity ) );
    }

    private GameObject Propel( GameObject objectToPropel ) {
        if ( !useForceOnDrop ) return null;

        Vector2 dropDir = new Vector2( Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f) );
        Rigidbody2D rb = objectToPropel.GetComponent<Rigidbody2D>();

        if ( rb != null ) { NormalizeRigidBody( rb ); rb.AddForce( dropDir * dropForce, forceMode ); }

        return objectToPropel;
    }

    private Transform Propel( Transform objectToPropel ) {
        if ( !useForceOnDrop ) return null;

        Vector2 dropDir = new Vector2( Random.Range( -1.0f, 1.0f ), Random.Range( -1.0f, 1.0f ) );
        Rigidbody2D rb = objectToPropel.GetComponent<Rigidbody2D>();

        if ( rb != null ) { NormalizeRigidBody( rb ); rb.AddForce( dropDir * dropForce, forceMode ); }

        return objectToPropel;
    }


    private void AttachParent( GameObject child, Transform parent ) { child?.transform.SetParent( parent ); }
    private void AttachParent( Transform child, Transform parent ) { child?.transform.SetParent( parent ); }

    //This may uselessly take up resources
    private void NormalizeRigidBody(Rigidbody2D rb) {
        if ( rb.mass != rbMass ) rb.mass = rbMass;
        if ( rb.drag != rbLiniearDrag ) rb.drag = rbLiniearDrag;
        if ( rb.angularDrag != rbAngularDrag ) rb.angularDrag = rbAngularDrag;
    }


    public void SpawnReadyResource( ResourceSO resource, Vector2 pos ) { AttachParent( SpawnPrefab( resource.prefab, pos ), itemsParent ); }
    public void SpawnReadyRescue( NPCRescueSO rescue, Vector2 pos ) { SpawnPrefab( rescue.prefab, pos ); }
    public void SpawnReadyExpPoint( ExpPointSO expPoint, Vector2 pos ) { AttachParent( SpawnPrefab( expPoint.prefab, pos ), itemsParent ); }
    public void SpawnReadyItem( ItemSO item, Vector2 pos ) { AttachParent( SpawnPrefab( item.prefab, pos ), itemsParent ); }
    public void SpawnReadyConsumable( ConsumableSO consumable, Vector2 pos ) { AttachParent( SpawnPrefab( consumable.prefab, pos ), itemsParent ); }
    public void SpawnReadyBasicDestruct( DestructibleSO destruct, Vector2 pos ) { SpawnPrefab( destruct.prefab, pos ); }
}
