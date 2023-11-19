using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinScore : MonoBehaviour
{
    [Tooltip("Move Player Here")]
    [SerializeField] private CoinPicker coinPicker;
    [SerializeField] private TextMeshProUGUI _coinScoreTMP;

    private void Update()
    {
        _coinScoreTMP.text = coinPicker.CoinScore.ToString();
    }
}
