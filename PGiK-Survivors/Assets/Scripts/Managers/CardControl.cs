using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardControl : MonoBehaviour {
    public static CardControl Instance { get; private set; }

    [Header( "UI Elements" )]
    [field: SerializeField] private GameObject cardUI;
    [field: SerializeField] private GameObject cardGroup;
    [field: SerializeField] private CardBox cardBoxPrefab;


    [field: SerializeField] private int maxCards = 3;

    [field: SerializeField] private List<CardSO> allCards;
    [field: SerializeField] private List<CardSO> currentCards;



    private List<CardBox> cardBoxes;
    private List<CardSO> rolledCards;

    private void Awake() {
        if ( Instance == null ) { Instance = this; } 
        else {
            Debug.Log( "There is more than one instance of CardControl. Destroying the new one." );
            Destroy( gameObject );
            return;
        }

        if ( cardUI.activeSelf ) Hide();

        cardBoxes = new List<CardBox>();
        rolledCards = new List<CardSO>();
        currentCards = new List<CardSO>();
    }

    private void Start() {
        AddClassCards();

        Shuffle( 1 );
    }

    private void AddClassCards() {
        ClassSO data = GameState.Instance.chosenPlayerClass;
        List<CardSO> cards = data.classCards;

        foreach(CardSO card in cards) {
            allCards.Add( card );
        }

    }

    #region CardPick

    private void AddNextCards( CardSO card ) {
        if ( card.nextCards == null || card.nextCards.Length <= 0 ) return;

        foreach( CardSO nextCard in card.nextCards ) {
            allCards.Add( nextCard );
        }
    }

    private void PurgeCards( CardSO card ) {
        if ( card.cardsToLock == null || card.cardsToLock.Length <= 0 ) return;

        foreach( CardSO purgeCard in card.cardsToLock) {
            currentCards.Remove( purgeCard );
        }
    }

    private void ApplyCard( CardSO card ) {
        List<StatModifier> mods = card.GetMods();

        RefCacheControl.Instance.Player.EntityStats.AddStatMod( mods, StatModHandlingOptions.NoDuplicateModAdd );

        foreach( StatModifier mod in mods ) {
            RefCacheControl.Instance.Player.EntityStats.UpdateStat( mod.affectedStat );
            RefCacheControl.Instance.Player.EntityStats.UpdateEdgeCase( mod.affectedStat );
        }

        mods.Clear();
    }

    public void OnCardClicked( CardSO card ) {

        AddNextCards( card );
        PurgeCards( card );
        ApplyCard( card );
        currentCards.Remove( card );

        Deactivate();
    }

    #endregion

    #region Shuffle

    public void Shuffle( int level ) {
        List<CardSO> toRemove = new List<CardSO>();

        foreach ( CardSO card in allCards ) {
            switch ( card.cardType ) {
                case CardType.Universal: { ShuffleUniversal( card, level, toRemove ); } break;
                case CardType.Class: { ShuffleClass( card, level, toRemove ); } break;
                case CardType.Skill: { ShuffleSkill( card, level, toRemove ); } break;
                case CardType.Special: { ShuffleSpecial( card, level, toRemove ); } break;
            }
        }

        foreach ( CardSO card in toRemove ) {
            allCards.Remove( card );
        }
    }

    private void ShuffleUniversal( CardSO card, int level, List<CardSO> toRemove ) {
        if( level >= card.unlockLevel ) {
            currentCards.Add( card );
            toRemove.Add( card );
        }

    }

    private void ShuffleClass( CardSO card, int level, List<CardSO> toRemove ) {
        if ( level >= card.unlockLevel ) {
            currentCards.Add( card );
            toRemove.Add( card );
        }
    }

    private void ShuffleSkill( CardSO card, int level, List<CardSO> toRemove ) {
        if ( level >= card.unlockLevel ) {
            currentCards.Add( card );
            toRemove.Add( card );
        }
    }

    private void ShuffleSpecial( CardSO card, int level, List<CardSO> toRemove ) {
        //....if there are no other cards in allCards then add them into the pool
    }

    #endregion

    #region Activation

    public void Activate( int playerLevel ) {
        PauseControl.SetPause( true );
        GameInput.Instance.SwitchToMap( ActionMapType.Dummy );

        Shuffle( playerLevel );

        RollCards();
        ShowCards();

        Show();
    }

    public void Deactivate() {
        ClearCards();
        rolledCards.Clear();
        Hide();

        PauseControl.SetPause( false );
        GameInput.Instance.SwitchToMap( ActionMapType.Player );
    }

    #endregion

    #region CardBoxes
    private void ClearCards() {
        for ( int i = 0; i < cardBoxes.Count; i++ ) {
            Destroy( cardBoxes[i].gameObject );
        }

        cardBoxes.Clear();
    }

    private void ShowCards() {
        int stop = maxCards;

        if ( rolledCards.Count < maxCards ) stop = rolledCards.Count;

        for (int i = 0; i < stop; i++ ) {
            CardBox newBox = Instantiate( cardBoxPrefab, cardGroup.transform );
            newBox.Init( rolledCards[i], this );

            cardBoxes.Add( newBox );
        }
    }
    #endregion

    #region Misc

    private void RollCards() {
        HashSet<int> cardNumbers = new HashSet<int>();

        int stop = maxCards;

        if ( currentCards.Count < maxCards ) stop = currentCards.Count;

        while ( cardNumbers.Count < stop ) {
            cardNumbers.Add( Random.Range( 0, currentCards.Count ) );
        }

        foreach(int number in cardNumbers) {
            rolledCards.Add( currentCards[number] );
        }
    }

    private void Show() { cardUI.SetActive( true ); }
    private void Hide() { cardUI.SetActive( false ); }

    #endregion
}
