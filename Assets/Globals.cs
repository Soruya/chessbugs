using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour
{

    [Header("Debug Stuff")]
    public Transform Hit0;

    [Header("Runtime Objects")]
    public Board Board;
    public BugHome BugHome;
    public enum PlayerTurn
    {
        PLAYER1,
        PLAYER2,
        BUGMOVE,
        COUNT
    };
    public PlayerTurn Turn = PlayerTurn.PLAYER1;

    [Header("Board Generation Params")]
    public int BoardWidth = 4;
    public int BoardHeight = 4;
    public float SquareWidth = 10.0f;
    public float SquareHeight = 10.0f;
    public Sprite SquareSprite;

    [Header("Bug Home Params")]
    public float BugHomeWidth = 5.0f;
    public float BugHomeHeight = 5.0f;
    public float BugHomeNavigationMin = 5.0f;
    public float BugHomeNavigationMax = 8.0f;

    [Header("Bug Board Params")]
    public float BugBoardSpeed = 5.0f;

    void Awake()
    {
        Board = GameObject.FindObjectOfType<Board>();
        BugHome = GameObject.FindObjectOfType<BugHome>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
