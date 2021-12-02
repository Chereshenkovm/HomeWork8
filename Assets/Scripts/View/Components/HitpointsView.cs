using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

namespace View.Components
{
	public class HitpointsView : MonoBehaviour, IPunObservable
	{
		public RotationConstraint RotationConstraint;
	
		public RectTransform Container;
		public RectTransform Line;
		public Text PlayerName;
	
		private float _value;
		private float _newValue;

		private void Update()
		{
			if (Math.Abs(_value - _newValue) < Mathf.Epsilon)
				return;

			SetValue(_newValue);
		}

		public void SetValue(float value)
		{
			_value = value;
			_newValue = value;
		
			Vector2 v = Container.sizeDelta;
			v.x = Mathf.Max(0.0f, v.x * value);
			Line.sizeDelta = v;
		}

		public void SetPlayerName(string playerName)
		{
			PlayerName.text = playerName;
		}
	
		public void SetRotationConstraint(Transform point)
		{
			RotationConstraint.AddSource(new ConstraintSource {sourceTransform = point, weight = 1});
		}

		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			if (stream.IsWriting)
			{
				stream.SendNext(_value);
			}
			else
			{
				_newValue = (float) stream.ReceiveNext();
			}
		}
	}
}