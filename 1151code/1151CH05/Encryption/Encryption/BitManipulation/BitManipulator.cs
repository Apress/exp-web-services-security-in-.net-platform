using System;

namespace BitManipulation
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class BitManipulator
	{
		public static Int64 ShiftLeft(Int64 input, Int32 offset)
		{
			return input << offset;
		}

		public static Int32 ShiftLeft(Int32 input, Int32 offset)
		{
			return input << offset;
		}

		public static Int64 ShiftRight(Int64 input, Int32 offset)
		{
			return input >> offset;
		}

		public static Int32 ShiftRight(Int32 input, Int32 offset)
		{
			return input >> offset;
		}

		public static Int32 ExtractLowOrder(Int64 input)
		{
			return (Int32)(input & 0xFFFFFFFF);
		}

		public static Int32 ExtractHighOrder(Int64 input)
		{
			return (Int32)(input >> 32);
		}

		public static Int64 PromoteToInt64(Byte input, Int32 leftShift)
		{
			return (input & 0xffL) << leftShift;
		}

		public static Byte ExtractByte(Int64 input, Int32 bytePosition)
		{
			return (Byte)(input >> bytePosition);
		}

		public static Int64 Combine(Int32 v1, Int32 v2)
		{
			return ((v1 & 0x00000000ffffffffL) << 32) | (v2 & 0x00000000ffffffffL);
		}

	}
}
