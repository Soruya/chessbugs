using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Bug : MonoBehaviour
{
    public Globals Globals;
    public enum Owner
    {
        PLAYER1,
        PLAYER2
    };
    public Owner owner;
    public enum BugType
    {
        SPIDER,
        NUM_BUGS
    };
    public BugType bug_type;
    public enum BugLocation
    {
        HOME,
        HAND,
        BOARD
    };
    public BugLocation bug_location = BugLocation.HOME;

    [Header("For Home")]
    public BugHome home;
    public float home_last_navigated = 999.0f;
    public Vector3 navigation_target;
    public float navigation_frequency;

    [Header("For Board")]
    public int row;
    public int col;
    public bool done_updating = false;

    void Start()
    {
        Globals = GameObject.FindObjectOfType<Globals>();
        home = GameObject.FindObjectOfType<BugHome>();
        navigation_target = home.transform.position;
        navigation_frequency = Random.Range(Globals.BugHomeNavigationMin, Globals.BugHomeNavigationMax);
    }

    public void UpdateVisuals()
    {
        SpriteRenderer sprite_renderer = GetComponent<SpriteRenderer>();
        sprite_renderer.color = owner == Owner.PLAYER1 ? Color.white : Color.black;
    }

    public void BoardUpdate()
    {
        Vector3 bug_pos = transform.position;

        // positive x is right
        // positive y is up
        switch (bug_type)
        {
            case BugType.SPIDER:
                // SPIDER by default moves up one square
                Vector3 delta = new Vector3(0, 1, 0);
                // but it can be rotated, so we need to rotate the delta by the same rotation that the bug has
                // luckily, this is as simple as rotating by the bug z rotation
                Vector3 rotated_delta = Quaternion.Euler(0, 0, transform.eulerAngles.z) * delta;
                int delta_x = Mathf.RoundToInt(rotated_delta.x);
                int delta_y = Mathf.RoundToInt(rotated_delta.y);
                // NOTE(JUN): columns are x, rows are y
                int desired_x = col + delta_x;
                int desired_y = row + delta_y;
                // NOTE(JUN): when moving more than one unit, you may want to check validity of ALL squares on the path
                if (Globals.Board.SquareEmpty(desired_y, desired_x))
                {
                    // move this bug to the desired square
                    Transform square_transform = Globals.Board.Squares[desired_y][desired_x].transform;
                    Vector3 desired_pos = square_transform.position;
                    desired_pos.z = -0.5f;

                    Vector3 frame_target_position = Vector3.MoveTowards(transform.position, desired_pos, Globals.BugBoardSpeed * Time.deltaTime); 
                    transform.position = frame_target_position;

                    if (Vector3.Distance(transform.position, desired_pos) < 0.0001f)
                    {
                        transform.SetParent(square_transform);
                        transform.position = desired_pos;
                        // set done flag to let Board know it can update the next bug
                        row = desired_y;
                        col = desired_x;
                        done_updating = true;
                    }
                }
                else
                {
                    // failed to move, so we're done
                    done_updating = true;
                }
                break;
            default:
                Assert.IsTrue(false); // Unhandled bug type
                break;
        }
    }

    // called directly by the BugHome per frame
    // NOTE(JUN): I imagine this prevents any threading optimizations that the monobehaviours do, but who cares
    public void HomeUpdate()
    {
        UpdateVisuals();

        home_last_navigated += Time.deltaTime;
        if (home_last_navigated > navigation_frequency)
        {
            // find a new navigation target
            float home_min_x = home.transform.position.x - Globals.BugHomeWidth / 2;
            float home_max_x = home.transform.position.x + Globals.BugHomeWidth / 2;
            float home_min_y = home.transform.position.y - Globals.BugHomeWidth / 2;
            float home_max_y = home.transform.position.y + Globals.BugHomeWidth / 2;
            navigation_target = new Vector3(Random.Range(home_min_x, home_max_x), Random.Range(home_min_y, home_max_y), home.transform.position.z);

            // reset frequency and set new frequency
            home_last_navigated = 0.0f;
            navigation_frequency = Random.Range(Globals.BugHomeNavigationMin, Globals.BugHomeNavigationMax);
        }
        float bug_speed;
        switch (bug_type)
        {
            case BugType.SPIDER:
                bug_speed = 0.0f;
                break;
            default:
                bug_speed = 1.0f;
                break;
        }
        transform.position = transform.position + (navigation_target - transform.position) * bug_speed * Time.deltaTime;
    }
}
