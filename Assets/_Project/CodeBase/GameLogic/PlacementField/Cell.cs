using _Project.CodeBase.GameLogic.Troop;
using UnityEngine;

namespace _Project.CodeBase.GameLogic.PlacementField
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private TrooperDrag _trooperDrag;

        public bool IsEmpty => _trooperDrag == null;


        public void SetHighlight(bool isHighlight)
        {
            if (isHighlight)
                _meshRenderer.material.color = Color.cyan;
            else
                _meshRenderer.material.color = Color.white;
        }

        public void TakeTrooper()
        {
            _trooperDrag = null;
        }
        
        public void PlaceTrooper(TrooperDrag currentTrooperDrag)
        {
            _trooperDrag = currentTrooperDrag;
            SetHighlight(false);
        }
    }
}