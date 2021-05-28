// TAC
// Strategy Game Architecture
// Copyright (c) 2020 Ted Brown

namespace Tac
{
	/// <summary>
	/// 
	/// </summary>
	public enum EffectOperation
	{
		Invalid = 0,

		/// <summary>
		/// Set a value, discarding any prior value.
		/// </summary>
		Set,

		/// <summary>
		/// Add to the prior value.
		/// </summary>
		Add,

		/// <summary>
		/// Subtract from the prior value.
		/// </summary>
		Subtract,

		/// <summary>
		/// Multiply the prior value by the layered effect's Modification.
		/// </summary>
		Multiply,

		/// <summary>
		/// Perform a bitwise "or" operation.
		/// </summary>
		BitwiseOr,

		/// <summary>
		/// Perform a bitwise "and" operation.
		/// </summary>
		BitwiseAnd,

		/// <summary>
		/// Perform a bitwise "exclusive or" operation.
		/// </summary>
		BitwiseXor
	}
}
