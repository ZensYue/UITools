using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
	[Serializable]
	public class AssetFormatTextureInfo: AssetFormatBaseInfo
	{
		public int maxTextureSize;
		public TextureImporterFormat textureImporterFormat;
		public TextureCompressionQuality textureCompressionQuality = TextureCompressionQuality.Normal;
	}

	/// <summary>
	/// 纹理格式设置
	/// </summary>
	[Serializable]
	public class AssetFormatTextureSetting: AssetFormatBaseSetting
	{
		public TextureImporterType textureType = TextureImporterType.Sprite;
		public SpriteImportMode spriteImportMode = SpriteImportMode.Single;
		public int maxTextureSize = 512;

		public List<AssetFormatTextureInfo> assetFormatTextureInfos = new List<AssetFormatTextureInfo>();
	}


}
