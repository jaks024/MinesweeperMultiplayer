using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MultiplayerRegionController : MonoBehaviourPunCallbacks
{
	[SerializeField] private Dropdown regionDropdown;
	private bool initialized = false;
	private List<string> regions =  new List<string>(){ "asia", "au", "cae", "eu", "in", "jp", "ru", "rue", "sa", "kr", "us", "usw" };

	private void Start()
	{
		List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();
		foreach (string s in regions)
		{
			list.Add(new Dropdown.OptionData() { text = s });
		}
		regionDropdown.options = list;
	}

	public override void OnConnectedToMaster()
	{
		if (initialized)
			return;


		int ind = regions.IndexOf(PhotonNetwork.CloudRegion);
		if (ind == -1)
		{
			PhotonNetwork.Disconnect();
			PhotonNetwork.ConnectToRegion("us");
			Debug.Log("changed");
		}
		else
			regionDropdown.value = ind;

		initialized = true;
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		initialized = false;
	}
	public void ChangeRegionDropdown(int ind)
	{
		if (!initialized)
			return;

		PhotonNetwork.Disconnect();
		PhotonNetwork.ConnectToRegion(regions[ind]);
		StartCoroutine(post());
	}

	private IEnumerator post()
	{
		yield return new WaitForSeconds(1);
		Debug.Log("connected to region " + PhotonNetwork.CloudRegion);
	}
}
