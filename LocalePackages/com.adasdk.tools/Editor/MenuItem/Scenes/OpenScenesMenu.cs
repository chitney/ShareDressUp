using UnityEditor;
using UnityEditor.SceneManagement;

public static class OpenScenesMenu
{
    [MenuItem("File/Open scene/game", false, 0)]
    public static void OpenStart()
    {
        OpenScene("game");
    }

    [MenuItem("File/Open scene/ui", false, 0)]
    public static void OpenMap()
    {
        OpenScene("ui");
    }

    private static void OpenScene(string name)
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            return;
        EditorSceneManager.OpenScene("Assets/Scenes/" + name + ".unity");
    }

}