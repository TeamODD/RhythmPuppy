using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TutorialsManager : MonoBehaviour
{
    [SerializeField]
    GameObject ADcorgi;
    [SerializeField]
    GameObject Shiftcorgi;
    [SerializeField]
    GameObject Jumpcorgi;
    [SerializeField]
    GameObject Click;

    Animator ADcorgianim;
    Animator Shiftcorgianim;
    Animator Jumpcorgianim;

    SpriteRenderer ClickSprite;

    // Start is called before the first frame update
    void Start()
    {
        ADcorgianim = ADcorgi.GetComponent<Animator>();
        Shiftcorgianim = Shiftcorgi.GetComponent<Animator>(); 
        Jumpcorgianim = Jumpcorgi.GetComponent<Animator>();

        ClickSprite = Click.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            float scaleX = ADcorgi.transform.localScale.x;
            float scaleY = ADcorgi.transform.localScale.y;
            float scaleZ = ADcorgi.transform.localScale.z;

            if (scaleX < 0)
            {
                ADcorgi.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
            }
            else
            {
                ADcorgi.transform.localScale = new Vector3(-scaleX, scaleY, scaleZ);
            }

            ADcorgianim.SetBool("IsAorDKeyDown", true);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            float scaleX = ADcorgi.transform.localScale.x;
            float scaleY = ADcorgi.transform.localScale.y;
            float scaleZ = ADcorgi.transform.localScale.z;

            if (scaleX > 0)
            {
                ADcorgi.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
            }
            else
            {
                ADcorgi.transform.localScale = new Vector3(-scaleX, scaleY, scaleZ);
            }

            ADcorgianim.SetBool("IsAorDKeyDown", true);
        }
        else
        {
            ADcorgianim.SetBool("IsAorDKeyDown", false);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {   
            Shiftcorgianim.SetBool("IsShiftKeyDown", true);
        }
        else
        {
            Shiftcorgianim.SetBool("IsShiftKeyDown", false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jumpcorgianim.SetBool("IsSpaceKeyDown", true);
        }
        else
        {
            Jumpcorgianim.SetBool("IsSpaceKeyDown", false);
        }
    }
}
