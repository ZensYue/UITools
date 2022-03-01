using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Framework.Editor
{

    public class BaseEditorWindow<T> : EditorWindow where T : EditorWindow
    {

        static T _instance;
        protected static T thisInstance 
        {
            get 
            { 
                if(_instance == null)
                {
                    _instance = EditorWindow.GetWindow(typeof(T)) as T;
                }

                return _instance;
            }
            private set { }
        }


        private bool isshow;
        protected void ShowWindow(string title,Vector2 minSize)
        {
            if (thisInstance == null)
            {
                return;
            }
            thisInstance.titleContent = new GUIContent(title,$"{title}");
            thisInstance.minSize = minSize;
            thisInstance.Show();
            Init();
        }

        protected virtual void Init() { }
    }

}