using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] float matchDurationInSeconds, timeToPlay;
    [SerializeField] TMP_Text timer, playerScoreText, enemyScoreText;
    [SerializeField] Transform spawnPlayer, spawnEnemy, spawnBall, player, enemy, ball;
    int playerScore, enemyScore;
    bool canPlay;

    private void Awake()
    {
        instance = this;
        Basket.goal += Goal;
        playerScore = 0;
        enemyScore = 0;
        DisplayTime();
    }

    private void Start()
    {
        Respawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canPlay) return;

        if(matchDurationInSeconds > 0)
        {
            matchDurationInSeconds -= Time.deltaTime;
        }
        else
        {
            matchDurationInSeconds = 0;
            CheckWinner();
        }
        DisplayTime();
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(timeToPlay);
        canPlay = true;
        ball.GetComponent<Rigidbody>().isKinematic = false;
    }

    void DisplayTime()
    {
        var minutes = Mathf.FloorToInt(matchDurationInSeconds / 60);
        var seconds = Mathf.FloorToInt(matchDurationInSeconds % 60);
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void Goal(bool fromPlayer)
    {
        if (fromPlayer)
            playerScoreText.text = (++playerScore).ToString();
        else
            enemyScoreText.text = (++enemyScore).ToString();

        Respawn();
    }

    void Respawn()
    {
        canPlay = false;
        player.transform.position = spawnPlayer.transform.position;
        player.transform.rotation = spawnPlayer.transform.rotation;
        enemy.transform.position = spawnEnemy.transform.position;
        enemy.transform.rotation = spawnEnemy.transform.rotation;
        ball.transform.position = spawnBall.transform.position;
        ball.transform.rotation = spawnBall.transform.rotation;
        ball.GetComponent<Ball>().StopParticle();
        ball.GetComponent<Rigidbody>().isKinematic = true;
        StartCoroutine("StartGame");
    }

    public bool CanPlay()
    {
        return canPlay;
    }

    void CheckWinner()
    {
        if(playerScore == enemyScore)
        {
            Debug.Log("Overtime");
            return;
        }
        else if(playerScore > enemyScore)
        {
            Debug.Log("You Won");
        }
        else
        {
            Debug.Log("You Lose");
        }
        canPlay = false;
    }
}
