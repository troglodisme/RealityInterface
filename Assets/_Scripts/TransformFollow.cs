using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformFollow : MonoBehaviour
{
    public GameObject follower;
    public GameObject follower2;

    public string influencerName;
    public Vector3 positionOffset;
    public Vector3 rotationOffset;
    private GameObject influencer;

    // Start is called before the first frame update
    void Start()
    {
        // yield return new WaitForSeconds(1f);

        // influencer = GameObject.Find("Knob");

        // yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        if(influencer == null) 
        {
            influencer = GameObject.Find(influencerName);
            Debug.Log("Looking for an influencer");
        }

        else
        {
            Debug.Log("Parent is " +  influencer.transform.parent.name);
            follower.transform.position = influencer.transform.position + positionOffset;
                        follower2.transform.position = influencer.transform.position + positionOffset;

            // follower.transform.eulerAngles = influencer.transform.eulerAngles + rotationOffset;
        }

    }
}
