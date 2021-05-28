// TAC
// Strategy Game Architecture
// Copyright (c) 2020 Ted Brown

using Jambox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tac
{
	/// <summary>
	/// Couplet for attribute + value. Probably a dumb idea.
	/// </summary>
	public struct AttrQuery
	{
		public readonly AttributeKey AttributeKey;
		public readonly int AttributeId;
		public readonly int Value;

		public AttrQuery (int attribute, int value)
		{
			AttributeKey = (AttributeKey) attribute;
			AttributeId = attribute;
			Value = value;
		}

		public AttrQuery (AttributeKey attribute, int value)
		{
			AttributeKey = attribute;
			AttributeId = (int)attribute;
			Value = value;
		}
	}
}
