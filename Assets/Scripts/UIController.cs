using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject loosePopup , winPopup;

    public void SetScore(int score)
    {
        scoreText.text = score.ToString();
        healthSlider.value = score / 100f;
    }

    public void ShowWin()
    {
        winPopup.SetActive(true);
    }

    public void ShowLoose()
    {
        loosePopup.SetActive(true);
    }
}