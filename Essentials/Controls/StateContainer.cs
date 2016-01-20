using System.Collections.Generic;
using Xamarin.Forms;

namespace Controls
{
    [ContentProperty("Conditions")]
    public class StateContainer : ContentView
    {
        public List<StateCondition> Conditions { get; set; } = new List<StateCondition>();

        public static readonly BindableProperty StateProperty = BindableProperty.Create<StateContainer, object>(x => x.State, null, propertyChanged: StateChanged);

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
                if (stateCondition.State.ToString().Equals(newValue.ToString()))
                {
                    this.Content = stateCondition.Content;
                }
            }
        }
    }
}