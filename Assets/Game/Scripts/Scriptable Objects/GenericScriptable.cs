using Animancer;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Game.Forms
{
    public abstract class GenericScriptable : ScriptableObject
    {
        [MyBox.Separator][Header("Generic Data")]
        [SerializeField] protected string _name;
        [SerializeField] protected int _cost;
        [SerializeField] protected Sprite _sprite;

        public Sprite Sprite { get { return _sprite; } }
        public string Name { get { return _name; } }
        public int Cost { get { return _cost; } }

        public abstract GameObject CreateObject(Transform _parent, Vector3 _position);
    }
}