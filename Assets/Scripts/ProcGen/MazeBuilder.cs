using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeBuilder : MonoBehaviour {
    
    public int mazeWidth;
    public int mazeHeight;

	public GameObject roomPrefab;
    public GameObject tunnelPrefab;
    public GameObject basicEnemy;
    public GameObject tankEnemy;
    public GameObject boss;

    private int currentLevel = 1;
    private const int BASE_DIFFICULTY = 3;

    private GameObject player;
    private GameObject startRoom;
    private List<GameObject> cells;

    private void Awake() {
    	cells = new List<GameObject>();
        player = GameObject.FindWithTag( "Player" );
    }

    public void MakeLevel() {
        ClearLevel();
        GenerateMaze();
        player.transform.position = startRoom.transform.position;
        player.GetComponent<PlayerScript>().StartSpawning();
    }

    private void ClearLevel() {
        ClearCells();
        GameObject[] objs;
        objs = GameObject.FindGameObjectsWithTag( "Bullet" );
        foreach( GameObject bullet in objs ) {
             Destroy( bullet );
        }
        objs = GameObject.FindGameObjectsWithTag( "Enemy" );
        foreach( GameObject enemy in objs ) {
             Destroy( enemy );
        }
    }

    private void ClearCells() {
        for ( int i = 0; i < cells.Count; i++ ) {
            Destroy( cells[ i ] );
        }
        while ( cells.Count > 0 ) {
            cells.RemoveAt( 0 );
        }
    }

    private void GenerateMaze() {
        int roomCount = mazeHeight * mazeWidth / 2;

        MazeNode[,] maze = new MazeNode[ mazeWidth, mazeHeight ];
        List<MazeNode> rooms = new List<MazeNode>();
        List<MazeNode> emptyRooms = new List<MazeNode>();

        CreateCells( maze, emptyRooms );
        CreateEdges( maze );
        CreateRooms( rooms, emptyRooms, roomCount );

        List<MazeNode> connectedNodes = new List<MazeNode>();
        List<MazeNode> disconnectedNodes = new List<MazeNode>( rooms );
        PopulateConnectedNodes( rooms[ 0 ], connectedNodes, disconnectedNodes );

        ConnectRemainingNodes( maze, connectedNodes, disconnectedNodes );
        
        InstantiateCells( maze ); 
    }

    private void ConnectRemainingNodes( MazeNode[,] maze, List<MazeNode> connectedNodes,
                                        List<MazeNode> disconnectedNodes ) {
        while ( disconnectedNodes.Count > 0 ) {
            MazeNode closestConnected = null;
            MazeNode closestDisconnected = null;
            float shortestDistance = maze.GetLength( 0 ) * maze.GetLength( 1 );
            for ( int i = 0; i < connectedNodes.Count; i++ ) {
                for ( int j = 0; j < disconnectedNodes.Count; j++ ) {
                    float distance = ( connectedNodes[ i ].Position - disconnectedNodes[ j ].Position ).magnitude;
                    if ( distance < shortestDistance ) {
                        shortestDistance = distance;
                        closestConnected = connectedNodes[ i ];
                        closestDisconnected = disconnectedNodes[ j ];
                    }
                }
            }
            ConnectNodes( closestConnected, closestDisconnected, maze );
            PopulateConnectedNodes( closestDisconnected, connectedNodes, disconnectedNodes );
        }
    }

    private void InstantiateCells( MazeNode[,] maze ) {
        for ( int i = 0; i < maze.GetLength( 0 ); i++ ) {
            for ( int j = 0; j < maze.GetLength( 1 ); j++ ) {
                if ( maze[ i, j ].CType == MazeNode.CellType.Room ) {
                    cells.Add( InstantiateRoom( maze[ i, j ] ) );
                } else if ( maze[ i, j ].CType == MazeNode.CellType.Tunnel ) {
                    cells.Add( InstantiateTunnel( maze[ i, j ] ) );
                }
            }
        }
    }

    private void ConnectNodes( MazeNode node1, MazeNode node2, MazeNode[,] maze ) {
        // if adjacent, they should already be connected
        if  ( ( node1.Position - node2.Position ).magnitude < 1.1 ) {
            return;
        } else {
            List<MazeNode> possibleTunnelCells = new List<MazeNode>();
            
            if ( node1.Position.y < node2.Position.y ) {
                possibleTunnelCells.Add( maze[ node1.Position.x, node1.Position.y + 1 ] );
            } else if ( node1.Position.y > node2.Position.y ){
                possibleTunnelCells.Add( maze[ node1.Position.x, node1.Position.y - 1 ] );
            }
            if ( node1.Position.x < node2.Position.x ) {
                possibleTunnelCells.Add( maze[ node1.Position.x + 1, node1.Position.y ] );
            } else if ( node1.Position.x > node2.Position.x ) {
                possibleTunnelCells.Add( maze[ node1.Position.x - 1, node1.Position.y ] );
            }

            MazeNode newTunnelCell = possibleTunnelCells[ Random.Range( 0, possibleTunnelCells.Count ) ];

            CreateTunnel( newTunnelCell );
            ConnectNodes( newTunnelCell, node2, maze );
        }
    }

    private void CreateTunnel( MazeNode newTunnelCell ) {
        newTunnelCell.CType = MazeNode.CellType.Tunnel;
        newTunnelCell.JoinAdjacentRooms();
    }

    private void CreateEdges( MazeNode[,] maze ) {
        for ( int x = 0; x < maze.GetLength( 0 ); x++ ) {
            for ( int y = 0; y < maze.GetLength( 1 ); y++ ) {
                maze[ x, y ].EdgeUp = new MazeEdge( maze[ x, y ] );
                maze[ x, y ].EdgeDown = new MazeEdge( maze[ x, y ] );
                maze[ x, y ].EdgeLeft = new MazeEdge( maze[ x, y ] );
                maze[ x, y ].EdgeRight = new MazeEdge( maze[ x, y ] );

                if ( x > 0 ) {
                    maze[ x, y ].EdgeLeft.OtherNode = maze[ x - 1, y ];
                    maze[ x, y ].EdgeLeft.OtherEdge = maze[ x - 1, y ].EdgeRight;

                    maze[ x - 1, y ].EdgeRight.OtherNode = maze[ x, y ];
                    maze[ x - 1, y ].EdgeRight.OtherEdge = maze[ x, y ].EdgeLeft;
                }
                if ( y > 0 ) {
                    maze[ x, y ].EdgeDown.OtherNode = maze[ x, y - 1 ];
                    maze[ x, y ].EdgeDown.OtherEdge = maze[ x, y - 1 ].EdgeUp;

                    maze[ x, y - 1 ].EdgeUp.OtherNode = maze[ x, y ];
                    maze[ x, y - 1 ].EdgeUp.OtherEdge = maze[ x, y ].EdgeDown;
                }
            }
        }
    }

    private void CreateCells( MazeNode[,] maze, List< MazeNode > emptyCells ) {
        for ( int x = 0; x < maze.GetLength( 0 ); x++ ) {
            for ( int y = 0; y < maze.GetLength( 1 ); y++ ) {
                MazeNode newNode = new MazeNode( new Vector2Int( x, y ) );
                maze[ x, y ] = newNode;
                emptyCells.Add( newNode );
            }
        }
    }

    private void CreateRooms( List<MazeNode> rooms, List<MazeNode> emptyCells, int roomCount ) {
        int levelDifficulty = currentLevel - 1;
        for ( int i = 0; i < roomCount; i++ ) {
            int newRoomIndex = Random.Range( 0, emptyCells.Count );
            MazeNode newRoom = emptyCells[ newRoomIndex ];
            emptyCells.RemoveAt( newRoomIndex );
            rooms.Add( newRoom );
            newRoom.CType = MazeNode.CellType.Room;
            newRoom.Difficulty = BASE_DIFFICULTY + ( levelDifficulty * 2 );
            newRoom.JoinAdjacentRooms();
        }

        MazeNode startNode = rooms[ 0 ];
        startNode.Difficulty = BASE_DIFFICULTY + levelDifficulty;
        startNode.RType = MazeNode.RoomType.LevelStart;
        MazeNode lastRoom = getFarthestRoom( rooms[ 0 ], rooms );
        lastRoom.RType = MazeNode.RoomType.LastRoom;
        lastRoom.Difficulty = BASE_DIFFICULTY + ( levelDifficulty * 3 );
    }

    private void PopulateConnectedNodes( MazeNode node, List<MazeNode> connectedNodes,
                                         List<MazeNode> disconnectedNodes ) {
        if ( connectedNodes.Contains( node ) ) { 
            return;
        }

        connectedNodes.Add( node );
        disconnectedNodes.Remove( node );

        if ( node.EdgeUp.IsOpen ) {
            PopulateConnectedNodes( node.EdgeUp.OtherNode, connectedNodes, disconnectedNodes );
        }
        if ( node.EdgeDown.IsOpen ) {
            PopulateConnectedNodes( node.EdgeDown.OtherNode, connectedNodes, disconnectedNodes );
        }
        if ( node.EdgeLeft.IsOpen ) {
            PopulateConnectedNodes( node.EdgeLeft.OtherNode, connectedNodes, disconnectedNodes );
        }
        if ( node.EdgeRight.IsOpen ) {
            PopulateConnectedNodes( node.EdgeRight.OtherNode, connectedNodes, disconnectedNodes );
        }
    }

    private MazeNode getFarthestRoom( MazeNode startRoom, List<MazeNode> rooms ) {
        float farthestDistance = 0;
        MazeNode farthest = startRoom;

        foreach ( MazeNode room in rooms ) {
            float currentDistance = ( startRoom.Position - room.Position ).magnitude;
            if  ( currentDistance > farthestDistance ) {
                farthest = room;
                farthestDistance = currentDistance;
            }
        }
        return farthest;
    }

    private GameObject InstantiateRoom( MazeNode node ) {
        GameObject room = Instantiate( roomPrefab );
        room.GetComponent<RoomScript>().SetupFromNode( node );
        if ( node.RType == MazeNode.RoomType.LevelStart ) {
            startRoom = room;
        }
        return room;
    }

    private GameObject InstantiateTunnel( MazeNode node ) {
        GameObject tunnel = Instantiate( tunnelPrefab );
        tunnel.GetComponent<TunnelScript>().SetupFromNode( node );
        return tunnel;
    }

    public void LevelUp() {
    	currentLevel++;
    }
}