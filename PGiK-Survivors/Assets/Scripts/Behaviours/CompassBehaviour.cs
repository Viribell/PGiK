using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompassBehaviour : MonoBehaviour {

    [Header("Basic Info")]
    [field: SerializeField] private Color pointerColor;
    [field: SerializeField] private GameObject pointerPrefab;

    [Header("State")]
    [field: SerializeField] private GameObject pointerObject;
    [field: SerializeField] private TextMeshProUGUI distanceText;
    [field: SerializeField] private Image shadow;
    [field: SerializeField] private RectTransform pointerRect;

    private float distance;
    private Transform target;

    private float edgeOffsetX = 20f;
    private float edgeOffsetY = 20f;

    private void Start() {
        KillSelf();

        target = transform;
        target.position.Set( target.position.x, target.position.y, Camera.main.transform.position.z );

        CreatePointer();
    }

    private void Update() {
        UpdateDistance();
        UpdateDistanceText();
        UpdatePointerPosition();
    }


    private void CreatePointer() {
        pointerObject = Instantiate( pointerPrefab, RefCacheControl.Instance.CompassUI.transform );

        distanceText = pointerObject.transform.Find( "DistanceText" ).GetComponent<TextMeshProUGUI>();
        shadow = pointerObject.transform.Find( "Shadow" ).GetComponent<Image>();
        pointerRect = pointerObject.GetComponent<RectTransform>();

        shadow.color = pointerColor;
    }

    private void KillSelf() {
        if ( UpgradesControl.Instance.HasUpgrade( UpgradeType.Compass ) ) return;

        Destroy( this );
    }

    private void UpdatePointerPosition() {
        Vector2 screenPos = Camera.main.WorldToScreenPoint( target.position );
        Vector2 fromPos = Camera.main.transform.position;
        Vector2 dir = ( new Vector2(target.position.x, target.position.y) - fromPos ).normalized;

        bool isOffScreen = screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height;

        if ( isOffScreen ) {
            Vector2 screenSize = new Vector2( Screen.width / 2, Screen.height / 2 );
            Vector2 borderPos;

            screenSize.x -= edgeOffsetX;
            screenSize.y -= edgeOffsetY;

            borderPos = dir * screenSize;

            pointerRect.localPosition = borderPos;

            pointerRect.gameObject.SetActive( true );
        } else {
            pointerRect.gameObject.SetActive( false );
        }
    }

    private void UpdateDistance() {
        distance = Vector2.Distance( Camera.main.transform.position, target.position );
    }

    private void UpdateDistanceText() {
        distanceText.text = Mathf.Ceil( distance ).ToString();
    }
}
