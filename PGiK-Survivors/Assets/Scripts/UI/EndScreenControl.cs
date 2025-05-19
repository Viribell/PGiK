using TMPro;
using UnityEngine;

public enum EndScreenType {
    DeathScreen,
    WinScreen
}

public class EndScreenControl : MonoBehaviour {
    public static EndScreenControl Instance { get; private set; }

    [Header( "Basic Info" )]
    [field: SerializeField] private GameObject endScreen;
    [field: SerializeField] private GameObject gameBonusArea;

    [Header( "Elements" )]
    [field: SerializeField] private TextMeshProUGUI endText;
    [field: SerializeField] private TextMeshProUGUI timeSurvivedText;
    [field: SerializeField] private TextMeshProUGUI timeBonusText;
    [field: SerializeField] private TextMeshProUGUI gameWonBonusText;

    private void Awake() {
        if ( Instance == null ) { Instance = this; } 
        else {
            Debug.Log( "Instance of EndScreenControl already exists!" );
        }

        if ( endScreen.activeSelf ) Hide();
    }

    public void OnLobbyClicked() {
        GameInput.Instance.DisableMaps();

        SaveControl.Instance.SaveGame();

        PauseControl.SetPause( false );

        SceneLoader.Load( SceneLoader.Scene.LobbyScene );
    }

    public void InitBonuses(float timeBonus, float gameBonus) {
        SetTimeBonusText( timeBonus.ToString() );
        SetGameWonBonusText( gameBonus.ToString() );
    }

    public void Activate(EndScreenType screen, float time ) {
        GameInput.Instance.SwitchToMap( ActionMapType.Dummy );
        PauseControl.SetPause( true );

        HandleScreenType( screen );
        HandleTime( time );

        Show();
    }

    private void HandleScreenType( EndScreenType screen ) {
        switch( screen ) {
            case EndScreenType.WinScreen: {
                SetEndText( "Wygra³eœ!" );
                SetEndTextColor( Color.green );

            } break;

            case EndScreenType.DeathScreen: {
                SetEndText( "Umar³eœ!" );
                SetEndTextColor( Color.red );
                gameBonusArea.SetActive( false );

            } break;
        }
    }

    private void HandleTime(float time) {
        float minutes = Mathf.Floor(time / 60);
        float seconds = Mathf.Floor(time % 60);

        string minutesString = AddZero( minutes );
        string secondsString = AddZero( seconds );

        SetTimeSurvivedText( minutesString + " : " + secondsString );
    }

    private string AddZero(float value) {
        string text = "";

        if ( value < 10 ) text = "0" + value.ToString();
        else text = value.ToString();

        return text;
    }

    private void SetTimeBonusText( string text ) { timeBonusText.text = text; }
    private void SetGameWonBonusText( string text ) { gameWonBonusText.text = text; }
    private void SetEndText( string text ) { endText.text = text; }
    private void SetEndTextColor( Color color ) { endText.color = color; }
    private void SetTimeSurvivedText( string text ) { timeSurvivedText.text = text; }
    private void Show() { endScreen.SetActive( true ); }
    private void Hide() { endScreen.SetActive( false ); }
}
