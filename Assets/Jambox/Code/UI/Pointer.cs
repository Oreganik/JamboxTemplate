// JAMBOX
// General purpose game code for Unity
// Copyright 2019 Ted Brown

using UnityEngine;

namespace Jambox
{
	// TODO: More elegant handling of holding button down too long on target without grab target
	// NOTE: To get BlockedByCanvas to work, add CanvasPointerCheck to all Canvas elements in your scene.
	public class Pointer : MonoBehaviour 
	{
		private enum State 
		{ 
			Seek, 	// Button up
			Inert,	// Button down with no target
			Active,	// Button down with target for less than duration
			Grab,	// Button down with target for more than duration
			BlockedByCanvas
		}

		public bool IsInert
		{
			get { return _state == State.Inert; }
		}

		public Collider HoveredCollider
		{
			get { return _hoveredCollider; }
		}

		public Transform GrabbedTransform
		{
			get { return _grabTarget != null ? _grabTarget.GetTransform() : null; }
		}

		public Transform HoveredTransform
		{
			get { return _hoveredCollider != null ? _hoveredCollider.transform : null; }
		}

		public Vector3 HitPoint
		{
			get { return _hitPoint; }
		}

		protected float ButtonDownDuration
		{
			get { return Time.timeSinceLevelLoad - _buttonDownTime; }
		}

		[SerializeField] protected LayerMask _layerMask;

		protected bool _isActive;
		protected Collider _hoveredCollider;
		protected float _buttonDownTime;
		protected float _maxClickDuration = 0.2f;
		protected IClickTarget _clickTarget;
		protected IGrabTarget _grabTarget;
		protected IHoverTarget _hoverTarget;
		private State _state;
		protected Vector3 _hitPoint;

		protected virtual Collider GetHoveredCollider () 
		{
			// Use the mouse pointer by default. Can also be camera center for reticle.
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 1000, _layerMask, QueryTriggerInteraction.Ignore))
			{
				_hitPoint = hit.point;
				return hit.collider;
			}
			return null; 
		}

		protected virtual bool IsPrimaryButtonDown () 
		{ 
			// Use the mouse by default
			return Input.GetMouseButton(0);
		}

		public void Activate (bool reset = true)
		{
			if (reset)
			{
				GoToInertState();
			}
			_state = State.Seek;
			_isActive = true;
		}

		public void Deactivate ()
		{
			GoToInertState();
			_isActive = false;
		}

		// clears the hovered object so it can re-hover
		public void Reset ()
		{
			_hoveredCollider = null;
			_clickTarget = null;
			_grabTarget = null;
			_hoverTarget = null;
			_state = State.Seek;
		}

		private void GoToInertState ()
		{
			if (_hoverTarget != null)
			{
				_hoverTarget.HandleUnhover(this);
			}

			_hoveredCollider = null;
			_clickTarget = null;
			_grabTarget = null;
			_hoverTarget = null;

			_state = State.Inert;
		}

		protected void Awake ()
		{
			Activate();
		}

		protected void Update ()
		{
			if (_isActive == false)
			{
				return;
			}

			if (CanvasPointerCheck.IsOverCanvas)
			{
				GoToInertState();
				_state = State.BlockedByCanvas;
				return;
			}
			// We're no longer pointed at a canvas: go to Seek state
			else if (_state == State.BlockedByCanvas)
			{
				_state = State.Seek;
			}

			// Store previous state
			Collider previousHoveredCollider = _hoveredCollider;
			IClickTarget previousClickTarget = _clickTarget;
			IGrabTarget previousGrabTarget = _grabTarget;
			IHoverTarget previousHoverTarget = _hoverTarget;

			bool isPrimaryButtonDown = IsPrimaryButtonDown();

			// An inert state is triggered when the button is pressed without a target
			if (_state == State.Inert)
			{
				if (isPrimaryButtonDown == false)
				{
					// Continue with normal Seek behavior
					_state = State.Seek;
				}
				else
				{
					// Do nothing and clear current state
					_hoveredCollider = null;
					_clickTarget = null;
					_grabTarget = null;
					_hoverTarget = null;
					return;
				}
			}

			// Always identify the object under the pointer
			_hoveredCollider = GetHoveredCollider();

			if (_hoveredCollider)
			{
				// note that these might be null
				_clickTarget = _hoveredCollider.transform.GetComponentInParent<IClickTarget>();
				_hoverTarget = _hoveredCollider.transform.GetComponentInParent<IHoverTarget>();
			}
			else
			{
				_clickTarget = null;
				_hoverTarget = null;
			}

			// Hover / Unhover
			if (_state == State.Seek)
			{
				// if we have a new target, unhover the old and hover the new
				if (_hoverTarget != previousHoverTarget)
				{
					if (previousHoverTarget != null)
					{
						previousHoverTarget.HandleUnhover(this);
					}
					if (_hoverTarget != null)
					{
						_hoverTarget.HandleHover(this);
					}
				}

				if (isPrimaryButtonDown)
				{
					if (_hoveredCollider == null)
					{
						GoToInertState();
					}
					else
					{
						_buttonDownTime = Time.timeSinceLevelLoad;
						_state = State.Active;
					}
				}

				return;
			}

			// Button is down. Click, Grab, or Cancel.
			// NOTE: This means it is possible for objects to move between the pointer and the target and interrupt events.
			if (_state == State.Active)
			{
				// Did the hovered collider change? Then unhover and cancel.
				if (_hoveredCollider != previousHoveredCollider)
				{
					if (previousHoverTarget != null)
					{
						previousHoverTarget.HandleUnhover(this);
					}
					GoToInertState();
					return;
				}

				// Still trying to click (mouse up within duration on same collider)
				if (ButtonDownDuration < _maxClickDuration)
				{
					// Try to click, then return to Seek
					if (isPrimaryButtonDown == false)
					{
						if (_clickTarget != null && _clickTarget == previousClickTarget)
						{
							_clickTarget.HandleClick(this);
							_clickTarget = null;
							_state = State.Seek;
						}
					}
				}
				// Button is still down, we are moving to grab state if we have a valid target
				else
				{
					_grabTarget = _hoveredCollider.transform.GetComponentInParent<IGrabTarget>();
					if (_grabTarget != null && _grabTarget == previousGrabTarget)
					{
						_grabTarget.HandleGrab(this);
						_state = State.Grab;
					}
					else
					{
						GoToInertState();
					}
				}

				return;
			}

			// The grabbed object has the reins at this point. We're just waiting for primary button up.
			if (_state == State.Grab)
			{
				if (isPrimaryButtonDown)
				{
					// the target was destroyed, bail.
					if (previousGrabTarget == null)
					{
						GoToInertState();
						return;
					}
				}
				else // release
				{
					// the target was destroyed, bail.
					if (previousGrabTarget != null)
					{
						previousGrabTarget.HandleRelease(this);
					}
					_state = State.Seek;
					return;
				}
			}

			Debug.LogError("Pointer fell through to unknown state " + _state.ToString());
		}
	}
}
