using SimpleJSON;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Window for create new package
/// </summary>
public class CreatePackageTools : EditorWindow
{
    /// <summary>
    /// path SDK folder
    /// </summary>
    private static string sdkPath = "C:/Users/chitney/Documents/GIT/AdaSDK/";
    private static string companyName = "AdaSDK";
    private static string editorFolderName = "Editor";
    private static string runtimeFolderName = "Runtime";
    /// <summary>
    /// path Unity/Packages/manifest.json
    /// </summary>
    private static string unityPackagesManifestPath = Directory.GetCurrentDirectory() + "/Packages/manifest.json";
    /// <summary>
    /// current manifest node
    /// </summary>
    private static JSONNode unityManifest;
    /// <summary>
    /// current packages list
    /// </summary>
    private static JSONObject dependencies;
    /// <summary>
    /// all packages in SDK folder
    /// </summary>
    private static List<string> packagesInFolder;
   [MenuItem("Window/CreatePackageTools")]
    public static void ShowWindow()
    {
        GetWindow(typeof(CreatePackageTools));
    }

    string inputName = "";
    string packageName = "";
    string displayName = "";
    string descriptionText = "";
    bool createEditorFolder = true;
    bool createRuntimeFolder = true;

    private void OnEnable()
    {
        unityManifest = ReadUnityManifest();
        dependencies = unityManifest["dependencies"].AsObject;
        packagesInFolder = new List<string>(Directory.GetDirectories(sdkPath, "com.*", SearchOption.TopDirectoryOnly));
    }

    /// <summary>
    /// delete package folder
    /// </summary>
    /// <param name="name"></param>
    private void DeletePackage(string path, string name)
    {
        if (EditorUtility.DisplayDialog("Attention",
                "Are you sure you want to remove " + name +"?", "Yes", "Cancel"))
        {
            if (dependencies.HasKey(name))
            {
                RemovePackageIntoManifest(name);
            }
            Directory.Delete(path, true);
            OnEnable();
        }
    }

    /// <summary>
    /// create package mode
    /// </summary>
    private bool createMode = false;
    private GUIStyle guiStyle = new GUIStyle();
    void OnGUI()
    {
        if (createMode)
        {
            inputName = EditorGUILayout.TextField("Package Name ", inputName);

            bool checkName = StringExtension.CheckString("^[a-zA-Z0-9_-]+$", inputName);

            if (checkName)
            {
                packageName = "com." + companyName.ToLower() + "." + inputName.ToLower();
                bool include = dependencies.HasKey(inputName) || Directory.Exists(Path.Combine(sdkPath, packageName));

                if (include)
                    GUILayout.Label("Package exists", GUIEditorTools.colorGUIStyle(guiStyle,Color.red));

               
                GUILayout.Label("Package: " + packageName, EditorStyles.boldLabel);
                displayName = companyName + "." + inputName;
                GUILayout.Label("DisplayName: " + displayName);
                descriptionText = EditorGUILayout.TextField("descriptionText", descriptionText);

                EditorGUILayout.BeginHorizontal();
                createEditorFolder = GUILayout.Toggle(createEditorFolder, "Editor folder");
                createRuntimeFolder = GUILayout.Toggle(createRuntimeFolder, "Runtime folder");
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.Label("The name contains forbidden symbols", GUIEditorTools.colorGUIStyle(guiStyle, Color.red));
            }

            EditorGUILayout.BeginHorizontal();

            if (checkName && GUILayout.Button("Add"))
            {
                CreatePackage();
                createMode = false;
            }

            if (GUILayout.Button("Cancel"))
                createMode = false;

            EditorGUILayout.EndHorizontal();
        }

        if (!createMode && GUILayout.Button("Create package"))
            createMode = true;

        GUIEditorTools.HorizontalLine(Color.black);
        GUILayout.Label("AdaSDK packages", EditorStyles.boldLabel);
        foreach (string _package in packagesInFolder)
        {
            EditorGUILayout.BeginHorizontal();
            string name = _package.Replace(sdkPath, "");
            bool include = dependencies.HasKey(name);
            bool toggle = GUILayout.Toggle(include, name);
            if (include!=toggle)
            {
                if (toggle)
                    AddPackageIntoManifest(name, _package);

                else RemovePackageIntoManifest(name);
                OnEnable();
            }

            GUIEditorTools.DrawRemoveBtn(() => DeletePackage(_package, name));
            EditorGUILayout.EndHorizontal();
        }
    }

