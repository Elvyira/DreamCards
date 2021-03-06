﻿#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public static class GUIStyleUtility
    {
        private static readonly RectOffset Zero = new RectOffset(0, 0, 0, 0);
        private static readonly RectOffset VerticalSeparation = new RectOffset(0, 0, 3, 4);
        private static readonly RectOffset LineMargin = new RectOffset(0, 0, 5, 5);

        private static readonly RectOffset LineOverflow = new RectOffset(13, 3, 0, 0);
        private static readonly RectOffset LightBoxOverflow = LineOverflow;
        private static readonly RectOffset DarkBoxOverflow = new RectOffset(14, 4, 0, 0);
        private static readonly RectOffset FoldGroupOverflow = DarkBoxOverflow;

        public static readonly GUIStyle White = new GUIStyle
        {
            normal = {background = EditorGUIUtility.whiteTexture}
        };
        
        #region Line

        public static GUIStyle HorizontalLine(bool drawSpace)
        {
            HorizontalLineStyle.margin = drawSpace ? LineMargin : Zero;
            return HorizontalLineStyle;
        }

        private static readonly GUIStyle HorizontalLineStyle = new GUIStyle
        {
            normal = {background = EditorGUIUtility.whiteTexture},
            fixedHeight = 2,
            stretchWidth = true
        };

        #endregion /Line

        #region Box

        public static GUIStyle LightBox(int indentLevel)
        {
            LightBoxStyle.margin.left = EditorDrawUtility.DoIndent(indentLevel);
            return LightBoxStyle;
        }

        public static GUIStyle DarkBox(int indentLevel)
        {
            DarkBoxStyle.margin.left = EditorDrawUtility.DoIndent(indentLevel);
            return DarkBoxStyle;
        }

        private static readonly GUIStyle LightBoxStyle = new GUIStyle(GUI.skin.box)
        {
            margin = VerticalSeparation,
            padding = VerticalSeparation,
            overflow = LightBoxOverflow
        };

        private static readonly GUIStyle DarkBoxStyle = new GUIStyle(GUI.skin.textField)
        {
            margin = VerticalSeparation,
            padding = VerticalSeparation,
            overflow = DarkBoxOverflow
        };

        public static readonly GUIStyle BoxGroupLabel = new GUIStyle(EditorStyles.boldLabel)
        {
            contentOffset = new Vector2(-14, 0)
        };

        #endregion /Box

        #region Fold

        public static GUIStyle FoldGroupHeader(int indentLevel)
        {
            FoldGroupHeaderStyle.margin.left = EditorDrawUtility.DoIndent(indentLevel);
            return FoldGroupHeaderStyle;
        }

        private static readonly GUIStyle FoldGroupHeaderStyle = new GUIStyle(GUI.skin.button)
        {
            margin = VerticalSeparation,
            padding = new RectOffset(1, 1, 0, 0),
            overflow = FoldGroupOverflow
        };

        public static readonly GUIStyle FoldGroupHeaderContent = new GUIStyle
        {
            margin = Zero,
//            padding = new RectOffset(4, 0, 0, 0)
        };

        public static readonly GUIStyle FoldGroupBody = new GUIStyle(GUI.skin.textField)
        {
            margin = Zero,
            padding = new RectOffset(0, 0, 0, 7),
            overflow = FoldGroupOverflow
        };

        #endregion /Fold

        #region Array

        public static readonly GUIStyle ButtonArray = new GUIStyle
        {
            margin = VerticalSeparation
        };

        #endregion /Array

        public static readonly GUIStyle BoldFoldout = new GUIStyle(EditorStyles.foldout)
        {
            margin = VerticalSeparation,
            fontStyle = FontStyle.Bold
        };
        
        public static readonly GUIStyle BigLabelStyle = new GUIStyle(EditorStyles.label)
        {
            fontSize = 25
        };       
        
        public static readonly GUIStyle BigBoldLabelStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 25
        };

        public static readonly GUIStyle InfoLabelStyle = new GUIStyle(EditorStyles.label)
        {
            fontSize = 15
        };

        public static readonly GUIStyle SimpleDarkBox = new GUIStyle(EditorStyles.textField)
        {
            margin = new RectOffset(3, 3, 3, 3)
        };
    }
}
#endif