using UnityEngine;
using System.Collections;

public class Piece : MonoBehaviour {
	public Team team;
	public int maxHealth;
	public int currentHealth;
	public int defense;
	public int attack;
	public int maxMoves;
	public int movesRemaining;
	public bool doneTakingMove;
}
