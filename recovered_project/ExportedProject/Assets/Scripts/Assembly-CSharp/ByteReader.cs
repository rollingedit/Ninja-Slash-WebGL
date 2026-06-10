using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class ByteReader
{
	private byte[] mBuffer;

	private int mOffset;

	public bool canRead
	{
		get
		{
			return mBuffer != null && mOffset < mBuffer.Length;
		}
	}

	public ByteReader(byte[] bytes)
	{
		mBuffer = bytes;
	}

	public ByteReader(TextAsset asset)
	{
		mBuffer = asset.bytes;
	}

	private static string ReadLine(byte[] buffer, int start, int count)
	{
		return Encoding.UTF8.GetString(buffer, start, count);
	}

	public string ReadLine()
	{
		int num = mBuffer.Length;
		while (mOffset < num && mBuffer[mOffset] < 32)
		{
			mOffset++;
		}
		int num2 = mOffset;
		if (num2 < num)
		{
			int num3;
			do
			{
				if (num2 < num)
				{
					num3 = mBuffer[num2++];
					continue;
				}
				num2++;
				break;
			}
			while (num3 != 10 && num3 != 13);
			string result = ReadLine(mBuffer, mOffset, num2 - mOffset - 1);
			mOffset = num2;
			return result;
		}
		mOffset = num;
		return null;
	}

	public Dictionary<string, string> ReadDictionary()
	{
		Dictionary<string, string> val = new Dictionary<string, string>();
		char[] array = new char[1] { '=' };
		while (canRead)
		{
			string text = ReadLine();
			if (text == null)
			{
				break;
			}
			string[] array2 = text.Split(array, 2, (StringSplitOptions)1);
			if (array2.Length == 2)
			{
				string text2 = array2[0].Trim();
				string text3 = array2[1].Trim();
				val[text2] = text3;
			}
		}
		return val;
	}
}
