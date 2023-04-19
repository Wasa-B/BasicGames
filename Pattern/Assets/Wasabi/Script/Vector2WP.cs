using System.Collections.Generic;
using UnityEngine;
using Wasabi.Attribute;
namespace Wasabi
{
    [System.Serializable]
    public class Vector2WP
    {

        public List<Vector2W> _positions;
        public enum PositionType { Random, Near, Far }
        public PositionType _positionType = PositionType.Near;

        public Vector2W GetRandom() => _positions[Random.Range(0, _positions.Count)];

        public Vector2W GetNear(Vector2W _vecw) => GetNear(_vecw.vector);
        public Vector2W GetNear(Vector2 _vector)
        {
            int index = -1;
            for(int i = 0; i < _positions.Count; i++)
            {
                if (index < 0) index = i;
                else if(_positions[index].Distance(_vector) > _positions[i].Distance(_vector)) index = i;
            }
            return _positions[index];
        }

        public Vector2W GetFar(Vector2W _vecw , bool without = false) => GetFar(_vecw.vector, without);
        public Vector2W GetFar(Vector2 _vec, bool without = false)
        {
            int index = -1;
            for(int i =0; i < _positions.Count; i++)
            {

                if (index < 0) index = i;
                else if (without && _vec == _positions[index].vector) continue;
                else if (_positions[index].Distance(_vec) > _positions[i].Distance(_vec)) index = i;
            }
            return _positions[index];
        }

    }
}