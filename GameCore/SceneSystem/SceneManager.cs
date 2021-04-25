using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoSingleton<ScenesManager>, IManager
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init()
    {

    }

    public void Release()
    {

    }

    public void LoadScene(string sceneName, Action finishCallback = null)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName, finishCallback));
        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode sceneMode) =>
        {
            if (finishCallback != null)
                finishCallback.Invoke();
        };
    }

    IEnumerator LoadSceneCoroutine(string sceneName, Action finishCallback)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        yield return null;
        op.allowSceneActivation = true;
    }
}
