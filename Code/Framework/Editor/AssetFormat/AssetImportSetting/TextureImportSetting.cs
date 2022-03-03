using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    public class TextureImportSetting : AssetPostprocessor
    {
        public void OnPostprocessTexture(Texture2D texture)
        {
            if (!AssetFormatDefine.NeedTextureImportSetting) return;

            AssetFormatDefine.isTextureImporting = true;
            AssetFormatSettingData.SetTextureFileFormat(EditorTools.AbsolutePathToAssetPath(assetPath));
            AssetFormatDefine.isTextureImporting = false;
        }
    }

}