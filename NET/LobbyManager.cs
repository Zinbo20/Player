using System.Collections;
using System.Collections.Generic;

using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEditor;
using TMPro;

using Unity.Services.Relay.Models;
using Unity.Services.Relay;


using UnityEngine;
using Unity.Networking.Transport.Relay;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using System;

public class LobbyManager : MonoBehaviour
{
    public TMP_InputField playerNameInput, lobbyCodeInput;

    public GameObject introLobby, lobbyPanel;
    public TMP_Text[] playerNameText;
    public TMP_Text lobbyCodeText;

    public Lobby hostLobby, joinnedLobby;


    public GameObject startGameButton;
    bool StartedGame;
    // Start is called before the first frame update
    async void Start()
    {
        await UnityServices.InitializeAsync(); //async para não travar
    }

    async Task Authenticate() //Tem que ser uma Task 
    {
        if (AuthenticationService.Instance.IsSignedIn)
            return;
        
        AuthenticationService.Instance.ClearSessionToken(); // Para Poder testar na mesma Maquina

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        Debug.Log("Usuário logado como " + AuthenticationService.Instance.PlayerId);
    }

    async public void CreateLobby()
    {
        await Authenticate(); //Ao ser Task posso colocar o await novamente

        CreateLobbyOptions options = new CreateLobbyOptions
        {
            Player = GetPlayer(),
            Data = new Dictionary<string, DataObject>                                        //Relay Data
            {
                {"StartGame", new DataObject(DataObject.VisibilityOptions.Member, "0")}
            }
        };

        hostLobby = await Lobbies.Instance.CreateLobbyAsync("Nome Lobby", 4, options);
        joinnedLobby = hostLobby;
        Debug.Log("Criou o Lobby");

        InvokeRepeating("SendLobbyHeartBeat",5,5); // Para o Lobby não fechar em 30s

        ShowPlayer();

        lobbyCodeText.text = joinnedLobby.LobbyCode;
        introLobby.SetActive(false);
        lobbyPanel.SetActive(true);
        startGameButton.SetActive(true);
    }

    async public void joinLobbyByCode()
    {
        await Authenticate();

        JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions
        {
            Player = GetPlayer()
        };

        joinnedLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCodeInput.text, options);
        Debug.Log("Entrou no lobby " + joinnedLobby.LobbyCode);

        ShowPlayer();

        lobbyCodeText.text = joinnedLobby.LobbyCode;
        introLobby.SetActive(false);
        lobbyPanel.SetActive(true);
        InvokeRepeating("CheckForUpadates", 3, 3);                   //Relay Data

    }

    Player GetPlayer()
    {
        Player player = new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
            {
                {"name", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, playerNameInput.text)}
            }
        };

        return player;
    }


    async void CheckForUpadates()                     //Relay Data
    {
        if (joinnedLobby == null || StartedGame)
            return;

        await updateLobby();
        ShowPlayer();

        if(joinnedLobby.Data["StartGame"].Value != "0")
        {
            if(hostLobby == null)
            {
                JoinRelay(joinnedLobby.Data["StartGame"].Value);
            }

            StartedGame = true;
        }
    } 

    async void SendLobbyHeartBeat() // Para o Lobby não fechar em 30s
    {

        if (joinnedLobby == null || StartedGame)
            return;

        if(hostLobby == null)       
            return;

        await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);


        await updateLobby();
        ShowPlayer();
    }

    void ShowPlayer()
    {
        for (int i = 0; i < joinnedLobby.Players.Count; i++)
        {
            //Debug.Log(joinnedLobby.Players[i].Id);
            //Debug.Log(joinnedLobby.Players[i].Data["name"].Value);
            playerNameText[i].text = joinnedLobby.Players[i].Data["name"].Value;
        }

    }

    async Task updateLobby() //  async void
    {
        if (joinnedLobby == null)
            return;

        joinnedLobby = await LobbyService.Instance.GetLobbyAsync(joinnedLobby.Id);
    }



    async Task<string> CreateRelay()
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(4);    //Define Quantos Players no lobby

        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        RelayServerData relayServerData = new RelayServerData(allocation, "dtls"); //numero de player e o tipo de conexão

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        NetworkManager.Singleton.StartHost();

        return joinCode;
    }

    public async void StartGame()
    {
        string relayCode = await CreateRelay();

        Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(joinnedLobby.Id, new UpdateLobbyOptions
        {
            Data = new Dictionary<string, DataObject>
            {
                {"StartGame", new DataObject(DataObject.VisibilityOptions.Member, relayCode)}
            }
        });

        joinnedLobby = lobby;

        lobbyPanel.SetActive(false);

    }


    async void JoinRelay (string joinCode)
    {
        JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);   

        RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        NetworkManager.Singleton.StartClient();

        lobbyPanel.SetActive(false);

    }


}
