using System;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UserInterface
{
    public class MainScreen : MonoBehaviour
    {
        private CancellationTokenSource _cts;
        
        [SerializeField]
        private TMP_Text pressMessage;

        private float _currentBlink;
        private Color _colorText = Color.white;
        private bool _isLoading;
        
        
        private async void LoadNewScene( string sceneName )
        {
            if ( _cts == null )
            {
                _cts = new CancellationTokenSource();
                try
                {
                    await PerformSceneLoading(_cts.Token, sceneName);
                }
                catch ( OperationCanceledException ex )
                {
                    if ( ex.CancellationToken == _cts.Token )
                    {
                        Debug.Log("Task Cancelled");
                    }
                }
                finally
                {
                    _cts.Cancel();
                    _cts = null;
                }
            }
            else
            {
                _cts.Cancel();
                _cts = null;
            }
        }

        private async Task PerformSceneLoading( CancellationToken token, string sceneName )
        {
            token.ThrowIfCancellationRequested();
            if ( token.IsCancellationRequested )
                return;

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            asyncOperation.allowSceneActivation = false;
            while ( true )
            {
                token.ThrowIfCancellationRequested();
                if ( token.IsCancellationRequested )
                    return;
                if(asyncOperation.progress >= 0.9f)
                    break;
            }

            asyncOperation.allowSceneActivation = true;
            _cts.Cancel();
            token.ThrowIfCancellationRequested();

            if ( token.IsCancellationRequested )
                return;
        }
        
        private void Update()
        {
            if ( _isLoading ) return;
            
            _colorText.a = Mathf.PingPong(Time.time, 1);
            pressMessage.color = _colorText;

            if ( !Keyboard.current.enterKey.wasPressedThisFrame ) return;
            _isLoading = true;

            LoadNewScene("Arena");
        }
    }
}