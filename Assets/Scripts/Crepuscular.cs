using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crepuscular : MonoBehaviour
{

	public Material material;
	public GameObject light;

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetVector("_LightPos", GetComponent<Camera>().WorldToViewportPoint(transform.position - light.transform.forward));
		Graphics.Blit(source, destination, material);
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
