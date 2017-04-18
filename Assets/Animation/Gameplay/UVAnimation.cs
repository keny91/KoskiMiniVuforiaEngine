using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVAnimation : MonoBehaviour {

    public int uvTileY = 2; //texture sheet columns
    public int uvTileX = 2;//Texture sheet rows

    public int fps = 30;

    private int index;
    private Vector2 size;
    private Vector2 offset;
    private Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

// Update is called once per frame
	void Update ()
    {
        //calculate index
        index = (int) (Time.time * fps);
        //repeat when exhausting all frames
        index = index % (uvTileX * uvTileY);

        //Size of each tile
        size = new Vector2(1.0f / uvTileY, 1.0f / uvTileX);

        //split into horizontal and vertical indexes
        var uIndex = index % uvTileX;
        var vIndex = index / uvTileX;

        //build the offset
        //v coordinate is at the bottom of the image in OpenGL, so we invert it
        offset = new Vector2(uIndex * size.x, 1.0f - size.y - vIndex * size.y);

        renderer.material.SetTextureOffset("_MainTex", offset);
        renderer.material.SetTextureScale("_MainTex", size);

    }
}
