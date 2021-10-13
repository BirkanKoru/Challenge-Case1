using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickManager : MonoBehaviour, IPointerClickHandler
{
    private Camera cam;
    [SerializeField] private GridGenerator gridGenerator = null;

    private void Start()
    {
        cam = GetComponent<Canvas>().worldCamera;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector3 clickPos = Camera.main.ScreenToWorldPoint(eventData.position);
        Vector2 clickPos2D = new Vector2(clickPos.x, clickPos.y);

        RaycastHit2D hit = Physics2D.Raycast(clickPos2D, Vector2.zero);
        Box hitBox = hit.transform.GetComponentInParent<Box>();

        if (hitBox != null && gridGenerator != null)
        {
            hitBox.Select();
            gridGenerator.NeighbourhoodControl();
        }
    }
}
