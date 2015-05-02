using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexTakeTurn : MonoBehaviour {
    private float Root3 = Mathf.Sqrt(3);
    
    //List of teams for the game
    [SerializeField]
    public List<HexTeam> teams = new List<HexTeam>();
    
    public int currentPlayerTurn = 0;
    public GenerateHexagon hexWorld;
    
    //Is a piece currently taking a turn
    private bool takingTurn = false;

    //The maximum height a piece can fall
    private int maxFallHeight = 3;
    
    //The current team taking a turn
    public HexTeam currentTeam;
    public int currentTeamNum = 0;
    
    //Used for storing highlighted tiles for taking turn
    public Dictionary<Vector2, int> selectedTiles = new Dictionary<Vector2,int>();

    //The current piece that can move
    private HexPiece currentPieceTakingTurn;
    
    public void SetUpTeams()
    {
        //Set starting team as team 0
        currentTeam = teams[0];
        hexWorld = GetComponent<GenerateHexagon>();

        //Instantiate all the pieces for all the different teams
        foreach (HexTeam team in teams)
        {
            int squareLength = (int)Mathf.Sqrt(team.startingPlayerCount);
            for (int q = 0; q < squareLength + 1; q++)
            {
                for (int r = 0; r < squareLength; r++)
                {
                    if (q + r >= team.startingPlayerCount)
                    {
                        break;
                    }
                    HexPiece piece = new HexPiece();
                    piece.team = team;
                    piece.position = new Vector2(q + team.currentCenterQ - squareLength / 2, r + team.currentCenterR - squareLength / 2);
                    hexWorld.pieces.Add( piece.position, piece);
                }
            }
        }
    }
   
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootRay();
        }
    }

    int piecesThatMoved = 0;

    void ShootRay()
    {
        //Get a ray from the mouse position on the screen
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            //Try to move character if it's their turn 
            if (hit.collider.tag.Equals("Character") && !takingTurn)
            {
                Debug.Log("clicked player");
                GameObject obj = hit.collider.gameObject;
                HexPieceMon piece = obj.GetComponent<HexPieceMon>();
                if (piece.hexPiece.team.Equals(currentTeam) && !piece.hexPiece.doneTakingMove)
                {
                    Debug.Log("Try take turn");
                    obj.GetComponent<Renderer>().material.color = Color.green;
                    takingTurn = true;
                    Vector2 hexPosition = obj.GetComponent<HexPieceMon>().hexPiece.position;
                    Debug.Log("The actual position of player: " + hexPosition);
                    Vector2 badPosition = RoundHex(NormalToHex(new Vector2(hit.point.x, hit.point.z)));
                    Debug.Log("What code so far thinks the position of the hit is: " + badPosition);
                    TryTakeTurn(hexPosition);
                    currentPieceTakingTurn = piece.hexPiece;
                }
            }


              
            else
            {
                if (takingTurn && hit.collider.tag.Equals("World"))
                {
                    Debug.Log("World hit");
                    Vector3 pos = hit.point;
                    Vector2 hexPos = RoundHex(NormalToHex(new Vector2(pos.x, pos.z)));
                    if (selectedTiles.ContainsKey(hexPos))
                    {
                        Debug.Log("hit selected tiles");
                        hexWorld.pieces.Remove(currentPieceTakingTurn.position);
                        currentPieceTakingTurn.position = hexPos;
                        hexWorld.pieces.Add(hexPos, currentPieceTakingTurn);
                        takingTurn = false;
                        ClearSelectedTiles();
                        piecesThatMoved++;
                        /*
                        if (piecesThatMoved == currentPieceTakingTurn.team.pieces.Count)
                        {
                            piecesThatMoved = 0;
                            foreach (Piece piece in currentTeam.pieces)
                            {
                                piece.doneTakingMove = false;
                            }
                            currentTeamNum = (currentTeamNum + 1) % teams.Count;
                            Debug.Log("Changing to team number " + currentTeamNum);
                            currentTeam = teams[currentTeamNum];
                        }*/
                    }
                }
            }

        }
    }


    //Clears all tiles that are selected colored.
    void ClearSelectedTiles()
    {
        foreach (Vector2 tile in selectedTiles.Keys)
        {
            GetChunkFromPos(new Vector2(tile.x,tile.y)).update = true;
        }
        selectedTiles.Clear();
    }



    void TryTakeTurn(Vector2 piecePos)
    {
        RecursiveMoveSelect(piecePos, 3);    
    }

    void RecursiveMoveSelect(Vector2 piecePos, int movesLeft)
    {
        
        if (!hexWorld.inWorld((int)piecePos.x, (int)piecePos.y))
            return;
        GetChunkFromPos(piecePos).update = true;
        if (movesLeft < 0)
            return;
        if (selectedTiles.ContainsKey(piecePos))
            selectedTiles.Remove(piecePos);
        if(!hexWorld.pieces.ContainsKey(piecePos))
            selectedTiles.Add(piecePos, movesLeft);
        List<Vector2> deltas = new List<Vector2>();
        deltas.Add(new Vector2(0,-1));
        deltas.Add(new Vector2(0,1));
        deltas.Add(new Vector2(-1,0));
        deltas.Add(new Vector2(1,0));
        deltas.Add(new Vector2(-1,1));
        deltas.Add(new Vector2(1,-1));


        foreach(Vector2 delta in deltas){
                int movesPrior;
                if(selectedTiles.TryGetValue(piecePos + delta, out movesPrior)){
                    if(movesPrior < movesLeft -1)
                        RecursiveMoveSelect(piecePos + delta, movesLeft -1);
                }
                else
                {
                    RecursiveMoveSelect(piecePos + delta, movesLeft - 1);
                }
         }
    }



    static int GetHash(int x, int z)
    {
        Vector2 vector = new Vector2(x, z);
        return vector.GetHashCode();
    }

    //Take normal xy coordinates and convert to qr
    public Vector2 NormalToHex(Vector2 normal)
    {
        return hexWorld.NormalToHex(normal);
    }

    public Vector2 HexToNormal(Vector2 hex)
    {
        return hexWorld.HexToNormal(hex);
    }

    public Vector2 RoundHex(Vector2 hex)
    {
        float x = hex.x;
        float z = hex.y;
        float y = -x - z;

        int rx = Mathf.RoundToInt(x);
        int ry = Mathf.RoundToInt(y);
        int rz = Mathf.RoundToInt(z);

        float x_diff = Mathf.Abs(rx - x);
        float y_diff = Mathf.Abs(ry - y);
        float z_diff = Mathf.Abs(rz - z);

        if (x_diff > y_diff && x_diff > z_diff)
            rx = -ry - rz;
        else if (y_diff > z_diff)
            ry = -rx - rz;
        else
            rz = -rx - ry;

        return new Vector2(rx, rz);

    }

    public HexChunk GetChunkFromPos(Vector2 pos)
    {

        int chunkQ = pos.x < 0 ? (int)(((pos.x + 1) / GenerateHexagon.chunkSize) - 1) : (int) (pos.x / GenerateHexagon.chunkSize);
        int chunkR = pos.y < 0 ? (int)(((pos.y + 1) / GenerateHexagon.chunkSize) - 1) : (int) (pos.y / GenerateHexagon.chunkSize);

        int chunkQShifted = chunkQ + (int)(GenerateHexagon.WorldRadius / GenerateHexagon.chunkSize);
        int chunkRShifted = chunkR + (int)(GenerateHexagon.WorldRadius / GenerateHexagon.chunkSize);

        return hexWorld.chunks[chunkQShifted, chunkRShifted];
    }


}
