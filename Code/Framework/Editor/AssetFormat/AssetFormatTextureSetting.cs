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
		public TextureImporterNPOTScale npotScale = TextureImporterNPOTScale.None;
		public TextureWrapMode wrapMode = TextureWrapMode.Clamp;
		public FilterMode filterMode = FilterMode.Bilinear;

		public bool streamingMipmaps = false;
		public bool mipmapEnabled = false;
		public bool borderMipmap = false;
		public TextureImporterMipFilter mipmapFilter = TextureImporterMipFilter.BoxFilter;
		public bool mipMapsPreserveCoverage = false;
		public bool fadeout = false;
		public int anisoLevel = 1;

		public List<AssetFormatTextureInfo> assetFormatTextureInfos = new List<AssetFormatTextureInfo>();
	}


}
