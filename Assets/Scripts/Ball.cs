using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public bool Upgrading = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!Upgrading && collision.gameObject.name == gameObject.name && gameObject.name != "L9(Clone)")
        {
            var NewPosition = (collision.gameObject.transform.position + gameObject.transform.position) / 2;
            var level = Convert.ToInt32(gameObject.name.Replace("L", string.Empty).Replace("(Clone)", string.Empty));
            var NewVelocity = (gameObject.GetComponent<Rigidbody2D>().velocity + collision.gameObject.GetComponent<Rigidbody2D>().velocity) / 2;

            collision.gameObject.GetComponent<Ball>().Upgrading = true;
            gameObject.GetComponent<Ball>().Upgrading = true;

            Destroy(collision.gameObject);
            Destroy(gameObject);

            var NewBall = Instantiate((GameObject)Resources.Load($"Prefabs/L{level + 1}"), NewPosition, Quaternion.identity);
            NewBall.GetComponent<Rigidbody2D>().velocity = NewVelocity;

            GameController.Game.Score += (int)Mathf.Pow(2.0f, level);
        }
    }
}
