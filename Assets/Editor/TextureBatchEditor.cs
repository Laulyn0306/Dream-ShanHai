using UnityEditor;
using UnityEngine;

public class TextureBatchEditor : EditorWindow
{

    private int pixelsPerUnit = 32;
    private SpriteImportMode spriteMode = SpriteImportMode.Single;
    
    [MenuItem("Tools/批量设置贴图参数")]
    public static void ShowWindow()
    {
        GetWindow<TextureBatchEditor>("贴图批量设置");
    }

    private void OnGUI()
    {
        GUILayout.Label("批量修改选中贴图", EditorStyles.boldLabel);

        pixelsPerUnit = EditorGUILayout.IntField("Pixels Per Unit", pixelsPerUnit);
        spriteMode = (SpriteImportMode)EditorGUILayout.EnumPopup("Sprite Mode", spriteMode);

        if (GUILayout.Button("✨ 应用设置到选中贴图"))
        {
            ApplySettings();
        }


        void ApplySettings()
        {
            Object[] selectedTextures = Selection.GetFiltered(typeof(Texture2D), SelectionMode.Assets);

            if (selectedTextures.Length == 0)
            {
                Debug.LogWarning("⚠ 没有选中任何贴图喔！");
                return;
            }

            foreach (Object obj in selectedTextures)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

                if (importer != null)
                {
                    importer.textureType = TextureImporterType.Sprite;
                    importer.spriteImportMode = spriteMode;
                    importer.spritePixelsPerUnit = pixelsPerUnit;

                    // 额外设置，可自行添加
                    importer.mipmapEnabled = false;
                    importer.filterMode = FilterMode.Point;
                    importer.wrapMode = TextureWrapMode.Clamp;

                    EditorUtility.SetDirty(importer);
                    importer.SaveAndReimport();

                    Debug.Log($"✅ {path} 已设置：Mode = {spriteMode} | PPU = {pixelsPerUnit}");
                }
            }

            AssetDatabase.Refresh();
            Debug.Log("🎉 批量设置完成啦，么么哒～💋");
        }
    }
}

