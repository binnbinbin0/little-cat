using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    [SerializeField] GameObject OverScreen;
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] TextMeshProUGUI score1;
    [SerializeField] TextMeshProUGUI score2;
    private float time = 30;
    private int score = 0;

    public int Scores{
        get
        {
            return score;
        }
        set
        {
            score = value;
            score1.text = $"Score : {value}";
            score2.text = $"Score : {value}";
        }
    }

    public static UIManager instance;
    private void Awake()
    {
        instance = this;
    }

    private void FixedUpdate()
    {
        time -= Time.fixedDeltaTime;
        timer.text = $"Remaining Time: {time:F0}";

        if (time <= 0) Over();
    }

    private void Over()
    {
        OverScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void Restart()
    {

        Time.timeScale = 1;
        // bruh
        SceneManager.LoadScene("SampleScene");
        
    }

}
