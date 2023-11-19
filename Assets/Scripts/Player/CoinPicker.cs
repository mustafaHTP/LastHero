using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class CoinPicker : MonoBehaviour
{
    [Header("Game Session Data")]
    [Space(5)]
    [SerializeField] private PlayerSessionInfoSO playerSessionInfo;

    [Header("SFX")]
    [Space(5)]
    [SerializeField] private AudioSource sfxCoinPickup;

    [Header("Coin Settings")]
    [Space(5)]
    [Min(1)]
    [Tooltip("High values causes overflow on score ui")]
    [SerializeField] private int CoinScoreMultiplier;

    private int _coinScore;
    public int CoinScore { get => _coinScore; }

    private bool _isTriggerEnabled = false;

    private void Awake()
    {
        _coinScore = playerSessionInfo.CoinScore;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin") && !_isTriggerEnabled)
        {
            _isTriggerEnabled = true;

            PlayCoinPickupSFX();
            IncreaseScore();
            GameObject coin = collision.gameObject;
            Destroy(coin);

            _isTriggerEnabled = false;
        }
    }

    private void IncreaseScore()
    {
        _coinScore += CoinScoreMultiplier;
        playerSessionInfo.CoinScore = _coinScore;
    }

    private void PlayCoinPickupSFX()
    {
        AudioClip coinPickupClip = sfxCoinPickup.clip;
        sfxCoinPickup.PlayOneShot(coinPickupClip);
    }
}
