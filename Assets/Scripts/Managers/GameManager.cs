using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] float matchDurationInSeconds, timeToPlay;
    [SerializeField] TMP_Text timer, playerScoreText, enemyScoreText, winnerText;
    [SerializeField] CanvasGroup scoreTableCanvas, controllerCanvas, endMatchCanvas, homeScreenCanvas;
    [SerializeField] CinemachineVirtualCamera playerCam, startCam;
    Transform spawnPlayer, spawnEnemy, spawnBall, player, enemy, ball;
    int playerScore, enemyScore;
    bool canPlay, isOvertime;
    const string overtimeStr = "OVERTIME";

    private void Awake()
    {
        Application.targetFrameRate = 60;
        instance = this;
        playerScore = 0;
        enemyScore = 0;
        isOvertime = false;
        canPlay = false;
        DisplayTime();
        spawnBall = GameObject.FindGameObjectWithTag("SpawnBall").transform;
        spawnPlayer = GameObject.FindGameObjectWithTag("SpawnPlayer").transform;
        spawnEnemy = GameObject.FindGameObjectWithTag("SpawnEnemy").transform;
        ball = GameObject.FindGameObjectWithTag("Ball").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
    }

    private void Start()
    {
        DisableCanvas(scoreTableCanvas);
        DisableCanvas(controllerCanvas);
        DisableCanvas(endMatchCanvas);
        EnableCanvas(homeScreenCanvas);
        startCam.Priority = 99;
        playerCam.Priority = -1;
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
        if(!isOvertime)
            DisplayTime();
    }

    public void StartGame()
    {
        playerCam.Priority = 99;
        startCam.Priority = -1;
        EnableCanvas(scoreTableCanvas);
        EnableCanvas(controllerCanvas);
        DisableCanvas(homeScreenCanvas);
        Respawn();
    }

    IEnumerator StartGameCR()
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
        player.GetComponent<Player>().ResetCharacter();
        enemy.GetComponent<Enemy>().ResetCharacter();
        StartCoroutine(StartGameCR());
    }

    public bool CanPlay()
    {
        return canPlay;
    }

    void CheckWinner()
    {
        if(playerScore == enemyScore)
        {
            isOvertime = true;
            timer.text = overtimeStr;
            return;
        }
        else if(playerScore > enemyScore)
        {
            winnerText.text = "Player";
        }
        else
        {
            winnerText.text = "Enemy";
        }
        EndGame();
    }

    void EndGame()
    {
        canPlay = false;
        Time.timeScale = 0;
        DisableCanvas(scoreTableCanvas);
        DisableCanvas(controllerCanvas);
        EnableCanvas(endMatchCanvas);
    }

    public void RestartGame()
    {
        if (Time.timeScale == 0) Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    void EnableCanvas(CanvasGroup canvas)
    {
        canvas.alpha = 1;
        canvas.interactable = true;
        canvas.blocksRaycasts = true;
    }
    void DisableCanvas(CanvasGroup canvas)
    {
        canvas.alpha = 0;
        canvas.interactable = false;
        canvas.blocksRaycasts = false;
    }

    private void OnEnable()
    {
        Basket.goal += Goal;
    }

    private void OnDisable()
    {
        Basket.goal -= Goal;
    }
}
