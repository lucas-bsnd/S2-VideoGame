using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapisRoulant : MonoBehaviour
{
    public float speed  = 5.0f;
    public float texturespeed = 0.5f;
    public bool ReverseTexture = false;
    Material mymaterial;

    // Start is called before the first frame update
    void Start()
    {
       mymaterial = gameObject.GetComponent<Renderer>().material;
    }
    // Update is called once per frame
    void Update()
    {
        ScrollUV();
    }

    private void OnCollisionStay(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Player" || collisionInfo.gameObject.tag == "IA")
            collisionInfo.gameObject.transform.Translate(0,0,Time.deltaTime * speed, transform);
    }

    private void OnCollisionExit(Collision other)
    {
        if(other.gameObject.tag == "Player")
            other.gameObject.GetComponent<CharacterControls>().IsGrounded = true;
    }

    void ScrollUV()
    {
        //manage texture
        
        if(!ReverseTexture){
            var material                = this.mymaterial;
            Vector2 offset              = material.mainTextureOffset;
            offset                     += Vector2.up * texturespeed * Time.deltaTime / material.mainTextureScale.y;
            material.mainTextureOffset  = offset;
        }
        if(ReverseTexture){
            var material                = this.mymaterial;

            Vector2 TextureScale        = this.mymaterial.mainTextureScale;
            TextureScale                = new Vector2(1,-3f);
            material.mainTextureScale   = TextureScale;

            Vector2 offset              = material.mainTextureOffset;
            offset                     += Vector2.down * texturespeed * Time.deltaTime / material.mainTextureScale.y;
            material.mainTextureOffset  = offset;
        }
    }
}
