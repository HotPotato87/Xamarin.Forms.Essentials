using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Views;
using Xamarin.Forms.Essentials.Controls.Controls;
using Xamarin.Forms.Platform.Android;

namespace Controls.Droid
{
    /// <summary>
	/// This is our class responsible for drawing our shapes
	/// </summary>
	public class CircleView : View
	{
		private readonly float QuarterTurnCounterClockwise = -90;

		public Circle ShapeView { get; set; }

		// Pixel density
		private readonly float density;

		// We need to make sure we account for the padding changes
		public new int Width {
			get { return base.Width - (int)(Resize (this.ShapeView.Padding.HorizontalThickness)); }
		}

		public new int Height {
			get{ return base.Height - (int)(Resize (this.ShapeView.Padding.VerticalThickness)); }
		}

		public CircleView (float density, Context context) : base (context)
		{
			this.density = density;
		}

		public CircleView (float density, Context context, IAttributeSet attributes) : base (context, attributes)
		{
			this.density = density;
		}

		public CircleView (float density, Context context, IAttributeSet attributes, int defStyle) : base (context, attributes, defStyle)
		{
			this.density = density;
		}

		protected override void OnDraw (Canvas canvas)
		{
			base.OnDraw (canvas);
			HandleShapeDraw (canvas);
		}

        protected virtual void HandleShapeDraw(Canvas canvas)
        {
            // We need to account for offsetting the coordinates based on the padding
            var x = GetX() + Resize(this.ShapeView.Padding.Left);
            var y = GetY() + Resize(this.ShapeView.Padding.Top);


            HandleStandardDraw(canvas, p => canvas.DrawCircle(x + this.Width / 2, y + this.Height / 2, (this.Width - 10) / 2, p));
        }

		/// <summary>
		/// A simple method that handles drawing our shape with the various colours we need
		/// </summary>
		/// <param name="canvas">Canvas.</param>
		/// <param name="drawShape">Draw shape.</param>
		/// <param name="lineWidth">Line width.</param>
		/// <param name="drawFill">If set to <c>true</c> draw fill.</param>
		protected virtual void HandleStandardDraw (Canvas canvas, Action<Paint> drawShape, float? lineWidth = null, bool drawFill = true)
		{
			var strokePaint = new Paint (PaintFlags.AntiAlias);
			strokePaint.SetStyle (Paint.Style.Stroke);
			strokePaint.StrokeWidth = Resize (lineWidth ?? ShapeView.StrokeThickness);
			strokePaint.StrokeCap = Paint.Cap.Round;
			strokePaint.Color = ShapeView.StrokeColor.ToAndroid ();
			var fillPaint = new Paint ();
			fillPaint.SetStyle (Paint.Style.Fill);
			fillPaint.Color = ShapeView.FillColor.ToAndroid ();

			if (drawFill)
				drawShape (fillPaint);
			drawShape (strokePaint);
		}

		// Helper functions for dealing with pizel density
		private float Resize (float input)
		{
			return input * density;
		}

		private float Resize (double input)
		{
			return Resize ((float)input);
		}
	}
}
