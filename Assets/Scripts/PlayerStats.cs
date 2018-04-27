using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    [Header("Player Class")]
    //public PlayerClass unitClass;

    [Header("Player Stats")]
    public bool moving = false;
    public int move = 3;
    public float moveSpeed = 10.0f;
    public float jumpHeight = 2.0f;
    public float jumpVelocity = 4.5f;

    [HideInInspector]
    public float halfHeight = 0;
    public Tile currentTile; //Tile occupied by this player.

}
