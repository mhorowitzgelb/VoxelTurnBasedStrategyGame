using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateHexagon : MonoBehaviour {

    public const float TILE_HEIGHT = 0.5f;

    List<Vector3> vertices;
    List<int> triangles;
    List<Vector2> uvs;
    int vertexOffset = 0;
	public  const int chunkSize = 16;
    public  const int WorldDiameter = 27;
	public  const int WorldRadius = WorldDiameter / 2;
    float Root3 = Mathf.Sqrt(3);
    byte[][][][] data;
    // Use this for initialization
	void Start () {
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
			return data[q/chunkSize][r/chunkSize][q%chunkSize][r%chunkSize];
		} else 
			return -1;
    }

    public void SetBlock(int q, int r, byte b)
    {
		data [q / chunkSize] [r / chunkSize] [q % chunkSize] [r % chunkSize] = b;    
    }

	public bool inWorld(int q, int r){
		if (Mathf.Abs (q) > WorldRadius || Mathf.Abs (r) > WorldRadius)
						return false;
		if (q * r >= 0 && Mathf.Abs (q + r) > WorldRadius)
						return false;
		return true;
	}

    void HexTop(Vector3 topLeft, byte block){
       
        vertices.Add(topLeft+ new Vector3(0, 0, 0));
        vertices.Add(topLeft + new Vector3(1, 0, 0));
        vertices.Add(topLeft + new Vector3(1, 0, -Root3));
        vertices.Add(topLeft + new Vector3(0, 0, -Root3));
        vertices.Add(topLeft + new Vector3(-0.5f, 0, -0.5f * Root3));
        vertices.Add(topLeft + new Vector3(1.5f, 0, -0.5f * Root3));


        triangles.Add(vertexOffset + 0);
        triangles.Add(vertexOffset + 1);
        triangles.Add(vertexOffset + 3);
        triangles.Add(vertexOffset + 1);
        triangles.Add(vertexOffset + 2);
        triangles.Add(vertexOffset + 3);
        triangles.Add(vertexOffset + 0);
        triangles.Add(vertexOffset + 3);
        triangles.Add(vertexOffset + 4);
        triangles.Add(vertexOffset + 1);
        triangles.Add(vertexOffset + 5);
        triangles.Add(vertexOffset + 2);

        uvs.Add(new Vector2(0.25f, 1));
        uvs.Add(new Vector2(0.75f, 1));
        uvs.Add(new Vector2(0.75f , 0));
        uvs.Add(new Vector2(0.25f, 0));
        uvs.Add(new Vector2(0, 0.5f));
        uvs.Add(new Vector2(1, 0.5f));


        vertexOffset += 6;
    }

    void HexSideTopLeft(Vector3 topLeft, byte block)
    {
        vertices.Add(topLeft + new Vector3(0, 0, 0));
        vertices.Add(topLeft + new Vector3(0, -TILE_HEIGHT, 0));
        vertices.Add(topLeft + new Vector3(-0.5f, 0, -0.5f * Root3));
        vertices.Add(topLeft + new Vector3(-0.5f, -TILE_HEIGHT, -0.5f * Root3));

        triangles.Add(vertexOffset + 0);
        triangles.Add(vertexOffset + 3);
        triangles.Add(vertexOffset + 1);
        triangles.Add(vertexOffset + 0);
        triangles.Add(vertexOffset + 2);
        triangles.Add(vertexOffset + 3);
        vertexOffset += 4;

        uvs.Add(new Vector2(0, 0.5f));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0.5f, 0.5f));
        uvs.Add(new Vector2(0.5f, 0));
    }

    void HexSideTop(Vector3 topLeft, byte block)
    {
        vertices.Add(topLeft + new Vector3(0, 0, 0));
        vertices.Add(topLeft + new Vector3(0, -TILE_HEIGHT, 0));
        vertices.Add(topLeft + new Vector3(1, 0, 0));
        vertices.Add(topLeft + new Vector3(1, -TILE_HEIGHT, 0));

        
        triangles.Add(vertexOffset + 0);
        triangles.Add(vertexOffset + 1);
        triangles.Add(vertexOffset + 3);
        triangles.Add(vertexOffset + 0);
        triangles.Add(vertexOffset + 3);
        triangles.Add(vertexOffset + 2);

        uvs.Add(new Vector2(0, 0.5f));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0.5f, 0.5f));
        uvs.Add(new Vector2(0.5f, 0));

        vertexOffset += 4;

    }

    void HexSideTopRight(Vector3 topLeft, byte block)
    {
        vertices.Add(topLeft + new Vector3(1, 0, 0));
        vertices.Add(topLeft + new Vector3(1, -TILE_HEIGHT, 0));
        vertices.Add(topLeft + new Vector3(1.5f, 0, -0.5f * Root3));
        vertices.Add(topLeft + new Vector3(1.5f, -TILE_HEIGHT, -0.5f * Root3));

        triangles.Add(vertexOffset + 1);
        triangles.Add(vertexOffset + 3);
        triangles.Add(vertexOffset + 0);
        triangles.Add(vertexOffset + 3);
        triangles.Add(vertexOffset + 2);
        triangles.Add(vertexOffset + 0);

        uvs.Add(new Vector2(0, 0.5f));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0.5f, 0.5f));
        uvs.Add(new Vector2(0.5f, 0));

        vertexOffset += 4;
    }

    void HexSideBottom(Vector3 topLeft, byte block)
    {
        vertices.Add(topLeft + new Vector3(0, 0, -Root3));
        vertices.Add(topLeft + new Vector3(0, -TILE_HEIGHT, -Root3));
        vertices.Add(topLeft + new Vector3(1, 0, -Root3));
        vertices.Add(topLeft + new Vector3(1, -TILE_HEIGHT, -Root3));


        triangles.Add(vertexOffset + 3);
        triangles.Add(vertexOffset + 1);
        triangles.Add(vertexOffset + 0);
        triangles.Add(vertexOffset + 2);
        triangles.Add(vertexOffset + 3);
        triangles.Add(vertexOffset + 0);

        uvs.Add(new Vector2(0, 0.5f));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0.5f, 0.5f));
        uvs.Add(new Vector2(0.5f, 0));


        vertexOffset += 4;
    }


    void HexSideBottomLeft(Vector3 topLeft, byte block)
    {
        vertices.Add(topLeft + new Vector3(0, 0, -Root3));
        vertices.Add(topLeft + new Vector3(0, -TILE_HEIGHT, -Root3));
        vertices.Add(topLeft + new Vector3(-0.5f, 0, -0.5f * Root3));
        vertices.Add(topLeft + new Vector3(-0.5f, -TILE_HEIGHT, -0.5f * Root3));

        triangles.Add(vertexOffset + 1);
        triangles.Add(vertexOffset + 3);
        triangles.Add(vertexOffset + 0);
        triangles.Add(vertexOffset + 3);
        triangles.Add(vertexOffset + 2);
        triangles.Add(vertexOffset + 0);
        vertexOffset += 4;

        uvs.Add(new Vector2(0, 0.5f));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0.5f, 0.5f));
        uvs.Add(new Vector2(0.5f, 0));
    }

    void HexSideBottomRIght(Vector3 topLeft, byte block)
    {
        vertices.Add(topLeft + new Vector3(1, 0, -Root3));
        vertices.Add(topLeft + new Vector3(1, -TILE_HEIGHT, -Root3));
        vertices.Add(topLeft + new Vector3(1.5f, 0, -0.5f * Root3));
        vertices.Add(topLeft + new Vector3(1.5f, -TILE_HEIGHT, -0.5f * Root3));

        triangles.Add(vertexOffset + 0);
        triangles.Add(vertexOffset + 3);
        triangles.Add(vertexOffset + 1);
        triangles.Add(vertexOffset + 0);
        triangles.Add(vertexOffset + 2);
        triangles.Add(vertexOffset + 3);

        uvs.Add(new Vector2(0, 0.5f));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0.5f, 0.5f));
        uvs.Add(new Vector2(0.5f, 0));

        vertexOffset += 4;

    }
	// Update is called once per frame
	void Update () {
	
	}
}
