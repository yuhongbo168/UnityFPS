using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CharacterPlayer : MonoBehaviour
{

    public PlayerController PC;

    

    private void Awake()
    {
        PC = GetComponent<PlayerController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
