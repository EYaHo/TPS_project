using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1";

    public TextMeshProUGUI connectionInfoText;
    public Button joinButton;
    public Button startButton;

    [SerializeField]
    private bool loadTestScene = true;

    void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        joinButton.interactable = false;
        startButton.interactable = false;
        connectionInfoText.text = "마스터 서버에 접속 중...";
    }

    public override void OnConnectedToMaster() {
        joinButton.interactable = true;
        connectionInfoText.text = "온라인 : 마스터 서버와 연결됨";
    }

    public override void OnDisconnected(DisconnectCause cause) {
        joinButton.interactable = false;
        connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음\n접속 재시도 중...";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        connectionInfoText.text = "빈 방이 없음, 새로운 방 생성...";
        PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = 4});
    }

    public override void OnJoinedRoom() {
        connectionInfoText.text = "방 참가 성공";
        if(PhotonNetwork.IsMasterClient) {
            startButton.interactable = true;
        }
    }

    public void Connect() {
        joinButton.interactable = false;
        if(PhotonNetwork.IsConnected) {
            connectionInfoText.text = "룸에 접속...";
            PhotonNetwork.JoinRandomRoom();
        } else {
            connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음\n접속 재시도 중...";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void StartGame() {
        if(loadTestScene) {
            photonView.RPC("LoadTestScene", RpcTarget.All);
        } else {
            photonView.RPC("LoadGameScene", RpcTarget.All);
        }
    }

    [PunRPC]
    public void LoadGameScene() {
        PhotonNetwork.LoadLevel("GameScene");
    }

    [PunRPC]
    public void LoadTestScene() {
        PhotonNetwork.LoadLevel("TestScene");
    }

    public void ToggleLoadTestScene(bool isOn) {
        loadTestScene = isOn;
    }
}
