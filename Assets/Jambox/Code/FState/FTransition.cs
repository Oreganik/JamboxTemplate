// JAMBOX
// General purpose game code for Unity
// Copyright 2020 Ted Brown

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jambox
{
    public class FTransition
    {
		public Enum Trigger;
		public Enum FromState;
		public Enum NextState;

		public FTransition (Enum trigger, Enum fromState, Enum toState)
		{
			Trigger = trigger;
			FromState = fromState;
			NextState = toState;
		}

		public override string ToString()
		{
			return string.Format("[{0}] {1} -> {2}", Trigger, FromState, NextState);
		}
    }
}
