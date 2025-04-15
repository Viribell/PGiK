using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStatuses : MonoBehaviour, IEntityComponent {

    private EntityController entityController;
    
    public void LoadEntityController( EntityController controller ) {
        entityController = controller;
    }
}
