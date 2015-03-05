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
    public  const int WorldDiameter = 15*43;
	public  const int WorldRadius = WorldDiameter / 2;
    float Root3 = Mathf.Sqrt(3);
    public HexChunk[,] chunks;
    byte[,,,] data;
    public GameObject ChunkPrefab;
    public GameObject player;
    public const int distToUnload = 180;
    public const int distToLoad = 175;




    // Use this for initialization
	void Start () {

        data = new byte[WorldDiameter/chunkSize,WorldDiameter/chunkSize,chunkSize,chunkSize];
        chunks = new HexChunk[WorldDiameter / chunkSize, WorldDiameter / chunkSize];

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

        /*
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
        }*/

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

    float timer = 0f;


    void LateUpdate()
    {
        timer += Time.deltaTime;
        if (timer > 1)
        {
            timer = 0;
            for (int chunkQ = -WorldRadius / chunkSize; chunkQ <= WorldRadius / chunkSize; chunkQ++)
            {
                for (int chunkR = -WorldRadius / chunkSize; chunkR <= WorldRadius / chunkSize; chunkR++)
                {
                    Vector2 hexPosition = new Vector2((chunkQ + 0.5f) *chunkSize, (chunkR + 0.5f )* chunkSize);
                    Vector2 normalPosition = HexToNormal(hexPosition);
                    int chunkQIndex = chunkQ + WorldRadius / chunkSize;
                    int chunkRIndex = chunkR + WorldRadius  / chunkSize;
                    HexChunk chunk = chunks[chunkQIndex, chunkRIndex];
                    Vector2 playerPosition = new Vector2(player.transform.position.x, player.transform.position.z);
                    if (chunk != null && Vector2.Distance(normalPosition, playerPosition) > distToUnload) 
                    {
                        Destroy(chunk.gameObject);
                        chunks[chunkQIndex, chunkRIndex] = null;
                    }
                    else if (chunk == null && Vector2.Distance(normalPosition, playerPosition) < distToLoad)
                    {
                        GameObject newObject = Instantiate(ChunkPrefab) as GameObject;
                        chunk = newObject.GetComponent<HexChunk>();
                        chunk.ChunkQ = chunkQ;
                        chunk.ChunkR = chunkR;
                        chunk.hexWorld = this;
                        chunk.mapPosition = normalPosition;
                        chunks[chunkQIndex, chunkRIndex] = chunk;
                        chunk.StartBuilding();
                    }
                }
            }
        }
    }

    public Vector2 HexToNormal(Vector2 vector)
    {
        Vector2 newVector = Vector2.zero;
        newVector += vector.y * new Vector2(0, -Root3);
        newVector += vector.x * new Vector2(1.5f, -Root3 / 2);
        return newVector;
    }

   
}
