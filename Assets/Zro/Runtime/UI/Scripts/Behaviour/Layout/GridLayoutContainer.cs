using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Zro.UI
{
    [RequireComponent(typeof(RectTransform))]
    [ExecuteAlways]
    public class GridLayoutContainer : UIBehaviour, ILayoutGroup
    {
        public MainAxis mainAxis;
        public CrossAxis crossAxis;

        public Vector2Int cellCount = new Vector2Int(4, 4);
        public Vector2 cellSize;

        public Vector2 gutter;

        [System.NonSerialized] private RectTransform m_Rect;
        protected RectTransform rectTransform
        {
            get
            {
                if (m_Rect == null)
                    m_Rect = GetComponent<RectTransform>();
                return m_Rect;
            }
        }

        protected override void OnRectTransformDimensionsChange()
        {
            SetLayout();
        }

        protected override void OnTransformParentChanged()
        {
            SetLayout();
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            if (cellCount.x <= 0) cellCount.x = 1;
            if (cellCount.y <= 0) cellCount.y = 1;

            SetDirty();
        }

        public void SetLayoutHorizontal()
        {
            SetLayout();
        }

        public void SetLayoutVertical()
        {
            // Empty
        }

        private void SetLayout()
        {
            // Ignore if cell count is invalid
            if (cellCount.x <= 0 || cellCount.y <= 0)
            {
                return;
            }

            // Get rect transforms
            RectTransform[] rectChildren = GetComponentsInChildren<RectTransform>(false);

            // Get container size
            float containerWidth = rectTransform.rect.width;
            float containerHeight = rectTransform.rect.height;

            // Get cell size
            cellSize.x = (containerWidth - (gutter.x * (cellCount.x - 1))) / cellCount.x;
            cellSize.y = (containerHeight - (gutter.y * (cellCount.y - 1))) / cellCount.y;

            // Get axis directions
            Vector2Int axisCount;
            if (mainAxis == MainAxis.Row || mainAxis == MainAxis.RowReverse)
            {
                axisCount = cellCount;
            }
            else // mainAxis == MainAxis.Column || mainAxis == MainAxis.ColumnReverse
            {
                axisCount = new Vector2Int(cellCount.y, cellCount.x);
            }

            // Get anchor point
            Vector2 anchorPoint;
            // Top left anchor
            if (crossAxis == CrossAxis.Normal && (mainAxis == MainAxis.Row || mainAxis == MainAxis.Column))
                anchorPoint = Vector2.up;
            // Bottom right anchor
            else if (crossAxis == CrossAxis.Reverse && (mainAxis == MainAxis.RowReverse || mainAxis == MainAxis.ColumnReverse))
                anchorPoint = Vector2.right;
            // Top right anchor
            else if ((mainAxis == MainAxis.Column && crossAxis == CrossAxis.Reverse) || (mainAxis == MainAxis.RowReverse && crossAxis == CrossAxis.Normal))
                anchorPoint = Vector2.one;
            // Bottom left anchor
            else
                anchorPoint = Vector2.zero;

            // Set child positions
            for (int i = 0; i < rectChildren.Length - 1; i++)
            {
                RectTransform rectChild = rectChildren[i + 1];

                //float mainAxisPosition = ((i % cellCount.x));
                //float crossAxisPosition = ((i / cellCount.y));
                float xPosition = (i % axisCount.x) * (cellSize.x + gutter.x);
                float yPosition = (i / axisCount.x) * (cellSize.y + gutter.y);
                Vector2 offset = new Vector2(xPosition, yPosition);

                rectChild.anchorMin = anchorPoint;
                rectChild.anchorMax = anchorPoint;

                rectChild.SetInsetAndSizeFromParentEdge(
                    anchorPoint.x < 0.5f ? RectTransform.Edge.Left : RectTransform.Edge.Right,
                    offset.x,
                    cellSize.x
                    );
                rectChild.SetInsetAndSizeFromParentEdge(
                    anchorPoint.y < 0.5f ? RectTransform.Edge.Bottom : RectTransform.Edge.Top,
                    offset.y,
                    cellSize.y
                    );
            }
        }

        /// <summary>
        /// Mark the GridLayoutContainer as dirty.
        /// </summary>
        protected void SetDirty()
        {
            if (!IsActive())
                return;

            if (!CanvasUpdateRegistry.IsRebuildingLayout())
                LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
            else
                StartCoroutine(DelayedSetDirty(rectTransform));
        }

        IEnumerator DelayedSetDirty(RectTransform rectTransform)
        {
            yield return null;
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }

        public enum MainAxis
        {
            Row,
            Column,
            RowReverse,
            ColumnReverse,
        }

        public enum CrossAxis
        {
            Normal,
            Reverse,
        }
    }
}
