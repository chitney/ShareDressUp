using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesLoader : MonoBehaviour
{
    [SerializeField]
    private string[] scenes = new string[0];
    void Awake()
    {
        foreach (string scene in scenes)
        {
            SceneManager.LoadScene(scene, LoadSceneMode.Additive);
        }
    }
}
