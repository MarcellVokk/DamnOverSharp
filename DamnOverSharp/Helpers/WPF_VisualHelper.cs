using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DamnOverSharp.Helpers
{
    public static class WPF_VisualHelper
    {
        public static List<FrameworkElement> GetAllChildren(FrameworkElement parent)
        {
            List<FrameworkElement> result = new List<FrameworkElement>();

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                if (child is UserControl)
                {
                    result.Add((UserControl)child);
                    if((child as UserControl).Content is FrameworkElement)
                    {
                        result.AddRange(GetAllChildren((child as UserControl).Content as FrameworkElement));
                    }
                }
                else if (child is FrameworkElement)
                {
                    result.Add(child as FrameworkElement);
                    result.AddRange(GetAllChildren(child as FrameworkElement));
                }
            }

            return result;
        }

        public static Rect GetVisualRect(FrameworkElement element, FrameworkElement parent)
        {
            return element.TransformToVisual(parent).TransformBounds(new Rect(element.RenderSize));
        }
    }
}
