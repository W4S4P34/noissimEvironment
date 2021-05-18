using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMinimapRoomBuilder : MonoBehaviour
{
    [SerializeField] private GameObject[] doors;
    [SerializeField] private GameObject bossIcon;

    [SerializeField] private SpriteRenderer[] spriteRenderers;

    private Vector3 position;
    private bool[] state;

    private GameObject[] neighbours;

    private bool isVisited;
    private bool isCurrentlyVisited;

    private void Awake()
    {
        isVisited = false;
        isCurrentlyVisited = false;

        neighbours = new GameObject[4];
    }

    public void InitRoomState(Vector3 position, bool[] state, bool isRoot, bool isBoss)
    {
        this.position = position;
        this.state = state;

        // Room's doors activated status
        for (int i = 0; i < doors.Length; i++)
        {
            if (state[i]) doors[i].SetActive(true);
            else doors[i].SetActive(false);
        }

        // Root room activated status
        if (isRoot)
        {
            isVisited = true;
            isCurrentlyVisited = true;
        }

        // Boss icon activated status
        if (isBoss) bossIcon.SetActive(true);
        else bossIcon.SetActive(false);
    }

    public void UpdateNeighbours()
    {
        GameObject[,] minimapRealization = DungeonMinimapBuilder.GetMapRealization();

        // Explicit casting
        int row = (int)position.x, column = (int)position.y;

        // Add neighbours
        for (int i = 0, direction = 4; i < direction; i++)
        {
            switch (i)
            {
                default:
                    neighbours[i] = null;
                    break;
                case 0:
                    neighbours[i] = state[i] ? minimapRealization[row - 1, column] : null;
                    break;
                case 1:
                    neighbours[i] = state[i] ? minimapRealization[row, column + 1] : null;
                    break;
                case 2:
                    neighbours[i] = state[i] ? minimapRealization[row + 1, column] : null;
                    break;
                case 3:
                    neighbours[i] = state[i] ? minimapRealization[row, column - 1] : null;
                    break;
            }
        }
    }

    #region Handle Events (Private)
    private void UpdateColor()
    {
        // Set color for room
        if (isCurrentlyVisited)
        {
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                spriteRenderers[i].color = new Color(1f, 1f, 1f);
            }

            return;
        }

        if (isVisited)
        {
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                spriteRenderers[i].color = new Color(0.5f, 0.5f, 0.5f);
            }
        }
        else
        {
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                spriteRenderers[i].color = new Color(0.15f, 0.15f, 0.15f);
            }
        }
    }

    private void UpdateVisibility()
    {
        if (isVisited || isCurrentlyVisited)
        {
            // Self
            gameObject.SetActive(true);

            // Neighbours
            foreach (GameObject neighbour in neighbours)
            {
                neighbour?.gameObject.SetActive(true);
            }
        }
        else
        {
            // Self
            gameObject.SetActive(false);

            // Neighbours
            foreach (GameObject neighbour in neighbours)
            {
                if (neighbour != null)
                {
                    DungeonMinimapRoomBuilder minimapRoomBuilder =
                        neighbour.gameObject.GetComponent<DungeonMinimapRoomBuilder>();

                    if (minimapRoomBuilder.isVisited
                        || minimapRoomBuilder.isCurrentlyVisited)
                    {
                        gameObject.SetActive(true);
                        break;
                    }
                }
            }
        }
    }
    #endregion

    #region Handle Events (Public)
    public void UpdateMinimapRoomAppearance()
    {
        UpdateColor();
        UpdateVisibility();
    }

    #region Properties
    public bool IsVisited
    {
        get { return isVisited; }
        set { isVisited = value; }
    }

    public bool IsCurrentlyVisited
    {
        get { return isCurrentlyVisited; }
        set { isCurrentlyVisited = value; }
    }

    public GameObject[] Neighbours
    {
        get { return neighbours; }
    }
    #endregion
    #endregion
}
