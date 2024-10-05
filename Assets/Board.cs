using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public List<List<Square>> Squares;
    public List<Bug> Bugs;
    public Globals Globals;

    [Header("BUGMOVE state")]
    public bool started_moving = false;
    public int bug_index = 0;

    public bool SquareEmpty(int x, int y)
    {
        // out of board bounds
        if (x < 0 || x >= Globals.BoardWidth || y < 0 || y >= Globals.BoardHeight)
        {
            return false;
        }

        // bug already on this square 
        if (Squares[x][y].transform.childCount > 0)
        {
            return false;
        }

        return true;
    }

    void Start()
    {
        Globals = GameObject.FindObjectOfType<Globals>();
    }

    void Update()
    {
        if (Globals.Turn == Globals.PlayerTurn.BUGMOVE)
        {
            if (!started_moving)
            {
                // reset state
                started_moving = true;
                bug_index = 0;
                foreach (Bug bug in Bugs)
                {
                    bug.done_updating = false;
                }
            }
            if (bug_index < Bugs.Count)
            {
                // allow this bug to update until it's done
                Bug bug = Bugs[bug_index];
                bug.BoardUpdate();

                // then check win condition and move on to the next one
                if (bug.done_updating)
                {
                    CheckGameOver();

                    bug_index++;
                }
            }
            else
            {
                // all bugs finished updating
                // reset state
                started_moving = false;

                // back to player 1 turn
                Globals.Turn = Globals.PlayerTurn.PLAYER1;
            }
        }
        
    }

    public void CheckGameOver()
    {
        // check the rows
        for (int i = 0; i < Globals.BoardWidth; i++)
        {
            bool fail_to_end = false;
            bool[] seen = new bool[] { false, false };
            for (int j = 0; j < Globals.BoardHeight; j++)
            {
                Square square = Globals.Board.Squares[i][j];
                if (!square.HasBug())
                {
                    fail_to_end = true;
                    break;
                }
                seen[square.GetOwner()] = true;
                if (seen[0] && seen[1])
                {
                    fail_to_end = true;
                    break;
                }
            }
            if (!fail_to_end)
            {
                print("game over");
                return;
            }
        }
        // check the columns
        for (int i = 0; i < Globals.BoardWidth; i++)
        {
            bool fail_to_end = false;
            bool[] seen = new bool[] { false, false };
            for (int j = 0; j < Globals.BoardHeight; j++)
            {
                Square square = Globals.Board.Squares[j][i];
                if (!square.HasBug())
                {
                    fail_to_end = true;
                    break;
                }
                seen[square.GetOwner()] = true;
                if (seen[0] && seen[1])
                {
                    fail_to_end = true;
                    break;
                }
            }
            if (!fail_to_end)
            {
                print("game over");
                return;
            }
        }
        // check diagonal 1
        {
            bool fail_to_end = false;
            bool[] seen = new bool[] { false, false };
            for (int i = 0; i < Globals.BoardWidth; i++)
            {
                Square square = Globals.Board.Squares[i][i];
                if (!square.HasBug())
                {
                    fail_to_end = true;
                    break;
                }
                seen[square.GetOwner()] = true;
                if (seen[0] && seen[1])
                {
                    fail_to_end = true;
                    break;
                }
            }
            if (!fail_to_end)
            {
                print("game over");
                return;
            }
        }
        // check diagonal 1
        {
            bool fail_to_end = false;
            bool[] seen = new bool[] { false, false };
            for (int i = 0; i < Globals.BoardWidth; i++)
            {
                Square square = Globals.Board.Squares[i][Globals.BoardWidth - i - 1];
                if (!square.HasBug())
                {
                    fail_to_end = true;
                    break;
                }
                seen[square.GetOwner()] = true;
                if (seen[0] && seen[1])
                {
                    fail_to_end = true;
                    break;
                }
            }
            if (!fail_to_end)
            {
                print("game over");
                return;
            }
        }

    }
}
