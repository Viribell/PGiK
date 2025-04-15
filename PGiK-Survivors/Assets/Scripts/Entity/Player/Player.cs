using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IEntityComponent {
    private PlayerController playerController;

    private void Start() {
        if ( playerController.EntityData != null ) Init();
    }

    private void Init() {
        if ( playerController.EntityData.sprite != null && playerController.SpriteRenderer != null ) playerController.SpriteRenderer.sprite = playerController.EntityData.sprite;
    }

    public void LoadEntityController( EntityController controller ) {
        playerController = (PlayerController)controller;
    }
}
