namespace Xamarin.Forms.Essentials.Controls
{
    [System.AttributeUsage(System.AttributeTargets.All)]
    public class PreserveAttribute : System.Attribute
    {
        public PreserveAttribute() { }
        public bool Conditional { get; set; }
        public bool AllMembers { get; set; }
    }
}