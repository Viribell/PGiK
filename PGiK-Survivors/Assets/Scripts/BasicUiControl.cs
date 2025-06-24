using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicUiControl : MonoBehaviour {
    public static BasicUiControl Instance { get; private set; }

    [field: SerializeField] private Image hpBar;
    [field: SerializeField] private Image xpBar;

    private void Awake() {
        if ( Instance == null ) { Instance = this; } else {
            Debug.Log( "There is more than one instance of BasicUiControl. Destroying the new one." );
            Destroy( gameObject );
            return;
        }
    }

    public void Init() {
        UpdateHP();
        UpdateXP();
    }

    public void UpdateHP() {
        float currHp = RefCacheControl.Instance.Player.EntityHealth.GetCurrentHealth();
        float maxHp = RefCacheControl.Instance.Player.EntityHealth.GetMaxHealth();

        hpBar.fillAmount = currHp / maxHp;
    }

    public void UpdateXP() {
        float curr = RefCacheControl.Instance.Player.PlayerLevel.GetCurrXP();
        float max = RefCacheControl.Instance.Player.PlayerLevel.GetRequiredXP();

        xpBar.fillAmount = curr / max;
    }
}
