using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
    #region SceneMembers

    public enum Scene {
        Undefined,
        LoadingScene,
        MainMenuScene,
        LobbyScene,
        ForestLevel,
        CemeteryLevel,
        DesertLevel,
        TundraLevel,
        CaveLevel,
        TestingScene
    }

    public static Scene prevScene;
    public static Scene nextScene;

    private static Scene ActiveScene { get {
            Scene active;

            return Enum.TryParse( SceneManager.GetActiveScene().name, false, out active ) ? active : Scene.Undefined;
    } }

    #endregion

    private static Action OnLoaderCallback;

    private static AsyncOperation loading;

    public static void Load( Scene scene, bool useLoadingScene = true) {
        OnLoaderCallback = () => {
            GameObject loadingObject = new GameObject("Loading...");
            loadingObject.AddComponent<SceneLoader>().StartCoroutine( AsyncLoadRoutine( scene ) );
        };

        if ( useLoadingScene ) LoadScene( Scene.LoadingScene );
        else Callback();
    }

    private static IEnumerator AsyncLoadRoutine( Scene scene ) {
        yield return null;

        loading = LoadSceneAsync( scene );

        while ( !loading.isDone ) {
            yield return null;
        }
    }

    public static float GetLoadingProgress(bool returnClamped = true) {
        if ( loading == null ) return 1.0f;

        float progress = Mathf.Clamp01( loading.progress / 0.9f );

        return returnClamped ? progress : progress * 100.0f;

    }

    public static void Callback() {
        if ( OnLoaderCallback == null ) return;

        OnLoaderCallback();
        OnLoaderCallback = null;
    }


    private static void Update(Scene target) {
        prevScene = ActiveScene;
        nextScene = target;
    }

    public static void LoadScene( Scene scene ) { Update( scene );  SceneManager.LoadScene( scene.ToString() ); }
    public static AsyncOperation LoadSceneAsync( Scene scene ) { Update( scene ); return SceneManager.LoadSceneAsync( scene.ToString() ); }

    public static void ReloadActive() { LoadScene( ActiveScene ); }
    public static AsyncOperation ReloadActiveAsync() { return LoadSceneAsync( ActiveScene ); }
}
