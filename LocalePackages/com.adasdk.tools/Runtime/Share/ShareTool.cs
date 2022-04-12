using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

#if UNITY_ANDROID
using NativeShareNamespace;
#endif

public static class ShareTool
{
	private static string PreviewsFolderPath => Application.persistentDataPath + "/Previews/";

	private static string PreviewsPath(string pName) => PreviewsFolderPath + pName + ".png";

	private static string GetPath(Sprite sprite)
	{
		if (!Directory.Exists(PreviewsFolderPath))
		{
			Directory.CreateDirectory(PreviewsFolderPath);
		}
		string path = PreviewsPath(sprite.name);

		if (Directory.Exists(path))
			return path;

		SaveImage(sprite);
		
		return path;
	}

	public static void SaveImage(Sprite sprite)
	{
		if (!Directory.Exists(PreviewsFolderPath))
		{
			Directory.CreateDirectory(PreviewsFolderPath);
		}
		string path = PreviewsPath(sprite.name);
		if (Directory.Exists(path))
			return;

		File.WriteAllBytes(path, sprite.texture.GetTexture(sprite.textureRect).EncodeToPNG());
	}

	public static void ShareImage(Sprite sprite)
	{
		string shareLink = "GooglePlay" + "\nhttps://play.google.com/store/apps/details?id=com.AdaIndieGames.ArtPuzzle";
		Share(GetPath(sprite), "", shareLink, "");
	}

	public static void UI_Share(Sprite sprite)
    {
		try
		{
			ShareImage(sprite);
		}
		catch (Exception ex)
		{
			const int ERROR_HANDLE_DISK_FULL = 0x27;
			const int ERROR_DISK_FULL = 0x70;

			int win32ErrorCode = System.Runtime.InteropServices.Marshal.GetHRForException(ex) & 0xFFFF;

			if (win32ErrorCode == ERROR_HANDLE_DISK_FULL || win32ErrorCode == ERROR_DISK_FULL)
			{
				HintsController.ShowText("DISK_FULL");
			}
			else
			{
				HintsController.ShowText("error");
			}
		}
	}

	#region NativeShare

	public enum ShareResult { Unknown = 0, Shared = 1, NotShared = 2 };

	public delegate void ShareResultCallback(ShareResult result, string shareTarget);

#if !UNITY_EDITOR && UNITY_ANDROID
	private static AndroidJavaClass m_ajc = null;
	private static AndroidJavaClass AJC
	{
		get
		{
			if( m_ajc == null )
				m_ajc = new AndroidJavaClass( "com.yasirkula.unity.NativeShare" );

			return m_ajc;
		}
	}

	private static AndroidJavaObject m_context = null;
	private static AndroidJavaObject Context
	{
		get
		{
			if( m_context == null )
			{
				using( AndroidJavaObject unityClass = new AndroidJavaClass( "com.unity3d.player.UnityPlayer" ) )
				{
					m_context = unityClass.GetStatic<AndroidJavaObject>( "currentActivity" );
				}
			}

			return m_context;
		}
	}
#endif

	static string[] targetPackages = new string[0];
	static string[] targetClasses = new string[0];

	private static void shareResultCallback(ShareResult result, string t)
    {

    }

	public static void Share(string filePath, string subject = "", string text = "", string title = "")
	{
		List<string> files = new List<string>(0);
		List<string> mimes = new List<string>(0);

		if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
		{
			files.Add(filePath);
			mimes.Add(string.Empty); //mime
		}

		ShareResultCallback callback = shareResultCallback;
#if UNITY_EDITOR
		Debug.Log("Shared!");
#elif UNITY_ANDROID
		AJC.CallStatic( "Share", Context, new NSShareResultCallbackAndroid( callback ), targetPackages, targetClasses, files.ToArray(), mimes.ToArray(), subject, text, title );
#endif
	}

	#endregion
}
