using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeNode {
	
	public enum CellType {
		Empty,
		Room,
		Tunnel
	}
	public enum RoomType {
		LevelStart,
		MiddleRoom,
		LastRoom
	}

	private MazeEdge edgeUp;
	public MazeEdge EdgeUp {
		get { return edgeUp; }
		set { edgeUp = value; }
	}

	private MazeEdge edgeDown;
	public MazeEdge EdgeDown {
		get { return edgeDown; }
		set { edgeDown = value; }
	}

	private MazeEdge edgeLeft;
	public MazeEdge EdgeLeft {
		get { return edgeLeft; }
		set { edgeLeft = value; }
	}

	private MazeEdge edgeRight;
	public MazeEdge EdgeRight {
		get { return edgeRight; }
		set { edgeRight = value; }
	}

	private CellType cType = CellType.Empty;
	public CellType CType {
		get { return cType; }
		set { cType = value; }	
	}

	private RoomType rType = RoomType.MiddleRoom;
	public RoomType RType {
		get { return rType; }
		set { rType = value; }	
	}

	private int difficulty = 0;
	public int Difficulty {
		get { return difficulty; }
		set { difficulty = value; }
	}

	private Vector2Int position;
	public Vector2Int Position {
		get { return position; }
		set { position = value; }
	}

	public MazeNode( Vector2Int position ) {
		Position = position;
	}

	public void JoinAdjacentRooms() {
		MazeEdge[] edges = {
			edgeUp,
			edgeDown,
			edgeLeft,
			edgeRight
		};

		for ( int i = 0; i < 4; i++ ) {
			if ( edges[ i ].OtherNode != null ) {
				if ( edges[ i ].OtherNode.CType != CellType.Empty ) {
					edges[ i ].Open();
				}
			}
		}

	}
}
