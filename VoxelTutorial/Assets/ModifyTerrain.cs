using UnityEngine;
using System.Collections;
using System.Security.Cryptography.X509Certificates;

public class ModifyTerrain : MonoBehaviour
{

    public GameObject player;
    private World world;
    private GameObject cameraGO;


	// Use this for initialization
	void Start ()
	{
	    world = gameObject.GetComponent<World>();
	    cameraGO = GameObject.FindGameObjectWithTag("MainCamera");

	}
	
	// Update is called once per frame
	void Update () {
        LoadChunks(player.transform.position,32,48);

	    if (Input.GetMouseButtonDown(0))
	    {
            print("pressed 0");
	        ReplaceBlockCursor(0);
	    }
        else if (Input.GetMouseButtonDown(1))
        {
            print("pressed 1");
            AddBlockCursor(1);
        }
	}

    public void ReplaceBlockCenter(float range, byte block)
    {
        Ray ray = new Ray(cameraGO.transform.position, cameraGO.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance < range)
            {
                ReplaceBlockAt(hit, block);
            }
        }

    }

    public void AddBlockCenter(float range, byte block)
    {
        Ray ray = new Ray(cameraGO.transform.position, cameraGO.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance < range)
            {
                AddBlockAt(hit, block);
            }
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * hit.distance, Color.green, 2);
        }

    }

    public void ReplaceBlockCursor(byte block)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            ReplaceBlockAt(hit,  block);
            Debug.DrawLine(ray.origin, ray.origin+(ray.direction * hit.distance), Color.red);

        }
    }

    public void AddBlockCursor( byte block)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            AddBlockAt(hit, block);
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * hit.distance, Color.red);
        }

}

    public void ReplaceBlockAt(RaycastHit hit, byte block)
    {
        //removes a block at these coordinates, you can raycast against the teraain and call this
        // wit hit.point
        Vector3 position = hit.point;
        position += (hit.normal*-0.5f);
        SetBlockAt(position, block);
    }

    public void AddBlockAt(RaycastHit hit, byte block)
    {
        Vector3 position = hit.point;
        position += (hit.normal*0.5f);
        SetBlockAt(position, block);

    }

    public void SetBlockAt(Vector3 position, byte block)
    {
        int x = Mathf.RoundToInt(position.x);
        int y = Mathf.RoundToInt(position.y);
        int z = Mathf.RoundToInt(position.z);

        SetBlockAt(x, y, z, block);

    }

    public void SetBlockAt(int x, int y, int z, byte block)
    {
        print("Adding: " + x + ", " + y + ", " + z);
        world.data[x, y, z] = block;
        UpdateChunkAt(x,y,z);
    }

    public void UpdateChunkAt(int x, int y, int z)
    {
        //Updates the chunk containing this block;

        int updateX = x/world.chunkSize;
        int updateY = y/world.chunkSize;
        int updateZ = z/world.chunkSize;

        print("Updating : " + updateX + ", " + updateY + ", " + updateZ);
        world.chunks[updateX, updateY, updateZ].update = true;

        if (x - (world.chunkSize * updateX) == 0 && updateX != 0)
        {
            world.chunks[updateX - 1, updateY, updateZ].update = true;
        }

        if (x - (world.chunkSize * updateX) == 15 && updateX != world.chunks.GetLength(0) - 1)
        {
            world.chunks[updateX + 1, updateY, updateZ].update = true;
        }

        if (y - (world.chunkSize * updateY) == 0 && updateY != 0)
        {
            world.chunks[updateX, updateY - 1, updateZ].update = true;
        }

        if (y - (world.chunkSize * updateY) == 15 && updateY != world.chunks.GetLength(1) - 1)
        {
            world.chunks[updateX, updateY + 1, updateZ].update = true;
        }

        if (z - (world.chunkSize * updateZ) == 0 && updateZ != 0)
        {
            world.chunks[updateX, updateY, updateZ - 1].update = true;
        }

        if (z - (world.chunkSize * updateZ) == 15 && updateZ != world.chunks.GetLength(2) - 1)
        {
            world.chunks[updateX, updateY, updateZ + 1].update = true;
        }
    }

    public void LoadChunks(Vector3 playerPos, float distToLoad, float distToUnload)
    {
        for (int x = 0; x < world.chunks.GetLength(0); x++)
        {
            for (int z = 0; z < world.chunks.GetLength(2); z++)
            {
                float dist = Vector2.Distance(new Vector2(x*world.chunkSize, z*world.chunkSize),
                    new Vector2(playerPos.x, playerPos.z));
                if (dist < distToLoad)
                {
                    if (world.chunks[x, 0, z] == null)
                    {
                        world.GenColumn(x,z);
                    }
                }
                else if (dist > distToUnload)
                {
                    if(world.chunks[x,0,z] != null)
                    world.UnloadColumn(x,z);
                }
            }
        }
}


}
