using System.IO;
using UnityEditor;
using UnityEngine;

public sealed class ScriptAssetKeywordsreplacer : UnityEditor.AssetModificationProcessor
{
    public static void OnWillCreateAsset(string path)
    {
        path = path.Replace(".meta", string.Empty);

        if(!path.EndsWith(".cs"))
        {
            return;
        }

        var systemPath = path.Insert(0, Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("Assets")));

        ReplaceScriptKeyowrds(systemPath, path);

        AssetDatabase.Refresh();
    }

    private static void ReplaceScriptKeyowrds(string systemPath, string projectPath)
    {
        // 戦闘のAssets/だけ削除する
        int index = projectPath.IndexOf("/") + 1;
        projectPath = projectPath.Substring(index, projectPath.Length - index);
        projectPath = projectPath.Substring(0, projectPath.LastIndexOf("/"));
        // /Scriptsは変換
        projectPath = projectPath.Replace("/Scripts", "").Replace('/', '.');

        var rootNamespace = string.IsNullOrWhiteSpace(EditorSettings.projectGenerationRootNamespace) ? 
        string.Empty : $"{EditorSettings.projectGenerationRootNamespace}.";

        var fullNamespace = $"{rootNamespace}{projectPath}";

        var fileData = File.ReadAllText(systemPath);

        fileData = fileData.Replace("#NAMESPACE#", fullNamespace);
        
        File.WriteAllText(systemPath, fileData);
    }
}