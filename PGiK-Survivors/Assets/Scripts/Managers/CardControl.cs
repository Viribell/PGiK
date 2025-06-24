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

    [Header( "Cards" )]
    [field: SerializeField] private List<CardSO> allCards;
    [field: SerializeField] private List<CardSO> currentCards;
    [field: SerializeField] private List<CardSO> specialCards;

    [Header("Food")]
    [field: SerializeField] private SerializableDictionary<FoodSO, int> foodAmount;
    [field: SerializeField] private List<FoodBox> foodBoxes;

    private List<CardBox> cardBoxes;
    private List<CardSO> rolledCards;

    public FoodBox selectedFood;

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

    private void DecreaseFood(FoodSO food) {
        foodAmount[food] -= 1;
    }

    public bool IsFoodAvailable(FoodSO food) {
        return foodAmount[food] > 0;
    }

    public int GetFoodLevel(FoodSO food) {
        return foodAmount[food];
    }

    public bool UseFood(FoodSO food) {
        if( IsFoodAvailable(food) ) {
            DecreaseFood( food );
            return true;
        } else {
            return false;
        }
    }

    private void InitFoodBoxes() {
        foreach(FoodBox box in foodBoxes) {
            box.Init();
        }
    }

    private void Start() {
        foodAmount = MaterialsSaveControl.Instance.GetFoodAmount();

        InitFoodBoxes();

        AddClassCards();

        Shuffle( 1 );

        UpdateNumberOfCards();
    }

    private void UpdateNumberOfCards() {
        if( UpgradesControl.Instance.HasUpgrade( UpgradeType.PlusCard ) ) {
            maxCards = 4;
        }
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

    private void LockCard( CardSO card ) {
        CardBox deletedBox = null;

        foreach (CardBox box in cardBoxes) {
            if ( box.card == card ) { deletedBox = box; break; }
        }

        DeleteCardBox( deletedBox );
        rolledCards.Remove( card );
        currentCards.Remove( card );

        if ( cardBoxes.Count <= 0 ) Reroll();
    }

    private void DeleteCardBox(CardBox box) {
        if ( box == null ) return;

        cardBoxes.Remove( box );
        Destroy( box.gameObject );
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

        if ( card.cardType == CardType.Special ) { HandleSpecial( card ); return; }

        if( selectedFood == null ) {
            AddNextCards( card );
            PurgeCards( card );
            ApplyCard( card );
            currentCards.Remove( card );

            Deactivate();
        } else {
            HandleFoodEffects( selectedFood.food.foodType, card );
        }
    }

    private void HandleSpecial(CardSO card) {
        const string GOLD = "Z³oto";
        const string HP = "¯ycie";

        if( card.cardName == GOLD ) {
            GameResources.Instance.AddGold( 10 );

        } else if( card.cardName == HP ) {
            RefCacheControl.Instance.Player.EntityHealth.Heal( 10 );
        }

        Deactivate();
    }

    public void HandleFoodEffects(FoodType type, CardSO card) {
        switch ( type ) {
            case FoodType.Stew: { UseStew( card ); } break;
            case FoodType.Syrup: { UseSyrup( card );  } break;
            case FoodType.Sunflower: { UseSunflower(); } break;
            case FoodType.Cake: { UseCake(); } break;
            case FoodType.Hotpot: { UseHotpot( card ); } break;
        }
    }

    private void UseStew(CardSO card) {
        LockCard( card );

        ConsumeFood();
    }
    
    private void UseSyrup(CardSO card) {
        AddNextCards( card );
        PurgeCards( card );
        ApplyCard( card );
        ApplyCard( card );
        currentCards.Remove( card );

        ConsumeFood();

        Deactivate();
    }

    private void UseSunflower() {
        Reroll();

        ConsumeFood();
    }

    private void UseCake() { }

    private void UseHotpot(CardSO card) {
        AddNextCards( card );
        PurgeCards( card );
        ApplyCard( card );
        currentCards.Remove( card );
        ConsumeFood();
        Deactivate();

        RefCacheControl.Instance.Player.PlayerLevel.InstantLevelUp();
    }

    private void Reroll() {
        ClearCards();
        rolledCards.Clear();

        RollCards();
        ShowCards();
    }

    private void ConsumeFood() {
        selectedFood.Unselect();
        selectedFood.UseSuccess();
        selectedFood = null;
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

        if ( currentCards.Count <= 0 ) { AddSpecial(); return; }

        if ( currentCards.Count < maxCards ) stop = currentCards.Count;

        while ( cardNumbers.Count < stop ) {
            cardNumbers.Add( Random.Range( 0, currentCards.Count ) );
        }

        foreach(int number in cardNumbers) {
            rolledCards.Add( currentCards[number] );
        }
    }

    private void AddSpecial() {
        foreach ( CardSO card in specialCards ) {
            rolledCards.Add( card );
        }
    }

    private void Show() { cardUI.SetActive( true ); }
    private void Hide() { cardUI.SetActive( false ); }

    #endregion
}
