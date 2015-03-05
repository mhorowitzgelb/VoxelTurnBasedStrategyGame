using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateHexagon : MonoBehaviour {

    public const float TILE_HEIGHT = 0.5f;

    List<Vector3> vertices;
    List<int> triangles;
    List<Vector2> uvs;
    int vertexOffset = 0;
	public  const int chunkSize = 15;
    public  const int WorldDiameter = 15*11;
	public  const int WorldRadius = WorldDiameter / 2;
    float Root3 = Mathf.Sqrt(3);
    byte[,,,] data;
    public GameObject ChunkPrefab;
    public Dictionary<Vector2, HexChunk> chunkMap;
    public GameObject player;




    // Use this for initialization
	void Start () {

        data = new byte[WorldDiameter/chunkSize,WorldDiameter/chunkSize,chunkSize,chunkSize];
        chunkMap = new Dictionary<Vector2, HexChunk>();

        for (int q = -WorldRadius; q <= WorldRadius; q++)
        {
            for (int r = -WorldRadius; r <= WorldRadius; r++)
            {
                if (inWorld(q, r))
                {
                   SetBlock(q,r,(byte) World.PerlinNoise(q, r, 0,20f, 10f, 2f));
                }
            }
        }


        for (int chunkQ = -WorldRadius / chunkSize; chunkQ < WorldRadius / chunkSize; chunkQ++)
        {
            for (int chunkR = -WorldRadius / chunkSize ; chunkR < WorldRadius / chunkSize; chunkR++)
            {
                GameObject chunk = Instantiate(ChunkPrefab, Vector3.zero, new Quaternion(0, 0, 0, 0)) as GameObject;
                HexChunk hexChunk = chunk.GetComponent<HexChunk>();
                hexChunk.ChunkQ = chunkQ;
                hexChunk.ChunkR = chunkR;
                hexChunk.hexWorld = this;
                Vector2 position = Vector2.zero;
                position += (chunkR * chunkSize + (chunkSize *0.5f)) * new Vector2(0, -Root3);
                position += (chunkQ * chunkSize + (chunkSize * 0.5f)) * new Vector2(1.5f, -Root3 / 2);
                hexChunk.mapPosition = position;
            }
        }

	}

    public byte GetBlockHeightTop(int q, int r)
    {
        
		if (inWorld(q,r)) {
			return data[(q+ WorldRadius)/chunkSize,(r+ WorldRadius)/chunkSize,(q+ WorldRadius)%chunkSize,(r+WorldRadius)%chunkSize];
		} else 
			return 0;
    }

    public void SetBlock(int q, int r, byte b)
    {
        data[(q + WorldRadius) / chunkSize,(r + WorldRadius) / chunkSize,(q + WorldRadius) % chunkSize,(r + WorldRadius) % chunkSize] = b;    
    }
    public bool inWorld(int q, int r){
		
        if (Mathf.Abs (q) > WorldRadius || Mathf.Abs (r) > WorldRadius)
						return false;
		if (q * r >= 0 && Mathf.Abs (q + r) > WorldRadius)
						return false;
         
		return true;
	}

    
	// Update is called once per frame
	void Update () {
	
	}
}
