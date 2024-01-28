using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
//using System;

public class MeshMapa2d : MonoBehaviour
{ 
    [Range(0, 100)]
    public int iniChance;        // Porcentagem de divisão entre os tiles

    [Range(1, 8)]
    public int birthLimit;       

    [Range(1, 8)]
    public int deathLimit;

    [Range(1, 10)]
    public int numR;

    private int[,] terrainMap;     // BitMapa do Chão
    private int[,] terrainMap2;    // BitMapa dos muros 

    public Tilemap GroundMap;     // Grid 1
    public Tile GroundTile1;      // Tiles do chão
    public Tile GroundTile2;
    public Tile WallGroundTile1;  // Tiles em baixo da parede 
    public Tile WallGroundTile2;


    [Range(0, 100)]
    public int PlantChance;
    public Tilemap Plantmap;         //Grid 2
    public Tile PlantTile;          

    [Range(0, 100)]
    public int randomFillPercent;
    public Tilemap Wallmap;          //Grid 3 com collisão
    public Tile WallTile;            

    public string seed;
    public bool useRandomSeed;

    public Vector3Int tmapSize;

    int width;
    int height;



    void Start()
    {
        GenerateMap();              //Gera as Paredes do mapa 
        GenerateGround(numR);
        GeneratePlant(numR);
    }

        void Update()
    {

        if (Input.GetKey(KeyCode.G)) 
        {
            if (terrainMap == null)
            {
                GenerateMap();
                GenerateGround(numR);
                GeneratePlant(numR);
            }
        }
        if (Input.GetKey(KeyCode.L)) 
        {
            clearMap(true);
        }


    }


