using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningBar : MonoBehaviour
{
    public float timer = 1.0f;
    private bool warning = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (!warning)
            {
                GameController.Game.WarningStartTime = Time.time;
                GameController.Game.WarningState = GameController.WarningStateEnum.Begining;

                warning = true;
            }
        }

        if (timer < -1.5f && !GameController.Game.Lost)
        {
            GameController.Game.Lose();
        }
    }

    private void Update()
    {
        if (timer < 1.0f)
        {
            timer += Time.deltaTime * 0.5f;
        }
        else if (warning)
        {
            GameController.Game.WarningStartTime = Time.time;
            GameController.Game.WarningState = GameController.WarningStateEnum.Ending;

            warning = false;
        }
    }
}
