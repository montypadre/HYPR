﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject player;
    public InputField playerNameInput;
    public string playerName;
    public GameObject[] asteroidObjects;
    

    public float maxRange = 10f;
    public float minRange = 5f;
    public float maximumScale = 5f;
    public float minimumScale = 5f;
    public float spawnInterval = 3f;

    public Vector3 screenCenter;

    float time = 0.0f;
    float minY;
    float maxY;
    float minX;
    float maxX;

    public float gameOverDelay = 1f;
    public float gameOverExpire = 10f;
    public float playerExpire = 30f;

    public GameObject scoreValue;
    public Text highScore;
    private bool isHighScore = false;
    public GameObject gamePanel;
    public HealthBar healthBar;
    public ShieldBar shieldBar;
    public GameObject highScorePanel;
    public GameObject gameOverPanel;
    private int playerShield;
    private int currentPlayerShield;

    bool isPlayerAlive = true;
    bool areShieldsUp = false;

    void Start()
    {
        // Setting the active panel
        gameOverPanel.SetActive(false);
        highScorePanel.SetActive(false);
        shieldBar.gameObject.SetActive(false);
        gamePanel.SetActive(true);

        healthBar.SetMaxHealth(100);

        // Instantiating player
        player = Instantiate(player, new Vector3(0, 0, 0), Quaternion.Euler(-90, 0, 0));
        screenCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        this.minY = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z)).y;
        this.maxY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, -Camera.main.transform.position.z)).y;
        this.minX = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z)).x;
        this.maxX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, -Camera.main.transform.position.z)).x;

        for (var i = 0; i < asteroidObjects.Length; i++)
        {
            Debug.Log(asteroidObjects[i].name);
        }
    }

    public Vector3 GetNewPosition(Vector3 position)
    {
        return new Vector3(screenCenter.x - position.x, screenCenter.y - position.y, 0);
    }

    bool FindPlayer()
    {
        Collider[] colliders = player.GetComponents<Collider>();

        Collider collider;

        if (colliders[0].isTrigger)
        {
            collider = colliders[0];
        }
        else
        {
            collider = colliders[1];
        }

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        if (GeometryUtility.TestPlanesAABB(planes, collider.bounds))
            return true;
        else
            return false;
    }

    void InstantiateRandomAsteroid()
    {
        bool targetPending = true;

        float spawnX = 0;
        float spawnY = 0;

        while (targetPending)
        {
            if (UnityEngine.Random.value > 0.5f)
            {
                Range[] rangesX = new Range[] { new Range(minX - maxRange, minX - minRange), new Range(maxX + minRange, maxX - maxRange) };
                spawnX = RandomValueFromRanges(rangesX);
                spawnY = UnityEngine.Random.Range(minY - maxRange, maxY + maxRange);
            }
            else
            {
                Range[] rangesY = new Range[] { new Range(minY - maxRange, minY - minRange), new Range(maxY + minRange, maxY + maxRange) };
                spawnX = UnityEngine.Random.Range(minX - maxRange, maxX + maxRange);
                spawnY = RandomValueFromRanges(rangesY);
            }

            // Avoiding spawning 2 asteroids on top of each other
            Collider[] colliders = Physics.OverlapBox(new Vector3(spawnX, spawnY, 0), new Vector3(1, 1, 1));

            targetPending = colliders.Length > 0;
        }

        GameObject asteroidObject = Instantiate(asteroidObjects[UnityEngine.Random.Range(0, asteroidObjects.Length)], new Vector3(spawnX, spawnY, 0), Quaternion.Euler(0, 0, 0));

        asteroidObject.transform.LookAt(screenCenter);
        float scale = UnityEngine.Random.Range(minimumScale, maximumScale);

        asteroidObject.transform.localScale = new Vector3(scale, scale, scale);
    }

    void Update()
    {
        if (isPlayerAlive)
        {
            // As player score increases, decrease spawn interval for asteroids
            if (Int64.Parse(scoreValue.GetComponent<Text>().text) < 1000)
            {
                spawnInterval = 3f;
            }
            else if (Int64.Parse(scoreValue.GetComponent<Text>().text) > 1000 && Int64.Parse(scoreValue.GetComponent<Text>().text) < 5000)
            {
                spawnInterval = 2f;
            }
            else if (Int64.Parse(scoreValue.GetComponent<Text>().text) > 5000 && Int64.Parse(scoreValue.GetComponent<Text>().text) < 10000)
            {
                spawnInterval = 1f;
            }
            else if (Int64.Parse(scoreValue.GetComponent<Text>().text) > 10000 && Int64.Parse(scoreValue.GetComponent<Text>().text) < 50000)
            {
                spawnInterval = 0.5f;
            }
            else
            {
                spawnInterval = 0.1f;
            }


            if (!FindPlayer())
            {
                playerExpire -= Time.deltaTime;
                if (playerExpire <= 0)
                {
                    PlayerDies();
                }
                player.transform.position = GetNewPosition(player.transform.position);
            }
            else
            {
                playerExpire = 30f;
            }

            time += Time.deltaTime;

            if (time >= spawnInterval)
            {
                time = time - spawnInterval;

                InstantiateRandomAsteroid();
            }
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.Return))
            {
                PlayerPrefs.SetString("PlayerName", playerNameInput.text);
                Debug.Log(playerNameInput.text);
            }

            if (!highScorePanel.activeSelf)
            {
                if (time < gameOverDelay)
                {
                    time = time + Time.deltaTime;
                }
                else if (Input.anyKey || time > gameOverExpire)
                {
                    SceneManager.LoadScene("MainMenu");
                }
            }
        }
    }

    public static float RandomValueFromRanges(Range[] ranges)
    {
        if (ranges.Length == 0)
            return 0;
        float count = 0;
        foreach (Range r in ranges)
            count += r.range;
        float sel = UnityEngine.Random.Range(0, count);
        foreach (Range r in ranges)
        {
            if (sel < r.range)
            {
                return r.min + sel;
            }
            sel -= r.range;
        }
        throw new Exception("This should never happen");
    }

    public void IncreaseScore(int score)
    {
        scoreValue.GetComponent<Text>().text = (Int64.Parse(scoreValue.GetComponent<Text>().text) + score).ToString();
    }

    public void SaveHighScore()
    {
        if (Convert.ToInt32(scoreValue.GetComponent<Text>().text) > PlayerPrefs.GetInt("HighScore", 0))
        {
            isHighScore = true;
            highScorePanel.SetActive(true);
            PlayerPrefs.SetInt("HighScore", Convert.ToInt32(scoreValue.GetComponent<Text>().text));
        }
        else
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void UpdateHealth(int health)
    {
        healthBar.SetHealth(health);
    }

    public void EngageShield()
    {
        playerShield = player.GetComponent<PlayerHealth>().GetShield();
        shieldBar.SetMaxShield(100);
        player.GetComponent<PlayerHealth>().SetCurrentShield(100);
        shieldBar.gameObject.SetActive(true);

        areShieldsUp = true;
    }

    public void DisengageShield()
    {
        shieldBar.gameObject.SetActive(false);

        areShieldsUp = false;
    }

    public bool ShieldsUp()
    {
        return areShieldsUp;
    }

    public void UpdateShield(int shield)
    {
        shieldBar.SetShield(shield);
    }

    public void Reset()
    {
        PlayerPrefs.DeleteKey("HighScore");
        highScore.text = "0";
    }

    public void PlayerDies()
    {
        gamePanel.SetActive(false);
        isPlayerAlive = false;
        time = 0.0f;
    }
}
