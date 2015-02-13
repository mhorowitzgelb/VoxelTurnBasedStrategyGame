using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour
{
	public GameObject grunt;
    public byte[,,] data;
	public Dictionary<Vector3, GameObject> pieces;
	public Dictionary<Vector3,int> selectedTiles;
    public int worldX = 16;
    public int worldY = 16;
    public int worldZ = 16;
    public GameObject chunk;
    public Chunk[,,] chunks;
    public int chunkSize = 16;
	public int viewingHeight;

    // Use this for initialization
    private void Start()
    {
		viewingHeight = worldY -1;
        data = new byte[worldX, worldY, worldZ];
		pieces = new Dictionary<Vector3,GameObject >();
		selectedTiles = new Dictionary<Vector3,int>();
        for (int x = 0; x < worldX; x++)
        {
            for (int z = 0; z < worldZ; z++)
            {
                int stone = PerlinNoise(x, 0, z, 10, 3, 1.2f);
                stone += PerlinNoise(x, 300, z, 20, 4, 0) + 10;
                int dirt = PerlinNoise(x, 100, z, 50, 2, 0) + 1; //Added +1 to make sure minimum grass height is 1

                for (int y = 0; y < worldY; y++)
                {
                    if (y <= stone)
                    {
                        data[x, y, z] = 1;
                    }
                    else if (y <= dirt + stone)
                    {
                        //Changed this line thanks to a comment
                        data[x, y, z] = 2;
                    }

                }
            }
        }

        chunks = new Chunk[worldX/chunkSize,
            worldY/chunkSize,
            worldZ/chunkSize];

        for (int x = 0; x < chunks.GetLength(0); x++)
        {
            for (int y = 0; y < chunks.GetLength(1); y++)
            {
                for (int z = 0; z < chunks.GetLength(2); z++)
                {
                    GameObject newChunk =
                        Instantiate(chunk, new Vector3(x*chunkSize - 0.5f, y*chunkSize + 0.5f, z*chunkSize - 0.5f),
                            new Quaternion(0, 0, 0, 0)) as GameObject;
                    chunks[x, y, z] = newChunk.GetComponent<Chunk>();
                    chunks[x, y, z].worldGO = gameObject;
                    chunks[x, y, z].chunkSize = chunkSize;
                    chunks[x, y, z].chunkX = x*chunkSize;
                    chunks[x, y, z].chunkY = y*chunkSize;
                    chunks[x, y, z].chunkZ = z*chunkSize;

                }
            }
        }
    }

    public static int PerlinNoise(int x, int y, int z, float scale, float height, float power)
    {
        float rValue;
        rValue = Noise.Noise.GetNoise(((double) x)/scale, ((double) y)/scale, ((double) z)/scale);
        rValue *= height;

        if (power != 0)
        {
            rValue = Mathf.Pow(rValue, power);
        }

        return (int) rValue;
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public byte Block(int x, int y, int z)
    {
        if (x >= worldX || x < 0 || y >= worldY || y < 0 || z >= worldZ || z < 0)
        {
            return 1;
        }
        return data[x, y, z];
    }

    public void GenColumn(int x, int z)
    {

        for (int y = 0; y < chunks.GetLength(1); y++)
        {
            //Create a temporary Gameobject for the new chunk instead of using chunks[x,y,z]
            GameObject newChunk = Instantiate(chunk, new Vector3(x*chunkSize - 0.5f,
                y*chunkSize + 0.5f, z*chunkSize - 0.5f), new Quaternion(0, 0, 0, 0)) as GameObject;

            chunks[x, y, z] = newChunk.GetComponent<Chunk>();
            chunks[x, y, z].worldGO = gameObject;
            chunks[x, y, z].chunkSize = chunkSize;
            chunks[x, y, z].chunkX = x*chunkSize;
            chunks[x, y, z].chunkY = y*chunkSize;
            chunks[x, y, z].chunkZ = z*chunkSize;
        }
    }

    public void UnloadColumn(int x, int z)
    {
        for (int y = 0; y < chunks.GetLength(1); y++)
        {
           Object.Destroy(chunks[x,y,z].gameObject);

			foreach(GameObject obj in chunks[x,y,z].characters){
				Destroy(obj);	
			}
			chunks[x,y,z].characters.Clear();
        }
        
    }

}
