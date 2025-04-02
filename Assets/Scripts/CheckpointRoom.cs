using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CheckpointRoom : Room
{
    public Tile leftWallTile;
    public Tile rightWallTile;
    public Tile topWallTile;
    public Tile bottomWallTile;
    public Tile leftDoorTile;
    public Tile rightDoorTile;
    private GameObject foreground;
    private Tilemap foregroundTilemap;
    private BoundsInt bounds;
    private TileBase[] allTiles;
    private Vector2 leftTopCorner = new Vector2(0, 0);
    private Vector2 rightTopCorner = new Vector2(0, 0);
    private Vector2 leftBottomCorner = new Vector2(0, 0);
    private Vector2 rightBottomCorner = new Vector2(0, 0);

    // Start is called before the first frame update
    public void SetupRoom(int inDoor, int outDoor){

        Debug.Log("Setting up checkpoint room");

        foreground = transform.GetChild(1).gameObject;

        //get the tilemap
        foregroundTilemap = foreground.GetComponent<Tilemap>();

        bounds = new BoundsInt((int)-width, (int)-height, 0, (int)(2 * width), (int)(2 * height), 1);
        allTiles = foregroundTilemap.GetTilesBlock(bounds);

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                int tileIndex = (x - bounds.xMin) + (y - bounds.yMin) * bounds.size.x;
                if (allTiles[tileIndex] != null)
                {
                    switch(allTiles[tileIndex].name)
                    {
                        case "tile015":
                            if (leftTopCorner == new Vector2(0, 0))
                            {
                                leftTopCorner = new Vector2(x, y);
                            }
                            break;
                        case "tile016":
                            if (rightTopCorner == new Vector2(0, 0))
                            {
                                rightTopCorner = new Vector2(x, y);
                            }
                            break;
                        case "tile024":
                            if (leftBottomCorner == new Vector2(0, 0))
                            {
                                leftBottomCorner = new Vector2(x, y);
                            }
                            break;
                        case "tile025":
                            if (rightBottomCorner == new Vector2(0, 0))
                            {
                                rightBottomCorner = new Vector2(x, y);
                            }
                            break;
                    }
                }
                else{
                    Debug.Log("Tile is null at: " + x + ", " + y);
                }
            }
        }

        //create the in door
        if(inDoor == 0){
            //remove the four tiles above the left bottom corner
            foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x, (int)leftBottomCorner.y + 1, 0), null);
            foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x, (int)leftBottomCorner.y + 2, 0), null);
            foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x, (int)leftBottomCorner.y + 3, 0), null);
            foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x, (int)leftBottomCorner.y + 4, 0), null);
            foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x, (int)leftBottomCorner.y + 5, 0), leftDoorTile);
            foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x, (int)leftBottomCorner.y, 0), bottomWallTile);
            leftSocket = 1;
        }
        else if(inDoor == 1){
            //remove the four tiles above the right bottom corner
            foregroundTilemap.SetTile(new Vector3Int((int)rightBottomCorner.x, (int)rightBottomCorner.y + 1, 0), null);
            foregroundTilemap.SetTile(new Vector3Int((int)rightBottomCorner.x, (int)rightBottomCorner.y + 2, 0), null);
            foregroundTilemap.SetTile(new Vector3Int((int)rightBottomCorner.x, (int)rightBottomCorner.y + 3, 0), null);
            foregroundTilemap.SetTile(new Vector3Int((int)rightBottomCorner.x, (int)rightBottomCorner.y + 4, 0), null);
            foregroundTilemap.SetTile(new Vector3Int((int)rightBottomCorner.x, (int)rightBottomCorner.y + 5, 0), rightDoorTile);
            foregroundTilemap.SetTile(new Vector3Int((int)rightBottomCorner.x, (int)rightBottomCorner.y, 0), bottomWallTile);
            rightSocket = 1;
        }
        else if(inDoor == 2){
            //remove the four tiles above the left top corner
            foregroundTilemap.SetTile(new Vector3Int((int)leftTopCorner.x + 4, (int)leftTopCorner.y, 0), null);
            foregroundTilemap.SetTile(new Vector3Int((int)leftTopCorner.x + 5, (int)leftTopCorner.y, 0), null);
            foregroundTilemap.SetTile(new Vector3Int((int)leftTopCorner.x + 6, (int)leftTopCorner.y, 0), null);
            foregroundTilemap.SetTile(new Vector3Int((int)leftTopCorner.x + 7, (int)leftTopCorner.y, 0), null);
            foregroundTilemap.SetTile(new Vector3Int((int)leftTopCorner.x + 3, (int)leftTopCorner.y, 0), leftDoorTile);
            foregroundTilemap.SetTile(new Vector3Int((int)leftTopCorner.x + 8, (int)leftTopCorner.y, 0), rightDoorTile);
            topSocket = 1;
            
        }
        else if(inDoor == 3){
            //remove the four tiles in the top-center
            foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x + 4, (int)leftBottomCorner.y, 0), null);
            foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x + 5, (int)leftBottomCorner.y, 0), null);
            foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x + 6, (int)leftBottomCorner.y, 0), null);
            foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x + 7, (int)leftBottomCorner.y, 0), null);
            foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x + 4, (int)leftBottomCorner.y, 0), leftDoorTile);
            foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x + 7, (int)leftBottomCorner.y, 0), rightDoorTile);
            bottomSocket = 1;

        }

        if(outDoor == 0){
            //remove the four tiles above the left bottom corner
            foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x, (int)leftBottomCorner.y + 1, 0), null);
            foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x, (int)leftBottomCorner.y + 2, 0), null);
            foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x, (int)leftBottomCorner.y + 3, 0), null);
            foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x, (int)leftBottomCorner.y + 4, 0), null);
            foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x, (int)leftBottomCorner.y + 5, 0), leftDoorTile);
            foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x, (int)leftBottomCorner.y, 0), bottomWallTile);
            leftSocket = -1;
        }
        else if(outDoor == 1){
            //remove the four tiles above the right bottom corner
            foregroundTilemap.SetTile(new Vector3Int((int)rightBottomCorner.x, (int)rightBottomCorner.y + 1, 0), null);
            foregroundTilemap.SetTile(new Vector3Int((int)rightBottomCorner.x, (int)rightBottomCorner.y + 2, 0), null);
            foregroundTilemap.SetTile(new Vector3Int((int)rightBottomCorner.x, (int)rightBottomCorner.y + 3, 0), null);
            foregroundTilemap.SetTile(new Vector3Int((int)rightBottomCorner.x, (int)rightBottomCorner.y + 4, 0), null);
            foregroundTilemap.SetTile(new Vector3Int((int)rightBottomCorner.x, (int)rightBottomCorner.y + 5, 0), rightDoorTile);
            foregroundTilemap.SetTile(new Vector3Int((int)rightBottomCorner.x, (int)rightBottomCorner.y, 0), bottomWallTile);
            rightSocket = -1;
        }
        else if(outDoor == 2){
            //remove the four tiles above the left top corner
            foregroundTilemap.SetTile(new Vector3Int((int)leftTopCorner.x + 4, (int)leftTopCorner.y, 0), null);
            foregroundTilemap.SetTile(new Vector3Int((int)leftTopCorner.x + 5, (int)leftTopCorner.y, 0), null);
            foregroundTilemap.SetTile(new Vector3Int((int)leftTopCorner.x + 6, (int)leftTopCorner.y, 0), null);
            foregroundTilemap.SetTile(new Vector3Int((int)leftTopCorner.x + 7, (int)leftTopCorner.y, 0), null);
            foregroundTilemap.SetTile(new Vector3Int((int)leftTopCorner.x + 3, (int)leftTopCorner.y, 0), leftDoorTile);
            foregroundTilemap.SetTile(new Vector3Int((int)leftTopCorner.x + 8, (int)leftTopCorner.y, 0), rightDoorTile);
            topSocket = -1;

        }
        else if(outDoor == 3){
            //remove the four tiles in the top-center
            foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x + 4, (int)leftBottomCorner.y, 0), null);
            foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x + 5, (int)leftBottomCorner.y, 0), null);
            foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x + 6, (int)leftBottomCorner.y, 0), null);
            foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x + 7, (int)leftBottomCorner.y, 0), null);
            foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x + 8, (int)leftBottomCorner.y, 0), leftDoorTile);
            foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x + 3, (int)leftBottomCorner.y, 0), rightDoorTile);
            bottomSocket = -1;

        }
    }

    
}
