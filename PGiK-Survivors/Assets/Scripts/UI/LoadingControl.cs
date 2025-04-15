using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingControl : MonoBehaviour {
    [field: SerializeField] private Image loadingBar;

    private bool isFirstUpdate = true;

    private void Update() {

        if ( isFirstUpdate ) {
            SceneLoader.Callback();
            isFirstUpdate = false;

        } else {
            loadingBar.fillAmount = SceneLoader.GetLoadingProgress();
        }
    }

}
