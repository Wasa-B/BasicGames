using System.Collections;
using UnityEngine;
using Wasabi.Attribute;

namespace Wasabi
{


    [System.Serializable]
    public class Vector2W
    {
        public enum Type { Point, Player, Camera, Target }

        //static
        static Transform _player;
        static Transform _camera;
        static internal void SetPlayer(Transform _player) { Vector2W._player = _player; }
        static internal void SetCamera(Transform _camera) { Vector2W._camera = _camera; }

        static public Vector2W zero { get => new Vector2W(0, 0); }
        static public Vector2W one { get => new Vector2W(1, 1); }
        static public Vector2W player { get => new Vector2W(_player.position); }
        static public Vector2W camera { get => new Vector2W(_camera.position); }
        static public Vector2 ToPlayer(Vector2 vector2) { return _player ? (Vector2)_player.position - vector2 : Vector2.zero; }
        static public Vector2 ToCamera(Vector2 vector2) { return _camera ? (Vector2)_camera.position - vector2 : Vector2.zero; }
        //

        [SerializeField]
        Type _type;
        [ValueEqual(nameof(_type), Type.Point)]
        [SelectVector]
        [SerializeField]
        Vector2 _position;
        [ValueEqual(nameof(_type), Type.Target)]
        [SerializeField]
        Transform _target;

        [ValueEqual(nameof(_type), Type.Point, true)]
        [SerializeField]
        Vector2 _pivot;
        //
        public Vector2W() { }
        public Vector2W(float x, float y) { _position.x = x; _position.y = y; }
        public Vector2W(Vector2 vector) { _position = vector; }
        public float x { get { return GetVector().x; } set { _position.x = value; } }
        public float y { get { return GetVector().y; } set { _position.y = value; } }
        public float magnitude { get => GetVector().magnitude; }
        public Vector2 normalized { get => GetVector().normalized; }
        public Vector2 vector { get => GetVector(); }
        public Vector2 toPlayer { get => _player ? (Vector2)_player.position - GetVector() : Vector2.zero; }


        Vector2 GetVector()
        {
            switch (_type)
            {
                case Type.Point:
                    return _position;
                case Type.Player:
                    return _player ? (Vector2)_player.position + _pivot : Vector2.zero;
                case Type.Camera:
                    return _camera ? (Vector2)_camera.position + _pivot : Vector2.zero;
                case Type.Target:
                    return _target ? (Vector2)_target.position + _pivot : Vector2.zero;
            }

            return _position;
        }
        public float Distance(Vector2W _vectorw) => Distance(_vectorw.vector);
        public float Distance(Vector2 _vector) => Vector2.Distance(GetVector(), _vector);

        // operator

        public static Vector2 operator +(Vector2W a) => a.vector;
        public static Vector2 operator -(Vector2W a) => a.vector;
        public static Vector2W operator +(Vector2W a, Vector2W b)
        {
            Vector2W vector2W = new Vector2W();
            vector2W._position = a.vector + b.vector;
            return vector2W;
        }

        public static Vector2W operator +(Vector2W a, Vector2 b) => new Vector2W(a.vector + b);
        public static Vector2 operator +(Vector2 a, Vector2W b) => (a + b.vector);
        public static bool operator <(Vector2W a, Vector2W b) => (a.magnitude < b.magnitude);
        public static bool operator >(Vector2W a, Vector2W b) => a.magnitude > b.magnitude;
        public static bool operator true(Vector2W a) => a.vector != Vector2.zero;
        public static bool operator false(Vector2W a) => a.vector == Vector2.zero;
        public static bool operator !(Vector2W a) => a.vector == Vector2.zero;
    }
}