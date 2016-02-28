using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms.Essentials.Controls.Extentions;

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
            get
            {
                return LeftSideContent.Children[0];
            }
            set
            {
                LeftSideContent.Children.Add(value);
            }
        }

        public double SlideGestureWidth { get; set; } = 25;
        public IList<object> ToolBarItems { get; set; } = new List<object>();

        #endregion

        #region Bindable Properties

        public static readonly BindableProperty DepthProperty =
            BindableProperty.Create<SidePanelControl, EnumPanelDepth>(x => x.Depth, EnumPanelDepth.Beside);

        public EnumPanelDepth Depth
        {
            get { return (EnumPanelDepth)this.GetValue(DepthProperty); }
            set { this.SetValue(DepthProperty, value); }
        }

        public static readonly BindableProperty SidePanelPercentageProperty =
            BindableProperty.Create<SidePanelControl, double>(x => x.SidePanelPercentage, 0.8);

        public double SidePanelPercentage
        {
            get { return (double)this.GetValue(SidePanelPercentageProperty); }
            set { this.SetValue(SidePanelPercentageProperty, value); }
        }

        public static readonly BindableProperty SlideDirectionProperty =
            BindableProperty.Create<SidePanelControl, EnumSlideDirection>(x => x.SlideDirectionFrom,
                EnumSlideDirection.Left);

        public EnumSlideDirection SlideDirectionFrom
        {
            get { return (EnumSlideDirection)this.GetValue(SlideDirectionProperty); }
            set { this.SetValue(SlideDirectionProperty, value); }
        }

        public static readonly BindableProperty SlideWithTouchProperty = BindableProperty.Create<SidePanelControl, bool>(x => x.SlideWithTouch, true);

        public bool SlideWithTouch
        {
            get { return (bool)this.GetValue(SlideWithTouchProperty); }
            set { this.SetValue(SlideWithTouchProperty, value); }
        }

        #endregion

        #region Instance Variables

        private MainState State { get; set; } = MainState.Closed;
        private bool _initialized = false;
        private double _distanceToMove;
        private double _originalSidePanelOffset = 0;
        private double _lastPanLocation;
        private double _startingX;

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

        private async void ArrangeContent()
        {
            if (this.Width <= 0) return;

            AbsoluteLayout.SetLayoutBounds(GetItemToAnimate(), this.Bounds);

            if (_initialized) return;
            _initialized = true;

            //calculate how much we will move the slide by, and apply the sizings to the grid columns
            _distanceToMove = this.Width * SidePanelPercentage;

            //set hidden content to distance and crop it to size
            var sidePanel = RightSideContent;
            sidePanel.WidthRequest = _distanceToMove;

            //position the hidden content
            AbsoluteLayout.SetLayoutBounds(RightSideContent,
                this.SlideDirectionFrom == EnumSlideDirection.Left
                    ? new Rectangle(-_distanceToMove, 0, sidePanel.Width, this.Height)
                    : new Rectangle(this.Width, 0, sidePanel.Width, this.Height));

            _originalSidePanelOffset = this.Depth == EnumPanelDepth.Overlay
                ? AbsoluteLayout.GetLayoutBounds(RightSideContent).X
                : AbsoluteLayout.GetLayoutBounds(BesideAnimationGrid).X;

            //set up toolbar item to correct grid
            this.SetupToolBarItems();

            //Set the pan view
            if (SlideWithTouch)
            {
                PanView.IsVisible = SlideWithTouch;
                if (this.SlideDirectionFrom == EnumSlideDirection.Right)
                {
                    PanView.AbsoluteLayoutTo(new Rectangle(this.Width - this.SlideGestureWidth, 0, SlideGestureWidth, this.Height));
                }
                PanView.GestureRecognizers.Add(CreateSlidePanGestureRecogniser());
            }

            AbsoluteLayout.SetLayoutBounds(GetItemToAnimate(), GetItemToAnimate().Bounds);
        }

        private IGestureRecognizer CreateTappedGestureRecognizer()
        {
            var result = new TapGestureRecognizer();
            result.Tapped += (sender, args) => ToggleIfOpen(null);
            return result;
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
            if (State == MainState.Closed)
            {
                OpenSidePanel();
            }
            else
            {
                CloseSidePanel();
            }
        }

        private void CloseSidePanel()
        {
            //decide to overlay the sidepanel or not
            View itemToAnimate = GetItemToAnimate();

            var currentPositioningX = AbsoluteLayout.GetLayoutBounds(itemToAnimate).X;

            itemToAnimate.Animate("CloseSidePanel", UpdateGridTranslation,
                start: currentPositioningX,
                end: _originalSidePanelOffset,
                finished: OnClose,
                easing: Easing.CubicOut);
            State = MainState.Closed;
        }

        private void OpenSidePanel()
        {
            //decide to overlay the sidepanel or not
            View itemToAnimate = GetItemToAnimate();

            //decide to slide from left or right
            double travelDistance = SlideDirectionFrom == EnumSlideDirection.Left ?
                _distanceToMove :
                -_distanceToMove;
            var currentPositioningX = AbsoluteLayout.GetLayoutBounds(itemToAnimate).X;

            itemToAnimate.Animate("OpenSidePanel", UpdateGridTranslation,
                start: currentPositioningX,
                end: travelDistance,
                finished: OnOpenFinished,
                easing: Easing.CubicOut);
            State = MainState.Open;
        }

        private void OnClose(double arg1, bool arg2)
        {
            GestureCloseArea.IsVisible = false;
            GestureCloseArea.GestureRecognizers.Clear();
        }

        private void OnOpenFinished(double arg1, bool arg2)
        {
            //overlay the gesture close area around the main grid and set to visible so it can take input
            var mainGrid = this.GetMainContentGrid();
            GestureCloseArea.IsVisible = true;
            GestureCloseArea.AbsoluteLayoutTo(mainGrid.Bounds);
            GestureCloseArea.GestureRecognizers.Add(CreateTappedGestureRecognizer());
        }

        private IGestureRecognizer CreateSlidePanGestureRecogniser()
        {
            var panRecognizer = new PanGestureRecognizer();
            panRecognizer.PanUpdated += PanRecognizerOnPanUpdated;
            return panRecognizer;
        }

        private void PanRecognizerOnPanUpdated(object sender, PanUpdatedEventArgs panInfo)
        {
            var item = GetItemToAnimate();
            if (panInfo.StatusType == GestureStatus.Started)
            {
                _startingX = AbsoluteLayout.GetLayoutBounds(item).X;
            }
            if (panInfo.StatusType == GestureStatus.Running)
            {
                var distanceItemTravelled = AbsoluteLayout.GetLayoutBounds(item).X;

                //make sure we can actually pan
                if (SlideDirectionFrom == EnumSlideDirection.Right)
                {
                    if (distanceItemTravelled < -_distanceToMove || distanceItemTravelled > 0)
                    {
                        return;
                    }
                }
                else if (SlideDirectionFrom == EnumSlideDirection.Left)
                {
                    if (distanceItemTravelled < 0 || distanceItemTravelled < _distanceToMove)
                    {
                        return;
                    }
                }

                //apply transformation
                item.AbsoluteLayoutXTo(_startingX + panInfo.TotalX);
                _lastPanLocation = panInfo.TotalX;
            }
            if (panInfo.StatusType == GestureStatus.Completed)
            {
                double distanceTravelled = Math.Abs(_lastPanLocation);
                double percentage = distanceTravelled / _distanceToMove;
                if (percentage > 0.6)
                {
                    //commit the transitions
                    ToggleSidePanel();
                }
                else
                {
                    //return the panel to it's original state
                    if (State == MainState.Open)
                    {
                        OpenSidePanel();
                    }
                    else
                    {
                        CloseSidePanel();
                    }
                }
            }
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
            AbsoluteLayout.SetLayoutBounds(item, new Rectangle(newValue, 0, item.Width, item.Height));
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