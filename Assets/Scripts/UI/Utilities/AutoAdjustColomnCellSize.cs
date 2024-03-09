using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class AutoAdjustColomnCellSize : UnityEngine.EventSystems.UIBehaviour
{
    private GridLayoutGroup grid;

    protected override void Awake()
    {
        
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnRectTransformDimensionsChange()
    {
        grid = GetComponent<GridLayoutGroup>();
        if(grid.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
        {
            //print(GetComponent<RectTransform>().rect.size);
            grid.cellSize = new Vector2(GetComponent<RectTransform>().rect.size.x / grid.constraintCount, grid.cellSize.y);
        }
    }
}
