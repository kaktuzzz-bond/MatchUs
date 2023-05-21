using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class LineChecker
{
    private readonly GameBoard _gameBoard;


    public LineChecker(GameBoard gameBoard)
    {
        _gameBoard = gameBoard;
    }


    public List<Chip> IsLineEmpty(int boardLine)
    {
        var hits = GetRaycastHits(boardLine);

        var chips = AreAllFadedOut(hits);

        return chips;
    }


    public static bool IsPathClear(Vector2 direction, float distance, [NotNull] Chip origin, [NotNull] Chip other)
    {
        ContactFilter2D filter = new();

        List<RaycastHit2D> hits = new();

        if (origin.TryGetComponent(out Collider2D component))
        {
            int count = component.Raycast(direction, filter, hits, distance);
        }

        foreach (RaycastHit2D hit in hits.Where(hit => hit.collider != null))
        {
            if (!hit.collider.TryGetComponent(out Chip chip)) continue;

            if (chip.Equals(other))
            {
                continue;
            }

            if (chip.CurrentState == Chip.States.LightOn)
            {
                return false;
            }
        }

        //Debug.DrawRay(origin.transform.position, direction, Color.red, 5f);

        return true;
    }


    private RaycastHit2D[] GetRaycastHits(int boardLine)
    {
        var hits = new RaycastHit2D[GameManager.Instance.gameData.width];

        ContactFilter2D filter = new();

        Vector2 origin = _gameBoard[0, boardLine];

        int result = Physics2D.Raycast(origin, Vector2.right, filter, hits, GameManager.Instance.gameData.width);

        if (result == 0)
        {
            Debug.LogError("CheckLine() caught the empty line!");
        }

        return hits;
    }


    private static List<Chip> AreAllFadedOut([NotNull] RaycastHit2D[] hits)
    {
        if (hits == null) throw new ArgumentNullException(nameof(hits));

        List<Chip> chips = new();

        foreach (RaycastHit2D hit in hits.Where(hit => hit.collider != null))
        {
            if (!hit.collider.TryGetComponent(out Chip chip)) continue;

            if (chip.CurrentState == Chip.States.LightOn)
            {
                return null;
            }

            chips.Add(chip);
        }

        return chips;
    }
}