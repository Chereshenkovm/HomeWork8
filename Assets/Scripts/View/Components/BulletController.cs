using System;
using UnityEngine;

namespace View.Components
{
	public class BulletController : MonoBehaviour
	{
		public event Action<BulletController, IBulletTarget> TargetReachedEvent;
		public float Speed = 20f;

		private Rigidbody _rigidbody;

		private void Awake () 
		{
			_rigidbody = GetComponent<Rigidbody>();
		}

		private void OnEnable()
		{
			_rigidbody.velocity = transform.forward * Speed;
		}

		private void OnCollisionEnter(Collision collision)
		{
			var target = collision.gameObject.GetComponentInParent<IBulletTarget>();
			if(target != null)
			{
				TargetReachedEvent?.Invoke(this, target);
			}
		}
	}
}
