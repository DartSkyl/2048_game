using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public static Field Instance;

    [Header("Field Properties")]
    public float CellSize;
    public float Spacing;
    public int FieldSize;
    public int InitCellsCount;

    [Space(10)]
    [SerializeField]
    private Cell cellPref;
    [SerializeField]
    private RectTransform rt;

    private Cell[,] field;

    private bool anyCellMoved;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        SwipeDetection.SwipeEvent += OnInput;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.A))
            OnInput(Vector2.left);
        if (Input.GetKeyDown(KeyCode.D))
            OnInput(Vector2.right);
        if (Input.GetKeyDown(KeyCode.W))
            OnInput(Vector2.up);
        if (Input.GetKeyDown(KeyCode.S))
            OnInput(Vector2.down);
#endif
    }

    private void OnInput(Vector2 direction)
    {
        if (!GameController.GameStarted)
            return;

        anyCellMoved = false;
        ResetCellsFlags();

        Move(direction);

        if (anyCellMoved)
        {
            GenerateRandomCell();
            CheckGameResult();
        }
    }

    private void Move(Vector2 direction)
    {
        int startXY = direction.x > 0 || direction.y < 0 ? FieldSize - 1 : 0;
        int dir = direction.x != 0 ? (int)direction.x : -(int)direction.y;

        for (int i = 0; i < FieldSize; i++)
        {
            for (int k = startXY; k >= 0 && k < FieldSize; k -= dir)
            {
                var cell = direction.x != 0 ? field[k, i] : field[i, k];

                if (cell.IsEmpty)
                    continue;

                var cellToMerge = FindCellToMerge(cell, direction);
                if (cellToMerge != null)
                {
                    cell.MergeWithCell(cellToMerge);
                    anyCellMoved = true;

                    continue;
                }

                var emptyCell = FindEmptyCell(cell, direction);
                if (emptyCell != null)
                {
                    cell.MoveToCell(emptyCell);
                    anyCellMoved = true;
                }
            }
        }
    }

    private Cell FindCellToMerge(Cell cell, Vector2 direction)
    {
        int startX = cell.X + (int)direction.x;
        int startY = cell.Y - (int)direction.y;

        for (int x = startX, y = startY;
            x >= 0 && x < FieldSize && y >= 0 && y < FieldSize;
            x += (int)direction.x, y -= (int)direction.y)
        {
            if (field[x, y].IsEmpty)
                continue;

            if (field[x, y].Value == cell.Value && !field[x, y].HasMerged)
                return field[x, y];

            break;
        }

        return null;
    }

    private Cell FindEmptyCell(Cell cell, Vector2 direction)
    {
        Cell emptyCell = null;

        int startX = cell.X + (int)direction.x;
        int startY = cell.Y - (int)direction.y;

        for (int x = startX, y = startY;
            x >= 0 && x < FieldSize && y >= 0 && y < FieldSize;
            x += (int)direction.x, y -= (int)direction.y)
        {
            if (field[x, y].IsEmpty)
                emptyCell = field[x, y];
            else
                break;
        }

        return emptyCell;
    }

    private void CheckGameResult()
    {
        bool lose = true;

        for (int x = 0; x < FieldSize; x++)
        {
            for (int y = 0; y < FieldSize; y++)
            {
                if (field[x, y].Value == Cell.MaxValue)
                {
                    GameController.Instance.Win();
                    return;
                }

                if (lose &&
                    field[x, y].IsEmpty ||
                    FindCellToMerge(field[x, y], Vector2.left) ||
                    FindCellToMerge(field[x, y], Vector2.right) ||
                    FindCellToMerge(field[x, y], Vector2.up) ||
                    FindCellToMerge(field[x, y], Vector2.down))
                {
                    lose = false;
                }
            }
        }

        if (lose)
            GameController.Instance.Lose();
    }

    private void CreateField()
    {
        field = new Cell[FieldSize, FieldSize];

        float fieldWidth = FieldSize * (CellSize + Spacing) + Spacing;
        rt.sizeDelta = new Vector2(fieldWidth, fieldWidth);

        float startX = -(fieldWidth / 2) + (CellSize / 2) + Spacing;
        float startY = (fieldWidth / 2) - (CellSize / 2) - Spacing;

        for (int x = 0; x < FieldSize; x++)
        {
            for (int y = 0; y < FieldSize; y++)
            {
                var cell = Instantiate(cellPref, transform, false);
                var position = new Vector2(startX + (x * (CellSize + Spacing)), startY - (y * (CellSize + Spacing)));
                cell.transform.localPosition = position;

                field[x, y] = cell;

                cell.SetValue(x, y, 0);
            }
        }
    }

    public void GenerateField()
    {
        if (field == null)
            CreateField();

        for (int x = 0; x < FieldSize; x++)
            for (int y = 0; y < FieldSize; y++)
                field[x, y].SetValue(x, y, 0);

        for (int i = 0; i < InitCellsCount; i++)
            GenerateRandomCell();
    }

    private void GenerateRandomCell()
    {
        var emptyCells = new List<Cell>();

        for (int x = 0; x < FieldSize; x++)
            for (int y = 0; y < FieldSize; y++)
                if (field[x, y].IsEmpty)
                    emptyCells.Add(field[x, y]);

        if (emptyCells.Count == 0)
            throw new System.Exception("There is no any empty cell!");

        int value = Random.Range(0, 10) == 0 ? 2 : 1;

        var cell = emptyCells[Random.Range(0, emptyCells.Count)];
        cell.SetValue(cell.X, cell.Y, value, false);

        CellAnimationController.Instance.SmoothAppear(cell);
    }

    private void ResetCellsFlags()
    {
        for (int x = 0; x < FieldSize; x++)
            for (int y = 0; y < FieldSize; y++)
                field[x, y].ResetFlags();
    }
}
