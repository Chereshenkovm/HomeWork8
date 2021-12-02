using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

namespace Game
{
    public class BonusManager : MonoBehaviour, IPunObservable
    {
        public event Action ChangedEventFast;
        public event Action ChangedEventFreeze;
        private bool _hasChangesFastID;
        private bool _hasChangesFreezeID;
        private bool _hasChangesFireUpID;

        public List<int> FastID = new List<int>();
        public List<int> FreezeID = new List<int>();
        public List<int> FireUpID = new List<int>();
        
        public void SetBonusEffect(int id, BonusEffect _bonusEffect)
        {
            switch (_bonusEffect)
            {
                case BonusEffect.Freeze:
                    if (FreezeID == null)
                    {
                        FreezeID.Add(id);
                    }
                    else if (!FreezeID.Contains(id))
                    {
                        FreezeID.Add(id);
                    }
                    _hasChangesFreezeID= true;
                    break;
                case BonusEffect.Fast:
                    if (FastID == null)
                    {
                        FastID.Add(id);
                    }
                    else if (!FastID.Contains(id))
                    {
                        FastID.Add(id);
                    }
                    _hasChangesFastID = true;
                    break;
                case BonusEffect.FireUp:
                    if (FireUpID == null)
                    {
                        FireUpID.Add(id);
                    }
                    else if (!FireUpID.Contains(id))
                    {
                        FireUpID.Add(id);
                    }
                    _hasChangesFireUpID = true;
                    break;
            }
        }

        public void DeleteBonusEffect(int id, BonusEffect _bonusEffect)
        {
            switch (_bonusEffect)
            {
                case BonusEffect.Freeze:
                    if (FreezeID == null || !FreezeID.Contains(id))
                    {
                        return;
                    }
                    else if (FreezeID.Contains(id))
                    {
                        FreezeID.RemoveAt(FreezeID.IndexOf(id));
                    }
                    _hasChangesFreezeID= true;
                    break;
                case BonusEffect.Fast:
                    if (FastID == null || !FastID.Contains(id))
                    {
                        return;
                    }
                    else if (FastID.Contains(id))
                    {
                        FastID.RemoveAt(FastID.IndexOf(id));
                    }
                    _hasChangesFastID = true;
                    break;
                case BonusEffect.FireUp:
                    if (FireUpID == null || !FireUpID.Contains(id))
                    {
                        return;
                    }
                    else if (FireUpID.Contains(id))
                    {
                        FireUpID.RemoveAt(FireUpID.IndexOf(id));
                    }
                    _hasChangesFireUpID = true;
                    break;
            }
        }
        
        public void Update()
        {
            if (_hasChangesFastID)
            {
                _hasChangesFastID = false;
                ChangedEventFast?.Invoke();
            }else if (_hasChangesFreezeID)
            {
                _hasChangesFreezeID = false;
                ChangedEventFreeze?.Invoke();
            }else if (_hasChangesFireUpID)
            {
                _hasChangesFireUpID = false;
                //ChangedEvent?.Invoke();
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                foreach (var id in FastID)
                {
                    stream.SendNext(id);
                }
                foreach (var id in FreezeID)
                {
                    stream.SendNext(id);
                }
                foreach (var id in FireUpID)
                {
                    stream.SendNext(id);
                }
            }
            else
            {
                int[] newId1 = new int[0];
                foreach (var id in FastID)
                {
                    newId1.Append((int) stream.ReceiveNext());
                }

                FastID.Clear();
                foreach (var id in newId1)
                {
                    _hasChangesFastID = true;
                    FastID.Append(id);
                }
                
                int[] newId2 = new int[0];
                foreach (var id in FreezeID)
                {
                    newId2.Append((int) stream.ReceiveNext());
                }
                FreezeID.Clear();
                foreach (var id in newId2)
                {
                    _hasChangesFreezeID = true;
                    FreezeID.Append(id);
                }

                int[] newId3 = new int[0];
                foreach (var id in FireUpID)
                {
                    newId3.Append((int) stream.ReceiveNext());
                }
                FireUpID.Clear();
                foreach (var id in newId3)
                {
                    _hasChangesFireUpID = true;
                    FireUpID.Append(id);
                }
            }
        }

        public enum BonusEffect
        {
            Fast,
            Freeze,
            FireUp
        }
    }
}
