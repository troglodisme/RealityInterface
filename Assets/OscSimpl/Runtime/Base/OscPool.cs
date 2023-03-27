/*
	Created by Carl Emil Carlsen.
	Copyright 2016-2021 Sixth Sensor.
	All rights reserved.
	http://sixthsensor.dk
*/

using System;
using System.Collections.Generic;
using OscSimpl;

/// <summary>
/// The Osc pool holds packets for reuse to reduce garbage generation.
/// </summary>
public static class OscPool
{
	static Dictionary<uint,FasterMessageStack> _messageStacks;
	static Stack<OscBundle> _bundles;
	static int _messageCount;
	static int _messagesConstructedCount;

	const int stackCapacityMax = 1024;
	const int stackMessageCapacityMax = 1024;
	const int stackMessageCapacityStep = 64;

	static OscPool()
	{
		_messageStacks = new Dictionary<uint,FasterMessageStack>();
		_bundles = new Stack<OscBundle>();
	}


	public static int internalHashedAddressCount { get { return _messageStacks.Count; } }
	public static int internalMessageCount { get { return _messageCount; } }
	public static int internalMessageConstructedCount { get { return _messagesConstructedCount; } }


	/// <summary>
	/// Recycle the specified message.
	/// </summary>
	public static void Recycle( OscMessage message )
	{
		//Debug.Log( Time.frameCount +  ": OscPool.Recycle: " + message.address + " " + message.GetAddressHash() + "\n" );
		if( message == null ) return;

		uint addressHash = message.GetAddressHash();
		FasterMessageStack stack;
		if( !_messageStacks.TryGetValue( addressHash, out stack ) ){
			if( _messageStacks.Count >= stackCapacityMax ) return;
			stack = new FasterMessageStack();
			_messageStacks.Add( addressHash, stack );
		} else {
			if( stack.count == stackMessageCapacityMax ) return;
			if( stack.messages.Length == stack.count ) stack.Expand();
		}
	
		stack.messages[ stack.count++ ] = message; // Push.

		_messageCount++;
	}


	/// <summary>
	/// Recycle the specified bundle.
	/// </summary>
	public static void Recycle( OscBundle bundle )
	{
		if( bundle == null ) return;
		if( _bundles.Count > stackMessageCapacityMax ) return;
		_bundles.Push( bundle );
	}


	/// <summary>
	/// Recycle the specified packet.
	/// </summary>
	public static void Recycle( OscPacket packet )
	{
		if( packet == null ) return;
		if( packet  is OscMessage ) Recycle( packet as OscMessage );
		Recycle( packet as OscBundle );
	}


	public static OscMessage GetMessage( uint addressHash )
	{
		//Debug.Log( "Pool lossy hash count: " + _messageStacks.Count );
		OscMessage message;
		FasterMessageStack stack;
		if( _messageStacks.TryGetValue( addressHash, out stack ) && stack.count > 0 ) {
			message = stack.messages[ --stack.count ]; // Pop
			_messageCount--;
			message.Clear();
			return message;
		}

		//Debug.Log( Time.frameCount +  ": OscPool.GetMessage CREATED MESSAGE! for hash " + hash + " when there was " + _messageStacks.Count + " stacks available.\n" );
		//foreach( KeyValuePair<int, Stack<OscMessage>> pair in _messageStacks ) {
		//	Debug.Log( "\tHash: " + pair.Key + ", message count: " + pair.Value.Count + "\n" );
		//}

		message = new OscMessage();
		_messagesConstructedCount++;
		return message;
	}

	
	public static OscMessage GetMessage( string address )
	{
		OscMessage m = GetMessage( OscStringHash.Pack( address ) );
		m.address = address;
		return m;
		/*
		OscMessage message;

		// Compute lossy hash and try to get message from pool.
		uint hash = OscStringHash.Pack( address );
		Stack<OscMessage> stack;
		if( _messageStacks.TryGetValue( hash, out stack ) && stack.Count > 0 ){
			message = stack.Pop();
			_messageCount--;
			message.Clear();
			//Debug.Log( "Reusing message: " + address );
			// Only set address if it differs. Ordinal (raw byte) comparison.
			if( string.Compare( message.address, address, StringComparison.Ordinal ) != 0 ) message.address = address;
		} else {
			message = new OscMessage( address );
			_messagesConstructedCount++;
		}
		return message;
		*/
	}
	

	public static OscBundle GetBundle()
	{
		if( _bundles.Count > 0 ) {
			OscBundle bundle = _bundles.Pop();
			bundle.Clear();
			return bundle;
		}
		return new OscBundle();
	}


	class FasterMessageStack
	{
		public OscMessage[] messages = new OscMessage[ stackMessageCapacityStep ];
		public int count;

		public void Expand()
		{
			OscMessage[] newMessages = new OscMessage[ messages.Length + stackMessageCapacityStep ];
			Array.Copy( messages, 0, newMessages, 0, messages.Length );
			messages = newMessages;
		}
	}
}