using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    public class AudioClipImportSetting : AssetPostprocessor
    {
        public void OnPreprocessAudio()
        {
            if (!AssetFormatDefine.NeedAudioClipImportSetting) return;

            AssetFormatDefine.isAudioClipImporting = true;
            AssetFormatSettingData.SetAudioClipFileFormat(EditorTools.AbsolutePathToAssetPath(assetPath));
            AssetFormatDefine.isAudioClipImporting = false;
        }
    }

}