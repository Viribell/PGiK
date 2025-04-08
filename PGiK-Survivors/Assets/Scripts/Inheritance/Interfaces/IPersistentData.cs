using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPersistentData {
    void LoadData( SaveData data );
    void SaveData( SaveData data );
}
