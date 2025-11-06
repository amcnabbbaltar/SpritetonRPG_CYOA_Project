using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteSortingOrder : MonoBehaviour
{
    [Tooltip("Higher values make this object appear behind lower ones.")]
    public int sortingOrderBase = 5000;
    [Tooltip("Offset if you want to tweak layer priority per object.")]
    public int offset = 0;
    [Tooltip("Set true to update every frame (recommended for moving objects).")]
    public bool runEveryFrame = true;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSortingOrder();
    }

    private void Update()
    {
        if (runEveryFrame)
            UpdateSortingOrder();
    }

    private void UpdateSortingOrder()
    {
        // Invert Y so lower on screen draws above
        spriteRenderer.sortingOrder = (int)(sortingOrderBase - transform.position.y * 100) + offset;
    }
}
