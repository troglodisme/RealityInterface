/*
	StringBuilderExtensions for garbage free Appends.

	Note that although these methods avoid generating garbage, they may be slower in practise.

	Original code by Gavin Pugh 9th March 2010.
		Copyright (c) Gavin Pugh 2010 - Released under the zlib license (http://www.opensource.org/licenses/zlib-license.php)
		https://www.gavpugh.com/2010/04/01/xnac-avoiding-garbage-when-working-with-stringbuilder/

	Modified and further extended by Carl Emil Carlsen
		Copyright (c) Carl Emil Carlsen 2020-2022
 */

using System.Text;
using UnityEngine;

namespace OscSimpl
{
	public static class StringBuilderExtensions
	{
		static readonly char[] digits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
		const int base10 = 10;


		public static StringBuilder AppendGarbageFree( this StringBuilder sb, uint value, bool includeLiteralPostFix = true )
		{
			const char postFix = 'u';

			// Zero case.
			if( value == 0 ) {
				sb.Append( '0' );

			} else {
				// Count length and adapt string builder.
				int count = 0;
				uint tmp = value;
				while( tmp > 0 ) {
					tmp /= base10;
					count++;
				}
				sb.Append( '0', count );

				// Add digits in reverse order.
				int pos = sb.Length;
				while( count > 0 ) {
					pos--;
					sb[ pos ] = digits[ value % base10 ];
					value /= base10;
					count--;
				}
			}

			return includeLiteralPostFix ? sb.Append( postFix ) : sb;
		}


		public static StringBuilder AppendGarbageFree( this StringBuilder sb, int value )
		{
			// Zero case.
			if( value == 0 ) return sb.Append( '0' );

			// Handle negative values.
			uint uValue;
			if( value < 0 ) {
				sb.Append( '-' );
				uValue = uint.MaxValue - ( (uint) value ) + 1;
			} else {
				uValue = (uint) value;
			}

			// Count length and adapt string builder.
			int count = 0;
			uint tmp = uValue;
			while( tmp > 0 ) {
				tmp /= base10;
				count++;
			}
			sb.Append( '0', count );

			// Add digits in reverse order.
			int pos = sb.Length;
			while( count > 0 ) {
				pos--;
				sb[ pos ] = digits[ uValue % base10 ];
				uValue /= base10;
				count--;
			}

			return sb;
		}


		public static StringBuilder AppendGarbageFree( this StringBuilder sb, ulong value, bool includeLiteralPostFix = true )
		{
			const string postFix = "ul";

			if( value == 0 ){
				// Zero case.
				sb.Append( '0' );

			} else {
				// Count length and adapt string builder.
				int count = 0;
				ulong tmp = value;
				while( tmp > 0 ) {
					tmp /= base10;
					count++;
				}
				sb.Append( '0', count );

				// Add digits in reverse order.
				int pos = sb.Length;
				while( count > 0 ) {
					pos--;
					sb[ pos ] = digits[ value % base10 ];
					value /= base10;
					count--;
				}
			}

			return includeLiteralPostFix ? sb.Append( postFix ) : sb;
		}


		public static StringBuilder AppendGarbageFree( this StringBuilder sb, long value, bool includeLiteralPostFix = true )
		{
			const char postFix = 'l';

			if( value == 0 ){
				// Zero case.
				sb.Append( '0' );

			} else {
				// Handle negative values.
				ulong uValue;
				if( value < 0 ) {
					sb.Append( '-' );
					uValue = ulong.MaxValue - ( (ulong) value ) + 1;
				} else {
					uValue = (ulong) value;
				}

				// Count length and adapt string builder.
				int count = 0;
				ulong tmp = uValue;
				while( tmp > 0 ) {
					tmp /= base10;
					count++;
				}
				sb.Append( '0', count );

				// Add digits in reverse order.
				int pos = sb.Length;
				while( count > 0 ) {
					pos--;
					sb[ pos ] = digits[ uValue % base10 ];
					uValue /= base10;
					count--;
				}
			}

			return includeLiteralPostFix ? sb.Append( postFix ) : sb;
		}


		public static StringBuilder AppendGarbageFree( this StringBuilder sb, float value, uint decimalCount = 2, bool includeLiteralPostFix = true )
		{
			const char postFix = 'f';

			if( value == 0f ) {
				// Zero case.
				sb.Append( '0' );

			} else {
				// Work on absolute value.
				float absValue;
				if( value > 0f ) {
					absValue = value;
				} else {
					absValue = -value;
					sb.Append( '-' );
				}

				// First part is easy, just cast to an integer.
				int absLongPart = (int) absValue;
				sb.AppendGarbageFree( absLongPart, false );

				// Work out remainder we need to print after the dot.
				float absRemainder = absValue - absLongPart;
				if( absRemainder > 0f )
				{
					int zeros = 0;
					while( decimalCount > 0 ) {
						absRemainder *= 10;
						decimalCount--;
						if( decimalCount > 0 && absRemainder < 0.9999999f ) zeros++;
					}

					sb.Append( '.' ).Append( '0', zeros );

					uint remainedUint = (uint) ( absRemainder + 0.5f ); // Round up.
					while( remainedUint > 0 && remainedUint - ( remainedUint / base10 ) * base10 == 0 ) remainedUint /= 10;
					if( remainedUint > 0 ) sb.AppendGarbageFree( remainedUint, false );
				}
			}
			
			return includeLiteralPostFix ? sb.Append( postFix ) : sb;
		}


		public static StringBuilder AppendGarbageFree( this StringBuilder sb, double value, uint decimalCount = 2, bool includeLiteralPostFix = true )
		{
			const char postFix = 'd';

			AppendGarbageFree( sb, (float) value, decimalCount, includeLiteralPostFix: false );
			if( includeLiteralPostFix ) sb.Append( postFix );
			return sb;
		}


		public static StringBuilder AppendGarbageFree( this StringBuilder sb, Color32 value )
		{
			return sb.Append( "RGBA(" )
				.AppendGarbageFree( (uint) value.r, false ).Append( ',' )
				.AppendGarbageFree( (uint) value.g, false ).Append( ',' )
				.AppendGarbageFree( (uint) value.b, false ).Append( ',' )
				.AppendGarbageFree( (uint) value.a, false ).Append( ')' );
		}
	}
}