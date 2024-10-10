using UnityEngine;

public class Square : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private GameObject hoverPiece;
    private Board board;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        board = GetComponentInParent<Board>();

        if (board == null)
        {
            Debug.LogError("Square is not a child of a Board object!");
        }
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider == null || hit.collider.gameObject != gameObject)
        {
            deleteHoverPiece();
            return;
        }

        showHoverPiece();

        if (Input.GetMouseButtonDown(0))
        {
            OnSquareClicked();
        }

    }

    void OnSquareClicked()
    {
        if (board == null) return;

        createPiece();
        // Optionally, disable further clicks on this square
        GetComponent<Collider2D>().enabled = false;

        // Get the number from name of this prefab, it should be square_1, square_2, ... square_9.
        int number = int.Parse(this.name.Split('_')[1]);
        // map the number to the board
        int row = (number - 1) / 3;
        int col = (number - 1) % 3;
        board.SetBoardValue(row, col, board.IsPlayerXTurn ? "X" : "O");

        // Switch to the other player's turn
        board.SwitchTurn();
    }

    private GameObject createPiece()
    {
        GameObject prefab = board.GetCurrentPlayerPrefab();
        Vector3 spawnPosition = transform.position;
        spawnPosition.z = -0.2f;
        GameObject playerPiece = Instantiate(prefab, spawnPosition, prefab.transform.rotation);
        playerPiece.transform.SetParent(transform);
        // Set the scale to 10 on all axes
        playerPiece.transform.localScale = new Vector3(10f, 10f, 10f);

        return playerPiece;
    }

    private void showHoverPiece()
    {
        if (hoverPiece != null) return;
        hoverPiece = createPiece();
    }

    private void deleteHoverPiece()
    {
        if (hoverPiece == null) return;
        Destroy(hoverPiece);
        hoverPiece = null;
    }
}
