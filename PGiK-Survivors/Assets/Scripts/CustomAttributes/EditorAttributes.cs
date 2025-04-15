using UnityEngine;
using System.Reflection;



#if UNITY_EDITOR
using UnityEditor;
#endif

#region MarkColorAttr

//Doesnt work on Lists
[System.Diagnostics.Conditional( "UNITY_EDITOR" )]
public class MarkColorAttribute : PropertyAttribute {
    private static Color DEFAULT_COLOR = Color.yellow;

    private float r,g,b,a;

    public MarkColorAttribute() { ColorToValues( DEFAULT_COLOR ); }

    public MarkColorAttribute( float r, float g, float b) {
        SetColorValues( r, g, b, 1.0f );
    }

#if UNITY_EDITOR
    public MarkColorAttribute( string color ) {
        ColorToValues( ColorUtil.GetColor( color ) );
    }
#endif

    private void SetColorValues( float r, float g, float b, float a) {
        this.r = Mathf.Clamp01(r);
        this.g = Mathf.Clamp01(g);
        this.b = Mathf.Clamp01(b);
        this.a = Mathf.Clamp01(a);
    }

    private void ColorToValues(Color color) {
        SetColorValues( color.r, color.g, color.b, color.a );
    }

    public Color Color { get { return new Color(r, g, b, a); } }

}

#endregion

#region LockAttr
[System.Diagnostics.Conditional( "UNITY_EDITOR" )]
public class LockAttribute : PropertyAttribute {
    public LockAttribute() { }
}
#endregion

#if UNITY_EDITOR

#region ResetAttr
[System.AttributeUsage( System.AttributeTargets.Field, AllowMultiple = false, Inherited = true )]
[System.Diagnostics.Conditional( "UNITY_EDITOR" )]
public class ResetAttribute : PropertyAttribute {
    public int intValue;
    public float floatValue;

    public ResetType resetType;
    public bool isValueStashed = false;

#if UNITY_EDITOR

    public ValueType valueType;

#endif

    public ResetAttribute( ) { resetType = ResetType.INITIAL_VALUE; valueType = ValueType.UNDEFINED; }
    public ResetAttribute( int value ) { intValue = value; valueType = ValueType.INT; resetType = ResetType.GIVEN_VALUE; }
    public ResetAttribute( float value ) { floatValue = value; valueType = ValueType.FLOAT; resetType = ResetType.GIVEN_VALUE; }

    public enum ResetType {
        INITIAL_VALUE,
        GIVEN_VALUE
    }

#if UNITY_EDITOR
    public void StashValue(SerializedProperty property) {
        if ( isValueStashed ) return;
        
        isValueStashed = true;

        ValueType type = ConverterUtil.StringToType( property.type );

        if ( type == ValueType.FLOAT ) floatValue = property.floatValue;
        else if ( type == ValueType.INT ) intValue = property.intValue;
        else isValueStashed = false;
    }

    public void SetField(FieldInfo field, object source) {
        if ( valueType == ValueType.FLOAT ) field.SetValue( source, floatValue );
        else if ( valueType == ValueType.INT ) field.SetValue( source, intValue );
    }

#endif

}

#endregion

#endif
