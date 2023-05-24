using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NonMono;
using NonMono.Chip_FSM;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public enum ChipState { LightOn, LightOff, Removed }

    [RequireComponent(typeof(Collider2D))]
    public class Chip : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        [ShowInInspector]
        public ChipState CurrentChipState => _chipInfo.state;

        private int ShapeIndex => _chipInfo.shapeIndex;

        private int ColorIndex => _chipInfo.colorIndex;

        public Vector2Int BoardPosition => Utils.ConvertWorldToBoardCoordinates(transform.position);

        private ChipFiniteStateMachine _chipFiniteStateMachine;

        private ChipInfo _chipInfo;

        private GameManager _gameManager;


        private void Awake()
        {
            _gameManager = GameManager.Instance;

            Activate(false);
        }


        public ChipInfo GetInfo()
        {
            _chipInfo.position = transform.position;

            return _chipInfo;
        }


        public void Init(ChipInfo info)
        {
            _chipInfo = info;

            spriteRenderer.sprite = _gameManager.gameData.GetShape(ShapeIndex);

            spriteRenderer.color = _gameManager.gameData.GetColor(ColorIndex, 0f);

            _chipFiniteStateMachine = new ChipFiniteStateMachine(this);

            SetState(ChipState.LightOn);
        }


        public void SetState(ChipState newChipState)
        {
            switch (newChipState)
            {
                case ChipState.LightOn:

                    _chipFiniteStateMachine.LightOn();

                    break;

                case ChipState.LightOff:

                    _chipFiniteStateMachine.LightOff();

                    break;

                case ChipState.Removed:

                    _chipFiniteStateMachine.Removed();

                    break;

                default:

                    throw new ArgumentOutOfRangeException(nameof(newChipState), newChipState, null);
            }

            _chipInfo.state = newChipState;
        }


        [Button("Fade")]
        public async UniTask Fade(float endValue)
        {
            await spriteRenderer
                    .DOFade(endValue, _gameManager.gameData.chipFadeTime)
                    .SetEase(Ease.Linear)
                    .ToUniTask();
        }


        public void Activate(bool isActive)
        {
            gameObject.SetActive(isActive);
        }


        public async UniTask MoveToAsync(Vector2Int boardPos)
        {
            Vector3 worldPos = Board.Board.Instance[boardPos.x, boardPos.y];

            await transform
                    .DOMove(worldPos, _gameManager.gameData.chipShuffleTime)
                    .SetEase(Ease.OutCubic)
                    .ToUniTask();
        }


        [Button("Move UP")]
        public async UniTask MoveUpAsync(float step = 1f)
        {
            Vector3 startPos = transform.position;

            await transform
                    .DOMoveY(startPos.y + step, _gameManager.gameData.chipMoveTime)
                    .SetEase(Ease.InOutCubic)
                    .ToUniTask();
        }


        [Button("Move DOWN")]
        public async UniTask MoveDownAsync(float step = 1f)
        {
            Vector3 startPos = transform.position;

            await transform
                    .DOMoveY(startPos.y - step, _gameManager.gameData.chipMoveTime)
                    .SetEase(Ease.InOutCubic)
                    .ToUniTask();
        }


        [Button("PLACE ON BOARD")]
        public async UniTask PlaceOnBoardAsync()
        {
            spriteRenderer.transform.localPosition =
                    new Vector3(0, _gameManager.gameData.placeOnBoardVerticalShift, 0);

            await spriteRenderer.transform
                    .DOLocalMoveY(0, _gameManager.gameData.chipMoveTime)
                    .ToUniTask();
        }


        [Button("REMOVE FROM BOARD")]
        public async UniTask RemoveFromBoardAsync()
        {
            await spriteRenderer.transform
                    .DOLocalMoveY(
                            _gameManager.gameData.placeOnBoardVerticalShift,
                            _gameManager.gameData.chipMoveTime)
                    .ToUniTask();
        }


        public void Destroy()
        {
            Destroy(gameObject);
        }


        public bool CompareShape(Chip other)
        {
            return ShapeIndex == other.ShapeIndex;
        }


        public bool CompareColor(Chip other)
        {
            return ColorIndex == other.ColorIndex;
        }


        public bool CompareHorizontalPosition(Chip other)
        {
            if (BoardPosition.y != other.BoardPosition.y) return false;

            Vector2 direction = other.BoardPosition - BoardPosition;

            float distance = Mathf.Abs(direction.x);

            return LineChecker.IsPathClear(direction, distance, this, other);
        }


        public bool CompareVerticalPosition(Chip other)
        {
            if (BoardPosition.x != other.BoardPosition.x) return false;

            Vector2 direction = other.BoardPosition - BoardPosition;

            float distance = Mathf.Abs(direction.y);

            return LineChecker.IsPathClear(-direction, distance, this, other);
        }


        public bool CompareMultilinePosition(Chip other)
        {
            if (Mathf.Abs(BoardPosition.y - other.BoardPosition.y) != 1) return false;

            var chips = new Chip[2];

            if (BoardPosition.y > other.BoardPosition.y)
            {
                chips[0] = other;
                chips[1] = this;
            }
            else
            {
                chips[0] = this;
                chips[1] = other;
            }

            bool isTopClear = LineChecker.IsPathClear(
                    Vector2.right,
                    _gameManager.gameData.width - chips[0].BoardPosition.x,
                    chips[0],
                    chips[1]);

            bool isBottomClear = LineChecker.IsPathClear(Vector2.left, chips[1].BoardPosition.x, chips[1], chips[0]);

            return isTopClear && isBottomClear;
        }
    }
}