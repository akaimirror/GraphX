﻿using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using GraphX.Controls;
using System;
using GraphX.METRO.Controls.Models.Interfaces;

namespace GraphX.Models.Animations
{
    public sealed class DeleteShrinkAnimation : IOneWayControlAnimation
    {
        public double Duration { get; set; }
        public bool Centered { get; set; }

        public DeleteShrinkAnimation(double duration = .3, bool centered = true)
        {
            Duration = duration;
            Centered = centered;
        }

        public void AnimateVertex(VertexControl target)
        {
            //get scale transform or create new one
            var transform = CustomHelper.GetScaleTransform(target);
            if (transform == null)
            {
                target.RenderTransform = new ScaleTransform();
                transform = target.RenderTransform as ScaleTransform;
                 target.RenderTransformOrigin = Centered ? new Point(.5, .5) : new Point(0, 0);
            }

            var sb = new Storyboard();
            //create and run animation
            var scaleAnimation = new DoubleAnimation { Duration = new Duration(TimeSpan.FromSeconds(Duration)), From = 1, To = 0  };
            //scaleAnimation.Completed += (sender, e) => OnCompleted(target as IGraphControl);
            Storyboard.SetTarget(scaleAnimation, target);
            Storyboard.SetTargetProperty(scaleAnimation, "(UIElement.RenderTransform).(CompositeTransform.ScaleX)");
            sb.Children.Add(scaleAnimation);

            scaleAnimation = new DoubleAnimation { Duration = new Duration(TimeSpan.FromSeconds(Duration)), From = 1, To = 0 };
            scaleAnimation.Completed += (sender, e) => OnCompleted(target as IGraphControl);
            Storyboard.SetTarget(scaleAnimation, target);
            Storyboard.SetTargetProperty(scaleAnimation, "(UIElement.RenderTransform).(CompositeTransform.ScaleY)");
            sb.Children.Add(scaleAnimation);

            //transform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            //transform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
            sb.Begin();
        }

        public void AnimateEdge(EdgeControl target)
        {
            //ALWAYS fire completed event to init delete procedure after the animation process
            OnCompleted(target);
        }

        /// <summary>
        /// Completed event that fires when animation is complete. Must be fired for correct object removal when animation ends.
        /// </summary>
        public event RemoveControlEventHandler Completed;

        public void OnCompleted(IGraphControl target)
        {
            if (Completed != null)
                Completed(this, new ControlEventArgs(target));
        }
    }
}
