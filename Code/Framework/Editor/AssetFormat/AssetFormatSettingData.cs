using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
	public static class AssetFormatSettingData
	{
		private static AssetFormatSetting _setting = null;

		private static bool isInitPath = false;

		public static AssetFormatSetting Setting
		{
			get
			{
				if (_setting == null)
				{
					LoadSettingData();
				}
				return _setting;
			}
		}

		private static void LoadSettingData()
        {
			InitCachePath();

			// 加载配置文件
			LoadSettingData(AssetFormatDefine.AssetFormatSettingFilePath);
		}

		private static void InitCachePath()
        {
			if (isInitPath) return;
			isInitPath = true;
			AssetFormatDefine.AssetFormatSettingFilePath = EditorPrefs.GetString(AssetFormatDefine.AssetFormatSettingFilePathKey, AssetFormatDefine.AssetFormatSettingFilePath);
		}

		public static void LoadSettingData(string filePath)
        {
			if (string.IsNullOrEmpty(filePath))
				return;
			if (!File.Exists(filePath))
				return;

			_setting = AssetDatabase.LoadAssetAtPath<AssetFormatSetting>(filePath);
			if(_setting && filePath != AssetFormatDefine.AssetFormatSettingFilePath)
            {
				EditorPrefs.SetString(AssetFormatDefine.AssetFormatSettingFilePathKey, filePath);
				AssetFormatDefine.AssetFormatSettingFilePath = filePath;
			}
		}

		public static void CreateSetting(string fileName)
        {
			Debug.LogWarning($"Create new {nameof(AssetFormatSetting)}.asset : {AssetFormatDefine.AssetFormatSettingFilePath}");
            var new_setting = ScriptableObject.CreateInstance<AssetFormatSetting>();
            EditorTools.CreateFileDirectory(fileName);
            AssetDatabase.CreateAsset(new_setting, fileName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

			LoadSettingData(fileName);
		}

		public static void SaveSetting()
        {
			if(Setting != null)
            {
				EditorUtility.SetDirty(Setting);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();

				Debug.Log($"Save {nameof(AssetFormatSetting)} file");
			}
		}

		public static void AddFormatSetting(AssetFormatType assetFormatType,string dir)
        {
			if (Setting == null) return;
			dir = EditorTools.AbsolutePathToAssetPath(dir);
			switch (assetFormatType)
			{
				case AssetFormatType.Texture:

					if (!ContainsDirectory(Setting.assetFormatTextureSettings, dir))
						{
						AssetFormatTextureSetting assetFormatTextureSetting = new AssetFormatTextureSetting()
						{
							directory = dir
						};

						Setting.assetFormatTextureSettings.Add(assetFormatTextureSetting);
					}
					break;
				case AssetFormatType.AudioClip:
					if (!ContainsDirectory(Setting.assetFormatAudioSettings,dir))
					{
						AssetFormatAudioSetting assetFormatAudioSetting = new AssetFormatAudioSetting()
						{
							directory = dir
						};

						AssetFormatAudioInfo assetFormatAudioInfo = new AssetFormatAudioInfo
						{
							isDefalut = true,
						};
						assetFormatAudioSetting.assetFormatAudioInfos.Add(assetFormatAudioInfo);
						Setting.assetFormatAudioSettings.Add(assetFormatAudioSetting);
					}
					break;
				default:
					break;
			}
		}

		public static bool ContainsDirectory<T>(List<T> list,string dir) where T: AssetFormatBaseSetting
		{
            foreach (var item in list)
            {
				if (item.directory == dir) return true;
            }
			return false;
        }

		public static T GetFileBestSetting<T>(List<T> list, string file) where T : AssetFormatBaseSetting
		{
			file = EditorTools.AbsolutePathToAssetPath(file);
			string bestDir = "";
			T t = null;
			foreach (var assetSetting in list)
			{
				// 满足条件的路径，越长越优
				if (file.Contains(assetSetting.directory) && assetSetting.directory.Length > bestDir.Length)
                {
					bestDir = assetSetting.directory;
					t = assetSetting;
				}
			}
			return t;
		}

		public static T AddTextureFormat<T>(List<T> assetFormatTextureInfos) where T:AssetFormatBaseInfo,new()
        {
			foreach (AssetFormatPlatform platform in System.Enum.GetValues(typeof(AssetFormatPlatform)))
			{
				if (!ContainPlatform(assetFormatTextureInfos, platform))
				{
					T assetFormatTextureInfo = new T()
					{
						platform = platform
					};
					assetFormatTextureInfos.Add(assetFormatTextureInfo);
					return assetFormatTextureInfo;
				}
			}
			return null;
		}

		/// <summary>
		/// 是否能改平台
		/// </summary>
		/// <param name="assetFormatTextureInfos"></param>
		/// <param name="assetFormatPlatform"></param>
		/// <param name="assetFormatTextureInfo">忽略查找对象</param>
		/// <returns></returns>
		public static bool ContainPlatform<T>(List<T> assetFormatTextureInfos, AssetFormatPlatform assetFormatPlatform, T assetFormatTextureInfo = null) where T : AssetFormatBaseInfo
		{
            foreach (var item in assetFormatTextureInfos)
            {
				if (!item.isDefalut && item != assetFormatTextureInfo && item.platform == assetFormatPlatform) return true;
            }
			return false;
        }

		public static bool ChangePlatform<T>(List<T> assetFormatTextureInfos,T assetFormatTextureInfo, AssetFormatPlatform assetFormatPlatform) where T: AssetFormatBaseInfo
		{
			if (assetFormatTextureInfo.platform == assetFormatPlatform) return false;

			if(!ContainPlatform(assetFormatTextureInfos,assetFormatPlatform,assetFormatTextureInfo))
            {
				assetFormatTextureInfo.platform = assetFormatPlatform;
				return true;
			}
			return false;
        }


		public static void SetAssetsFormat(AssetFormatType assetFormatType)
        {
			if (Setting == null)
				return;

			if (assetFormatType == AssetFormatType.Texture)
            {
                foreach (var item in Setting.assetFormatTextureSettings)
                {
					SetAssetsFormat(item);
				}
			} else if (assetFormatType == AssetFormatType.AudioClip) {
				foreach (var item in Setting.assetFormatAudioSettings)
				{
					SetAssetsFormat(item);
				}
			}

			AssetDatabase.Refresh();
		}

		public static void SetAssetsFormat(AssetFormatTextureSetting assetFormatTextureSetting)
        {
			var files = GetAssetFiles(assetFormatTextureSetting.directory, AssetFormatType.Texture);
            foreach (var file in files)
            {
				// 需要用文件路径去拿最优设置
				SetTextureFileFormat(file);
			}
		}

		public static void SetTextureFileFormat(string file)
        {
			if (Setting == null) 
				return;
			var formatSetting = GetFileBestSetting(Setting.assetFormatTextureSettings, file);
			if (formatSetting == null)
				return;
			SetTextureFileFormat(formatSetting, file);
		}

		public static void SetTextureFileFormat(AssetFormatTextureSetting assetFormatTextureSetting,string file)
        {
			bool isDirty = false;
			TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(file);

			if (importer.textureType != assetFormatTextureSetting.textureType)
			{
				importer.textureType = assetFormatTextureSetting.textureType;
				isDirty = true;
			}
			if (importer.spriteImportMode != assetFormatTextureSetting.spriteImportMode)
			{
				importer.spriteImportMode = assetFormatTextureSetting.spriteImportMode;
				isDirty = true;
			}
			if (importer.maxTextureSize != assetFormatTextureSetting.maxTextureSize)
			{
				importer.maxTextureSize = assetFormatTextureSetting.maxTextureSize;
				isDirty = true;
			}

			if (importer.npotScale != assetFormatTextureSetting.npotScale)
			{
				importer.npotScale = assetFormatTextureSetting.npotScale;
				isDirty = true;
			}

			if (importer.wrapMode != assetFormatTextureSetting.wrapMode)
			{
				importer.wrapMode = assetFormatTextureSetting.wrapMode;
				isDirty = true;
			}

			
			if (importer.filterMode != assetFormatTextureSetting.filterMode)
			{
				importer.filterMode = assetFormatTextureSetting.filterMode;
				isDirty = true;
			}

            if (importer.isReadable)
            {
				importer.isReadable = false;
				isDirty = true;
			}

			if (importer.streamingMipmaps != assetFormatTextureSetting.streamingMipmaps)
			{
				importer.streamingMipmaps = assetFormatTextureSetting.streamingMipmaps;
				isDirty = true;
			}

			if (importer.mipmapEnabled != assetFormatTextureSetting.mipmapEnabled)
			{
				importer.mipmapEnabled = assetFormatTextureSetting.mipmapEnabled;
				isDirty = true;
			}

			if(assetFormatTextureSetting.mipmapEnabled)
            {
				if (importer.borderMipmap != assetFormatTextureSetting.borderMipmap)
				{
					importer.borderMipmap = assetFormatTextureSetting.borderMipmap;
					isDirty = true;
				}

				if (importer.mipmapFilter != assetFormatTextureSetting.mipmapFilter)
				{
					importer.mipmapFilter = assetFormatTextureSetting.mipmapFilter;
					isDirty = true;
				}


				if (importer.mipMapsPreserveCoverage != assetFormatTextureSetting.mipMapsPreserveCoverage)
				{
					importer.mipMapsPreserveCoverage = assetFormatTextureSetting.mipMapsPreserveCoverage;
					isDirty = true;
				}

				if (importer.fadeout != assetFormatTextureSetting.fadeout)
				{
					importer.fadeout = assetFormatTextureSetting.fadeout;
					isDirty = true;
				}

				if (importer.anisoLevel != assetFormatTextureSetting.anisoLevel)
				{
					importer.anisoLevel = assetFormatTextureSetting.anisoLevel;
					isDirty = true;
				}
			}


			foreach (var item in assetFormatTextureSetting.assetFormatTextureInfos)
			{
				var textureSetting = new TextureImporterPlatformSettings
				{
					allowsAlphaSplitting = true,
					overridden = true,
					name = item.platform.ToString(),
					maxTextureSize = item.maxTextureSize,
					format = item.textureImporterFormat,
					androidETC2FallbackOverride = AndroidETC2FallbackOverride.Quality16Bit,
					compressionQuality = (int)item.textureCompressionQuality,
				};

				var old_TextureImporterPlatformSettings = importer.GetPlatformTextureSettings(textureSetting.name);

				bool sameSetting= old_TextureImporterPlatformSettings != null &&
					old_TextureImporterPlatformSettings.maxTextureSize == textureSetting.maxTextureSize &&
					old_TextureImporterPlatformSettings.format == textureSetting.format &&
					old_TextureImporterPlatformSettings.compressionQuality == textureSetting.compressionQuality;

                if (!sameSetting)
                {
					importer.SetPlatformTextureSettings(textureSetting);
					isDirty = true;
				}
			}
			if (isDirty && !AssetFormatDefine.isTextureImporting)
			{
				importer.SaveAndReimport();
			}
		}

		public static void SetAssetsFormat(AssetFormatAudioSetting assetFormatAudioSetting)
		{
			var files = GetAssetFiles(assetFormatAudioSetting.directory, AssetFormatType.AudioClip);
			foreach (var file in files)
			{
				// 需要用文件路径去拿最优设置
				SetAudioClipFileFormat(file);
			}
		}

		public static void SetAudioClipFileFormat(string file)
		{
			var formatSetting = GetFileBestSetting(Setting.assetFormatAudioSettings, file);
			if (formatSetting == null)
				return;
			SetAudioClipFileFormat(formatSetting, file);
		}

		public static void SetAudioClipFileFormat(AssetFormatAudioSetting assetFormatAudioSetting, string file)
		{
			bool isDirty = false;
			AudioImporter importer = (AudioImporter)AssetImporter.GetAtPath(file);

			if(importer.loadInBackground != assetFormatAudioSetting.loadInBackground)
            {
				importer.loadInBackground = assetFormatAudioSetting.loadInBackground;
				isDirty = true;
			}
			if (importer.forceToMono != assetFormatAudioSetting.forceToMono)
			{
				importer.forceToMono = assetFormatAudioSetting.forceToMono;
				isDirty = true;
			}
			if (importer.preloadAudioData != assetFormatAudioSetting.preloadAudioData)
			{
				importer.preloadAudioData = assetFormatAudioSetting.preloadAudioData;
				isDirty = true;
			}


			foreach (var item in assetFormatAudioSetting.assetFormatAudioInfos)
			{
				AudioImporterSampleSettings old_Settings;
                if (item.isDefalut)
                {
					old_Settings = importer.defaultSampleSettings;
				}
				else
                {
					old_Settings = importer.GetOverrideSampleSettings(item.platform.ToString());
				}

				bool sameSetting = old_Settings.loadType == item.loadType &&
					old_Settings.compressionFormat == item.compressionFormat &&
					old_Settings.quality == item.quality &&
					old_Settings.sampleRateSetting == item.sampleRateSetting;

				if (!sameSetting)
				{
					var audioImporterSampleSettings = new AudioImporterSampleSettings
					{
						loadType = item.loadType,
						compressionFormat = item.compressionFormat,
						quality = item.quality,
						sampleRateSetting = item.sampleRateSetting,
						//sampleRateOverride = item.sampleRateOverride
					};
					if (item.isDefalut)
						importer.defaultSampleSettings = audioImporterSampleSettings;
					else
						importer.SetOverrideSampleSettings(item.platform.ToString(), audioImporterSampleSettings);
					isDirty = true;
				}
			}
			if (isDirty && !AssetFormatDefine.isAudioClipImporting)
			{
				importer.SaveAndReimport();
			}
		}

		public static string[] GetAssetFiles(string filePath, AssetFormatType assetFormatType)
        {
			return EditorTools.FindAssets(assetFormatType.ToString(), filePath);
        }
	}
}

