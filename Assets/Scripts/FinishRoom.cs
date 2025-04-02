using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FinishRoom : Room
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

    public void SetupRoom(int inDoor)
    {
        Debug.Log("Setting up the finish room");
    
        foreground = transform.GetChild(2).gameObject;

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
            }
        }

        int doorSide = inDoor;
        Debug.Log("Finish Room Setup");
        Debug.Log("DoorSide: " + doorSide);

        if (doorSide == 0)
        {

                //remove the four tiles above the leftBottomCorner
                foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x, (int)leftBottomCorner.y + 1, 0), null);
                foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x, (int)leftBottomCorner.y + 2, 0), null);
                foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x, (int)leftBottomCorner.y + 3, 0), null);
                foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x, (int)leftBottomCorner.y + 4, 0), null);
                foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x, (int)leftBottomCorner.y + 5, 0), leftDoorTile);
                foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x, (int)leftBottomCorner.y, 0), bottomWallTile);
        }
        else if (doorSide == 1)
        {
                //remove the four tiles above the leftBottomCorner
                foregroundTilemap.SetTile(new Vector3Int((int)rightBottomCorner.x, (int)leftBottomCorner.y + 1, 0), null);
                foregroundTilemap.SetTile(new Vector3Int((int)rightBottomCorner.x, (int)leftBottomCorner.y + 2, 0), null);
                foregroundTilemap.SetTile(new Vector3Int((int)rightBottomCorner.x, (int)leftBottomCorner.y + 3, 0), null);
                foregroundTilemap.SetTile(new Vector3Int((int)rightBottomCorner.x, (int)leftBottomCorner.y + 4, 0), null);
                foregroundTilemap.SetTile(new Vector3Int((int)rightBottomCorner.x, (int)leftBottomCorner.y + 5, 0), rightDoorTile);
                foregroundTilemap.SetTile(new Vector3Int((int)rightBottomCorner.x, (int)leftBottomCorner.y, 0), bottomWallTile);
                Debug.Log("Removed tile at: " + rightBottomCorner.x + ", " + (leftBottomCorner.y + 1));
                Debug.Log("Removed tile at: " + rightBottomCorner.x + ", " + (leftBottomCorner.y + 2));
                Debug.Log("Removed tile at: " + rightBottomCorner.x + ", " + (leftBottomCorner.y + 3));
                Debug.Log("Removed tile at: " + rightBottomCorner.x + ", " + (leftBottomCorner.y + 4));
        }
        else if (doorSide == 2)
        {
                //remove the four tiles above the leftBottomCorner
                foregroundTilemap.SetTile(new Vector3Int((int)leftTopCorner.x + 4, (int)leftTopCorner.y, 0), null);
                foregroundTilemap.SetTile(new Vector3Int((int)leftTopCorner.x + 5, (int)leftTopCorner.y, 0), null);
                foregroundTilemap.SetTile(new Vector3Int((int)leftTopCorner.x + 6, (int)leftTopCorner.y, 0), null);
                foregroundTilemap.SetTile(new Vector3Int((int)leftTopCorner.x + 7, (int)leftTopCorner.y, 0), null);
                foregroundTilemap.SetTile(new Vector3Int((int)leftTopCorner.x + 8, (int)leftTopCorner.y, 0), rightDoorTile);
                foregroundTilemap.SetTile(new Vector3Int((int)leftTopCorner.x + 3, (int)leftTopCorner.y, 0), leftDoorTile);
                Debug.Log("Removed tile at: " + rightBottomCorner.x + ", " + (leftBottomCorner.y + 1));
                Debug.Log("Removed tile at: " + rightBottomCorner.x + ", " + (leftBottomCorner.y + 2));
                Debug.Log("Removed tile at: " + rightBottomCorner.x + ", " + (leftBottomCorner.y + 3));
                Debug.Log("Removed tile at: " + rightBottomCorner.x + ", " + (leftBottomCorner.y + 4));
        }
        else if (doorSide == 3)
        {
                //remove the four tiles above the leftBottomCorner
                foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x + 4, (int)leftBottomCorner.y, 0), null);
                foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x + 5, (int)leftBottomCorner.y, 0), null);
                foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x + 6, (int)leftBottomCorner.y, 0), null);
                foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x + 7, (int)leftBottomCorner.y, 0), null);
                foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x + 8, (int)leftBottomCorner.y, 0), leftDoorTile);
                foregroundTilemap.SetTile(new Vector3Int((int)leftBottomCorner.x + 3, (int)leftBottomCorner.y, 0), rightDoorTile);
                Debug.Log("Removed tile at: " + rightBottomCorner.x + ", " + (leftBottomCorner.y + 1));
                Debug.Log("Removed tile at: " + rightBottomCorner.x + ", " + (leftBottomCorner.y + 2));
                Debug.Log("Removed tile at: " + rightBottomCorner.x + ", " + (leftBottomCorner.y + 3));
                Debug.Log("Removed tile at: " + rightBottomCorner.x + ", " + (leftBottomCorner.y + 4));
        }

    }
}