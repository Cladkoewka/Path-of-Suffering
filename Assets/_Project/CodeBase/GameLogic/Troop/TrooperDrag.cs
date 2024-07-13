using _Project.CodeBase.GameLogic.PlacementField;
using UnityEngine;

namespace _Project.CodeBase.GameLogic.Troop
{
    public class TrooperDrag : MonoBehaviour
    {
        [SerializeField] private Collider _draggableCollider;
        [SerializeField] private Cell _cell;
        public Cell Cell => _cell;

        public void SetColliderActive(bool isActive) => 
            _draggableCollider.enabled = isActive;

        public void Take()
        {
            _cell.TakeTrooper();
        }

        public void Place(Cell cell)
        {
            _cell = cell;
            _cell.PlaceTrooper(this);
            transform.position = _cell.transform.position;
        }
    }
}