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

	// Use this for initialization
	void Start () {
		for(int q = ChunkQ * GenerateHexagon.chunkSize; q < (ChunkQ + 1)* GenerateHexagon.chunkSize; q ++){
			for(int r = ChunkR * GenerateHexagon.chunkSize; r < (ChunkR + 1) * GenerateHexagon.chunkSize; r ++){
				if(hexWorld.GetBlockHeight() > 0)		
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
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
}
