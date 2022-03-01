using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Editor
{
	[Serializable]
	public class AssetFormatBaseInfo
	{
		public AssetFormatPlatform platform;
		public bool isDefalut = false;
	}

	/// <summary>
	/// 基础设置
	/// </summary>
	[Serializable]
	public class AssetFormatBaseSetting
	{
		public string directory;
	}
}
