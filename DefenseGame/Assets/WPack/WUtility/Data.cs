using UnityEngine.Events;
namespace WUtility
{

    [System.Serializable]
    public class Data<T>
    {
        public UnityEvent<T> OnChange;

        protected T _value;
        public T value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnChange?.Invoke(_value);
            }
        }

        public Data() { }
        public Data(T data) { value = data;}

        public static implicit operator T(Data<T> data) { return data.value; }
    }

    [System.Serializable]
    public class DataInt : Data<int>
    {
        public DataInt(int data=0) { value = data;}

        public static DataInt operator ++(DataInt d)
        {
            d.value++;
            return d;
        }
        public static DataInt operator --(DataInt d)
        {
            d.value--;
            return d;
        }
    }

    [System.Serializable]
    public class DataFloat : Data<float>
    {
        public DataFloat(float data = 0) { value = data;}
    }

    [System.Serializable]
    public class DataStr : Data<string>
    {
        public DataStr(string data="") { value = data;}
    }
}