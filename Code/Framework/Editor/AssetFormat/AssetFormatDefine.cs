using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Editor
{
	/// <summary>
	/// 资源格式类型
	/// </summary>
	public enum AssetFormatType
    {
		Texture = 0,
		AudioClip
	}

	/// <summary>
	/// 资源设置格式平台
	/// </summary>
	public enum AssetFormatPlatform
    {
		Android = 0,
		iPhone
    }

	public static class AssetFormatDefine
    {
		public static string[] platform_names = new string[]
		{
			"Android",
			"iPhone"
		};


		public static int[] texture_sizes = new int[]
		{
			64,128,256,512,1024,2048,4096
		};

		static string[] _sizes_str = null;
		public static string[] texture_sizes_string 
		{
			get {
				if (_sizes_str == null)
				{
					_sizes_str = new string[texture_sizes.Length];
                    for (int i = 0; i < texture_sizes.Length; i++)
                    {
						_sizes_str[i] = texture_sizes[i].ToString();
					}
				}
				return _sizes_str;
			}
			private set { }
		}

		public static string AssetFormatSettingFilePath;
		public static string AssetFormatSettingFilePathKey = "AssetFormatSettingFilePath";


		#region Import是否触发设置  开关不让放入Setting
		/// <summary>
		///  是否导入纹理自动设置格式
		/// </summary>
		public static bool isTextureImportSetting = true;
		public static bool isTextureImporting = false;

		/// <summary>
		///  是否导入音效自动设置格式
		/// </summary>
		public static bool isAudioClipImportSetting = true;
		public static bool isAudioClipImporting = false;
        #endregion
    }
}

