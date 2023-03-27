/*
	Copyright © Carl Emil Carlsen 2019-2021
	http://cec.dk

	Note on targeting public variables.
		For targeting properties, we are using a fast technique using delegates.
		For targeting fields, the same technique is not possible. Instead it is recommended to use
		Exporession (System.Linq.Expressions) or Dynamic Method (System.Reflection.Emit). Both are
		not well supported on all platforms, so I've decided to not implement targeting of fields for now.
		https://forum.unity.com/threads/notsupportedexception-with-log-detail.543644/
		https://forum.unity.com/threads/are-c-expression-trees-or-ilgenerator-allowed-on-ios.489498/
*/

using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OscSimpl
{
	/// <summary>
	/// An Entry holds a reference to one method on one targetObject. If the targetObject is a Component,
	/// then targetGameObject will also be set.
	/// </summary>
	[Serializable]
	public class OscMappingEntry
	{
		/// <summary>
		/// If target object is a Component, then this will hold the GameObject it is attatched to. Optional.
		/// </summary>
		public GameObject targetGameObject;

		/// <summary>
		/// The object of which the target method is a member.
		/// </summary>
		public Object serializedTargetObject;

		/// <summary>
		/// Alternatively to the above; a non-serialiazed runtime object of which the target method is a member.
		/// </summary>
		public object nonSerializedTargetObject;

		/// <summary>
		/// Name of nonSerializedTargetObject.
		/// </summary>
		public string nonSerializedTargetObjectName;

		/// <summary>
		/// Name of target method. If the target method is a property, it is the set method.
		/// </summary>
		public string targetMethodName;

		/// <summary>
		/// The parameter type of target method. For Impulse, Null and Empty OSCMessages this string is empty.
		/// </summary>
		public string targetParamAssemblyQualifiedName;


		public object targetObject {
			get {
				if( serializedTargetObject != null ) return serializedTargetObject;
				return nonSerializedTargetObject;
			}
		}


		public OscMappingEntry() { }
		public OscMappingEntry( object targetObject, string targetMethodName, string targetParamAssemblyQualifiedName )
		{
			if( targetObject is Object ){
				serializedTargetObject = targetObject as Object;
				if( targetObject is Component ) targetGameObject = ( targetObject as Component ).gameObject;
			} else {
				nonSerializedTargetObject = targetObject;
				nonSerializedTargetObjectName = targetObject != null ? targetObject.GetType().Name : string.Empty;
			}

			this.targetMethodName = targetMethodName;
			this.targetParamAssemblyQualifiedName = targetParamAssemblyQualifiedName;
		}
	}
}