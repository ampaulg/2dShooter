using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeEdge {

	private MazeNode thisNode;
	public MazeNode ThisNode {
		get { return thisNode; }
		set { thisNode = value; }
	}

	private MazeNode otherNode;
	public MazeNode OtherNode {
		get { return otherNode; }
		set { otherNode = value; }
	}

	private MazeEdge otherEdge;
	public MazeEdge OtherEdge {
		get { return otherEdge; }
		set { otherEdge = value; }
	}

	private bool isOpen = false;
	public bool IsOpen {
		get { return isOpen; }
		set { isOpen = value; }
	}	

	public MazeEdge( MazeNode thisNode ) {
         ThisNode = thisNode;
    }

    public void Open() {
    	if ( otherEdge != null ) {
    		isOpen = true;
    		otherEdge.IsOpen = true;
    	}
    }
}
