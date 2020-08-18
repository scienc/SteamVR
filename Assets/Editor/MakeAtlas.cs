using System.IO;
using UnityEditor;
using UnityEngine;

public class AtlasEditor {
    [MenuItem ("Assets/AtlasMaker", false, 10)]
    static private void MakeAtlas () {
        EditorSettings.serializationMode = SerializationMode.ForceText;
        string path = AssetDatabase.GetAssetPath (Selection.activeObject);
        if (!path.Contains ("UIAtlas/"))
            return;
        string[] dataFile = path.Split ('/');
        string DirectoryFile = dataFile[dataFile.Length - 1];
        string spriteDir = Application.dataPath + "/Resources/UIAtlas/" + DirectoryFile;
        if (!Directory.Exists (spriteDir)) {
            Directory.CreateDirectory (spriteDir);
        }
        DirectoryInfo rootDirInfo = new DirectoryInfo (Application.dataPath + "/UI/UIAtlas/" + DirectoryFile);
        Debug.Log (Application.dataPath + "/UI/UIAtlas/" + DirectoryFile);
        foreach (FileInfo pngFile in rootDirInfo.GetFiles ("*.png", SearchOption.AllDirectories)) {
            string allPath = pngFile.FullName;
            string assetPath = allPath.Substring (allPath.IndexOf ("Assets"));
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite> (assetPath);
            GameObject go = new GameObject (sprite.name);
            go.AddComponent<SpriteRenderer> ().sprite = sprite;
            allPath = spriteDir + "/" + sprite.name + ".prefab";
            string prefabPath = allPath.Substring (allPath.IndexOf ("Assets"));
            bool isSuccess = false;
            PrefabUtility.SaveAsPrefabAsset (go, prefabPath, out isSuccess);
            Debug.Log (prefabPath);
            Debug.Log ("Create Prefab " + isSuccess);
            GameObject.DestroyImmediate (go);
        }
    }
}