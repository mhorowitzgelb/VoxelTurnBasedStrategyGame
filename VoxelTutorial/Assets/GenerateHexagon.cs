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
    byte[,,,] data;
    public GameObject ChunkPrefab;
    public HexChunk[,] chunks;
    public GameObject player;

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
                   SetBlock(q,r,(byte) Mathf.Clamp(World.PerlinNoise(q, r, 0,20f, 10f, 2f), 0, 50));
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

        /*
		data = new byte[WorldDiameter][];
        for (int i = 0; i < WorldDiameter / 2; i++)
        {
            data[i] = new byte[WorldDiameter / 2 + i+1];    
        }
        for (int i = WorldDiameter / 2; i < WorldDiameter; i ++ )
        {
            data[i] = new byte[WorldDiameter - i + WorldDiameter / 2];
        }
        vertices = new List<Vector3>();


        triangles = new List<int>();
        uvs = new List<Vector2>();

		for (int q = -WorldDiameter / 2; q <= 0 ; q++)
        {
            for (int r = -(WorldDiameter/2 - Mathf.Abs(q)); r <= (WorldDiameter/2); r++)
            {
                Vector3 position = Vector3.zero;
                position += r * new Vector3(0, 0, -Root3);
                position += q * new Vector3(1.5f, 0, -Root3 / 2);
				int height = World.PerlinNoise(q,300,r,20,4,0);
				position += new Vector3(0,height,0);
				for(float i = 0; i <= height; i +=0.5f){
					Vector3 sidePos = position - new Vector3(0,i,0);
					HexSideBottomLeft(sidePos,0);
					HexSideBottomRIght(sidePos,0);
					HexSideBottom(sidePos,0);
					HexSideTop(sidePos,0);
					HexSideTopLeft(sidePos,0);
					HexSideTopRight(sidePos,0);
				}
                HexTop(position, 1);
                SetBlock(q, r, 1);
            }
        }

		for (int q = 1; q <= WorldDiameter/2; q ++) {
			for(int r = -WorldDiameter/ 2; r <= WorldDiameter / 2 - q; r++){
				Vector3 position = Vector3.zero;
				position += r * new Vector3(0, 0, -Root3);
				position += q * new Vector3(1.5f, 0, -Root3 / 2);
				int height = World.PerlinNoise(q,300,r,20,4,0);
				position += new Vector3(0,height,0);
				for(float i = 0; i <= height; i +=0.5f){
					Vector3 sidePos = position - new Vector3(0,i,0);
					HexSideBottomLeft(sidePos,0);
					HexSideBottomRIght(sidePos,0);
					HexSideBottom(sidePos,0);
					HexSideTop(sidePos,0);
					HexSideTopLeft(sidePos,0);
					HexSideTopRight(sidePos,0);
				}
				HexTop(position, 1);
				SetBlock(q, r, 1);
			}
		}

        Vector3 position = new Vector3(1, 1, 1);
        HexTop(position, 0);
        HexSideTopLeft(position, 0);
        HexSideTop(position, 0);
        HexSideTopRight(position, 0);
        HexSideBottom(position, 0);
        HexSideBottomLeft(position, 0);
        HexSideBottomRIght(position, 0);

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
		mesh.Optimize ();
		mesh.RecalculateNormals ();
		*/
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
