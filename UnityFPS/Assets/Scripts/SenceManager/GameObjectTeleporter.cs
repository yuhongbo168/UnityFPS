using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class GameObjectTeleporter : MonoBehaviour
{
    public static GameObjectTeleporter Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }

            instance = FindObjectOfType<GameObjectTeleporter>();

            if (instance != null)
            {
                return instance;
            }

            GameObject gameObejctTeleporter = new GameObject("GameObjectTeleporter");
            instance = gameObejctTeleporter.AddComponent<GameObjectTeleporter>();

            return instance;

        }
    }

    public static bool Transitioning
    {
        get { return Instance.m_Transitioning; }
    }

    protected static GameObjectTeleporter instance;

    public PlayerInput m_PlayerInput;
    protected bool m_Transitioning;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        m_PlayerInput = FindObjectOfType<PlayerInput>();
    }

    public static void Teleport(GameObject transitioningGameObject,Vector3 destinationPosition)
    {
        Instance.StartCoroutine(Instance.Transition(transitioningGameObject, false, false, destinationPosition, false));
    }

    protected IEnumerator Transition(GameObject transitioningGameObject, bool releaseControl, bool resetInputValues, Vector3 destinationPosition, bool fade)
    {
        m_Transitioning = true;
        if (releaseControl)
        {
            if (m_PlayerInput == null)
            {
                m_PlayerInput = FindObjectOfType<PlayerInput>();
            }
            m_PlayerInput.actions.Disable();
        }

        //if (fade)
        //{
        //    yield return StartCoroutine()
        //}

        yield return new WaitForSeconds(1f);

        transitioningGameObject.transform.position = destinationPosition;

        if (releaseControl)
        {
            m_PlayerInput.actions.Enable();
        }

        m_Transitioning = false;
    }

}
