using System.Collections.Generic;

namespace Xamarin.Forms.Essentials.Controls
{
    [ContentProperty("Conditions")]
    [Preserve(AllMembers = true)]
    public class StateContainer : ContentView
    {
        public List<StateCondition> Conditions { get; set; } = new List<StateCondition>();

        public static readonly BindableProperty StateProperty = BindableProperty.Create<StateContainer, object>(x => x.State, null, propertyChanged: StateChanged);

        public static void Init()
        {
            //for linker
        }

        private static void StateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var parent = bindable as StateContainer;
            parent?.ChooseStateProperty(newValue);
        }

        public object State
        {
            get { return this.GetValue(StateProperty); }
            set { this.SetValue(StateProperty, value); }
        }

        private void ChooseStateProperty(object newValue)
        {
            foreach (StateCondition stateCondition in Conditions)
            {
                if (stateCondition.Is != null)
                {
                    if (stateCondition.Is.ToString().Equals(newValue.ToString()))
                    {
                        this.Content = stateCondition.Content;
                    }
                }
                else if (stateCondition.IsNot != null)
                {
                    if (!stateCondition.IsNot.ToString().Equals(newValue.ToString()))
                    {
                        this.Content = stateCondition.Content;
                    }
                }
            }
        }
    }
}