using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Editor
{
	public class AssetFormatSetting : ScriptableObject
	{

		public List<AssetFormatTextureSetting> assetFormatTextureSettings = new List<AssetFormatTextureSetting>();

		public List<AssetFormatAudioSetting> assetFormatAudioSettings = new List<AssetFormatAudioSetting>();
	}
}

