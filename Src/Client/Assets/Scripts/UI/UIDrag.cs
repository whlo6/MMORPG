using SkillBridge.Message;
using UnityEngine;
using UnityEngine.EventSystems;

//ui可拖拽脚本
public class UIDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("要拖动的窗口本体")]
    public RectTransform windowRoot;

    private RectTransform _canvasRt;
    private Vector2 _offset;

    void Awake()
    {
        _canvasRt = windowRoot.parent as RectTransform;
    }

    // 按下标题栏，记录偏移，防止窗口瞬间跳到鼠标
    public void OnBeginDrag(PointerEventData eventData)
    {

        Vector2 localMousePos;
        //把鼠标【屏幕像素坐标】转换成【Canvas 内部局部坐标】
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasRt,
            eventData.position,
            eventData.pressEventCamera,
            out localMousePos
        );
        _offset = windowRoot.anchoredPosition - localMousePos;
    }

    // 拖拽中，更新窗口位置，附带边界限制，防止拖出屏幕
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localMousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasRt,
            eventData.position,
            eventData.pressEventCamera,
            out localMousePos
        );
        Vector2 newAnchoredPos = localMousePos + _offset;

        #region 边界限制（可选，不想限制就删掉这一块）
        Rect windowRect = windowRoot.rect;
        Rect canvasRect = _canvasRt.rect;
        float minX = -canvasRect.width / 2 + windowRect.width / 2;
        float maxX = canvasRect.width / 2 - windowRect.width / 2;
        float minY = -canvasRect.height / 2 + windowRect.height / 2;
        float maxY = canvasRect.height / 2 - windowRect.height / 2;

        newAnchoredPos.x = Mathf.Clamp(newAnchoredPos.x, minX, maxX);
        newAnchoredPos.y = Mathf.Clamp(newAnchoredPos.y, minY, maxY);
        #endregion

        windowRoot.anchoredPosition = newAnchoredPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 拖拽结束回调，可在这里保存窗口坐标
    }
}
