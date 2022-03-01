using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
	[Serializable]
	public class AssetFormatAudioInfo: AssetFormatBaseInfo
	{
		//AudioSetting
		//AudioImporterSampleSettings setting = new AudioImporterSampleSettings();


        public AudioClipLoadType loadType;
		public AudioCompressionFormat compressionFormat = AudioCompressionFormat.Vorbis;
		public float quality = 0.1f;
		public AudioSampleRateSetting sampleRateSetting = AudioSampleRateSetting.OptimizeSampleRate;
	}

	/// <summary>
	/// 音效格式设置
	/// </summary>
	[Serializable]
	public class AssetFormatAudioSetting: AssetFormatBaseSetting
	{
		// importer
		public bool loadInBackground = false;
		public bool forceToMono = true;
		public bool preloadAudioData = true;


		public List<AssetFormatAudioInfo> assetFormatAudioInfos = new List<AssetFormatAudioInfo>();
	}
}
