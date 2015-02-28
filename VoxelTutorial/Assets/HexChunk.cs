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
	public void StartBuilding () {
		for(int q = ChunkQ * GenerateHexagon.chunkSize; q < (ChunkQ + 1)* GenerateHexagon.chunkSize; q ++){
			for(int r = ChunkR * GenerateHexagon.chunkSize; r < (ChunkR + 1) * GenerateHexagon.chunkSize; r ++){
                if (hexWorld.inWorld(q,r))
                {
                    Vector3 position = Vector3.zero;
                    position += r * new Vector3(0, 0, -Root3);
                    position += q * new Vector3(1.5f, 0, -Root3 / 2);
                    int height = World.PerlinNoise(q, 300, r, 20, 4, 0);
                    position += new Vector3(0, height, 0);
                    for (float i = 0; i <= height; i += 0.5f)
                    {
                        Vector3 sidePos = position - new Vector3(0, i, 0);
                        HexSideBottomLeft(sidePos, 0);
                        HexSideBottomRIght(sidePos, 0);
                        HexSideBottom(sidePos, 0);
                        HexSideTop(sidePos, 0);
                        HexSideTopLeft(sidePos, 0);
                        HexSideTopRight(sidePos, 0);
                    }
                    HexTop(position, 1);
                    hexWorld.SetBlock(q, r, 1);
                }		
			}
		}
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();
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
