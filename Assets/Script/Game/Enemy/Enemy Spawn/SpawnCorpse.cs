using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCorpse : MonoBehaviour
{
    public GameObject spriteCorpse;

    // Start is called before the first frame update
    private void OnDestroy()
    {
        spriteCorpse.transform.localScale=gameObject.transform.localScale;
        spriteCorpse.transform.position=gameObject.transform.position;
        Instantiate(spriteCorpse,gameObject.transform.position,gameObject.transform.rotation);
    }
}
