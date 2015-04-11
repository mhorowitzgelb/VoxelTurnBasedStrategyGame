using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexChunk : MonoBehaviour {

	public GenerateHexagon hexWorld;
	public int ChunkQ;
	public int ChunkR;
	private List<Vector3> vertices = new List<Vector3>();
	private List<int> triangles = new List<int>();
	private List<Vector2> uvs = new List<Vector2>();
	private int vertexOffset = 0;
	private float TILE_HEIGHT = 0.5f;
	private float Root3 = Mathf.Sqrt(3);
    public Vector2 mapPosition;
    public bool update;
    private List<GameObject> pieces = new List<GameObject>();
    //Has the information been reset
    private bool free = true;


    void Update()
    {
        if (update)
        {
            update = false;
            if (!free)
            {
                Deallocate();
            }
            StartBuilding();
        }
    }

    public void Deallocate()
    {
        free = true;
        vertexOffset = 0;
        vertices.Clear();
        triangles.Clear();
        uvs.Clear();
        GetComponent<MeshFilter>().mesh.Clear();
        foreach (GameObject obj in pieces)
        {
            Destroy(obj);
        }
        pieces.Clear();
    }







	// Use this for initialization
    private void StartBuilding()
    {
        free = false;
        for (int q = ChunkQ * GenerateHexagon.chunkSize; q < (ChunkQ + 1) * GenerateHexagon.chunkSize; q++)
        {
            for (int r = ChunkR * GenerateHexagon.chunkSize; r < (ChunkR + 1) * GenerateHexagon.chunkSize; r++)
            {
                if (hexWorld.inWorld(q, r))
                {
                    Vector3 position = Vector3.zero;
                    position += r * new Vector3(0, 0, -Root3);
                    position += q * new Vector3(1.5f, 0, -Root3 / 2);
                    int height = hexWorld.GetBlockHeightTop(q, r);
                    position += new Vector3(0, height / 2.0f, 0);
                    byte bottomLeft = hexWorld.GetBlockHeightTop(-1 + q, 1 + r);
                    byte bottom = hexWorld.GetBlockHeightTop(q, 1 + r);
                    byte bottomRight = hexWorld.GetBlockHeightTop(1 + q, r);
                    byte topLeft = hexWorld.GetBlockHeightTop(q - 1, r);
                    byte top = hexWorld.GetBlockHeightTop(q, -1 + r);
                    byte topRight = hexWorld.GetBlockHeightTop(q + 1, r - 1);


                    HexSideBottomLeft(position, height - bottomLeft, 0);
                    HexSideBottomRIght(position, height - bottomRight, 0);
                    HexSideBottom(position, height - bottom, 0);
                    HexSideTop(position, height - top, 0);
                    HexSideTopLeft(position, height - topLeft, 0);
                    HexSideTopRight(position, height - topRight, 0);
                    HexTop(position, 1, hexWorld.hexTakeTurn.selectedTiles.ContainsKey(new Vector2(q, r)));

                    HexPiece piece;
                    if (hexWorld.pieces.TryGetValue(new Vector2(q, r), out piece))
                    {
                        GameObject obj = Instantiate(hexWorld.piecePrefab) as GameObject;
                        obj.transform.position = new Vector3(1.5f * q + 0.5f, height / 2.0f + 0.5f, r * (-Root3) + q * (-Root3 / 2) - Root3 / 2);
                        HexPieceMon hexPiece = obj.GetComponent<HexPieceMon>();
                        hexPiece.hexPiece = piece;
                        obj.renderer.material.color = piece.team.color;
                        pieces.Add(obj);
                    }

                }
            }
        }

        MeshFilter filter = GetComponent<MeshFilter>();
        Mesh mesh = filter.mesh;
        if (mesh == null)
        {
            mesh = new Mesh();
            filter.mesh = mesh;
        }

        GetComponent<MeshFilter>().mesh = mesh;
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();

        MeshCollider col = GetComponent<MeshCollider>();
        col.sharedMesh = null;
        col.sharedMesh = mesh;
    }
	
	
	void HexTop(Vector3 topLeft, byte block, bool isSelected){
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


        if (!isSelected)
        {
            uvs.Add(new Vector2(0.125f, 1));
            uvs.Add(new Vector2(0.26125f, 1));
            uvs.Add(new Vector2(0.26125f, 0));
            uvs.Add(new Vector2(0.125f, 0));
            uvs.Add(new Vector2(0, 0.5f));
            uvs.Add(new Vector2(0.5f, 0.5f));
        }
        else
        {
            uvs.Add(new Vector2(0.6125f, 1));
            uvs.Add(new Vector2(0.76125f, 1));
            uvs.Add(new Vector2(0.76125f, 0));
            uvs.Add(new Vector2(0.6125f, 0));
            uvs.Add(new Vector2(0.5f, 0.5f));
            uvs.Add(new Vector2(1, 0.5f));
        }
		    
					
					
		vertexOffset += 6;
	}
				
	void HexSideTopLeft(Vector3 topLeft, int height, byte block)
	{
        if (height <= 0)
            return;
		vertices.Add(topLeft + new Vector3(0, 0, 0));
		vertices.Add(topLeft + new Vector3(0, -TILE_HEIGHT * height, 0));
		vertices.Add(topLeft + new Vector3(-0.5f, 0, -0.5f * Root3));
		vertices.Add(topLeft + new Vector3(-0.5f, -TILE_HEIGHT * height, -0.5f * Root3));
					
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
				
	void HexSideTop(Vector3 topLeft, int height,  byte block)
	{
        if (height <= 0)
            return;
		vertices.Add(topLeft + new Vector3(0, 0, 0));
		vertices.Add(topLeft + new Vector3(0, -TILE_HEIGHT * height, 0));
		vertices.Add(topLeft + new Vector3(1, 0, 0));
		vertices.Add(topLeft + new Vector3(1, -TILE_HEIGHT * height, 0));
					
					
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
				
	void HexSideTopRight(Vector3 topLeft, int height, byte block)
	{
        if (height <= 0)
            return;
		vertices.Add(topLeft + new Vector3(1, 0, 0));
		vertices.Add(topLeft + new Vector3(1, -TILE_HEIGHT * height, 0));
		vertices.Add(topLeft + new Vector3(1.5f, 0, -0.5f * Root3));
		vertices.Add(topLeft + new Vector3(1.5f, -TILE_HEIGHT * height, -0.5f * Root3));
					
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
				
	void HexSideBottom(Vector3 topLeft, int height, byte block)
	{
        if (height <= 0)
            return;
		vertices.Add(topLeft + new Vector3(0, 0, -Root3));
		vertices.Add(topLeft + new Vector3(0, -TILE_HEIGHT * height, -Root3));
		vertices.Add(topLeft + new Vector3(1, 0, -Root3));
		vertices.Add(topLeft + new Vector3(1, -TILE_HEIGHT * height, -Root3));
					
					
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
				
				
	void HexSideBottomLeft(Vector3 topLeft, int height, byte block)
	{
        if (height <= 0)
            return;
		vertices.Add(topLeft + new Vector3(0, 0, -Root3));
		vertices.Add(topLeft + new Vector3(0, -TILE_HEIGHT * height, -Root3));
		vertices.Add(topLeft + new Vector3(-0.5f, 0, -0.5f * Root3));
		vertices.Add(topLeft + new Vector3(-0.5f, -TILE_HEIGHT * height, -0.5f * Root3));
					
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
				
	void HexSideBottomRIght(Vector3 topLeft, int height, byte block)
	{
        if (height <= 0)
            return;
		vertices.Add(topLeft + new Vector3(1, 0, -Root3));
		vertices.Add(topLeft + new Vector3(1, -TILE_HEIGHT * height, -Root3));
		vertices.Add(topLeft + new Vector3(1.5f, 0, -0.5f * Root3));
		vertices.Add(topLeft + new Vector3(1.5f, -TILE_HEIGHT * height, -0.5f * Root3));
					
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
}
