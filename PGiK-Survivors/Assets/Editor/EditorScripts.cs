using UnityEngine;
using System.Collections.Generic;
using System.Reflection;




#if UNITY_EDITOR
using UnityEditor;

#region MarkColorAttrDrawer
[CustomPropertyDrawer(typeof(MarkColorAttribute))]
public class MarkColorPropertyDrawer : PropertyDrawer {

    private MarkColorAttribute Attr { get { return ( MarkColorAttribute )attribute; } }

    private static GUIStyle labelStyle = new GUIStyle( EditorStyles.label );
    private static GUIStyle foldoutStyle = new GUIStyle( EditorStyles.foldout );

    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {
        OverrideLabelColor( Attr.Color );
        SetFoldOutStyle( Attr.Color );

        EditorGUI.BeginProperty( position, label, property );

        Rect fieldRect = EditorGUI.PrefixLabel( position, label, labelStyle );
        EditorGUI.PropertyField( fieldRect, property, GUIContent.none, true );

        EditorGUI.EndProperty();
    }

    private static void SetFoldOutStyle(Color color) {
        foldoutStyle.normal.textColor = color;
        foldoutStyle.onActive.textColor = color;
        foldoutStyle.onFocused.textColor = color;
        foldoutStyle.active.textColor = color;
        foldoutStyle.hover.textColor = color;
        foldoutStyle.onHover.textColor = color;
        foldoutStyle.onNormal.textColor = color;
    }

    private static void OverrideLabelColor(Color color) {
        labelStyle.normal.textColor = color;
        labelStyle.onActive.textColor = color;
        labelStyle.onFocused.textColor = color;
        labelStyle.active.textColor = color;
        labelStyle.hover.textColor = color;
        labelStyle.onHover.textColor = color;
        labelStyle.onNormal.textColor = color;
    }
}
#endregion

#region LockAttrDrawer

//Doesnt work on lists
[CustomPropertyDrawer( typeof( LockAttribute ) )]
public class LockPropertyDrawer : PropertyDrawer {
    private LockAttribute Attr { get { return ( LockAttribute )attribute; } }


    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {

        EditorGUI.BeginDisabledGroup( true );

        EditorGUI.PropertyField( position, property, label );

        EditorGUI.EndDisabledGroup();
    }
}


#endregion


#endif