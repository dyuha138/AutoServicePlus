using MahApps.Metro.Controls;
using System.Windows;

namespace AutoServicePlus;

// for lazy loading of HamburgerMenu.Content
internal sealed class MyHamburgerMenuProperies
{
    public static readonly DependencyProperty DeferredContentTypeProperty =
        DependencyProperty.RegisterAttached("DeferredContentType", typeof(Type), typeof(MyHamburgerMenuProperies));

    public static Type GetDeferredContentType(HamburgerMenuItemBase element) =>
        (Type)element.GetValue(DeferredContentTypeProperty);

    public static void SetDeferredContentType(HamburgerMenuItemBase element, Type value) =>
        element.SetValue(DeferredContentTypeProperty, value);
}
