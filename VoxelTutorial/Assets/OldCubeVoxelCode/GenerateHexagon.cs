using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateHexagon : MonoBehaviour {

    public const float TILE_HEIGHT = 0.5f;

	public  const int chunkSize = 15;
    public  const int WorldDiameter = 15*11;
	public  const int WorldRadius = WorldDiameter / 2;
    float Root3 = Mathf.Sqrt(3);
    //List of utilized chunks in array position based off their actual chunkPosition
    public HexChunk[,] chunks;

    //The heightmap data position by position
    byte[,,,] data;
    public Dictionary<Vector2, HexPiece> pieces;
    public GameObject piecePrefab;
    //Free chunks that can be used to draw/update the map
    //The key point of this is to prevent unity for instantiating and destroying a lot
    //of objects and eating up memory. (Although it doesn't actually eat up the memory. The amount of allocated memory is fine
    //but unity is stupid and decides to keep increasing the size of the heap when it doesn't actually need to do that.
    //Thanks unity developers.
    public List<HexChunk> freeChunks; 

    //The chunk prefab we will be instantiating for drawing each chunk
    public GameObject ChunkPrefab;

    //The player/camera
    public GameObject player;

    //How far away when we start unloading chunks
    public const int distToUnload = 180;

    //How close when we start loading chunks
    public const int distToLoad = 175;

    //The take turn monobehaviour
    public HexTakeTurn hexTakeTurn;




    // Use this for initialization
	void Start () {
        pieces = new Dictionary<Vector2, HexPiece>();
        data = new byte[WorldDiameter/chunkSize,WorldDiameter/chunkSize,chunkSize,chunkSize];
        chunks = new HexChunk[WorldDiameter / chunkSize, WorldDiameter / chunkSize];
        freeChunks = new List<HexChunk>();
        for (int q = -WorldRadius; q <= WorldRadius; q++)
        {
            for (int r = -WorldRadius; r <= WorldRadius; r++)
            {
                if (inWorld(q, r))
                {
                    SetBlock(q, r, (byte)(World.PerlinNoise(q, r, 0, 20f, 10, 1.5f) + World.PerlinNoise(q, r, 0, 10f, 5f, 0.5f) + World.PerlinNoise(q, r, 0, 5f, 2.5f, 0.5f)));
                }
            }
        }
        hexTakeTurn = GetComponent<HexTakeTurn>();
        hexTakeTurn.SetUpTeams();
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


    List<Vector2> chunksToAdd = new List<Vector2>();

    void LateUpdate()
    {
        timer += Time.deltaTime;
        if (timer > 1)
        {
            timer = 0;
            chunksToAdd.Clear();
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
                        chunk.Deallocate();
                        freeChunks.Add(chunk);
                        chunks[chunkQIndex, chunkRIndex] = null;
                    }
                    else if (chunk == null && Vector2.Distance(normalPosition, playerPosition) < distToLoad)
                    {
                        chunksToAdd.Add(new Vector2(chunkQ, chunkR));
                    }
                }
            }
            foreach (Vector2 hexPosition in chunksToAdd)
            {
                //Take a free chunk if we have one availiable
                if (freeChunks.Count > 0)
                {
                    HexChunk chunk = freeChunks[0];
                    freeChunks.RemoveAt(0);
                    chunk.ChunkQ = (int) hexPosition.x;
                    chunk.ChunkR = (int) hexPosition.y;
                    chunks[chunk.ChunkQ + WorldRadius / chunkSize, chunk.ChunkR + WorldRadius / chunkSize] = chunk;
                    chunk.update = true;
                }
                //We need to instantiate a new object
                else
                {
                    GameObject chunkObject = Instantiate(ChunkPrefab) as GameObject;
                    HexChunk chunk = chunkObject.GetComponent<HexChunk>();
                    chunk.ChunkQ = (int) hexPosition.x;
                    chunk.ChunkR = (int) hexPosition.y;
                    chunk.hexWorld = this;
                    chunks[chunk.ChunkQ + WorldRadius / chunkSize, chunk.ChunkR + WorldRadius /chunkSize] = chunk;
                    chunk.update = true;
                }
            }
        }
    }

    //Convert Hexagon coordinates to Normal XZ coordinates
    public Vector2 HexToNormal(Vector2 vector)
    {
        Vector2 newVector = Vector2.zero;
        newVector += vector.y * new Vector2(0, -Root3);
        newVector += vector.x * new Vector2(1.5f, -Root3 / 2);
        return newVector;
    }


    public Vector2 NormalToHex(Vector2 vector)
    {
        float q = vector.x / 1.5f;
        return new Vector2(q, (vector.y + (Root3/2) * q) / (-Root3));
    }
   
}
