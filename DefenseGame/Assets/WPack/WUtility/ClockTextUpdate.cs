public class ClockTextUpdate : TextUpdate
{
    public override void SetText(int _value)
    {
        base.SetText($"{(_value/6000).ToString("D2")} : {(_value/100 %60).ToString("D2")} : {(_value % 100).ToString("D2")}");
    }
}
