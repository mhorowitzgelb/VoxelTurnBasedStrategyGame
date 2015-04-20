using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexTakeTurn : MonoBehaviour {
    private float Root3 = Mathf.Sqrt(3);
    [SerializeField]
    public List<HexTeam> teams = new List<HexTeam>();
    public int currentPlayerTurn = 0;
    public GenerateHexagon hexWorld;
    private bool takingTurn = false;
    private int maxFallHeight = 3;
    public HexTeam currentTeam;
    public int currentTeamNum = 0;
    public Dictionary<Vector2, int> selectedTiles = new Dictionary<Vector2,int>();
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
                    piece.position = new Vector2(q, r);
                    hexWorld.pieces.Add(new Vector2(q + team.currentCenterQ - squareLength / 2, r + team.currentCenterR - squareLength / 2), piece);
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
                GameObject obj = hit.collider.gameObject;
                HexPieceMon piece = obj.GetComponent<HexPieceMon>();
                if (piece.hexPiece.team.Equals(currentTeam) && !piece.hexPiece.doneTakingMove)
                {
                    obj.GetComponent<Renderer>().material.color = Color.green;
                    takingTurn = true;
                    TryTakeTurn(NormalToHex(new Vector2((int)obj.transform.position.x,(int)obj.transform.position.z)));
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
        int q = (int)(normal.x / 1.5f);
        int r = (int)((normal.y - q * (-Root3 /2)) / (-Root3));
        return new Vector2(q, r);
    }




}
