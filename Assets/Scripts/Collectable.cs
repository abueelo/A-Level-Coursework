using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public enum CollectableType { // enum for collectable types
        Key,
        Potion,
        Sword,
        PathItem,
        Axe,
    }

    public CollectableType type;
    public Sprite[] sprites;
    public Player player;
    public SpriteRenderer spriteRenderer;

    void Start()
    {
        player = GameObject.Find("Player(Clone)").GetComponent<Player>(); // this is to get the player object so that i can call the collect function when the player collides with the collectable

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[(int)type];
    }

    void Update()
    {
        // checks to see if the player is ontop of a collectable
        if (transform.position == player.transform.position)
        {
            player.CollectCollectable(type); // adds the collectable to the players inventory
            Destroy(gameObject); // destroys the collectable from the game board
        }
    }
}
