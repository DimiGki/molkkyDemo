using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    private AudioSource sound;
    public AudioClip woodSound;
    public AudioClip grassSound;

    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //sound.PlayOneShot(woodSound);
        if (collision.gameObject.tag == "Grass")
        {
            sound.PlayOneShot(grassSound, 0.5f);
        }
        for (int piece = 1; piece < 13; piece++)
        {
            if (collision.gameObject.tag == piece.ToString())
            {
                sound.PlayOneShot(woodSound, 1f);
            }
        }
    }
}
