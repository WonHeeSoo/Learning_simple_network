using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldEdit : MonoBehaviour
{
	public Transform contentField;
	public Transform textPrefab;
	private List<GameObject> chatTextList;

	private void Awake()
	{
		chatTextList = new List<GameObject>();
	}

	public void OnEndEditText()
	{
		chatTextList.Add(Instantiate(textPrefab).gameObject);
		chatTextList[0].transform.SetParent(contentField);
	}
}
