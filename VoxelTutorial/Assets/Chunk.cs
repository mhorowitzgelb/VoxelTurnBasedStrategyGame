﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

public class Chunk : MonoBehaviour
{
    public bool update;
    public int chunkX;
    public int chunkY;
    public int chunkZ;
    public int chunkSize = 16;
    public GameObject worldGO;
    public World world;
    private List<Vector3> newVertices = new List<Vector3>();
    private List<int> newTriangles = new List<int>();
    private List<Vector2> newUV = new List<Vector2>();

    private float tUnit = 0.0625f;
    private Vector2 tStone = new Vector2(0,14);
    private Vector2 tGrass =  new Vector2(3,15);
    private Vector2 tGrasstop = new Vector2(0,15);
	private Vector2 tSelect = new Vector2 (7, 14);
    private Mesh mesh;
    private MeshCollider col;
    private int faceCount;

	public List<GameObject> characters = new List<GameObject>();

    void LateUpdate()
    {
        if (update)
        {
			foreach(GameObject obj in characters){
				Destroy(obj);	
			}
			characters.Clear();

			for(int x = chunkX; x < chunkX + chunkSize; x ++){
				for(int z = chunkZ; z < chunkZ + chunkSize; z ++){
					for(int y = chunkY; y < chunkY + chunkSize; y ++){	
						if(world.data[x,y,z] != 0 && world.data[x,y+1,z] == 0){
							world.heightMap[x,z] = y;
							break;
						}
					}

				}
			}



            GenerateMesh();
            update = false;
        }
    }

	// Use this for initialization
	void Start ()
	{
	    world = worldGO.GetComponent<World>();
	    mesh = GetComponent<MeshFilter>().mesh;
	    col = GetComponent<MeshCollider>();
	   GenerateMesh();
	}

    void CubeTop(int x, int y, int z, byte block)
    {

        newVertices.Add(new Vector3(x, y, z + 1));
        newVertices.Add(new Vector3(x + 1, y, z + 1));
        newVertices.Add(new Vector3(x + 1, y, z));
        newVertices.Add(new Vector3(x, y, z));

        Vector2 texturePos = new Vector2(0,0);

		if (world.selectedTiles.ContainsKey(new Vector2(chunkX + x, chunkZ + z)
		         )) {
			texturePos = tSelect;		
		}
		else if (Block(x, y, z) == 1)
        {
            texturePos = tStone;
        }
        else if (Block(x, y, z) == 2)
        {
            texturePos = tGrasstop;
        }

        Cube(texturePos);
		if (world.characters [chunkX +x, chunkZ + z] != 0) {
			characters.Add(Instantiate (world.grunt, new Vector3 (chunkX +x, world.heightMap[chunkX + x, chunkZ + z] + 1, chunkZ + z), new Quaternion (0, 0, 0, 0)) as GameObject);
		}
    }

    void CubeNorth(int x, int y, int z, byte block)
    {
        newVertices.Add(new Vector3(x + 1, y - 1, z + 1));
        newVertices.Add(new Vector3(x + 1, y, z + 1));
        newVertices.Add(new Vector3(x, y, z + 1));
        newVertices.Add(new Vector3(x, y - 1, z + 1));
        Vector2 texturePos = new Vector2(0, 0);
		if (Block(x, y, z) == 1)
        {
            texturePos = tStone;
        }
        else if (Block(x, y, z) == 2)
        {
            texturePos = tGrass;
        }

        Cube(texturePos);
    
    }

    void CubeEast(int x, int y, int z, byte block)
    {
        newVertices.Add(new Vector3(x + 1, y - 1, z));
        newVertices.Add(new Vector3(x + 1, y, z));
        newVertices.Add(new Vector3(x + 1, y, z + 1));
        newVertices.Add(new Vector3(x + 1, y - 1, z + 1));
        Vector2 texturePos = new Vector2(0, 0);
        if (Block(x, y, z) == 1)
        {
            texturePos = tStone;
        }
        else if (Block(x, y, z) == 2)
        {
            texturePos = tGrass;
        }

        Cube(texturePos);
    }