    private void CreatePackage()
    {
        //create folders 
        string packagePath = Path.Combine(sdkPath, packageName);
        Directory.CreateDirectory(packagePath);

        //create package.json
        JSONObject package = new JSONObject();
        package.Add("name", packageName);
        package.Add("displayName", displayName);
        package.Add("version", "1.0.0");
        package.Add("unity", "2020.1");
        package.Add("description", descriptionText);
        package.Add("hideInEditor", false);
        string json = package.ToString(1);

        //write package.json
        FileStream file = File.Create(packagePath + "/package.json");
        byte[] byteArray = new UTF8Encoding(true).GetBytes(json);
        file.Write(byteArray,0, byteArray.Length);
        file.Close();

        //create and write README.md
        file = File.Create(packagePath + "/README.md");
        byteArray = new UTF8Encoding(true).GetBytes("# "+packageName);
        file.Write(byteArray, 0, byteArray.Length);
        file.Close();

        //create asmdef
        string[] emptyArray = new string[0];
        JSONObject asmdef = new JSONObject();
        asmdef.Add("name", "");
        asmdef.Add("rootNamespace", "");
        asmdef.Add("references", emptyArray);
        asmdef.Add("includePlatforms", emptyArray);
        asmdef.Add("excludePlatforms", emptyArray);
        asmdef.Add("allowUnsafeCode", false);
        asmdef.Add("overrideReferences", false);
        asmdef.Add("precompiledReferences", emptyArray);
        asmdef.Add("autoReferenced", true);
        asmdef.Add("defineConstraints", emptyArray);
        asmdef.Add("versionDefines", emptyArray);
        asmdef.Add("noEngineReferences", false);

        if (createEditorFolder)
        {
            string pathEditorFolder = Path.Combine(packagePath, editorFolderName);
            Directory.CreateDirectory(pathEditorFolder);
           
            //create Editor.asmdef
            asmdef["name"] = displayName + ".Editor";
            asmdef["includePlatforms"] = new string[1] { "Editor" };
            string jsonAsmdefEditor = asmdef.ToString(1);

            //write Editor.asmdef
            file = File.Create(pathEditorFolder + "/" + displayName + ".Editor.asmdef");
            byteArray = new UTF8Encoding(true).GetBytes(jsonAsmdefEditor);
            file.Write(byteArray, 0, byteArray.Length);
            file.Close();
        }

        if (createRuntimeFolder || !createRuntimeFolder&&!createEditorFolder)
        {
            string pathRuntimeFolder = createRuntimeFolder? Path.Combine(packagePath, runtimeFolderName) : packagePath;
            Directory.CreateDirectory(pathRuntimeFolder);

            //create Runtime.asmdef
            asmdef["name"] = displayName;
            asmdef["includePlatforms"] = emptyArray;
            string jsonAsmdef = asmdef.ToString(1);


            //write Runtime.asmdef
            file = File.Create(pathRuntimeFolder + "/" + displayName + ".asmdef");
            byteArray = new UTF8Encoding(true).GetBytes(jsonAsmdef);
            file.Write(byteArray, 0, byteArray.Length);
            file.Close();
        }

        //add into manifest
        AddPackageIntoManifest(packageName, packagePath);
        Debug.Log("Creation of the " + packageName + " is completed");
    }

    /// <summary>
    /// get unity manifest 
    /// </summary>
    private static JSONNode ReadUnityManifest()
    {
        string lines = File.ReadAllText(unityPackagesManifestPath);
        return JSONNode.Parse(lines);
    }

    /// <summary>
    /// save unity manifest 
    /// </summary>
    private static void WriteUnityManifest(string json)
    {
        File.WriteAllText(unityPackagesManifestPath, json);
    }

    /// <summary>
    /// Add package into Unity/Packages/manifest.json
    /// </summary>
    private static void AddPackageIntoManifest(string packageName, string path)
    {
        unityManifest = ReadUnityManifest();
        dependencies = unityManifest["dependencies"].AsObject;
        dependencies.Add(packageName, "file:" + path);
        unityManifest.Clear();
        unityManifest.Add("dependencies", dependencies);

        WriteUnityManifest(unityManifest.ToString(1));

        Debug.Log("Added package " + packageName);
    }

    /// <summary>
    /// Remove package from Unity/Packages/manifest.json
    /// </summary>
    private static void RemovePackageIntoManifest(string packageName)
    {
        unityManifest = ReadUnityManifest();
        dependencies = unityManifest["dependencies"].AsObject;
        dependencies.Remove(packageName);
        unityManifest.Clear();
        unityManifest.Add("dependencies", dependencies);

        WriteUnityManifest(unityManifest.ToString(1));

        Debug.Log("Removed package " + packageName);
    }

}
