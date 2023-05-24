using System;
using System.Collections.Generic;
using System.Linq;
using Game;
using JetBrains.Annotations;
using UnityEngine;

namespace NonMono
{
    public static class LineChecker
    {
      


        public static List<List<Chip>> GetEmptyLines(Chip first, Chip second)
        {
            int firstLine = first.BoardPosition.y;

            int secondLine = second.BoardPosition.y;

            // a single line
            if (firstLine == secondLine)
            {
                var line = IsLineEmpty(firstLine);

                if (line != null)
                {
                    return new List<List<Chip>>() { line };
                }
            }

            // two lines
            int topLineNum = Mathf.Min(firstLine, secondLine);

            int bottomLineNum = Mathf.Max(firstLine, secondLine);

            List<List<Chip>> lines = new();

            var topLine = IsLineEmpty(topLineNum);

            var bottomLine = IsLineEmpty(bottomLineNum);

            if (bottomLine != null)
            {
                Debug.Log($"Found emptyLine (bottom) ({bottomLineNum})");
                lines.Add(bottomLine);
            }

            if (topLine != null)
            {
                Debug.Log($"Found emptyLine (top) ({topLineNum})");
                lines.Add(topLine);
            }

            return lines;
        }


        private static List<Chip> IsLineEmpty(int boardLine)
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

                if (chip.CurrentChipState == ChipState.LightOn)
                {
                    return false;
                }
            }

            //Debug.DrawRay(origin.transform.position, direction, Color.red, 5f);

            return true;
        }


        private static RaycastHit2D[] GetRaycastHits(int boardLine)
        {
            var hits = new RaycastHit2D[GameManager.Instance.gameData.width];

            ContactFilter2D filter = new();

            Vector2 origin =  GameManager.Instance.gameData.Board[0, boardLine];

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

                if (chip.CurrentChipState == ChipState.LightOn)
                {
                    return null;
                }

                chips.Add(chip);
            }

            return chips;
        }
    }
}