using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardBox : MonoBehaviour {
    [field: SerializeField] private Button button;
    [field: SerializeField] private TextMeshProUGUI nameText;
    [field: SerializeField] private TextMeshProUGUI contentText;

    public CardSO card;
    private CardControl cardControlRef;

    public void Init( CardSO card, CardControl control ) {
        this.card = card;
        cardControlRef = control;

        SetNameText( card.cardName );
        SetContentText( card.cardContent );
    }

    private void Start() {
        button.onClick.AddListener( OnClick );
    }

    private void OnClick() {
        cardControlRef.OnCardClicked( card );
    }

    public void SetNameText( string text ) { nameText.text = text; }
    public void SetContentText( string text ) { contentText.text = text; }
}
