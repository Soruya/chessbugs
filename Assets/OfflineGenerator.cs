using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineGenerator : MonoBehaviour
{
    public Globals Globals;

    void Start()
    {
        Globals = GameObject.FindObjectOfType<Globals>();
        Generate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ExecuteInEditMode]
    void Degenerate()
    {
        List<GameObject> to_destroy = new List<GameObject>();
        foreach (Transform child in Globals.Board.transform)
        {
            to_destroy.Add(child.gameObject);
        }
        foreach (GameObject game_object in to_destroy)
        {
            GameObject.DestroyImmediate(game_object);
        }
    }

    [ExecuteInEditMode]
    public void Generate()
    {
        Degenerate();

        Globals.Board.Squares = new List<List<Square>>();
        for (int row = 0; row < Globals.BoardWidth; row++)
        {
            List<Square> col_list = new List<Square>();
            for (int col = 0; col < Globals.BoardHeight; col++)
            {
                GameObject game_object = new GameObject();

                // logical object
                game_object.name = "Square_" + row + "_" + col;
                Square square = game_object.AddComponent<Square>();
                square.row = row;
                square.col = col;
                col_list.Add(square);

                // rendered object
                bool odd = (row + col) % 2 == 0;
                game_object.transform.SetParent(Globals.Board.transform);
                Vector3 base_pos = Globals.Board.transform.position;
                game_object.transform.position = base_pos + new Vector3(col * Globals.SquareWidth, row * Globals.SquareHeight, 0);
                game_object.layer = LayerMask.NameToLayer("BOARD");
                SpriteRenderer sprite_renderer = game_object.AddComponent<SpriteRenderer>();
                BoxCollider2D collider = game_object.AddComponent<BoxCollider2D>();
                // TODO(JUN): not sure why we need to divide by 14 here
                float hack_scale = 14.0f;
                collider.size = new Vector2(Globals.SquareWidth, Globals.SquareHeight) / hack_scale;
                sprite_renderer.sprite = Globals.SquareSprite;
                sprite_renderer.color = odd ? Color.gray : Color.gray;
                // TODO(JUN): not sure why we need to multiply by 6 here
                hack_scale = 6.0f;
                game_object.transform.localScale = new Vector3(Globals.SquareWidth, Globals.SquareHeight, 1) * hack_scale;
                

            }
            Globals.Board.Squares.Add(col_list);
        }
    }
    
}
