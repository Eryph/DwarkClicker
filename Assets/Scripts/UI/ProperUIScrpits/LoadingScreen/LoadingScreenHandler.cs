using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenHandler : MonoBehaviour
{
    private AsyncOperation _asyncOp = null;
    private bool _isLoading = false;


    public void Load(AsyncOperation asyncOp)
    {
        _asyncOp = asyncOp;
        _isLoading = true;
    }

    private void Update()
    {
        if (_isLoading)
        {
            if (_asyncOp.progress == 0.9)
                SceneManager.UnloadSceneAsync("Preload");
        }
    }
}
