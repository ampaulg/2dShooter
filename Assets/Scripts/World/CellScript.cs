using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CellScript : MonoBehaviour {

	protected enum tileType {
		Floor,
		Wall
	}

	public static int width = 25;
	public static int height = 25;
    public static int doorWidth = 3;
    protected int enemyGridSize = 5;

    protected float yInitial;
    protected float xInitial;

	public GameObject wallTile;
	public GameObject floorTile;
	protected float tileWidth;
    protected int openingCount = 0;

	protected bool openUp = false;
    public bool OpenUp {
        get { return openUp; }
        set { openUp = value; }  
    }
    protected bool openLeft = false;
    public bool OpenLeft {
        get { return openLeft; }
        set { openLeft = value; }  
    }
    protected bool openRight = false;
    public bool OpenRight {
        get { return openRight; }
        set { openRight = value; }  
    }
    protected bool openDown = false;
    public bool OpenDown {
        get { return openDown; }
        set { openDown = value; }  
    }

    protected GameObject[,] tiles;
    protected BoxCollider2D bc2d;

    protected bool entered = false;

	private void Awake() {
        tileWidth = wallTile.GetComponent<SpriteRenderer>().size.x;
    }

    protected virtual void Start() {
        yInitial = -( tileWidth * ( height - 1 ) ) / 2;
        xInitial = -( tileWidth * ( width - 1 ) ) / 2;
        tiles = new GameObject[ height, width ];


        bc2d = GetComponent<BoxCollider2D>();
        bc2d.size = new Vector2( tileWidth * ( width - 2 ),
                                 tileWidth * ( height - 2) );
        TileSetup();
    }

    protected void SetPosition( int x, int y ) {
        transform.position = new Vector3( tileWidth * ( width * x ), tileWidth * ( height *  y ), 0 );
    }

    public virtual void SetupFromNode( MazeNode node ) {
    	SetPosition( node.Position.x, node.Position.y );
        openUp = node.EdgeUp.IsOpen;
        openDown = node.EdgeDown.IsOpen;
        openLeft = node.EdgeLeft.IsOpen;
        openRight = node.EdgeRight.IsOpen;

        bool[] openings = { openUp, openDown, openLeft, openRight };
        foreach ( bool opening in openings ) {
            if ( opening ) {
                openingCount++;
            }
        }
    }

    protected abstract void TileSetup();

    protected void makeTile( tileType type, int y, int x ) {
        float yPos = yInitial + ( tileWidth * y );
        float xPos = xInitial + ( tileWidth * x );
        
        GameObject newTile;

        if ( type == tileType.Floor ) {
            newTile = Instantiate( floorTile, transform, false );
        } else {
            newTile = Instantiate( wallTile, transform, false );
        }
        tiles[ y, x ] = newTile;
        newTile.transform.localPosition = new Vector3( xPos, yPos, 0.0f );
    }
}
