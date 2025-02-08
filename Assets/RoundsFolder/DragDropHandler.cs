using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private GameObject draggedObject;
    private Camera mainCamera;

    public GameObject prefabToSpawn;

    private RoundManager.RoundPhase currentPhase;

    void OnEnable()
    {
        RoundManager.OnPhaseStart += UpdateState;
    }

    void OnDisable()
    {
        RoundManager.OnPhaseStart -= UpdateState;
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void UpdateState(RoundManager.RoundPhase phase, int roundNumber)
    {
        currentPhase = phase;

        switch (phase)
        {
            case RoundManager.RoundPhase.Preparation:
                break;
            case RoundManager.RoundPhase.Battle:
                break;
            case RoundManager.RoundPhase.Scoring:
                break;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentPhase != RoundManager.RoundPhase.Preparation)
        {
            return;
        }

        draggedObject = Instantiate(prefabToSpawn);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentPhase != RoundManager.RoundPhase.Preparation || draggedObject == null)
        {
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 newPosition = hit.point;
            newPosition.y = draggedObject.transform.position.y;
            draggedObject.transform.position = newPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (currentPhase != RoundManager.RoundPhase.Preparation || draggedObject == null)
        {
            return;
        }

        GridSystem gridSystem = FindFirstObjectByType<GridSystem>();
        if (gridSystem != null)
        {
            Vector2 worldPosition2D = new Vector2(draggedObject.transform.position.x, draggedObject.transform.position.z);
            Vector2 snappedPosition2D = gridSystem.GetSnappedPosition(worldPosition2D);

            // Проверяем, находится ли объект за пределами сетки
            if (snappedPosition2D.x < 0 || snappedPosition2D.x >= gridSystem.width ||
                snappedPosition2D.y < 0 || snappedPosition2D.y >= gridSystem.height)
            {
                return;
            }
            else
            {
                draggedObject.transform.position = new Vector3(snappedPosition2D.x, draggedObject.transform.position.y, snappedPosition2D.y);
            }
        }
    }
}
