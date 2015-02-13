using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateHexagon : MonoBehaviour {

    public const float TILE_HEIGHT = 0.5f;

    List<Vector3> vertices;
    List<int> triangles;
    List<Vector2> uvs;
    int vertexOffset = 0;

    float Root3 = Mathf.Sqrt(3);
    // Use this for initialization
	void Start () {
        vertices = new List<Vector3>();
        triangles = new List<int>();
        uvs = new List<Vector2>();
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
