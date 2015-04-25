using UnityEngine;

[System.Serializable]
public class HexPiece  {
    [System.NonSerialized]
    public HexTeam team;
    public int maxHealth;
    [System.NonSerialized]
    public int currentHealth;
    public int defense;
    public int attack;
    public int maxMoves;
    [System.NonSerialized]
    public int movesRemaining;
    [System.NonSerialized]
    public bool doneTakingMove;
    [System.NonSerialized]
    public Vector2 position;
}
