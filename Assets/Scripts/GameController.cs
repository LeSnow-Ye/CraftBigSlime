using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class GameController : MonoBehaviour
{
    private int score = 0;
    private float LosingStartTime = 0;

    public bool Lost = false;
    public float WarningStartTime = 0;
    public WarningStateEnum WarningState = WarningStateEnum.Idel;

    public static GameController Game;

    public GameObject ScoreLable;
    public GameObject LostScoreLable;
    public GameObject LostImg;
    public GameObject WarningImg;
    public GameObject BallInHand;

    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            ScoreLable.GetComponent<Text>().text = "Score: " + score;
            LostScoreLable.GetComponent<Text>().text = "Score: " + score;
        }
    }

    public enum WarningStateEnum
    {
        Idel,
        Begining,
        Ending,
    }

    private void Awake()
    {
        Game = this;
    }

    private void Update()
    {
        if (!Lost)
        {
            var newPosition = BallInHand.transform.position;
            newPosition.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            newPosition.z = 0;
            BallInHand.transform.position = newPosition;

            if (Input.GetMouseButtonDown(0))
            {
                Instantiate((GameObject)Resources.Load($"Prefabs/L{Random.Range(0, 2)}"), newPosition, Quaternion.identity);
            }

            float t = (Time.time - WarningStartTime) / 1.0f;
            if (WarningState == WarningStateEnum.Begining)
            {
                WarningImg.transform.position = new Vector3(0, Mathf.SmoothStep(-10.0f, 0f, t), 0);
                if (WarningImg.transform.position.y == 0) WarningState = WarningStateEnum.Idel;
            }
            else if (WarningState == WarningStateEnum.Ending)
            {
                WarningImg.transform.position = new Vector3(0, Mathf.SmoothStep(0f, -10.0f, t), 0);
                if (WarningImg.transform.position.y == -7.0f) WarningState = WarningStateEnum.Idel;
            }
        }
        else
        {
            float dt = (Time.time - LosingStartTime) / 1.0f;
            LostImg.transform.position = new Vector3(0, Mathf.SmoothStep(-15.0f, 0f, dt), 0);
        }
    }

    public void Lose()
    {
        Lost = true;
        LosingStartTime = Time.time;

        try
        {
            var webClient = new WebClient();
            webClient.DownloadString("http://42.192.8.111/api/cbs?score=" + Score);
        }
        catch { }
    }

    public void Retry()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
