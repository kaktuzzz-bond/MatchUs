using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipController : MonoBehaviour
{
   private int _chipCounter;

   private Board _board;

   private List<Chip> _chips;
   private void Awake()
   {
      _board = Board.Instance;
   }





   private Vector2Int GetBoardPosition()
   {

      return Vector2Int.zero;
   }
}
