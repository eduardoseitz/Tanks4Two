using System;
using UnityEngine;

[Serializable]
public class TankManager
{
    public Color m_PlayerColor;            
    public Transform m_SpawnPoint;         
    [HideInInspector] public int m_PlayerNumber;             
    [HideInInspector] public string m_ColoredPlayerText;
    [HideInInspector] public GameObject m_Instance;          
    [HideInInspector] public int m_Wins;                     


    private TankMovement _movement;       
    //private ShootingController _shooting;
    private GameObject _canvasGameObject;


    public void Setup()
    {
        _movement = m_Instance.GetComponent<TankMovement>();
        //_shooting = m_Instance.GetComponent<ShootingController>();
        _canvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;

        //m_Movement.m_PlayerNumber = m_PlayerNumber;
        //_shooting.m_PlayerNumber = m_PlayerNumber;

        m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = m_PlayerColor;
        }
    }


    public void DisableControl()
    {
        _movement.enabled = false;
        //_shooting.enabled = false;

        _canvasGameObject.SetActive(false);
    }


    public void EnableControl()
    {
        _movement.enabled = true;
        //_shooting.enabled = true;

        _canvasGameObject.SetActive(true);
    }


    public void Reset()
    {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }
}
