using UnityEngine;
using System.Collections;
using System.Security.Cryptography.X509Certificates;

public class ModifyTerrain : MonoBehaviour
{

    public GameObject player;
    private World world;
    private GameObject cameraGO;
	private float timeToCheckPlayer;
	private const float checkPlayerInterval = 0.25f;
	public KeyCode worldUp = KeyCode.I;
	public KeyCode worldDown = KeyCode.K;
	public float distToload = 100;
	public float distToUnload = 104;

	void FixedUpdate(){
		if (Time.time > timeToCheckPlayer) {
			LoadChunks(player.transform.position,distToload,distToUnload);
			timeToCheckPlayer  = Time.time + checkPlayerInterval;
		}
	}


	// Use this for initialization
	void Start ()
	{
	    world = gameObject.GetComponent<World>();
		cameraGO = GameObject.FindGameObjectWithTag ("MainCamera");
		timeToCheckPlayer = Time.time + checkPlayerInterval;

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(worldUp)){
			if(world.viewingHeight < world.worldY - 1){
				world.viewingHeight ++;
				Debug.Log("Current viewing height :" +world.viewingHeight);
				int chunkViewingHeight = world.viewingHeight / world.chunkSize;
				UpdateChunksAtViewHeight(chunkViewingHeight);
			}
		}
		else if(Input.GetKeyDown (worldDown)){
			if(world.viewingHeight > 0 ){
				world.viewingHeight --;
				Debug.Log("Current viewing height :" +world.viewingHeight);
				int chunkViewingHeight = world.viewingHeight / world.chunkSize;
				UpdateChunksAtViewHeight(chunkViewingHeight);
				//Check if we changed chunks
				if((world.viewingHeight  + 1) % world.chunkSize == 0){
					UpdateChunksAtViewHeight(chunkViewingHeight +1);
				}
			}
		}
	    
	}

	void UpdateChunksAtViewHeight(int y){
		for(int x = 0; x < world.worldX / world.chunkSize; x ++){ 
			for(int z = 0; z < world.worldZ / world.chunkSize; z ++){
				world.chunks[x, y, z].update = true;
			}
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

	public void AddPlayerCursor(){
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit)){
			if(! hit.collider.gameObject.tag.Equals("Character")){
				Vector3 position = hit.point;
				position += (hit.normal*-0.5f);
				int x = Mathf.RoundToInt(position.x);
				int z = Mathf.RoundToInt(position.z);
				int y = Mathf.RoundToInt(position.y);
				world.characters[x,z] = 1;
				Debug.Log ("Adding player at " + x + " , " +y +1 + " , " + z);
				UpdateChunkAt(x,y,z);
			}
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
