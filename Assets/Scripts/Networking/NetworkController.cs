using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkController : MonoBehaviourPunCallbacks
{
	// Start is called before the first frame update

	private static NetworkController instance;


	private void Awake()
	{
		DontDestroyOnLoad(this);

		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}

	void Start()
    {
    }

	public override void OnConnectedToMaster()
	{
		Debug.Log("connected to the " + PhotonNetwork.CloudRegion + " server");
	}
}
