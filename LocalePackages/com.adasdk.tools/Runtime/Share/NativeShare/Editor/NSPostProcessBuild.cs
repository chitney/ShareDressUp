using System.IO;
using UnityEditor;
using UnityEngine;
#if UNITY_IOS
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
#endif

namespace NativeShareNamespace
{
	[System.Serializable]
	public class Settings
	{
		private const string SAVE_PATH = "ProjectSettings/NativeShare.json";

		public bool AutomatedSetup = true;
		public string PhotoLibraryUsageDescription = "The app requires access to Photos to save media to it.";

		private static Settings m_instance = null;
		public static Settings Instance
		{
			get
			{
				if( m_instance == null )
				{
					try
					{
						if( File.Exists( SAVE_PATH ) )
							m_instance = JsonUtility.FromJson<Settings>( File.ReadAllText( SAVE_PATH ) );
						else
							m_instance = new Settings();
					}
					catch( System.Exception e )
					{
						Debug.LogException( e );
						m_instance = new Settings();
					}
				}

				return m_instance;
			}
		}

		public void Save()
		{
			File.WriteAllText( SAVE_PATH, JsonUtility.ToJson( this, true ) );
		}
	}

}