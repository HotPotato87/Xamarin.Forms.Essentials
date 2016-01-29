using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Xamarin.Forms.Essentials.Controls
{
    public partial class SidePanelControl : ContentView
    {
        #region Public Setters

        //todo : turn into bindable properties
        public View SidePanelView
        {
            set
            {
                RightSideContent.Children.Add(value);
            }
        }

        public View MainContentView
        {
            set
            {
                LeftSideContent.Children.Add(value);
            }
        }

        public IList<object> ToolBarItems { get; set; } = new List<object>();

        #endregion
        
        #region Bindable Properties

        public static readonly BindableProperty DepthProperty =
            BindableProperty.Create<SidePanelControl, EnumPanelDepth>(x => x.Depth, EnumPanelDepth.Beside);

        public EnumPanelDepth Depth
        {
            get { return (EnumPanelDepth) this.GetValue(DepthProperty); }
            set { this.SetValue(DepthProperty, value); }
        }

        public static readonly BindableProperty SidePanelPercentageProperty =
            BindableProperty.Create<SidePanelControl, double>(x => x.SidePanelPercentage, 0.8);

        public double SidePanelPercentage
        {
            get { return (double) this.GetValue(SidePanelPercentageProperty); }
            set { this.SetValue(SidePanelPercentageProperty, value); }
        }

        public static readonly BindableProperty SlideDirectionProperty =
            BindableProperty.Create<SidePanelControl, EnumSlideDirection>(x => x.SlideDirectionFrom,
                EnumSlideDirection.Left);

        public EnumSlideDirection SlideDirectionFrom
        {
            get { return (EnumSlideDirection) this.GetValue(SlideDirectionProperty); }
            set { this.SetValue(SlideDirectionProperty, value); }
        }

        #endregion

        #region Instance Variables

        private MainState State { get; set; } = MainState.Closed;
        private bool _initialized = false;
        private double _distanceToMove;
        private double _originalSidePanelOffset = 0;

        #endregion


        public SidePanelControl()
        {
            InitializeComponent();
        }

        private void AddCurrentToolBarItems()
        {
            foreach (var toggleItem in this.ToolBarItems.OfType<ContentView>())
            {
                AddToolBarItem(toggleItem);
            }
            
        }

        private void AddToolBarItem(ContentView toggleItem)
        {
            toggleItem.GestureRecognizers.Add(new TapGestureRecognizer(view => ToggleSidePanel()));
            this.LeftToolBarItems.Children.Add(toggleItem);
        }

        private void ArrangeContent()
        {
            if (this.Width <= 0) return;

            //calculate how much we will move the slide by, and apply the sizings to the grid columns
            _distanceToMove = this.Width * SidePanelPercentage;

            //set hidden content to distance and crop it to size
            var sidePanel = RightSideContent;
            sidePanel.WidthRequest = _distanceToMove;

            //position the hidden content
            if (this.SlideDirectionFrom == EnumSlideDirection.Left)
            {
                if (Depth == EnumPanelDepth.Overlay)
                {
                    AbsoluteLayout.SetLayoutBounds(RightSideContent, new Rectangle(-_distanceToMove, 0, sidePanel.Width, this.Height));
                }
                else
                {

                    //move to the left, by % of side panel
                    RightSideContent.TranslationX = -_distanceToMove;
                }
            }
            else
            {
                if (Depth == EnumPanelDepth.Overlay)
                {
                    AbsoluteLayout.SetLayoutBounds(RightSideContent, new Rectangle(this.Width, 0, sidePanel.Width, this.Height));
                }
                else
                {
                    //move to the right completely
                    RightSideContent.TranslationX = this.Width;
                }
            }

            _originalSidePanelOffset = AbsoluteLayout.GetLayoutBounds(RightSideContent).X;

            //set up toolbar item to correct grid
            this.SetupToolBarItems();
            
            GetMainContentGrid().GestureRecognizers.Add(new TapGestureRecognizer(ToggleIfOpen));
        }

        

        private void SetupToolBarItems()
        {
            var mainContent = GetMainContentGrid();
            mainContent.RowDefinitions.Add(new RowDefinition() { Height = 50 });
            mainContent.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            BesideAnimationGrid.Children.Remove(this.LeftToolBarItems);
            mainContent.Children.Add(this.LeftToolBarItems);
            
            Grid.SetRow(LeftToolBarItems, 0);
            Grid.SetRowSpan(mainContent.Children[0], 2);
        }

        //todo : toggle from bindable property too
        public void ToggleSidePanel()
        {
            //decide to slide from left or right
            double travelDistance = SlideDirectionFrom == EnumSlideDirection.Left ?
                _distanceToMove :
                -_distanceToMove;

            //decide to overlay the sidepanel or not
            View itemToAnimate = GetItemToAnimate();

            var currentPositioningX = GetCurrentItemPositiion(itemToAnimate);

            if (State == MainState.Closed)
            {
                itemToAnimate.Animate("OpenSidePanel", UpdateGridTranslation,
                    start: currentPositioningX,
                    end: currentPositioningX + travelDistance,
                    easing: Easing.CubicOut);
                State = MainState.Open;
            }
            else
            {
                itemToAnimate.Animate("CloseSidePanel", UpdateGridTranslation,
                   start: currentPositioningX,
                   end: _originalSidePanelOffset,
                   finished: (d, b) => {},
                   easing: Easing.CubicOut);
                State = MainState.Closed;
            }
        }

        private double GetCurrentItemPositiion(View itemToAnimate)
        {
            return this.Depth == EnumPanelDepth.Overlay
                ? AbsoluteLayout.GetLayoutBounds(itemToAnimate).X
                : itemToAnimate.TranslationX;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            Init();
            ArrangeContent();
        }

        private void Init()
        {
            if (!_initialized && ToolBarItems.Any())
            {
                AddCurrentToolBarItems();
                _initialized = true;
            }
        }

        private View GetItemToAnimate()
        {
            return Depth == EnumPanelDepth.Beside ?
                (View)BesideAnimationGrid :
                GetHiddenContentGrid();
        }

        private Grid GetMainContentGrid()
        {
            return LeftSideContent;
        }

        private Grid GetHiddenContentGrid()
        {
            return RightSideContent;
        }

        private void ToggleIfOpen(View obj)
        {
            if (this.State == MainState.Open)
            {
                ToggleSidePanel();
            }
        }

        private void UpdateGridTranslation(double newValue)
        {
            var item = GetItemToAnimate();

            if (Depth == EnumPanelDepth.Overlay)
            {
                AbsoluteLayout.SetLayoutBounds(item, new Rectangle(newValue, 0, item.Width, item.Height));
            }
            else
            {
                item.TranslationX = newValue;   
            }
        }

        private enum MainState
        {
            Open,
            Closed
        }

        public enum EnumPanelDepth
        {
            Beside,
            Overlay
        }

        public enum EnumSlideDirection
        {
            Left,
            Right
        }
    }
}