    void CubeSouth(int x, int y, int z, byte block)
    {
        newVertices.Add(new Vector3(x, y - 1, z));
        newVertices.Add(new Vector3(x, y, z));
        newVertices.Add(new Vector3(x + 1, y, z));
        newVertices.Add(new Vector3(x + 1, y - 1, z));
        Vector2 texturePos = new Vector2(0, 0);
        if (Block(x, y, z) == 1)
        {
            texturePos = tStone;
        }
        else if (Block(x, y, z) == 2)
        {
            texturePos = tGrass;
        }

        Cube(texturePos);
    }

    void CubeWest(int x, int y, int z, byte block)
    {
        newVertices.Add(new Vector3(x, y - 1, z + 1));
        newVertices.Add(new Vector3(x, y, z + 1));
        newVertices.Add(new Vector3(x, y, z));
        newVertices.Add(new Vector3(x, y - 1, z));
        Vector2 texturePos = new Vector2(0, 0);
        if (Block(x, y, z) == 1)
        {
            texturePos = tStone;
        }
        else if (Block(x, y, z) == 2)
        {
            texturePos = tGrass;
        }

        Cube(texturePos);
    }

    void CubeBot(int x, int y, int z, int block)
    {
        newVertices.Add(new Vector3(x, y - 1, z));
        newVertices.Add(new Vector3(x + 1, y - 1, z));
        newVertices.Add(new Vector3(x + 1, y - 1, z + 1));
        newVertices.Add(new Vector3(x, y - 1, z + 1));
        Vector2 texturePos = new Vector2(0, 0);
        if (Block(x, y, z) == 1)
        {
            texturePos = tStone;
        }
        else if (Block(x, y, z) == 2)
        {
            texturePos = tGrass;
        }

        Cube(texturePos);
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = newVertices.ToArray();
        mesh.uv = newUV.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();

        col.sharedMesh = null;
        col.sharedMesh = mesh;
        
        newVertices.Clear();
        newUV.Clear();
        newTriangles.Clear();
        faceCount = 0;
    }

    void Cube(Vector2 texturePos)
    {
        newTriangles.Add(faceCount * 4); //1
        newTriangles.Add(faceCount * 4 + 1); //2
        newTriangles.Add(faceCount * 4 + 2); //3
        newTriangles.Add(faceCount * 4); //1
        newTriangles.Add(faceCount * 4 + 2); //3
        newTriangles.Add(faceCount * 4 + 3); //4

        newUV.Add(new Vector2(tUnit * texturePos.x + tUnit, tUnit * texturePos.y));
        newUV.Add(new Vector2(tUnit * texturePos.x + tUnit, tUnit * texturePos.y + tUnit));
        newUV.Add(new Vector2(tUnit * texturePos.x, tUnit * texturePos.y + tUnit));
        newUV.Add(new Vector2(tUnit * texturePos.x, tUnit * texturePos.y));

        faceCount ++;
    }

    public void GenerateMesh()
    {
        for (int x = 0; x < chunkSize; x++)
        {
			for (int y = 0; y < chunkSize && chunkY + y < world.viewingHeight + 1; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {

                    if (Block(x, y, z) != 0)
                    {
                        //If the block is solid

                        if (Block(x, y + 1, z) == 0 || chunkY + y == world.viewingHeight)
                        {
                            //Block above is air
                            CubeTop(x, y, z, Block(x, y, z));
                        }

                        if (Block(x, y - 1, z) == 0)
                        {
                            //Block below is air
                            CubeBot(x, y, z, Block(x, y, z));

                        }

                        if (Block(x + 1, y, z) == 0)
                        {
                            //Block east is air
                            CubeEast(x, y, z, Block(x, y, z));

                        }

                        if (Block(x - 1, y, z) == 0)
                        {
                            //Block west is air
                            CubeWest(x, y, z, Block(x, y, z));

                        }

                        if (Block(x, y, z + 1) == 0)
                        {
                            //Block north is air
                            CubeNorth(x, y, z, Block(x, y, z));

                        }

                        if (Block(x, y, z - 1) == 0)
                        {
                            //Block south is air
                            CubeSouth(x, y, z, Block(x, y, z));

                        }

                    }
                }
            }
        }
        UpdateMesh();
    }

    byte Block(int x, int y, int z)
    {
        return world.Block(x + chunkX, y + chunkY, z + chunkZ);
    }
	// Update is called once per frame
	void Update () {
	
	}
}
