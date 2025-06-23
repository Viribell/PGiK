using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipShow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [field: SerializeField] private GameObject tooltipObject;
    [field: SerializeField] private TextMeshProUGUI tooltipText;


    public void OnPointerEnter( PointerEventData eventData ) {
        Activate();
    }

    public void OnPointerExit( PointerEventData eventData ) {
        Deactivate();
    }

    public void Activate() {
        tooltipObject.SetActive( true );
    }

    public void Deactivate() {
        tooltipObject.SetActive( false );
    }

    public void SetTooltipText(string text) { tooltipText.text = text; }
}
