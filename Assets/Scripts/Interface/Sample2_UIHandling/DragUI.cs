using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;     // これを使用

/* Unity の UIにおける 入力ハンドリング
入力の イベント に対応した インターフェース をを実装することで実現する
UI を作成した時、自動生成される EventSystem が使用される

この インターフェースを 多重継承して、欲しい機能を実装していく
*/
public class DragUI : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler,
    IDragHandler
{
    // クリック(Down)された時実行される
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * 1.2f;
    }
    // クリックして指を離した時(Up)実行される
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
    }
    // ドラッグしている時に実行される
    public void OnDrag(PointerEventData eventData)
    {
        // 現在のスクリーン座標を代入
        transform.position = eventData.position;
    }

}
