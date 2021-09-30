using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using CoreGraphics;
using Foundation;
using SmeData.Mobile.CustomControls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ListViewWithGrouping), typeof(SmeData.Mobile.iOS.ListViewRendererFixer))]
namespace SmeData.Mobile.iOS
{
    public sealed class ListViewRendererFixer : ListViewRenderer
    {
        private static readonly FieldInfo fieldInfo_ListViewRenderer_dataSource = typeof(ListViewRenderer).GetField("_dataSource", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly Type type_ListViewRenderer_ListViewDataSource = typeof(ListViewRenderer).GetNestedType("ListViewDataSource", BindingFlags.NonPublic);
        private static readonly Type type_ListViewRenderer_UnevenListViewDataSource = typeof(ListViewRenderer).GetNestedType("UnevenListViewDataSource", BindingFlags.NonPublic);
        private static readonly ConstructorInfo ctorInfo_ListViewRenderer_ListViewDataSource = type_ListViewRenderer_ListViewDataSource.GetConstructor(new[] { fieldInfo_ListViewRenderer_dataSource.FieldType });
        private static readonly ConstructorInfo ctorInfo_ListViewRenderer_UnevenListViewDataSource = type_ListViewRenderer_UnevenListViewDataSource.GetConstructor(new[] { fieldInfo_ListViewRenderer_dataSource.FieldType });

        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                Control.Source = new CustomListViewDataSource(Control.Source);
            }

            if (Control != null)
            {
                Control.RowHeight = UITableView.AutomaticDimension;
                Control.EstimatedRowHeight = 40;
                Control.SectionHeaderHeight = UITableView.AutomaticDimension;
                Control.EstimatedSectionHeaderHeight = 40;
                Control.SectionFooterHeight = 0;
                Control.EstimatedSectionFooterHeight = 0;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == ListView.HasUnevenRowsProperty.PropertyName)
            {
                var ctor = Element.HasUnevenRows ? ctorInfo_ListViewRenderer_UnevenListViewDataSource : ctorInfo_ListViewRenderer_ListViewDataSource;
                var dataSource = fieldInfo_ListViewRenderer_dataSource.GetValue(this);
                var tvs = (UITableViewSource)ctor.Invoke(new[] { dataSource });
                fieldInfo_ListViewRenderer_dataSource.SetValue(this, tvs);
                Control.Source = new CustomListViewDataSource(tvs);
                Control.ReloadData();
            }
            else
            {
                base.OnElementPropertyChanged(sender, e);
            }
        }

        public sealed class CustomListViewDataSource : UITableViewSource
        {
            private readonly UITableViewSource inner;
            private readonly MethodInfo methodInfo_DetermineEstimatedRowHeight;
            private PropertyInfo propertyInfo_HeaderWrapperView_Cell;

            public CustomListViewDataSource(UITableViewSource inner)
            {
                this.inner = inner;
                methodInfo_DetermineEstimatedRowHeight = inner.GetType().BaseType.GetMethod("DetermineEstimatedRowHeight", BindingFlags.Instance | BindingFlags.NonPublic);
            }

            public override nfloat GetHeightForHeader(UITableView tableView, nint section)
            {
                var r = inner.GetHeightForHeader(tableView, section);
                r = UITableView.AutomaticDimension;
                return r;
            }

            public override UIView GetViewForHeader(UITableView tableView, nint section)
            {
                var innerWrapper = inner.GetViewForHeader(tableView, section);

                if (innerWrapper == null)
                {
                    return innerWrapper;
                }

                var pi = propertyInfo_HeaderWrapperView_Cell ?? (propertyInfo_HeaderWrapperView_Cell = innerWrapper.GetType().GetProperty("Cell", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public));
                var cell = (Cell)pi.GetValue(innerWrapper);

                var cellView = innerWrapper.Subviews[0];


                //var newWrapper = new HeaderWrapperView(cell, cellView);
                //return newWrapper;
                var renderer = (CellRenderer)Xamarin.Forms.Internals.Registrar.Registered.GetHandlerForObject<IRegisterable>(cell);
                return new HeaderWrapperView(cell, renderer.GetCell(cell, null, tableView));
            }

            public override void HeaderViewDisplayingEnded(UITableView tableView, UIView headerView, nint section)
            {
                if (headerView is HeaderWrapperView wrapper)
                {
                    wrapper.Cell?.SendDisappearing();
                    wrapper.Cell = null;
                }
            }

            public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
            {
                if (methodInfo_DetermineEstimatedRowHeight != null)
                    methodInfo_DetermineEstimatedRowHeight.Invoke(inner, new object[0]);
            }

            #region Irrelevant
            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                return inner.GetCell(tableView, indexPath);
            }

            public override nint RowsInSection(UITableView tableView, nint section)
            {
                return inner.RowsInSection(tableView, section);
            }

            public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
            {
                return inner.GetHeightForRow(tableView, indexPath);
            }

            public override void DraggingEnded(UIScrollView scrollView, bool willDecelerate)
            {
                inner.DraggingEnded(scrollView, willDecelerate);
            }

            public override void DraggingStarted(UIScrollView scrollView)
            {
                inner.DraggingStarted(scrollView);
            }

            public override nint NumberOfSections(UITableView tableView)
            {
                return inner.NumberOfSections(tableView);
            }

            public override void RowDeselected(UITableView tableView, NSIndexPath indexPath)
            {
                inner.RowDeselected(tableView, indexPath);
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                inner.RowSelected(tableView, indexPath);
            }

            public override void Scrolled(UIScrollView scrollView)
            {
                inner.Scrolled(scrollView);
            }

            public override string[] SectionIndexTitles(UITableView tableView)
            {
                return inner.SectionIndexTitles(tableView);
            }
            #endregion

            protected override void Dispose(bool disposing)
            {
                inner.Dispose();
            }

            private class HeaderWrapperView : UIView
            {
                public Cell Cell { get; set; }
                public UIView Subview { get; private set; }

                public HeaderWrapperView(Cell cell, UIView subview)
                {
                    Cell = cell;
                    Subview = subview;
                    AddSubview(subview);
                }

                public override CGSize SizeThatFits(CGSize size)
                {
                    return Subview.SizeThatFits(size);
                }

                public override CGSize IntrinsicContentSize => Subview.IntrinsicContentSize;

                public override void LayoutSubviews()
                {
                    base.LayoutSubviews();
                    Subview.Frame = Bounds;
                }
            }
        }
    }
}