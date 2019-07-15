using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelScript : CellScript {

    protected override void TileSetup() {
        int vertDoorStart = ( height / 2 ) - ( doorWidth / 2 );
        int horizDoorStart = ( width / 2 ) - ( doorWidth / 2 );

        for ( int y = vertDoorStart; y < vertDoorStart + doorWidth; y++ ) {
            for ( int x = horizDoorStart; x < vertDoorStart + doorWidth; x++ ) {
                makeTile( tileType.Floor, y, x );
            }
        }

        if ( openUp ) {
            for ( int y = vertDoorStart + doorWidth; y < height; y++ ) {
               for ( int x = horizDoorStart - 1; x < horizDoorStart + doorWidth + 1; x++ ) {
                    if ( tiles[ y, x ] == null ) {
                        if ( ( x == horizDoorStart - 1 ) || ( x == horizDoorStart + doorWidth ) ) {
                            makeTile( tileType.Wall, y, x );
                        } else {
                            makeTile( tileType.Floor, y, x );
                        }
                    }
                }
            }    
        }
        if ( openDown ) {
            for ( int y = 0; y < vertDoorStart; y++ ) {
               for ( int x = horizDoorStart - 1; x < horizDoorStart + doorWidth + 1; x++ ) {
                    if ( tiles[ y, x ] == null ) {
                        if ( ( x == horizDoorStart - 1 ) || ( x == horizDoorStart + doorWidth ) ) {
                            makeTile( tileType.Wall, y, x );
                        } else {
                            makeTile( tileType.Floor, y, x );
                        }
                    }
                }
            }    
        }
        if ( openLeft ) {
            for ( int y = vertDoorStart - 1; y < vertDoorStart + doorWidth + 1; y++ ) {
               for ( int x = 0; x < horizDoorStart; x++ ) {
                    if ( tiles[ y, x ] == null ) {
                        if ( ( y == vertDoorStart - 1 ) || ( y == vertDoorStart + doorWidth ) ) {
                            makeTile( tileType.Wall, y, x );
                        } else {
                            makeTile( tileType.Floor, y, x );
                        }
                    }
                }
            }
        }
        if ( openRight ) {
            for ( int y = vertDoorStart - 1; y < vertDoorStart + doorWidth + 1; y++ ) {
               for ( int x = horizDoorStart + doorWidth; x < width; x++ ) {
                    if ( tiles[ y, x ] == null ) {
                        if ( ( y == vertDoorStart - 1 ) || ( y == vertDoorStart + doorWidth ) ) {
                            makeTile( tileType.Wall, y, x );
                        } else {
                            makeTile( tileType.Floor, y, x );
                        }
                    }
                }
            }
        }
        
        // fill out the area around the center tile as needed
        for ( int y = vertDoorStart - 1; y < vertDoorStart + doorWidth + 1; y++ ) {
            for ( int x = horizDoorStart - 1; x < vertDoorStart + doorWidth + 1; x++ ) {
                if ( tiles[ y, x ] == null ) {
                    makeTile( tileType.Wall, y, x );
                }
            }
        }
    }
}
