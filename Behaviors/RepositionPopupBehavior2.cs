using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

namespace WpfErrorTemplateTest.Behaviors
{
    // https://social.msdn.microsoft.com/Forums/en-US/8e4e15a5-2c03-44e6-83fb-cad6b6a0f674/automatically-pop-up-the-popup-control-to-upper-or-bottom?forum=silverlightgen
    public class RepositionPopupBehavior2 : Behavior<Popup>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.Opened += this.OnPopupOpened;
            this.AssociatedObject.Closed += this.OnPopupClosed;
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.Opened -= this.OnPopupOpened;
            this.AssociatedObject.Closed -= this.OnPopupClosed;

            this.DetachSizeChangeHandlers();

            base.OnDetaching();
        }

        private void OnPopupOpened(object sender, EventArgs e)
        {
            this.UpdatePopupOffsets();
            this.AttachSizeChangeHandlers();
        }

        private void OnPopupClosed(object sender, EventArgs e)
        {
            this.DetachSizeChangeHandlers();
        }

        private void AttachSizeChangeHandlers()
        {
            var child = this.AssociatedObject.Child as FrameworkElement;
            if (child != null)
            {
                child.SizeChanged += this.OnChildSizeChanged;
            }

            var parent = this.AssociatedObject.Parent as FrameworkElement;
            if (parent != null)
            {
                parent.SizeChanged += this.OnParentSizeChanged;
            }
        }

        private void DetachSizeChangeHandlers()
        {
            var child = this.AssociatedObject.Child as FrameworkElement;
            if (child != null)
            {
                child.SizeChanged -= this.OnChildSizeChanged;
            }

            var parent = this.AssociatedObject.Parent as FrameworkElement;
            if (parent != null)
            {
                parent.SizeChanged -= this.OnParentSizeChanged;
            }
        }

        private void OnChildSizeChanged(object sender, EventArgs e)
        {
            this.UpdatePopupOffsets();
        }

        private void OnParentSizeChanged(object sender, EventArgs e)
        {
            this.UpdatePopupOffsets();
        }

        private void UpdatePopupOffsets()
        {
            if (this.AssociatedObject != null)
            {
                var child = this.AssociatedObject.Child as FrameworkElement;
                var parent = this.AssociatedObject.Parent as FrameworkElement;

                if (child != null && parent != null)
                {
                    var anchor = new Point(parent.ActualWidth, parent.ActualHeight);

                    this.AssociatedObject.HorizontalOffset = (anchor.X / 2) - (child.ActualWidth / 2);
                    //double fx = (parent.ActualHeight + child.ActualHeight);
                    this.AssociatedObject.VerticalOffset = (parent.ActualHeight - parent.ActualHeight) - parent.ActualHeight + (child.ActualHeight / 2) + 4;
                }
                //this.AssociatedObject.HorizontalOffset = anchor.X - child.ActualWidth;
                //this.AssociatedObject.VerticalOffset = anchor.Y - child.ActualHeight;
            }
        }
    }
}
