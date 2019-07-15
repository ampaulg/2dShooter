using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : CellScript {

    private const float MIN_SPAWN_DISTANCE = 6.5f;

    public List <GameObject> possibleEnemies;
    public GameObject bossPrefab;

    public GameObject healthPrefab;
    public GameObject treasurePrefab;

    private bool isGoalRoom;
    public bool IsGoalRoom {
        get { return isGoalRoom; }
        set { isGoalRoom = value; }  
    }
    private bool isStartRoom;
    public bool IsStartRoom {
        get { return IsStartRoom; }
        set { IsStartRoom = value; }  
    }

    private bool isDeadEnd;

    private int difficulty;
    private bool spawning = false;
    private GameObject[] wallTriggers;
    List< GameObject > spawnList;
    List< GameObject > enemies;
    public GameObject wallTriggerPrefab;
    public GameObject portalPrefab;

    public int spawnRate;
    private int spawnTimer = 0;

    private int liveEnemyCount = 0;

    private const int DROP_ROLL_RANGE = 256;
    public float deadEndDropChance;
    public float midLevelDropChance;

    protected override void Start() {
        base.Start();
        MakeWallTriggers();
        EnemySetup();
    }

    private void Update() {
        if ( spawning ){
            SpawnEnemies();
        }
    }

    private void SpawnEnemies() {
        if ( spawnTimer == 0 ) {
            GameObject newEnemy = Instantiate( spawnList[ 0 ] );
            newEnemy.GetComponent<EnemyScript>().Owner = this;
            enemies.Add( newEnemy );
            PlaceEnemy( newEnemy );
            spawnList.RemoveAt( 0 );

            spawnTimer = spawnRate;
            if ( spawnList.Count == 0 ) {
                spawning = false;
            }

            liveEnemyCount++;
        }
        spawnTimer--;
    }

    public override void SetupFromNode( MazeNode node ) {
        base.SetupFromNode( node );

        isStartRoom = ( node.RType == MazeNode.RoomType.LevelStart );
        isGoalRoom = ( node.RType == MazeNode.RoomType.LastRoom );
        difficulty = node.Difficulty;
        isDeadEnd = ( ( openingCount == 1 ) && ( !isStartRoom ) && ( !isGoalRoom ) );
    }

    private void PlaceEnemy( GameObject enemy ) {
        bool spotFound = false;

        float gridWidth = ( (  width * tileWidth ) / ( enemyGridSize + 1 ) );
        float spawnAreaWidth = gridWidth * ( enemyGridSize - 1 );
        float spawnXInitial = transform.position.x - ( spawnAreaWidth / 2 );
        float spawnYInitial = transform.position.y - ( spawnAreaWidth / 2 );
        GameObject player = GameObject.FindWithTag("Player");

        while ( !spotFound ) {
            Vector3 spawnPosition = new Vector3( spawnXInitial, spawnYInitial, 0 );
            int spawnXCoord = Random.Range( 0, enemyGridSize );
            int spawnYCoord = Random.Range( 0, enemyGridSize );
            spawnPosition.x += ( spawnXCoord * gridWidth );
            spawnPosition.y += ( spawnYCoord * gridWidth );

            if ( ( spawnPosition - player.transform.position ).magnitude > MIN_SPAWN_DISTANCE ) {
                enemy.transform.position = spawnPosition;
                spotFound = true;
            }
        }
    }

    void EnemySetup() {
        spawnList = new List< GameObject >();
        enemies = new List< GameObject >();
        int remainingSpace = difficulty;
        if ( isGoalRoom ) {
            spawnList.Add( bossPrefab );
            remainingSpace -= bossPrefab.GetComponent<EnemyScript>().powerLevel;
            if ( difficulty > bossPrefab.GetComponent<EnemyScript>().powerLevel * 2 ) {
                possibleEnemies.Add( bossPrefab );
            }
        }

        while ( remainingSpace > 0 ) {
            GameObject newEnemy = possibleEnemies[ Random.Range( 0, possibleEnemies.Count ) ];
            spawnList.Add( newEnemy );
            remainingSpace -= newEnemy.GetComponent<EnemyScript>().powerLevel;
        }
    }

    void MakeWallTriggers() {
        wallTriggers = new GameObject[ 4 ];

        GameObject upTrigger = Instantiate( wallTriggerPrefab );
        upTrigger.transform.parent = transform;
        upTrigger.transform.localPosition = new Vector3( 0, -yInitial, 0);
        upTrigger.GetComponent<BoxCollider2D>().size = new Vector2( tileWidth * width,
                                                                    tileWidth );
        upTrigger.tag = "WallTriggerU";

        GameObject downTrigger = Instantiate( wallTriggerPrefab );
        downTrigger.transform.parent = transform;
        downTrigger.transform.localPosition = new Vector3( 0, yInitial, 0);
        downTrigger.GetComponent<BoxCollider2D>().size = new Vector2( tileWidth * width,
                                                                      tileWidth );
        downTrigger.tag = "WallTriggerD";

        GameObject leftTrigger = Instantiate( wallTriggerPrefab );
        leftTrigger.transform.parent = transform;
        leftTrigger.transform.localPosition = new Vector3( xInitial, 0, 0);
        leftTrigger.GetComponent<BoxCollider2D>().size = new Vector2( tileWidth,
                                                                      tileWidth * height);
        leftTrigger.tag = "WallTriggerL";

        GameObject rightTrigger = Instantiate( wallTriggerPrefab );
        rightTrigger.transform.parent = transform;
        rightTrigger.transform.localPosition = new Vector3( -xInitial, 0, 0);
        rightTrigger.GetComponent<BoxCollider2D>().size = new Vector2( tileWidth,
                                                                       tileWidth * height);
        rightTrigger.tag = "WallTriggerR";
    }

    protected override void TileSetup() {
        for ( int y = 0; y < height; y++ ) {
            for ( int x = 0; x < width; x++ ) {

                tileType type;
                if ( IsBoundary( y, x ) && !( IsDoor( y, x ) ) ) {
                    type = tileType.Wall;
                } else {
                    type = tileType.Floor;
                }
                makeTile( type, y, x );
            }       
        }
    }

    bool IsBoundary( int y, int x ) {
    	return ( y == 0 ) ||
    		   ( y == ( height - 1 ) ) ||
    		   ( x == 0 ) ||
    		   ( x == ( width - 1 ) );
    }

    bool IsDoor( int y, int x ) {
        int vertDoorStart = ( height / 2 ) - ( doorWidth / 2 );
        if ( ( height % 2 == 0 ) || ( doorWidth % 2  == 0 ) ) {
            vertDoorStart--;
        }

        int horizDoorStart = ( width / 2 ) - ( doorWidth / 2 );
        if ( ( width % 2 == 0 ) || ( doorWidth % 2  == 0 ) ) {
            horizDoorStart--;
        }

        // if it's a bottom door
        if ( ( y == 0 ) && ( x >= horizDoorStart) && ( x < horizDoorStart + doorWidth ) && openDown ) {
            return true;
        }
        // if it's a top door
        if ( ( y == height - 1 ) && ( x >= horizDoorStart) && ( x < horizDoorStart + doorWidth ) && openUp ) {
            return true;
        }
        // if it's a left door
        if ( ( x == 0 ) && ( y >= vertDoorStart) && ( y < vertDoorStart + doorWidth ) && openLeft ) {
            return true;
        }
        // if it's a right door
        if ( ( x == width - 1 ) && ( y >= vertDoorStart) && ( y < vertDoorStart + doorWidth ) && openRight ) {
            return true;
        }

        // else return false
        return false;
    }

    public void AddEnemy( GameObject newEnemy ) {
        spawnList.Add( newEnemy );
        newEnemy.transform.parent = transform;
    }

    public void ActivateRoom() {
        if ( !entered ) {
            entered = true;
            spawning = true;
        }
    }

    public void ClearEnemies() {
        for ( int i = 0; i < enemies.Count; i++ ) {
            Destroy( enemies[ i ] );
        }
        while ( enemies.Count > 0 ) {
            enemies.RemoveAt( 0 );
        }
    }

    public void DecrementEnemyCount() {
        liveEnemyCount--;
        if ( liveEnemyCount == 0 ) {
            if ( isGoalRoom ) {
                Instantiate( portalPrefab, transform );   
            } else {
                if ( PickupDropSuccess() ) {
                    GameObject player = GameObject.FindWithTag("Player");
                    if ( player.GetComponent<PlayerScript>().Health == 
                         player.GetComponent<PlayerScript>().MaxHealth ) {
                        Instantiate( treasurePrefab, transform );
                    } else {
                        Instantiate( healthPrefab, transform );   
                    }
                }
            }
        }
    }

    private bool PickupDropSuccess() {
        float successRange;
        if ( isDeadEnd ) {
            successRange = DROP_ROLL_RANGE * deadEndDropChance;
        } else {
            successRange = DROP_ROLL_RANGE * midLevelDropChance;
        }
        int roll = Random.Range( 0, DROP_ROLL_RANGE );
        return ( roll < successRange );
    }

    // private void TestSpawnPoints() {
    //     float gridWidth = ( (  width * tileWidth ) / ( enemyGridSize + 1 ) );
    //     Debug.Log( "gridWidth:" + gridWidth );
    //     float totalWidth = gridWidth * ( enemyGridSize - 1 );

    //     float spawnXInitial = transform.position.x - ( totalWidth / 2 );
    //     float spawnYInitial = transform.position.y - ( totalWidth / 2 );
    //     Debug.Log( "spawnXInitial:" + spawnXInitial );
    //     Debug.Log( "spawnYInitial:" + spawnYInitial );
    //     GameObject player = GameObject.FindWithTag("Player");
    //     Debug.Log( "player:" + player.transform.position );
    //     for ( int i = 0; i < enemyGridSize; i++ ) {
    //         for ( int j = 0; j < enemyGridSize; j++ ) {
    //             Vector3 spawnPosition = new Vector3( spawnXInitial, spawnYInitial, 0 );
    //             spawnPosition.x += ( i * gridWidth );
    //             spawnPosition.y += ( j * gridWidth );
    //             Debug.Log( "spawnPosition" + i + " " + j + ": " + spawnPosition );
    //             if ( ( spawnPosition - player.transform.position ).magnitude < MIN_SPAWN_DISTANCE ) {
    //                 Debug.Log( "too close=======================" );
    //             } else {
    //                 Debug.Log( "can spawn here" );
    //                 GameObject testEnemy = Instantiate( bossPrefab, spawnPosition, Quaternion.identity );
    //             }
    //         }
    //     }
    // }
   
}
