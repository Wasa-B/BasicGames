namespace TYCOON
{
    /*
     * 재료, 평점 
     */


    [System.Serializable]
    public class Item
    {
        public enum ItemType {MATERIAL, PRODUCT}

        string name = "Defalut";
        string text = "Defalut Text";
        int price = 1;
        int value = 1;


        public Item() { }
        public Item(string name, string text, int price, int value)
        {
            this.name = name;
            this.text = text;
            this.price = price;
            this.value = value;
        }

        public string Name {get { return name; }}
        public string Text {get { return text; }}
        public int Price {get { return price; }}
        public int Value {get { return value; }}

    }

}