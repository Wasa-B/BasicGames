using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Wasabi
{
    public static class W_Input
    {
        static InputActionAsset currentAction;


        static Dictionary<string, List<string>> _keyboardPath = new Dictionary<string, List<string>>();

        internal static void SetInputAction(InputActionAsset inputAction)
        {
            currentAction = inputAction;
        }

        internal static void GetKeyboardList()
        {
            if (currentAction != null)
            {
                
                var maps = currentAction.actionMaps.ToArray();
                foreach (var map in maps)
                {
                    Debug.Log(map.name);
                    var actions = map.actions.ToArray();
                    foreach (var action in actions)
                    {
                        Debug.Log(map.name + " : " + action.name);
                        var bindings = action.bindings.ToArray();
                        foreach (var binding in bindings)
                        {
                            Debug.Log(map.name + " : " + action.name + " : " + binding.name + ", " + binding.path);
                            if (binding.path.Contains("Keyboard"))
                            {

                                if (_keyboardPath.ContainsKey(binding.name))
                                {
                                    if (_keyboardPath[binding.name].Contains(PathConvert(binding.path)) == false)
                                    {
                                        _keyboardPath[binding.name].Add(PathConvert(binding.path));
                                    }
                                }
                                else
                                {
                                    var paths = new List<string>();
                                    paths.Add(PathConvert(binding.path));
                                    _keyboardPath.Add(binding.name, paths);
                                }
                            }
                        }
                    }
                }
            }
            Debug.Log(Keyboard.current.rightArrowKey.path);
        }

        static string PathConvert(string _path)
        {
            return _path.Replace("<Keyboard>", "");
        }

        internal static void GetDown()
        {
            if (currentAction != null)
            {
                var keys = _keyboardPath.Keys;
                foreach (var key in keys)
                {
                    var paths = _keyboardPath[key];
                    foreach (var path in paths)
                    {
                        Debug.Log(key + " is Down : " + Keyboard.current[path].IsPressed() + " ("+path+")");
                    }
                }
            }
        }
        internal static bool GetKey(string keyname)
        {
            
            if (_keyboardPath.ContainsKey(keyname))
            {
                var paths = _keyboardPath[keyname];
                foreach (var path in paths)
                    if(Keyboard.current[path].IsPressed()) return true;
            }
            return false;
        }
    }
}