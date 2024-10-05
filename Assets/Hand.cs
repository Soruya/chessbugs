using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool holding = false;
    public Bug held_bug;

    public Globals Globals;
    void Start()
    {
        Globals = GameObject.FindObjectOfType<Globals>();
    }

    // Update is called once per frame
    void Update()
    {
        // update visuals
        SpriteRenderer sprite_renderer = GetComponent<SpriteRenderer>();
        sprite_renderer.color = Globals.Turn == Globals.PlayerTurn.PLAYER1 ? Color.white : Color.black;
        if (Globals.Turn == Globals.PlayerTurn.BUGMOVE)
        {
            sprite_renderer.color = Color.clear;
            return;
        }

        // move hand to cursor pos
        Vector3 hand_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        hand_position.z = -1;
        transform.position = hand_position;

        // if clicked, pick up or set down (check validity of actions)
        if (Input.GetMouseButtonDown(0))
        {
            // pick up - remove from home and add to hand
            if (!holding)
            {
                // check if clicked a bug and that bug is your bug
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Globals.Hit0.transform.gameObject.activeInHierarchy)
                {
                    Vector3 debug_point = ray.origin + ray.direction;
                    debug_point.z = -1;
                    Globals.Hit0.transform.position = debug_point;
                }

                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, (1 << LayerMask.NameToLayer("BUG")));
                if(hit.collider != null)
                {
                    Bug hit_bug = hit.transform.gameObject.GetComponent<Bug>();
                    if ((int)Globals.Turn == (int)hit_bug.owner && hit_bug.bug_location == Bug.BugLocation.HOME)
                    {
                        held_bug = hit_bug;
                        holding = true;
                        Globals.BugHome.Bugs.Remove(held_bug);
                        held_bug.transform.SetParent(transform);
                        held_bug.bug_location = Bug.BugLocation.HAND;
                    }
                }
            }
            // set down - remove from hand and add to board
            else if (holding)
            {
                // check validity of drop position
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Globals.Hit0.transform.gameObject.activeInHierarchy)
                {
                    Vector3 debug_point = ray.origin + ray.direction;
                    debug_point.z = -1;
                    Globals.Hit0.transform.position = debug_point;
                }

                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, (1 << LayerMask.NameToLayer("BOARD")));
                if(hit.collider != null)
                {
                    Square hit_square = hit.transform.gameObject.GetComponent<Square>();
                    int row = hit_square.row;
                    int col = hit_square.col;

                    bool square_empty = hit_square.transform.childCount == 0;

                    if (square_empty)
                    {
                        held_bug.transform.SetParent(hit_square.transform);
                        Vector3 bug_pos = held_bug.transform.position;
                        bug_pos.z = -0.5f;
                        held_bug.transform.position = bug_pos;
                        Globals.Board.Bugs.Add(held_bug);
                        held_bug.row = row;
                        held_bug.col = col;
                        held_bug.bug_location = Bug.BugLocation.BOARD;
                        held_bug = null;
                        holding = false;

                        Globals.Board.CheckGameOver();

                        Globals.Turn = (Globals.PlayerTurn)(((int)Globals.Turn + 1) % (int)Globals.PlayerTurn.COUNT);
                    }
                }
            }
        }
        else if (Input.GetMouseButtonDown(1) && holding)
        {
            held_bug.transform.Rotate(new Vector3(0, 0, 1), 90.0f);
        }

        // update bug visuals
        if (holding)
        {
            held_bug.transform.position = transform.position;
        }
        
    }
}
