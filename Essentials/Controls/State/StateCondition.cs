namespace Xamarin.Forms.Essentials.Controls
{
    [ContentProperty("Content")]
    [Preserve(AllMembers = true)]
    public class StateCondition : View
    {
        public object Is { get; set; }
        public object IsNot { get; set; }
        public View Content { get; set; }
    }
}
