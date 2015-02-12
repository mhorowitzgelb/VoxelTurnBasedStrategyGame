using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Team  {
	public int startingPlayerCount;
	public int currentCenterX;
	public int currentCenterZ;
	public Color color;
	public string name;
	public List<Piece> pieces = new List<Piece>();
}
