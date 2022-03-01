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
            if (!AssetFormatDefine.isTextureImportSetting) return;

            AssetFormatDefine.isTextureImportSetting = true;
            AssetFormatSettingData.SetTextureFileFormat(EditorTools.AbsolutePathToAssetPath(assetPath));
            AssetFormatDefine.isTextureImportSetting = false;
        }
    }

}