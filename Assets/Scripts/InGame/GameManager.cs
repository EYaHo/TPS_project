using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameManager instance {
        get {
            if(m_instance == null) {
                m_instance = FindObjectOfType<GameManager>();
            }

            return m_instance;
        }
    }

    public GameObject playerPrefab;
    public bool isGameover { get; private set; }

    private static GameManager m_instance;

    private void Awake() {

    }

    private void Start() {
        Vector3 randomSpawnPos = Random.insideUnitSphere * 5f;
        randomSpawnPos.y = 5f;

        PhotonNetwork.Instantiate(playerPrefab.name, randomSpawnPos, Quaternion.identity);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnLeftRoom() {
        SceneManager.LoadScene("Lobby");
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if(stream.IsWriting) {  // 로컬 오브젝트인 경우
            
        } else {                // 리모트 오브젝트인 경우

        }
    }

}
