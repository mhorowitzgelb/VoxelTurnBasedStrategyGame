using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class HexTeam {
    public int startingPlayerCount;
    public int currentCenterQ;
    public int currentCenterR;
    public Color color;
    public string name;
    public List<HexPiece> pieces = new List<HexPiece>();
}
