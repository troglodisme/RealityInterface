/*
	Copyright © Carl Emil Carlsen 2019-2020
	http://cec.dk
*/

using System;

namespace OscSimpl
{
	public class SerializedOscMessageBuffer
	{
		byte[] _data;
		int _occupiedByteSize;
		int[] _messageSizes;
		int _messageCount;

		readonly int capacityStep;
		const int messageCapacityStep = 64;

		public int count { get { return _messageCount; } }

		public byte[] data { get { return _data; } }


		public SerializedOscMessageBuffer( int capacity )
		{
			capacityStep = capacity;
			_data = new byte[ capacity ];
			_messageSizes = new int[ messageCapacityStep ];
		}


		public void Add( OscMessage message )
		{
			int messageSize = message.Size();

			// Adapt size.
			int requiredCapacity = _occupiedByteSize + messageSize;
			if( requiredCapacity > _data.Length ) {
				byte[] newData = new byte[ _data.Length + capacityStep ];
				Buffer.BlockCopy( _data, 0, newData, 0, _occupiedByteSize );
				_data = newData;
			}

			// Write.
			int byteIndex = _occupiedByteSize;
			message.TryWriteTo( _data, ref byteIndex );

			// Update counts.
			_occupiedByteSize = byteIndex;
			if( _messageCount == _messageSizes.Length ) {
				int[] newMessageSizes = new int[ _messageSizes.Length + messageCapacityStep ];
				Buffer.BlockCopy( _messageSizes, 0, newMessageSizes, 0, _messageSizes.Length );
				_messageSizes = newMessageSizes;
			}
			_messageSizes[ _messageCount++ ] = messageSize;

			//UnityEngine.Debug.Log( "Buffered message in Frame num " + UnityEngine.Time.frameCount + "\n" + message );
		}


		public void Clear()
		{
			_messageCount = 0;
			_occupiedByteSize = 0;
		}


		public int GetSize( int messageIndex )
		{
			if( messageIndex < 0 || messageIndex > _messageCount - 1 ) return 0;
			return _messageSizes[messageIndex];
		}
	}
}