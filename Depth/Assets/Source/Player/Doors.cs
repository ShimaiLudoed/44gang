using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Doors : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private PlayerView playerMove;
    [SerializeField] private bool _isFinish;
    [SerializeField] private LayerMask playerLayer;
    private void OnTriggerEnter(Collider other)
    {
        if (LayerMaskCheck.ContainsLayer(playerLayer, other.gameObject.layer))
        {
            text.gameObject.SetActive(true);
            text.text = "Use LBM to open Door.";
            playerMove.OnFinish += Teleport; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (LayerMaskCheck.ContainsLayer(playerLayer, other.gameObject.layer))
        {
            text.text = " ";
            text.gameObject.SetActive(false);
            playerMove.OnFinish -= Teleport;
        }
    }

    private void Teleport()
    {
        if (_isFinish == true)
        {
            SceneManager.LoadScene("level");   
        }
        else
        {
            text.text = "Hmm, this door won't open, I need to find another one.";
        }
    }
}
