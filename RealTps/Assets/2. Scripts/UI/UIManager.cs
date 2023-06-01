using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public static UIManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<UIManager>();
            return instance;
        }
    }

    [SerializeField] private GameObject gameoverUI;
    //[SerializeField] private Crosshair crosshair;
    [SerializeField] private Text healthText;
    [SerializeField] private Text lifeText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text ammothText;
    [SerializeField] private Text waveText;

    [SerializeField] private Crosshair crosshair;

    public void UpdateAmmoText(int remainAmmo)
    {
        ammothText.text = remainAmmo + "/" + 20;
    }

    public void UpdateScoreText(int newScore)
    {
        scoreText.text = "SCORE : " + newScore;
    }

    public void UpdateWaveText(int count)
    {
        waveText.text = "Enemy Left : " + count;
    }

    public void UpdateLifeText(int count)
    {
        lifeText.text = "LIFE : " + count;
    }

    public void UpdateCrossHairPosition(Vector3 worldPosition)
    {
        crosshair.UpdatePosition(worldPosition);
    }

    public void UpdateHealthText(float health)
    {
        healthText.text = Mathf.Floor(health).ToString();
    }

    public void SetActiveCrosshair(bool active)
    {
       crosshair.SetActiveCrosshair(active);
    }

    public void SetActiveGameoverUI(bool active)
    {
        gameoverUI.SetActive(active);
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
