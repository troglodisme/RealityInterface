using System.Collections;
using System.Collections.Generic;
using UnityEngine;


	public class ReceiveDemo : MonoBehaviour
	{
		[SerializeField] OscIn _oscIn;

		const string address1 = "//1/faderA";


		void Start()
		{
			// Ensure that we have a OscIn component and start receiving on port 7000.
			if( !_oscIn ) _oscIn = gameObject.AddComponent<OscIn>();
			_oscIn.Open( 7000 );
		}


		void OnEnable()
		{
			// You can "map" messages to methods in two ways:

			// 1) For messages with a single argument, route the value using the type specific map methods.
			_oscIn.MapFloat( address1, OnTest1 );

	
		}


		void OnDisable()
		{
			// If you want to stop receiving messages you have to "unmap".
			_oscIn.UnmapFloat( OnTest1 );
		}

		
		void OnTest1( float value )
		{
			Debug.Log( "Received test1\n" + value + "\n" ); // Composing strings like this generates garbage, use StringBuider IRL.
		}

	}

