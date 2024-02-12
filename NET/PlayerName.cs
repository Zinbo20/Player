using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Netcode;
using TMPro;
using Unity.Services.Lobbies.Models;
public class PlayerName : NetworkBehaviour
{

    TMP_Text playerNameText;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        playerNameText = GetComponentInChildren<TMP_Text>();

        LobbyManager lobby = FindObjectOfType<LobbyManager>();

        if(IsServer)
        {
            while(NetworkManager.Singleton.ConnectedClients.Count != lobby.joinnedLobby.Players.Count)
                yield return new WaitForSeconds(0.5f);

                for (int i = 0; i < NetworkManager.Singleton.ConnectedClients.Count; i++)
                {
                    NetworkManager.Singleton.ConnectedClients[(ulong)i].PlayerObject.GetComponentInChildren<PlayerName>().
                        SetPlayerNameClientRpc(lobby.joinnedLobby.Players[i].Data["name"].Value);
                }
        }


        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform);
        
    }

    [ClientRpc]
    public void SetPlayerNameClientRpc(string playerName)
    {
        playerNameText.text = playerName;

    }


}
