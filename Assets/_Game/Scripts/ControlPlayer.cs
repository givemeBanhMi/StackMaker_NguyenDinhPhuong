using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ControlPlayer : MonoBehaviour
{
    [SerializeField]
    private LayerMask wallLayer; // LayerMask for walls

    [SerializeField]
    private LayerMask disableBrickLayer; // LayerMask for visible bricks

    [SerializeField]
    private LayerMask groundLayer; // LayerMask for ground

    //[SerializeField]
    private Vector3 playerPosition; // Player's position

    [SerializeField]
    private float speed = 5; // Movement speed

    // Constants representing movement directions
    private const int left = 0;
    private const int right = 1;
    private const int forward = 2;
    private const int back = 3;
    private const int stay = 4;

    private List<GameObject> bricks = new List<GameObject>();
    private Vector3 rayShootPosition; // Raycast shooting position
    private Vector3 groundPosition; // Ground position
    private List<Vector3> UsedBridgePosition = new List<Vector3>(); // List of used bridge positions
    private bool isMoving = false; // Movement state
    private bool isFinish = false; // Finish state
    private bool isMovingOnBridge = false; // Movement state on bridge
    private bool isMoveingAfterBridge = true; // Movement state after crossing the bridge
    private int Direction = 4; // Default movement direction
    private int score = 0; // Score

    // Method called on start
    private void Start()
    {
        // Initialize score
        // UIManager.instance.SetScore(score);
    }

    void Update()
    {
        if (isFinish)
            return;

        if (isMovingOnBridge)
        {
            MoveOnBridge();
            return;
        }

        if (!isMoveingAfterBridge)
        {
            Move(Direction);
            isMoveingAfterBridge = true;
        }
        if (!isMoving)
        {
            int getDirection = GetTouchDirection();

            if (getDirection != stay)
            {
                Direction = getDirection;
                isMoving = Move(Direction);
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, new Vector3(playerPosition.x, this.transform.position.y, playerPosition.z)) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    playerPosition,
                    speed * Time.deltaTime
                );
            }
            else
            {
                transform.position = playerPosition;

                isMoving = false;
            }
        }
    }

    // Get vector direction based on direction constant
    Vector3 VectorDirection(int Direct)
    {
        if (Direct == left)
            return Vector3.left;
        else if (Direct == right)
            return Vector3.right;
        else if (Direct == forward)
            return Vector3.forward;
        else if (Direct == back)
            return Vector3.back;
        return Vector3.zero;
    }

    // Move in the specified direction
    bool Move(int Direction)
    {
        rayShootPosition = transform.position;
        rayShootPosition.y = 7;
        groundPosition = transform.position;
        groundPosition.y = -0f;
        groundPosition += VectorDirection(Direction);
        int countAbleBrick = 0;

        while (
            !Physics.Raycast(
                rayShootPosition,
                groundPosition - rayShootPosition,
                Mathf.Infinity,
                wallLayer
            )
            && countAbleBrick < 100
        )
        {
            countAbleBrick++;
            groundPosition += VectorDirection(Direction);

            rayShootPosition += VectorDirection(Direction);
            transform.LookAt(transform.position + VectorDirection(Direction));
            Debug.DrawLine(rayShootPosition, groundPosition, Color.red, 3f);
            if (
                Physics.Raycast(
                    rayShootPosition,
                    groundPosition - rayShootPosition,
                    Mathf.Infinity,
                    disableBrickLayer
                )
            )
            {
                isMovingOnBridge = true;
                Debug.DrawLine(rayShootPosition, groundPosition, Color.white, 5f);
                break;
            }
        }

        playerPosition = transform.position + VectorDirection(Direction) * countAbleBrick;

        return countAbleBrick != 0;
    }

    // Check if position is in the used bridge position list
    bool CheckInList(Vector3 position)
    {
        foreach (Vector3 pos in UsedBridgePosition)
        {
            if (Vector3.Distance(pos, position) < 0.01f)
                return true;
        }
        return false;
    }

    // Move on the bridge
    void MoveOnBridge()
    {
        if (Vector3.Distance(transform.position, playerPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerPosition.x, this.transform.position.y, playerPosition.z) ,2f * speed * Time.deltaTime);
            return;
        }

        transform.position = playerPosition;

        for (int i = 0; i < 4; i++)
        {
            rayShootPosition = transform.position;
            rayShootPosition.y = 10;
            groundPosition = transform.position;
            groundPosition.y = -0f;

            groundPosition += VectorDirection(i);

            if (CheckInList(groundPosition))
            {
                groundPosition -= VectorDirection(i);
                continue;
            }

            if (
                Physics.Raycast(
                    rayShootPosition,
                    groundPosition - rayShootPosition,
                    Mathf.Infinity,
                    disableBrickLayer
                )
            )
            {
                Debug.DrawLine(rayShootPosition, groundPosition, Color.black, 5f);

                playerPosition = transform.position + VectorDirection(i);

                UsedBridgePosition.Add(groundPosition);
                return;
            }
        }

        for (int i = 0; i < 4; i++)
        {
            rayShootPosition = transform.position;
            rayShootPosition.y = 10;

            groundPosition = transform.position;
            groundPosition.y = -0f;

            groundPosition += VectorDirection(i);
            Debug.DrawLine(rayShootPosition, groundPosition, Color.green, 5f);

            if (
                Physics.Raycast(
                    rayShootPosition,
                    groundPosition - rayShootPosition,
                    Mathf.Infinity,
                    groundLayer
                )
            )
            {
                playerPosition = transform.position + VectorDirection(i);
                isMovingOnBridge = false;
                UsedBridgePosition.Clear();
                Direction = i;
                isMoveingAfterBridge = false;
                return;
            }

            groundPosition -= VectorDirection(i);
        }
    }

    // Get touch direction based on touch input
    private int GetTouchDirection()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 previousTouchPosition = touch.position - touch.deltaPosition;
                Vector2 currentTouchPosition = touch.position;

                float deltaX = currentTouchPosition.x - previousTouchPosition.x;
                float deltaY = currentTouchPosition.y - previousTouchPosition.y;
                if (Mathf.Abs(deltaX) <= 5f && Mathf.Abs(deltaY) <= 5f)
                {
                    return stay;
                }

                if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY) + 0.1f)
                {
                    if (deltaX < 0.00f)
                    {
                        return left;
                    }
                    else
                    {
                        return right;
                    }
                }
                else
                {
                    if (deltaY < 0.00f)
                    {
                        return back;
                    }
                    else
                    {
                        return forward;
                    }
                }
            }
        }
        return stay;
    }

    // Handle trigger events
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Brick"))
        {
            Debug.Log("Triggered " + other.tag);
            other.transform.SetParent(this.transform);
            transform.position += Vector3.up * 0.5f;
            bricks.Add(other.gameObject);
            other.tag = "Player";
            other.transform.localPosition = new Vector3(0, -0.5f * bricks.Count, 0);
        }

        if (other.CompareTag("UnBrick"))
        {
            Destroy(bricks[bricks.Count - 1]);
            bricks.RemoveAt(bricks.Count - 1);
            other.tag = "Untagged";
            transform.position -= Vector3.up * 0.5f;
        }
    }

    // Coroutine for ending the level
    IEnumerator EndLevel(float durationTime)
    {
        yield return new WaitForSeconds(durationTime);
        // Update final score
        // UIManager.instance.SetLastScore(score);
    }
}
