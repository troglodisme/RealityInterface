/*
	Created by Carl Emil Carlsen.
	Copyright 2016-2021 Sixth Sensor.
	All rights reserved.
	http://sixthsensor.dk
*/

using System;

namespace OscSimpl
{
	[Serializable]
	public abstract class OscPacket
	{
		static readonly string logPrepend = "<b>[" + nameof( OscPacket ) + "]</b> "; 

		public abstract int Size();

		public abstract bool TryWriteTo( byte[] data, ref int index );

		public static bool TryReadFrom( byte[] data, int dataLength, ref int index, out OscPacket packet )
		{
			if( index >= dataLength ) {
				packet = null;
				if( OscGlobals.logWarnings ) UnityEngine.Debug.LogError( logPrepend + "Failed reading packet.\n");
				return false;
			}

			packet = null;

			byte prefix = data[index];
			if( prefix == OscConst.bundlePrefixByte ){
				OscBundle bundle;
				//UnityEngine.Debug.Log( "READ BUNDLE\n" );
				if( OscBundle.TryReadFrom( data, dataLength, ref index, out bundle ) ) packet = bundle;
				//else UnityEngine.Debug.LogError( "Bundle read failed.\n");
			} else if( prefix == OscConst.addressPrefixByte ){
				OscMessage message = null;
				//UnityEngine.Debug.Log( "READ MESSAGE\n" );
				if( OscMessage.TryReadFrom( data, ref index, ref message ) ) packet = message;
				//else UnityEngine.Debug.LogError( "Message read failed.\n" );
			} else {
				if( OscGlobals.logWarnings ) UnityEngine.Debug.LogWarning( logPrepend + "Falied reading packet. INVALID PREFIX.\n Index: " + index + ", Buffer length: " + data.Length + ", Prefix byte: '" + prefix + "'.\n" );
				index++;
			}

			// Read trailing zero padding.
			for( int i = 0; i < 4 && index < dataLength; i++ ) if( data[index] == 0 ) index++;

			return packet != null;
		}
	}
}