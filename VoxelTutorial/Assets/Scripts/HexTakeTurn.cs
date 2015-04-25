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
                    Vector2 badPosition = NormalToHex(new Vector2(hit.point.x, hit.point.z));
                    badPosition.x = Mathf.RoundToInt(badPosition.x);
                    badPosition.y = Mathf.RoundToInt(badPosition.y);
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
                    Vector2 hexPos = NormalToHex(new Vector2(pos.x, pos.z));
                    hexPos.x = Mathf.RoundToInt(hexPos.x);
                    hexPos.y = Mathf.RoundToInt(hexPos.y);
                    hexPos = new Vector2((int)hexPos.x, (int)hexPos.y);
                    if (hexWorld.selectedTiles.ContainsKey(hexPos))
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
        foreach (Vector2 tile in hexWorld.selectedTiles.Keys)
        {
            int q = Mathf.RoundToInt(tile.x) / GenerateHexagon.chunkSize;
            int r = Mathf.RoundToInt(tile.y) / GenerateHexagon.chunkSize;
            hexWorld.chunks[q + GenerateHexagon.WorldRadius / GenerateHexagon.chunkSize,r + GenerateHexagon.WorldRadius / GenerateHexagon.chunkSize].update = true;
        }
        hexWorld.selectedTiles.Clear();
    }



    void TryTakeTurn(Vector2 piecePos)
    {
        RecursiveMoveSelect(piecePos, 3);    
    }

    void RecursiveMoveSelect(Vector2 piecePos, int movesLeft)
    {
        
        if (!hexWorld.inWorld((int)piecePos.x, (int)piecePos.y))
            return;
        hexWorld.chunks[(int)(piecePos.x / GenerateHexagon.chunkSize) + GenerateHexagon.WorldRadius / GenerateHexagon.chunkSize, (int)(piecePos.y / GenerateHexagon.chunkSize) + GenerateHexagon.WorldRadius / GenerateHexagon.chunkSize].update = true;
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




}
