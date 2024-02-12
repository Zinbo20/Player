using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

using Unity.Netcode;

//Add Network Object to your Player Object
//Client Network Animetor and Client NetworkTranform
//Packages/com.unity.multiplayer.samples.coop/Utilities/Net/ClientAuthority
//Add Game Prefab in Network manager


[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM
//[RequireComponent(typeof(PlayerInput))]
#endif


public class SyncPlayer : NetworkBehaviour
{

#if ENABLE_INPUT_SYSTEM
    public PlayerInput _playerInput;
#endif

    // Start is called before the first frame update
    void Start()
    {

        if (IsOwner) { 
            _playerInput.enabled = true;
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (!IsOwner)
            return;

    }


}

