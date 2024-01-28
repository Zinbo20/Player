using System.Collections;
using System.Collections.Generic;

using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEditor;
using TMPro;

using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public TMP_InputField playerNameInput, lobbyCodeInput;

    public GameObject introLobby, lobbyPanel;
    public TMP_Text[] playerNameText;
    public TMP_Text lobbyCodeText;

    Lobby hostLobby, joinnedLobby;
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
            Player = GetPlayer()
        };

        hostLobby = await Lobbies.Instance.CreateLobbyAsync("Nome Lobby", 4, options);
        joinnedLobby = hostLobby;
        Debug.Log("Criou o Lobby");

        InvokeRepeating("SendLobbyHeartBeat",10,10); // Para o Lobby não fechar em 30s

        ShowPlayer();

        lobbyCodeText.text = joinnedLobby.LobbyCode;
        introLobby.SetActive(false);
        lobbyPanel.SetActive(true);
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

    async void SendLobbyHeartBeat() // Para o Lobby não fechar em 30s
    {
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

    async Task updateLobby()
    {
        if (joinnedLobby == null)
            return;

        joinnedLobby = await LobbyService.Instance.GetLobbyAsync(joinnedLobby.Id);
    }




}
