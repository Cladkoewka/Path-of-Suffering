using System;
using _Project.CodeBase.GameLogic.PlacementField;
using _Project.CodeBase.GameLogic.Troop;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    private const float MaxRaycastDistance = 100f;

    [SerializeField] private LayerMask _raycastLayerMask;
    
    private TrooperDrag _currentTrooperDrag;
    private TrooperDrag _selectTrooperDrag;
    private Cell _currentCell;
    private Cell _initialCell;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (CastRay().collider != null)
            Debug.Log(CastRay().collider.name);
        
        if (_currentTrooperDrag == null)
        {
            if (CastRay().collider != null)
            {
                TrooperDrag trooperDrag = CastRay().collider.GetComponent<TrooperDrag>();

                if (trooperDrag != null) 
                    SelectTrooper(trooperDrag);
                else
                    UnselectTrooper();

                if (CanTakeTrooper(trooperDrag))
                {
                    TakeTrooper(trooperDrag);
                }
            }
            else
            {
                UnselectTrooper();
            }
            
        }
        else
        {
            MoveCurrentTrooper();
            if (Input.GetMouseButtonDown(0)) 
                PlaceTrooper();
        }
        
    }

    private void MoveCurrentTrooper()
    {
        if (CastRay().collider != null)
        {
            Cell cell = CastRay().collider.GetComponentInParent<Cell>();
            if (cell != null)
            {
                if (cell.IsEmpty)
                {
                    UpdateCell(cell);
                    _currentTrooperDrag.transform.position = CastRay().collider.transform.position;
                }
            }
            else
            {
                RemoveCell();
                _currentTrooperDrag.transform.position = CastRay().point;
            }
        }
        else
        {
            RemoveCell();
        }
            
    }

    private void PlaceTrooper()
    {
        if (_currentCell != null)
            _currentTrooperDrag.Place(_currentCell);
        else
            _currentTrooperDrag.Place(_initialCell);
        
        _currentTrooperDrag.SetColliderActive(true);
        _currentTrooperDrag = null;
    }

    private void TakeTrooper(TrooperDrag trooperDrag)
    {
        _currentTrooperDrag = trooperDrag;
        _currentTrooperDrag.Take();
        _initialCell = _currentTrooperDrag.Cell;
        _currentTrooperDrag.SetColliderActive(false);
    }

    private void RemoveCell()
    {
        if (_currentCell != null)
            _currentCell.SetHighlight(false);
        _currentCell = null;
    }

    private void UpdateCell(Cell cell)
    {
        if (_currentCell != null)
            _currentCell.SetHighlight(false);
        _currentCell = cell;
        _currentCell.SetHighlight(true);
    }


    private RaycastHit CastRay()
    {
        Vector3 screenMousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _camera.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _camera.nearClipPlane);
        Vector3 worldMousePosFar = _camera.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = _camera.ScreenToWorldPoint(screenMousePosNear);
       
        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit, MaxRaycastDistance, _raycastLayerMask);
        return hit;
    }

    private void UnselectTrooper()
    {
        if (_selectTrooperDrag != null)
            _selectTrooperDrag.transform.localScale = Vector3.one;
        _selectTrooperDrag = null;
    }

    private void SelectTrooper(TrooperDrag trooperDrag)
    {
        _selectTrooperDrag = trooperDrag;
        _selectTrooperDrag.transform.localScale = Vector3.one * 1.1f;
    }

    private static bool CanTakeTrooper(TrooperDrag trooperDrag) => 
        trooperDrag != null && Input.GetMouseButtonDown(0);
}
