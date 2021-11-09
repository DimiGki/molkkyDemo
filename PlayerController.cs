using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody throwPieceRb;
    private Rigidbody piece1Rb;
    private Rigidbody piece2Rb;
    private Rigidbody piece3Rb;
    private Rigidbody piece4Rb;
    private Rigidbody piece5Rb;
    private Rigidbody piece6Rb;
    private Rigidbody piece7Rb;
    private Rigidbody piece8Rb;
    private Rigidbody piece9Rb;
    private Rigidbody piece10Rb;
    private Rigidbody piece11Rb;
    private Rigidbody piece12Rb;
    private GameManager gameManager;

    private float yPosFallenPieceLimit = 0.53f;
    private int waitSeconds = 2;
    private int totalScoreP1;
    private int p1MissedThrows = 0;
    private bool p1Turn = true;
    private int totalScoreP2;
    private int p2MissedThrows = 0;
    private bool canThrow;
    private int timesCheckPiecesRun = 0;

    public TextMeshProUGUI scoreTextP1;
    public TextMeshProUGUI scoreTextP2;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI currentThrowScore;
    public Slider powerSlider;
    public Slider heightSlider;
    public Slider turnSlider;
    public Slider positionSlider;

    public float throwForce;
    public float startXRotationRelative; // Up or down throw
    public float startYRotationRelative; // Right or left throw
    public float startZRotationRelative; // 0 standing 90 straight

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        piece1Rb = GameObject.FindGameObjectWithTag("1").GetComponent<Rigidbody>();
        piece2Rb = GameObject.FindGameObjectWithTag("2").GetComponent<Rigidbody>();
        piece3Rb = GameObject.FindGameObjectWithTag("3").GetComponent<Rigidbody>();
        piece4Rb = GameObject.FindGameObjectWithTag("4").GetComponent<Rigidbody>();
        piece5Rb = GameObject.FindGameObjectWithTag("5").GetComponent<Rigidbody>();
        piece6Rb = GameObject.FindGameObjectWithTag("6").GetComponent<Rigidbody>();
        piece7Rb = GameObject.FindGameObjectWithTag("7").GetComponent<Rigidbody>();
        piece8Rb = GameObject.FindGameObjectWithTag("8").GetComponent<Rigidbody>();
        piece9Rb = GameObject.FindGameObjectWithTag("9").GetComponent<Rigidbody>();
        piece10Rb = GameObject.FindGameObjectWithTag("10").GetComponent<Rigidbody>();
        piece11Rb = GameObject.FindGameObjectWithTag("11").GetComponent<Rigidbody>();
        piece12Rb = GameObject.FindGameObjectWithTag("12").GetComponent<Rigidbody>();

        throwPieceRb = GetComponent<Rigidbody>();

        throwPieceRb.useGravity = false;
        canThrow = true;

        totalScoreP1 = 0;
        totalScoreP2 = 0;

        scoreTextP1.color = Color.green;

        // Listeners to get the current slide value for the throwing piece
        positionSlider.onValueChanged.AddListener(delegate { ChangePosition(); });
        turnSlider.onValueChanged.AddListener(delegate { ChangeTurn(); });
    }

    // Update is called once per frame
    void Update()
    {
        // Assigning throw values from the sliders
        throwForce = powerSlider.value;
        startXRotationRelative = heightSlider.value;
        startYRotationRelative = turnSlider.value;
        startZRotationRelative = positionSlider.value;

        // When space is hit and there is no other thow at the moment the piece is thrown
        if (Input.GetKeyDown(KeyCode.Space) && canThrow)
        {
            // Reseting the current score object
            currentThrowScore.gameObject.SetActive(false);
            currentThrowScore.CrossFadeAlpha(1.0f, 0f, false);

            canThrow = false;
            throwPieceRb.useGravity = true;
            powerSlider.interactable = false;
            heightSlider.interactable = false;
            turnSlider.interactable = false;
            positionSlider.interactable = false;

            // Setting the throwing piece to the players configurations
            throwPieceRb.transform.rotation = Quaternion.Euler(startXRotationRelative, startYRotationRelative, startZRotationRelative);

            // z is 1 in the AddRealativeForce so that the throw is only forward
            throwPieceRb.AddRelativeForce(startXRotationRelative, startYRotationRelative * throwForce, 1 * throwForce, ForceMode.Impulse);
            StartCoroutine(CheckPieces());
        }
    }

    // Wait for  waitSeconds after the throw and check if the pieces are moving
    IEnumerator CheckPieces()
    {
        yield return new WaitForSeconds(waitSeconds);

        if (throwPieceRb.IsSleeping() &&
             piece1Rb.IsSleeping() &&
             piece2Rb.IsSleeping() &&
             piece3Rb.IsSleeping() &&
             piece4Rb.IsSleeping() &&
             piece5Rb.IsSleeping() &&
             piece6Rb.IsSleeping() &&
             piece7Rb.IsSleeping() &&
             piece8Rb.IsSleeping() &&
             piece9Rb.IsSleeping() &&
             piece10Rb.IsSleeping() &&
             piece11Rb.IsSleeping() &&
             piece12Rb.IsSleeping() ||
             timesCheckPiecesRun == 10)
        {
            int turnScore = CalculateScore();
            CalculatePlayerScore(turnScore);
            timesCheckPiecesRun = 0;

            scoreTextP1.text = $"P1 Score: { totalScoreP1, 3 }   Missed: { p1MissedThrows }";
            scoreTextP2.text = $"P2 Score: { totalScoreP2, 3 }   Missed: { p2MissedThrows }";

            if (totalScoreP1 == 50 || p1MissedThrows == 3 || totalScoreP2 == 50 || p2MissedThrows == 3)
            {
                if (totalScoreP1 == 50 || p2MissedThrows == 3)
                {
                    winText.text = "Player 1 wins";
                }
                else if (totalScoreP2 == 50 || p1MissedThrows == 3)
                {
                    winText.text = "Player 2 wins";
                }
                gameManager.GameOver();
            }
        }
        else
        {
            StartCoroutine(CheckPieces());
            // Checking how many times the function was used so that it does
            // not enter an infinite loop
            timesCheckPiecesRun++;
        }
    }

    int CalculateScore()
    {
        int fallenPieces = 0;
        int oneFallenPiece = 0;
        int score = 0;

        // Check the numbers of fallen pieces by looking at the y position of the piece
        for (int pieceNumber = 1; pieceNumber < 13; pieceNumber++)
        {
            if (GameObject.FindGameObjectWithTag($"{pieceNumber}").transform.position.y < yPosFallenPieceLimit)
            {
                fallenPieces++;
                oneFallenPiece = pieceNumber;
            }
        }

        if (fallenPieces == 1)
        {
            score = oneFallenPiece;
        }
        else
        {
            score = fallenPieces;
        }

        // Showing the score from current throw and fade
        currentThrowScore.text = $"{score}";
        currentThrowScore.gameObject.SetActive(true);
        currentThrowScore.CrossFadeAlpha(0.0f, 3f, false);

        ResetPieces();
        return score;
    }

    void ResetPieces()
    {
        // Reseting the throwing piece
        throwPieceRb.position = new Vector3(0, 3, 0);
        throwPieceRb.transform.rotation = Quaternion.Euler(0, turnSlider.value, positionSlider.value);
        throwPieceRb.useGravity = false;

        // Reseting the pieces
        for (int pieceNumber = 1; pieceNumber < 13; pieceNumber++)
        {
            float previousX = GameObject.FindGameObjectWithTag($"{pieceNumber}").transform.position.x;
            float previousZ = GameObject.FindGameObjectWithTag($"{pieceNumber}").transform.position.z;
            float recoveryHeight = 1.2f;

            GameObject.FindGameObjectWithTag($"{pieceNumber}").transform.position = new Vector3(previousX, recoveryHeight, previousZ);
            GameObject.FindGameObjectWithTag($"{pieceNumber}").transform.rotation = Quaternion.Euler(0, -180, 0);
        }

        canThrow = true;
        powerSlider.interactable = true;
        heightSlider.interactable = true;
        turnSlider.interactable = true;
        positionSlider.interactable = true;
    }

    void CalculatePlayerScore(int turnScore)
    {
        if (p1Turn == true)
        {
            // Check for missed shot
            if (turnScore == 0)
            {
                p1MissedThrows++;
            }
            else
            {
                p1MissedThrows = 0;
            }

            totalScoreP1 += turnScore;
            
            // Check if total score is over 50 and drop to 25 if it is
            if (totalScoreP1 > 50)
            {
                totalScoreP1 = 25;
            }

            p1Turn = false;
            // Change the color of the player text to know whose turn it is
            scoreTextP2.color = Color.green;
            scoreTextP1.color = Color.black;
        }
        else
        {
            if (turnScore == 0)
            {
                p2MissedThrows++;
            }
            else
            {
                p2MissedThrows = 0;
            }

            totalScoreP2 += turnScore;

            // Check if total score is over 50 and drop to 25 if it is
            if (totalScoreP2 > 50)
            {
                totalScoreP2 = 25;
            }

            p1Turn = true;
            scoreTextP1.color = Color.green;
            scoreTextP2.color = Color.black;
        }
    }

    void ChangePosition()
    {
        startZRotationRelative = positionSlider.value;
        throwPieceRb.transform.rotation = Quaternion.Euler(startXRotationRelative, startYRotationRelative, startZRotationRelative);
    }

    void ChangeTurn()
    {
        startYRotationRelative = -turnSlider.value;
        throwPieceRb.transform.rotation = Quaternion.Euler(startXRotationRelative, startYRotationRelative, startZRotationRelative);
    }
}