    public void GenerateMap()  //GenerateMap
    {
        clearMap(false);
        width = tmapSize.x;
        height = tmapSize.y;

        terrainMap = new int[width, height];    //cria o bitmapa
        terrainMap2 = new int[width, height];

        RandomFillMap();

        for (int i = 0; i < 5; i++)
        {
            SmoothMap();
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                    if (terrainMap2[x, y] == 1)
                    Wallmap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), WallTile);
            }
        }

        Drops();
    }

    public void GenerateGround(int numR)
    {

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                terrainMap[x, y] = Random.Range(1, 101) < iniChance ? 1 : 0;
            }

        }

        for (int i = 0; i < height; i++)
        {
            terrainMap = genTilePos(terrainMap);
        }
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (terrainMap[x, y] == 1)
                {
                    if (terrainMap2[x, y] == 1)
                        GroundMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), WallGroundTile1);
                    else
                        GroundMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), GroundTile1);
                }
                else
                {
                    if (terrainMap2[x, y] == 1)
                        GroundMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), WallGroundTile2);
                    else
                        GroundMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), GroundTile2);
                }

            }

        }

    }

    public void GeneratePlant(int numR)
    {

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                terrainMap[x, y] = Random.Range(1, 101) < PlantChance ? 1 : 0;
            }

        }

        for (int i = 0; i < height; i++)
        {
            terrainMap = genTilePos(terrainMap);
        }
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (terrainMap[x, y] == 0)
                {
                    if (terrainMap2[x, y] == 0)
                        Plantmap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), PlantTile);
                }


            }

        }
    }

    public GameObject _object;

    public int numb;

    public GameObject _playerspawn;

    public GameObject _portal;

    bool collision = true;
    bool collision2 = true;

    int Count = 0;
    int xPos;
    int yPos;

    void Drops()
    {
        while (Count < numb)   
        {
            xPos = Random.Range(0, tmapSize.x);                                                                                       //Gera uma coordenada aleatoria 
            yPos = Random.Range(0, tmapSize.y);

            if (terrainMap2[xPos, yPos] == 0 && terrainMap2[xPos-1, yPos] == 0 && terrainMap2[xPos+1, yPos] == 0 &&                   //verifica os pontos em volta para não ter collisão
                terrainMap2[xPos-1, yPos-1] == 0 && terrainMap2[xPos, yPos-1] == 0 && terrainMap2[xPos+1, yPos-1] == 0
                && terrainMap2[xPos -1, yPos + 1] == 0 && terrainMap2[xPos, yPos + 1] == 0 && terrainMap2[xPos + 1, yPos + 1] == 0)
            {
                Instantiate(_object, new Vector3(-xPos + width / 2, -yPos + height / 2, 0), Quaternion.identity);
                Count += 1;
            }
        }

        while (collision)
        {
            xPos = Random.Range(0, tmapSize.x);
            yPos = Random.Range(0, tmapSize.y);

            if (terrainMap2[xPos, yPos] == 0 && terrainMap2[xPos - 1, yPos] == 0 && terrainMap2[xPos + 1, yPos] == 0 &&
                terrainMap2[xPos - 1, yPos - 1] == 0 && terrainMap2[xPos, yPos - 1] == 0 && terrainMap2[xPos + 1, yPos - 1] == 0
                && terrainMap2[xPos - 1, yPos + 1] == 0 && terrainMap2[xPos, yPos + 1] == 0 && terrainMap2[xPos + 1, yPos + 1] == 0)
            {
                collision = false;
                _playerspawn.transform.position = new Vector2(-xPos + width / 2, -yPos + height / 2);
            }
        }

        while (collision2)
        {
            xPos = Random.Range(0, tmapSize.x);
            yPos = Random.Range(0, tmapSize.y);

            if (terrainMap2[xPos, yPos] == 0 && terrainMap2[xPos - 1, yPos] == 0 && terrainMap2[xPos + 1, yPos] == 0 &&
                terrainMap2[xPos - 1, yPos - 1] == 0 && terrainMap2[xPos, yPos - 1] == 0 && terrainMap2[xPos + 1, yPos - 1] == 0
                && terrainMap2[xPos - 1, yPos + 1] == 0 && terrainMap2[xPos, yPos + 1] == 0 && terrainMap2[xPos + 1, yPos + 1] == 0)
            {
                collision2 = false;
                _portal.transform.position = new Vector3(-xPos + width / 2, -yPos + height / 2, 0);
            }
        }


    }


    public void RandomFillMap()
    {
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    terrainMap2[x, y] = 1;
                }
                else
                {
                    terrainMap2[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
                }
            }
        }
    }

    public void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);

                if (neighbourWallTiles > 4)
                    terrainMap2[x, y] = 1;
                else if (neighbourWallTiles < 4)
                    terrainMap2[x, y] = 0;

            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += terrainMap2[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }   

    public int[,] genTilePos(int[,] oldMap)
    {
    int[,] newMap = new int[width, height];
    int neighb;
    BoundsInt myB = new BoundsInt(-1, -1, 0, 3, 3, 1);

    for (int x = 0; x < width; x++)
    {
        for (int y = 0; y < height; y++)
        {
            neighb = 0;
            foreach (var b in myB.allPositionsWithin)
            {
                if (b.x == 0 && b.y == 0) continue;
                if (x + b.x >= 0 && x + b.x < width && y + b.y >= 0 && y + b.y < height)
                {
                    neighb += oldMap[x + b.x, y + b.y];
                }
                else
                {
                    neighb++;
                }
            }

            if (oldMap[x, y] == 1)
            {
                if (neighb < deathLimit) newMap[x, y] = 1;
                else
                {
                    newMap[x, y] = 1;
                }
            }
            if (oldMap[x, y] == 0)
            {
                if (neighb < birthLimit) newMap[x, y] = 1;
                else
                {
                    newMap[x, y] = 0;
                }
            }

        }

    }
    return newMap;
    }

    public void clearMap(bool complete)
    {
        GroundMap.ClearAllTiles();
        Wallmap.ClearAllTiles();
        Plantmap.ClearAllTiles();

        if (complete)
        {
            terrainMap = null;
        }
    }

    }
