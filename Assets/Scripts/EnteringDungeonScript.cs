using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EnteringDungeonScript : MonoBehaviour
{
    [SerializeField]
    private GameObject alertText;

    private void Start()
    {
        alertText.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided");
        if (collision.gameObject.tag == "Player")
        {
            alertText.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        alertText.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene(1);
        }
    }
}
