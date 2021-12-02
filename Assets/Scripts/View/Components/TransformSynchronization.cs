using Photon.Pun;
using UnityEngine;

namespace View.Components
{
    public class TransformSynchronization : MonoBehaviour, IPunObservable
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private PhotonView _photonView;

        private bool _firstData = true;
        private Vector3 _correctPosition = Vector3.zero;
        private Quaternion _correctRotation = Quaternion.identity;
        private Vector3 _correctVelocity = Vector3.zero;


        public void Update()
        {
            if (_photonView.IsMine)
                return;
            
            transform.position = Vector3.Lerp(transform.position, _correctPosition, Time.deltaTime * 5f);
            transform.rotation = Quaternion.Lerp(transform.rotation, _correctRotation, Time.deltaTime * 5f);
            _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, _correctVelocity, Time.deltaTime * 5f);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
                stream.SendNext(_rigidbody.velocity);
            }
            else
            {
                _correctPosition = (Vector3)stream.ReceiveNext();
                _correctRotation = (Quaternion)stream.ReceiveNext();
                _correctVelocity = (Vector3)stream.ReceiveNext() * 0.5f;

                if (_firstData)
                {
                    transform.position = _correctPosition;
                    transform.rotation = _correctRotation;
                    _rigidbody.velocity = _correctVelocity;
                    _firstData = false;
                }
            }
        }
    }
}