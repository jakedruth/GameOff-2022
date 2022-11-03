using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Pawn : Actor
{
    public Rigidbody2D rb2D { get; private set; }

    public override void Awake()
    {
        base.Awake();
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 deltaPos)
    {
        Vector2 newPos = transform.position;
        newPos += deltaPos;
        rb2D.MovePosition(newPos);
    }

    public void MovePosition(Vector2 newPos)
    {
        rb2D.MovePosition(newPos);
    }
}
