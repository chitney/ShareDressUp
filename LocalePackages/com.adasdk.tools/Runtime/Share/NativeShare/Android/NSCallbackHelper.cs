#if UNITY_EDITOR || UNITY_ANDROID
using UnityEngine;

namespace NativeShareNamespace
{
	public class NSCallbackHelper : MonoBehaviour
	{
		public ShareTool.ShareResultCallback callback;

		private ShareTool.ShareResult result = ShareTool.ShareResult.Unknown;
		private string shareTarget = null;

		private bool resultReceived;

		private void Awake()
		{
			DontDestroyOnLoad( gameObject );
		}

		private void Update()
		{
			if( resultReceived )
			{
				resultReceived = false;

				try
				{
					if( callback != null )
						callback( result, shareTarget );
				}
				finally
				{
					Destroy( gameObject );
				}
			}
		}

		private void OnApplicationFocus( bool focus )
		{
			if( focus )
			{
				// Share sheet is closed and now Unity activity is running again. Send Unknown result if OnShareCompleted wasn't called
				resultReceived = true;
			}
		}

		public void OnShareCompleted( int resultRaw, string shareTarget )
		{
			ShareTool.ShareResult shareResult = (ShareTool.ShareResult) resultRaw;

			if( result == ShareTool.ShareResult.Unknown )
			{
				result = shareResult;
				this.shareTarget = shareTarget;
			}
			else if( result == ShareTool.ShareResult.NotShared )
			{
				if( shareResult == ShareTool.ShareResult.Shared )
				{
					result = ShareTool.ShareResult.Shared;
					this.shareTarget = shareTarget;
				}
				else if( shareResult == ShareTool.ShareResult.NotShared && !string.IsNullOrEmpty( shareTarget ) )
					this.shareTarget = shareTarget;
			}
			else
			{
				if( shareResult == ShareTool.ShareResult.Shared && !string.IsNullOrEmpty( shareTarget ) )
					this.shareTarget = shareTarget;
			}

			resultReceived = true;
		}
	}
}
#endif