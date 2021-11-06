using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Button startButton;
    public Button restartButton;
    public TextMeshProUGUI scoreTextP1;
    public TextMeshProUGUI scoreTextP2;
    public TextMeshProUGUI winText;
    public GameObject titleScreen;
    public GameObject gameOverText;
    public GameObject pieces;
    public GameObject thowingpiece;
    public Slider powerSlider;
    public Slider heightSlider;
    public Slider turnSlider;
    public Slider positionSlider;
    
    /* Unity functions not used in this script
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    */

    public void StartGame()
    {
        titleScreen.gameObject.SetActive(false);
        scoreTextP1.gameObject.SetActive(true);
        scoreTextP2.gameObject.SetActive(true);
        thowingpiece.gameObject.SetActive(true);
        pieces.gameObject.SetActive(true);
        powerSlider.gameObject.SetActive(true);
        heightSlider.gameObject.SetActive(true);
        turnSlider.gameObject.SetActive(true);
        positionSlider.gameObject.SetActive(true);
    }

    public void GameOver()
    {
        restartButton.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);
        winText.gameObject.SetActive(true);

        thowingpiece.gameObject.SetActive(false);
        pieces.gameObject.SetActive(false);
        powerSlider.gameObject.SetActive(false);
        heightSlider.gameObject.SetActive(false);
        turnSlider.gameObject.SetActive(false);
        positionSlider.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
