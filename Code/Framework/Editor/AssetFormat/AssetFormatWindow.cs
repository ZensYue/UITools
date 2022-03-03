using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Framework.Editor
{
	public class AssetFormatWindow : BaseEditorWindow<AssetFormatWindow>
	{
		[MenuItem("Tools/AssetFormat", false, 101)]
		static void ShowWindow()
		{
			var size = new Vector2(1250, 800);

			thisInstance.ShowWindow("资源格式", size);
			thisInstance.maxSize = size;
        }


        protected override void Init()
        {
			if(AssetFormatSettingData.Setting != null)
            {
				settingFilePath = AssetFormatDefine.AssetFormatSettingFilePath;
			}
		}

		private string settingFilePath;

		// GUI相关
		private const float GuiHeight = 40f;
		private const float spaceSize = 10f;
		private const float GuiBoxHeight = 700f;
		private const float GuiBtnSize = 60f;

		private const float GuiDirecotryMinSize = 300f;
		private const float GuiDirecotryMaxSize = 300f;

		private const float GuiPopEnumSize = 100f;
		private const float GuiPopIntSize = 100f;
		private const float GuiSliderSize = 130f;

		public const float GuiSelectionGridSize = 80f;

		private Vector2 _scrollPos = Vector2.zero;

		private Color deleteColor = new Color(174, 53, 73, 255) / 255;
		private Color attributeColor = new Color(169, 121, 195, 255)/255;
		private Color addPlatformColor = new Color(163, 219, 95, 255) / 255;
		private Color addFolderColor = new Color(138, 204, 103, 255) / 255;
		private Color settingFormatColor = new Color(206, 186, 65, 255) / 255;
		private Color settingFolderColor = new Color(231, 156, 7, 255) / 255;
		private Color saveColor = new Color(121, 138, 184, 255) / 255;

		private GUIStyle tipLabelStyle;

		private string _lastOpenFolderPath = "Assets/";

		private bool isInitGUI = false;
		void InitGUI()
        {
			if (isInitGUI) return;
			isInitGUI = true;

			tipLabelStyle = new GUIStyle(GUI.skin.GetStyle("Label"))
			{
				fontSize = 11,
				fontStyle = FontStyle.Bold
			};
		}

		private void OnGUI()
		{
			InitGUI();

			OnDrawDir();
            OnDrawFormat();
		}

		private void OnDrawDir()
        {
			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField($"配置文件 {settingFilePath}", GUILayout.Width(GuiDirecotryMaxSize * 1.6f));
				{
					GUI.color = addFolderColor;
					// 添加按钮
					if (GUILayout.Button("创建", GUILayout.Width(GuiBtnSize)))
					{
						OpenFolderPanel();
					}
					GUI.color = Color.white;

					GUI.color = settingFolderColor;
					// 添加按钮
					if (GUILayout.Button("加载", GUILayout.Width(GuiBtnSize)))
					{
						LoadAssetFile();
					}
					GUI.color = Color.white;
				}
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField($"***在AssetFormatDefine.cs可以开关是否在资源Import时触发Setting", tipLabelStyle);

			EditorGUILayout.EndHorizontal();

			//AssetFormatDefine
		}

		void LoadAssetFile()
        {
			var selectPath = EditorUtility.OpenFilePanel("选择配置", _lastOpenFolderPath, "asset");
			if (!string.IsNullOrEmpty(selectPath))
			{
				selectPath = EditorTools.AbsolutePathToAssetPath(selectPath);
				if (!selectPath.EndsWith("asset")) return;
				AssetFormatSettingData.LoadSettingData(selectPath);

				settingFilePath = AssetFormatDefine.AssetFormatSettingFilePath;
			}
		}

		void OpenFolderPanel()
        {
			var selectPath = EditorUtility.OpenFolderPanel("选择配置文件夹", _lastOpenFolderPath, "");
			if (!string.IsNullOrEmpty(selectPath))
			{
				selectPath = EditorTools.AbsolutePathToAssetPath(selectPath);
				selectPath = Path.Combine(selectPath, nameof(AssetFormatSetting) + ".asset");
				selectPath = EditorTools.AbsolutePathToAssetPath(selectPath);
				AssetFormatSettingData.CreateSetting(selectPath);
				settingFilePath = AssetFormatDefine.AssetFormatSettingFilePath;
			}
		}

		private const string textureTip = "";
		private const string audioClipTip = "背景音乐：loadInBackground=true,loadType=Streaming；音效：loadInBackground=false,loadType=CompressedInMemory";
		private void OnDrawHeadBar()
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField($"***紫色需要注意 {((AssetFormatType)_gridindex == AssetFormatType.Texture ? textureTip : audioClipTip)}", tipLabelStyle);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();
			EditorGUILayout.Space();
			
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(spaceSize);
			EditorGUILayout.LabelField("Directory", GUILayout.MinWidth(GuiDirecotryMinSize), GUILayout.MaxWidth(GuiDirecotryMaxSize), GUILayout.Height(GuiHeight));

			switch ((AssetFormatType)_gridindex)
			{
				case AssetFormatType.Texture:
                    EditorGUILayout.LabelField("ImporterType", GUILayout.Width(GuiPopEnumSize), GUILayout.Height(GuiHeight));
                    EditorGUILayout.LabelField("ImportModel", GUILayout.Width(GuiPopEnumSize), GUILayout.Height(GuiHeight));
                    EditorGUILayout.LabelField("MaxSize", GUILayout.Width(GuiPopIntSize), GUILayout.Height(GuiHeight));
                    EditorGUILayout.LabelField("NoPowerOf2", GUILayout.Width(GuiPopIntSize), GUILayout.Height(GuiHeight));
                    EditorGUILayout.LabelField("WrapMode", GUILayout.Width(GuiPopIntSize), GUILayout.Height(GuiHeight));
                    EditorGUILayout.LabelField("FiterMode", GUILayout.Width(GuiPopIntSize), GUILayout.Height(GuiHeight));
					EditorGUILayout.LabelField("Delete", GUILayout.Width(GuiBtnSize), GUILayout.Height(GuiHeight));
					EditorGUILayout.LabelField("Set", GUILayout.Width(GuiBtnSize), GUILayout.Height(GuiHeight));

					GUILayout.Space(GuiBtnSize * 0.5f);
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.BeginHorizontal();
					GUILayout.Space(spaceSize);
					EditorGUILayout.LabelField("", GUILayout.MinWidth(GuiDirecotryMinSize), GUILayout.MaxWidth(GuiDirecotryMaxSize), GUILayout.Height(GuiHeight));
					EditorGUILayout.LabelField("streamingMipmaps", GUILayout.Width(GuiPopIntSize), GUILayout.Height(GuiHeight));
					EditorGUILayout.LabelField("mipmapEnabled", GUILayout.Width(GuiPopIntSize), GUILayout.Height(GuiHeight));
					EditorGUILayout.LabelField("borderMipmap", GUILayout.Width(GuiPopIntSize), GUILayout.Height(GuiHeight));
					EditorGUILayout.LabelField("mipmapFilter", GUILayout.Width(GuiPopIntSize), GUILayout.Height(GuiHeight));
					EditorGUILayout.LabelField("mipMapsPreserveCoverage", GUILayout.Width(GuiPopIntSize), GUILayout.Height(GuiHeight));
					EditorGUILayout.LabelField("fadeout", GUILayout.Width(GuiPopIntSize), GUILayout.Height(GuiHeight));
					EditorGUILayout.LabelField("anisoLevel", GUILayout.Width(GuiSliderSize), GUILayout.Height(GuiHeight));
					
					break;
				case AssetFormatType.AudioClip:
					EditorGUILayout.LabelField("loadInBackground", GUILayout.Width(GuiPopEnumSize), GUILayout.Height(GuiHeight));
					EditorGUILayout.LabelField("forceToMono", GUILayout.Width(GuiPopEnumSize), GUILayout.Height(GuiHeight));
					EditorGUILayout.LabelField("preload", GUILayout.Width(GuiPopEnumSize), GUILayout.Height(GuiHeight));
					EditorGUILayout.LabelField("Delete", GUILayout.Width(GuiBtnSize), GUILayout.Height(GuiHeight));
					EditorGUILayout.LabelField("Set", GUILayout.Width(GuiBtnSize), GUILayout.Height(GuiHeight));
					break;
				default:
					break;
			}

			

			GUILayout.Space(GuiBtnSize * 0.5f);
			EditorGUILayout.EndHorizontal();
		}

		private int _gridindex = 0;
        private string[] grids = new string[] {"纹理","音频" };

		private void OnDrawFormat()
		{
			// 列表显示
			EditorGUILayout.Space();
			EditorGUILayout.BeginVertical("HelpBox");
			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.BeginVertical("HelpBox", GUILayout.MaxHeight(GuiBoxHeight),GUILayout.MinWidth(GuiSelectionGridSize)); //Left Content Begin
			{
				GUILayout.Space(GuiHeight);
				_gridindex = GUILayout.SelectionGrid(_gridindex, grids, 1,GUILayout.MinWidth(GuiSelectionGridSize),GUILayout.MinHeight(GuiHeight*grids.Length));
			}
			EditorGUILayout.EndVertical(); // Left Content End

			EditorGUILayout.BeginVertical("HelpBox", GUILayout.MaxHeight(GuiBoxHeight));
			{
                OnDrawHeadBar();
                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
				{
					switch ((AssetFormatType)_gridindex)
                    {
						case AssetFormatType.Texture:
							DrawTextureFormat();
							break;
						case AssetFormatType.AudioClip:
							DrawAudioClipFormat();
							break;
						default:
							break;
                    }

				}
				EditorGUILayout.EndScrollView();

				GUILayout.Space( 20f);
				GUI.color = addFolderColor;
				// 添加按钮
				if (GUILayout.Button($"Add {((AssetFormatType)_gridindex)} Folder", GUILayout.Height(GuiHeight)))
				{
					string resultPath = EditorTools.OpenFolderPanel("Select Folder", _lastOpenFolderPath);
					if (resultPath != null)
                    {
						AssetFormatSettingData.AddFormatSetting((AssetFormatType)_gridindex, resultPath);
					}
				}

				GUI.color = settingFolderColor;
				if (GUILayout.Button($"Set {((AssetFormatType)_gridindex)} Format", GUILayout.Height(GuiHeight)))
				{
					string content = $"是否设置所有{((AssetFormatType)_gridindex)}的格式";
					if (EditorUtility.DisplayDialog("提示", content, "Yes", "No"))
					{
						AssetFormatSettingData.SetAssetsFormat((AssetFormatType)_gridindex);
						AssetDatabase.Refresh();
					}
				}
				GUI.color = Color.white;

				EditorGUILayout.Space();
			}
			EditorGUILayout.EndVertical(); // Right Scroll

			EditorGUILayout.EndHorizontal();
			
			GUI.color = saveColor;
			// 添加按钮
			if (GUILayout.Button("Save Setting", GUILayout.Height(GuiHeight)))
			{
				AssetFormatSettingData.SaveSetting();
			}
			GUI.color = Color.white;
			EditorGUILayout.EndVertical(); // End ALl
		}

		void DrawTextureFormat()
        {
			if(AssetFormatSettingData.Setting == null)
            {
				return;
            }

			foreach (var item in AssetFormatSettingData.Setting.assetFormatTextureSettings)
			{
				EditorGUILayout.BeginVertical("HelpBox");

				EditorGUILayout.BeginHorizontal();
				{
					GUILayout.Space(spaceSize);
					// Directory
					EditorGUILayout.LabelField(item.directory, tipLabelStyle, GUILayout.MinWidth(GuiDirecotryMinSize), GUILayout.MaxWidth(GuiDirecotryMaxSize));

					GUI.color = attributeColor;
					item.textureType = (TextureImporterType)EditorGUILayout.EnumPopup(item.textureType, GUILayout.Width(GuiPopEnumSize));
					GUI.color = Color.white;

					item.spriteImportMode = (SpriteImportMode)EditorGUILayout.EnumPopup(item.spriteImportMode, GUILayout.Width(GuiPopEnumSize));

					GUI.color = attributeColor;
					int index = EditorTools.GetArraryIndex(AssetFormatDefine.texture_sizes, item.maxTextureSize, 3);
					int new_index = EditorGUILayout.Popup(index, AssetFormatDefine.texture_sizes_string, GUILayout.Width(GuiPopIntSize));
					if(index != new_index)
                    {
						item.maxTextureSize = AssetFormatDefine.texture_sizes[new_index];
					}
					GUI.color = Color.white;

					item.npotScale = (TextureImporterNPOTScale)EditorGUILayout.EnumPopup(item.npotScale, GUILayout.Width(GuiPopEnumSize));
					item.wrapMode = (TextureWrapMode)EditorGUILayout.EnumPopup(item.wrapMode, GUILayout.Width(GuiPopEnumSize));
					item.filterMode = (FilterMode)EditorGUILayout.EnumPopup(item.filterMode, GUILayout.Width(GuiPopEnumSize));

					GUI.color = deleteColor;
					if (GUILayout.Button("-", GUILayout.Width(GuiBtnSize), GUILayout.Height(14f)))
                    {
						AssetFormatSettingData.Setting.assetFormatTextureSettings.Remove(item);
						break;
					}

					GUI.color = settingFormatColor;
					if (GUILayout.Button("Set", GUILayout.Width(GuiBtnSize), GUILayout.Height(14f)))
					{
						string content = $"是否设置文件夹：{item.directory} 内的 {AssetFormatSettingData.GetAssetFiles(item.directory, (AssetFormatType)_gridindex).Length} 个{((AssetFormatType)_gridindex)}的格式";
						if (EditorUtility.DisplayDialog("提示", content, "Yes", "No"))
                        {
							AssetFormatSettingData.SetAssetsFormat(item);
							AssetDatabase.Refresh();
						}
					}
					GUI.color = Color.white;
					GUILayout.Space(GuiBtnSize * 0.5f);
				}
				EditorGUILayout.EndHorizontal();


				EditorGUILayout.BeginHorizontal();
                {
					GUILayout.Space(spaceSize);
					// Directory
					EditorGUILayout.LabelField("", tipLabelStyle, GUILayout.MinWidth(GuiDirecotryMinSize), GUILayout.MaxWidth(GuiDirecotryMaxSize));
					item.streamingMipmaps = EditorGUILayout.Toggle(item.streamingMipmaps, GUILayout.Width(GuiPopEnumSize));
					item.mipmapEnabled = EditorGUILayout.Toggle(item.mipmapEnabled, GUILayout.Width(GuiPopEnumSize));
					EditorGUI.BeginDisabledGroup(!item.mipmapEnabled);
					item.borderMipmap = EditorGUILayout.Toggle(item.borderMipmap, GUILayout.Width(GuiPopEnumSize));
					item.mipmapFilter = (TextureImporterMipFilter)EditorGUILayout.EnumPopup(item.mipmapFilter, GUILayout.Width(GuiPopEnumSize));
					item.mipMapsPreserveCoverage = EditorGUILayout.Toggle(item.mipMapsPreserveCoverage, GUILayout.Width(GuiPopEnumSize));
					item.fadeout = EditorGUILayout.Toggle(item.fadeout, GUILayout.Width(GuiPopEnumSize));
					item.anisoLevel = EditorGUILayout.IntSlider(item.anisoLevel, -1, 9, GUILayout.Width(GuiSliderSize));
					EditorGUI.EndDisabledGroup();
				}
				EditorGUILayout.EndHorizontal();


				// Draw Platform Setting
				EditorGUILayout.BeginHorizontal();
				{
					// Left
					EditorGUILayout.BeginVertical(GUILayout.MinWidth(GuiDirecotryMinSize + spaceSize), GUILayout.MaxWidth(GuiDirecotryMaxSize + spaceSize));
					{
						EditorGUILayout.BeginHorizontal();
						GUILayout.Space(spaceSize);
						EditorGUILayout.LabelField("Platform Setting", GUILayout.MinWidth(GuiDirecotryMinSize), GUILayout.MaxWidth(GuiDirecotryMaxSize));
						EditorGUILayout.EndHorizontal();
					}
					EditorGUILayout.EndVertical();

					// Right
					EditorGUILayout.BeginVertical("HelpBox", GUILayout.Width(GuiPopEnumSize * 3 + GuiPopIntSize + GuiBtnSize));
					{
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Platform", GUILayout.Width(GuiPopEnumSize));
						EditorGUILayout.LabelField("Max Size", GUILayout.Width(GuiPopIntSize));
						EditorGUILayout.LabelField("Quality", GUILayout.Width(GuiPopEnumSize));
						EditorGUILayout.LabelField("Format", GUILayout.Width(GuiPopEnumSize));
						EditorGUILayout.EndHorizontal();

						foreach (var assetFormatTextureInfo in item.assetFormatTextureInfos)
						{
							EditorGUILayout.BeginHorizontal();

							GUI.color = attributeColor;
							var new_asssetFormatPlatform = (AssetFormatPlatform)EditorGUILayout.EnumPopup(assetFormatTextureInfo.platform, GUILayout.Width(GuiPopEnumSize));
							AssetFormatSettingData.ChangePlatform(item.assetFormatTextureInfos, assetFormatTextureInfo, new_asssetFormatPlatform);
							GUI.color = Color.white;

							int index = EditorTools.GetArraryIndex(AssetFormatDefine.texture_sizes, assetFormatTextureInfo.maxTextureSize, 3);
							int new_index = EditorGUILayout.Popup(index, AssetFormatDefine.texture_sizes_string, GUILayout.Width(GuiPopIntSize));
							if (index != new_index)
							{
								assetFormatTextureInfo.maxTextureSize = AssetFormatDefine.texture_sizes[new_index];
							}

							assetFormatTextureInfo.textureCompressionQuality = (TextureCompressionQuality)EditorGUILayout.EnumPopup(assetFormatTextureInfo.textureCompressionQuality, GUILayout.Width(GuiPopEnumSize));

							GUI.color = attributeColor;
							assetFormatTextureInfo.textureImporterFormat = (TextureImporterFormat)EditorGUILayout.EnumPopup(assetFormatTextureInfo.textureImporterFormat, GUILayout.Width(GuiPopEnumSize));
							GUI.color = Color.white;

							GUI.color = deleteColor;
							if (GUILayout.Button("-", GUILayout.Width(GuiBtnSize), GUILayout.Height(14f)))
							{
								item.assetFormatTextureInfos.Remove(assetFormatTextureInfo);
								break;
							}
							GUI.color = Color.white;
							EditorGUILayout.EndHorizontal();
						}

						if (item.assetFormatTextureInfos.Count < System.Enum.GetNames(typeof(AssetFormatPlatform)).Length)
						{
							GUI.color = addPlatformColor;
							if (GUILayout.Button("add platform"))
							{
								var assetFormatTextureInfo = AssetFormatSettingData.AddTextureFormat(item.assetFormatTextureInfos);
								assetFormatTextureInfo.maxTextureSize = item.maxTextureSize;
								if (assetFormatTextureInfo.platform == AssetFormatPlatform.Android)
								{
									assetFormatTextureInfo.textureImporterFormat = TextureImporterFormat.ASTC_RGBA_4x4;
								}
								else if (assetFormatTextureInfo.platform == AssetFormatPlatform.iPhone)
								{
									assetFormatTextureInfo.textureImporterFormat = TextureImporterFormat.ASTC_RGBA_4x4;
								}
							}

							GUI.color = Color.white;
						}
					}
					EditorGUILayout.EndVertical();
				}
				EditorGUILayout.EndHorizontal();



				EditorGUILayout.Space();
				EditorGUILayout.EndVertical();
			}
        }

		void DrawAudioClipFormat()
		{
			if (AssetFormatSettingData.Setting == null)
			{
				return;
			}

			foreach (var item in AssetFormatSettingData.Setting.assetFormatAudioSettings)
			{
				EditorGUILayout.BeginVertical("HelpBox");

				EditorGUILayout.BeginHorizontal();
				{
					GUILayout.Space(spaceSize);
					// Directory
					EditorGUILayout.LabelField(item.directory, tipLabelStyle, GUILayout.MinWidth(GuiDirecotryMinSize), GUILayout.MaxWidth(GuiDirecotryMaxSize));

					GUI.color = attributeColor;
					item.loadInBackground = EditorGUILayout.Toggle( item.loadInBackground, GUILayout.Width(GuiPopEnumSize));
					GUI.color = Color.white;

					item.forceToMono = EditorGUILayout.Toggle(item.forceToMono, GUILayout.Width(GuiPopEnumSize));
					item.preloadAudioData = EditorGUILayout.Toggle(item.preloadAudioData, GUILayout.Width(GuiPopEnumSize));

					GUI.color = deleteColor;
					if (GUILayout.Button("-", GUILayout.Width(GuiBtnSize), GUILayout.Height(14f)))
					{
						AssetFormatSettingData.Setting.assetFormatAudioSettings.Remove(item);
						break;
					}

					GUI.color = settingFormatColor;
					if (GUILayout.Button("Set", GUILayout.Width(GuiBtnSize), GUILayout.Height(14f)))
					{
						string content = $"是否设置文件夹：{item.directory} 内的 {AssetFormatSettingData.GetAssetFiles(item.directory, (AssetFormatType)_gridindex).Length} 个{((AssetFormatType)_gridindex)}的格式";
						if (EditorUtility.DisplayDialog("提示", content, "Yes", "No"))
						{
							AssetFormatSettingData.SetAssetsFormat(item);
							AssetDatabase.Refresh();
						}
					}
					GUI.color = Color.white;
					GUILayout.Space(GuiBtnSize * 0.5f);
				}
				EditorGUILayout.EndHorizontal();


				// Draw Platform Setting
				EditorGUILayout.BeginHorizontal();
				{
					// Left
					EditorGUILayout.BeginVertical(GUILayout.MinWidth(GuiDirecotryMinSize + spaceSize), GUILayout.MaxWidth(GuiDirecotryMaxSize + spaceSize));
					{
						EditorGUILayout.BeginHorizontal();
						GUILayout.Space(spaceSize);
						EditorGUILayout.LabelField("Platform Setting", GUILayout.MinWidth(GuiDirecotryMinSize), GUILayout.MaxWidth(GuiDirecotryMaxSize));
						EditorGUILayout.EndHorizontal();
					}
					EditorGUILayout.EndVertical();

					// Right
					EditorGUILayout.BeginVertical("HelpBox", GUILayout.Width(GuiPopEnumSize * 4 + GuiSliderSize + GuiBtnSize));
					{
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Platform", GUILayout.Width(GuiPopEnumSize));
						EditorGUILayout.LabelField("loadType", GUILayout.Width(GuiPopEnumSize));
						EditorGUILayout.LabelField("compression", GUILayout.Width(GuiPopEnumSize));
						EditorGUILayout.LabelField("Quality", GUILayout.MinWidth(GuiSliderSize), GUILayout.MaxWidth(GuiSliderSize));
						EditorGUILayout.LabelField("sampleRate", GUILayout.Width(GuiPopEnumSize));
						EditorGUILayout.LabelField("Delete", GUILayout.MinWidth(GuiBtnSize), GUILayout.MaxWidth(GuiBtnSize));
						EditorGUILayout.EndHorizontal();

						foreach (var assetFormatAudioClipInfo in item.assetFormatAudioInfos)
						{
							EditorGUILayout.BeginHorizontal();

							GUI.color = attributeColor;
							if (assetFormatAudioClipInfo.isDefalut)
							{
								EditorGUILayout.LabelField("Defalut", GUILayout.Width(GuiPopEnumSize));
							}
							else
							{
								var new_asssetFormatPlatform = (AssetFormatPlatform)EditorGUILayout.EnumPopup(assetFormatAudioClipInfo.platform, GUILayout.Width(GuiPopEnumSize));
								AssetFormatSettingData.ChangePlatform(item.assetFormatAudioInfos, assetFormatAudioClipInfo, new_asssetFormatPlatform);
							}
							GUI.color = Color.white;

							GUI.color = attributeColor;
							assetFormatAudioClipInfo.loadType = (AudioClipLoadType)EditorGUILayout.EnumPopup(assetFormatAudioClipInfo.loadType, GUILayout.Width(GuiPopEnumSize));
							GUI.color = Color.white;

							assetFormatAudioClipInfo.compressionFormat = (AudioCompressionFormat)EditorGUILayout.EnumPopup(assetFormatAudioClipInfo.compressionFormat, GUILayout.Width(GuiPopEnumSize));

							assetFormatAudioClipInfo.quality = EditorGUILayout.Slider(assetFormatAudioClipInfo.quality, 0.01f, 1, GUILayout.Width(GuiSliderSize));
							assetFormatAudioClipInfo.sampleRateSetting = (AudioSampleRateSetting)EditorGUILayout.EnumPopup(assetFormatAudioClipInfo.sampleRateSetting, GUILayout.Width(GuiPopEnumSize));

							if (!assetFormatAudioClipInfo.isDefalut)
							{
								GUI.color = deleteColor;
								if (GUILayout.Button("-", GUILayout.Width(GuiBtnSize), GUILayout.Height(14f)))
								{
									item.assetFormatAudioInfos.Remove(assetFormatAudioClipInfo);
									break;
								}
								GUI.color = Color.white;
							}
							EditorGUILayout.EndHorizontal();

						}

						if (item.assetFormatAudioInfos.Count < System.Enum.GetNames(typeof(AssetFormatPlatform)).Length + 1)
						{
							GUI.color = addPlatformColor;
							if (GUILayout.Button("add platform"))
							{
								var assetFormatTextureInfo = AssetFormatSettingData.AddTextureFormat(item.assetFormatAudioInfos);
							}

							GUI.color = Color.white;
						}
					}
					EditorGUILayout.EndVertical();
				}
				EditorGUILayout.EndHorizontal();
				



				EditorGUILayout.Space();
				EditorGUILayout.EndVertical();
			}
		}
	}
}
