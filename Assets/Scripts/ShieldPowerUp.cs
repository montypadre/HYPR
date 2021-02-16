﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerUp : MonoBehaviour
{
    public GameController gameController;
    public AudioClip shieldPowerUpClip;

    void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Pickup(other);
        }
    }

    void Pickup(Collider player)
    {
        AudioSource.PlayClipAtPoint(shieldPowerUpClip, 0.9f * Camera.main.transform.position + 0.1f * transform.position, 10f);
        gameController.EngageShield();

        Destroy(gameObject);
    }
}
