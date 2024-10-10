using UnityEngine;
using System.Linq;
using TMPro;

public class Board : MonoBehaviour
{
    public bool IsPlayerXTurn { get; private set; } = false;
    public GameObject XPrefab;
    public GameObject OPrefab;
    private string[,] board = new string[3, 3];
    private bool isGameOver = false;

    public void SwitchTurn()
    {
        IsPlayerXTurn = !IsPlayerXTurn;
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    private void triggerGameOver()
    {
        isGameOver = true;
        GameObject gameOverText = GameObject.Find("Winner Text");
        Debug.Log("gameOverText: " + gameOverText);
        gameOverText.GetComponent<TextMeshProUGUI>().text = "Player " + (IsPlayerXTurn ? "X" : "O") + " wins!";
        gameOverText.GetComponent<TextMeshProUGUI>().enabled = true;

        GameObject textBackground = GameObject.Find("Text Background");
        textBackground.GetComponent<SpriteRenderer>().enabled = true;
        // Get the VFX_Explosion prefab particle
        GameObject explosion = GameObject.Find("VFX_Explosion");
        // explosion is a particle system
        ParticleSystem ps = explosion.GetComponent<ParticleSystem>();
        ps.Play();
    }

    public GameObject GetCurrentPlayerPrefab()
    {
        return IsPlayerXTurn ? XPrefab : OPrefab;
    }

    public void SetBoardValue(int row, int col, string value)
    {
        board[row, col] = value;
        // Print the entire board in a single message
        // each row should be on a new line
        string boardString = "\n";
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                boardString += "[" + board[i, j] + "]";
            }
            boardString += "\n";
        }
        Debug.Log(boardString);

        if (checkWin())
        {
            Debug.Log("Player " + (IsPlayerXTurn ? "X" : "O") + " wins!");
            triggerGameOver();
        }
    }

    private bool checkWin()
    {
        // For each row, check if all elements are the same
        for (int i = 0; i < 3; i++)
        {
            if (board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2] && board[i, 0] != null)
            {
                return true;
            }
        }

        // For each column, check if all elements are the same
        for (int i = 0; i < 3; i++)
        {
            if (board[0, i] == board[1, i] && board[1, i] == board[2, i] && board[0, i] != null)
            {
                return true;
            }
        }

        // Check the two diagonals
        if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2] && board[0, 0] != null)
        {
            return true;
        }
        if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0] && board[0, 2] != null)
        {
            return true;
        }

        return false;
    }
}
