using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinScoreText;
    [SerializeField] private PlayerSessionInfoSO playerSessionInfo;

    private void OnEnable()
    {
        coinScoreText.text = "Coins: " + playerSessionInfo.CoinScore;
    }

}
