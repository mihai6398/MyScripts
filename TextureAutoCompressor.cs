using System.Collections;
using System.Collections.Generic;
using UnityEngine;

# if UNITY_EDITOR
using UnityEditor;
using Unity.EditorCoroutines.Editor;

public class TextureAutoCompressor : EditorWindow
{
    public TextureImporterFormat compressType;
    int texturesFound;
    int texturesNew;
    UnityEngine.Rendering.Universal.ShadowResolution textureMaxSize;
    bool forceCompressAll;

    [MenuItem("MidnightTools/TextureCompressor")]
    static void Init()
    {
        TextureAutoCompressor window =
            (TextureAutoCompressor)EditorWindow.GetWindow(typeof(TextureAutoCompressor));
    }

    void OnGUI()
    {

        EditorGUILayout.BeginVertical();


        forceCompressAll = EditorGUI.Toggle(new Rect(0, 35, position.width, 15), "Force Compress All: ", forceCompressAll);

        textureMaxSize = (UnityEngine.Rendering.Universal.ShadowResolution)EditorGUILayout.EnumPopup(textureMaxSize);

        EditorGUILayout.Space(80);

        compressType = (TextureImporterFormat)EditorGUILayout.EnumPopup(compressType);

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Textures Found: ", texturesFound + "");

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Textures Changed: ", texturesNew + "");

        EditorGUILayout.Space(5);

        if (GUILayout.Button("Compress"))
        {
            EditorCoroutineUtility.StartCoroutine(StartCompressing(), this);
        }

        EditorGUILayout.EndVertical();
    }


    IEnumerator StartCompressing()
    {
        yield return new WaitForSeconds(0.1f);
        Compress();
    }


    public void Compress()
    {
        System.GC.Collect();
        EditorUtility.UnloadUnusedAssetsImmediate();
        AssetDatabase.Refresh();

        var textures = AssetDatabase.FindAssets("t:texture", null);

        texturesFound = textures.Length;
        texturesNew = 0;

        foreach (string guid in textures)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
            if (textureImporter)
            {

                TextureImporterFormat temp;
                int currentSize = 0;
                int compressionQuality = 0;

                var canRead = textureImporter.GetPlatformTextureSettings("iPhone", out currentSize, out temp, out compressionQuality);

                if (textureImporter.textureType != TextureImporterType.Sprite)
                    if (!textureImporter.assetPath.Contains("Lightmap")
                         && !textureImporter.assetPath.Contains("ReflectionProbe")
                            && !textureImporter.assetPath.Contains("Packages/"))
                    {
                        if (canRead)
                        {

                            //if (textureImporter.textureType == TextureImporterType.Default || textureImporter.textureShape == TextureImporterShape.TextureCube)
                            //{

                            TextureImporterPlatformSettings settingsToCopy = textureImporter.GetPlatformTextureSettings("iPhone");

                            if (!settingsToCopy.overridden)
                            {
                                settingsToCopy.overridden = true;
                            }

                            bool needCompress = false;

                            if (temp != compressType)
                            {
                                needCompress = true;
                            }

                            if (currentSize != (int)textureMaxSize)
                            {
                                needCompress = true;
                            }

                            var compQuality = (int)settingsToCopy.compressionQuality;

                            if (compQuality != 100)
                            {
                                Debug.Log(compQuality + " != 100");
                                needCompress = true;
                            }

                            //if (forceCompressAll)
                            //{
                            //    needCompress = true;
                            //}

                            if (needCompress)
                            {
                                Debug.Log("NEED COMPRESS: " + needCompress);

                                texturesNew++;
                                //textureImporter.textureType = TextureImporterType.NormalMap;
                                //textureImporter.npotScale = TextureImporterNPOTScale.None;

                                settingsToCopy.format = compressType;
                                //TextureCompressionQuality.Best;
                                settingsToCopy.textureCompression = TextureImporterCompression.CompressedHQ;
                                settingsToCopy.maxTextureSize = (int)textureMaxSize;

                                textureImporter.SetPlatformTextureSettings(settingsToCopy);

                                //textureImporter.wrapMode = TextureWrapMode.Clamp;
                                //textureImporter.isReadable = true;
                                //textureImporter.mipmapEnabled = false;
                                AssetDatabase.ImportAsset(path);
                                Debug.Log(textureImporter.assetPath);
                            }
                            //}
                            //else if (textureImporter.textureType == TextureImporterType.NormalMap)
                            //{

                            //}
                        }
                        else
                        {
                            Debug.Log("Cant Read: " + textureImporter.assetPath);
                            TextureImporterPlatformSettings settingsToCopy = textureImporter.GetPlatformTextureSettings("iPhone");

                            settingsToCopy.overridden = true;
                            settingsToCopy.format = compressType;
                            settingsToCopy.compressionQuality = 100; //TextureCompressionQuality.Best;
                            settingsToCopy.maxTextureSize = (int)textureMaxSize;

                            textureImporter.SetPlatformTextureSettings(settingsToCopy);

                            Debug.Log("Enabled override for: " + textureImporter.assetPath);
                            AssetDatabase.ImportAsset(path);
                        }
                    }
            }
        }


        Debug.Log("Compressed " + texturesNew + " Textures");

    }
}




# endif