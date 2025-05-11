public enum StatModType {
    Flat = 100,
    BasePercentAdd = 200,
    BasePercentMult = 300,
    PercentAdd = 400,
    PercentMult = 500
}

[System.Serializable]
public class StatModifier {
    public float DefaultValue { get; private set; }
    public float value;
    public StatType affectedStat;
    public StatModType type;
    public int applyOrder;

    public StatModifier(float value, StatModType type, int applyOrder) {
        this.value = value;
        this.type = type;
        this.applyOrder = applyOrder;

        DefaultValue = value;
    }

    public StatModifier( float value, StatModType type ) : this( value, type, ( int )type ) { }

    public StatModifier( float value, StatModType type, StatType affectedStat ) : this( value, type, ( int )type ) { this.affectedStat = affectedStat; }
}
