using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private float yPosFallenPieceLimit = 0.51f;
    private int waitSeconds = 2;
    private int totalScoreP1;

    public float throwForce = 10;
    public float startXRotationRelative = 0; // Up or down throw
    public float startYRotationRelative = 0; // Right or left throw
    public float startZRotationRelative = 0; // 0 standing 90 straight
    public int turn = 0;

    // Start is called before the first frame update
    void Start()
    {
        throwPieceRb = GetComponent<Rigidbody>();
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

        throwPieceRb.useGravity = false;
        totalScoreP1 = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))    // When space is hit the piece is thrown
        {
            throwPieceRb.useGravity = true;
            // Setting the throwing piece to the players configurations
            throwPieceRb.transform.rotation = Quaternion.Euler(startXRotationRelative, startYRotationRelative, startZRotationRelative);
            // z is 1 in the AddRealativeForce so that the throw is only forward
            throwPieceRb.AddRelativeForce(startXRotationRelative, startYRotationRelative * throwForce, 1 * throwForce, ForceMode.Impulse);
            StartCoroutine(CheckPieces());
            turn++;
            Debug.Log($"Turn number: { turn }.");
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
             piece12Rb.IsSleeping())
        {
            Debug.Log("No piece move");
            int turnScore = CalculateScore();
            totalScoreP1 += turnScore;
            Debug.Log($"Turns score is: { turnScore }.");
            Debug.Log($"Total score is: { totalScoreP1 }.");
        }
        else
        {
            StartCoroutine(CheckPieces());
        }
    }

    // Calculate score
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

        ResetPieces();
        return score;
    }

    void ResetPieces()
    {
        // Reseting the throwing piece
        throwPieceRb.position = new Vector3(0, 3, -8.73f);
        throwPieceRb.transform.rotation = Quaternion.Euler(0, 0, 90);
        throwPieceRb.useGravity = false;

        // Reseting the pieces
        for (int pieceNumber = 1; pieceNumber < 13; pieceNumber++)
        {
            float previousX = GameObject.FindGameObjectWithTag($"{pieceNumber}").transform.position.x;
            float previousZ = GameObject.FindGameObjectWithTag($"{pieceNumber}").transform.position.z;
            float recoveryHight = 1.5f;

            GameObject.FindGameObjectWithTag($"{pieceNumber}").transform.position = new Vector3(previousX, recoveryHight, previousZ);
            GameObject.FindGameObjectWithTag($"{pieceNumber}").transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
