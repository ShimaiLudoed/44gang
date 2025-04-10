using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlotMachine : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private PlayerView player;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private Material checkpointActivatedMaterial; // <-- вот оно, новое свойство

    private Renderer rend;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (LayerMaskCheck.ContainsLayer(playerLayer, other.gameObject.layer))
        {
            text.gameObject.SetActive(true);
            text.text = "Use R to Save.";
            player.OnSave += ActivateCheckpoint; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (LayerMaskCheck.ContainsLayer(playerLayer, other.gameObject.layer))
        {
            text.text = " ";
            text.gameObject.SetActive(false);
            player.OnSave -= ActivateCheckpoint; 
            Debug.Log("jopa44");
        }
    }

    private void ActivateCheckpoint()
    {
        playerManager.SavePlayerPosition(transform.position);

        if (checkpointActivatedMaterial != null && rend != null)
        {
            rend.material = checkpointActivatedMaterial;
        }
    }
}
