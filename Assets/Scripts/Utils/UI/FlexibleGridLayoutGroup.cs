using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(GridLayoutGroup))]
[ExecuteInEditMode()]
public class FlexibleGridLayoutGroup : MonoBehaviour
{
    public int rows = 0;
    public int cols = 0;
    private RectTransform parentRect = null;
    private GridLayoutGroup gridLayout = null;

    //This behaviour should be on START/ONENABLE, can't be AWAKE or UI will broke
    private void OnEnable()
    {
        //Reescalate gridLayout size
        parentRect = gameObject.GetComponent<RectTransform>();
        gridLayout = gameObject.GetComponent<GridLayoutGroup>();
        gridLayout.cellSize = new Vector2(parentRect.rect.width / cols, parentRect.rect.height / rows);
    }

    private void Update()
    {
        //Reescalate gridLayout size
        parentRect = gameObject.GetComponent<RectTransform>();
        gridLayout = gameObject.GetComponent<GridLayoutGroup>();
        gridLayout.cellSize = new Vector2(parentRect.rect.width / cols, parentRect.rect.height / rows);
    }
}