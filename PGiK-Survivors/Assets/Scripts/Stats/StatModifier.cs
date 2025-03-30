public enum StatModType {
    Flat = 100,
    PercentAdd = 200,
    PercentMult = 300
}

[System.Serializable]
public class StatModifier {
    public float value;
    public StatType affectedStat;
    public StatModType type;
    public int applyOrder;

    public StatModifier(float value, StatModType type, int applyOrder) {
        this.value = value;
        this.type = type;
        this.applyOrder = applyOrder;
    }

    public StatModifier( float value, StatModType type ) : this( value, type, ( int )type ) { }

    public StatModifier( float value, StatModType type, StatType affectedStat ) : this( value, type, ( int )type ) { this.affectedStat = affectedStat; }
}
