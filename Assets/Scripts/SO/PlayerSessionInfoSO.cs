using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSessionInfoSO", menuName = "Player Session Info")]
public class PlayerSessionInfoSO : ScriptableObject
{
    public int Health;
    public int CoinScore;

    public const int DefaultHealth = 100;
    public const int DefaultCoinScore = 0;
}
