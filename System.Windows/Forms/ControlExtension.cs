namespace System.Windows.Forms
{
    using Collections.Generic;
    using Drawing;

    /// <summary>
    ///   Extension methods for System.Windows.Forms.Control.
    /// </summary>
    public static class ControlExtension
    {
        /// <summary>
        ///   Returns <c>true</c> if target control is in design mode or one of the target's parent is in design mode.
        ///   Othervise returns <c>false</c>.
        /// </summary>
        /// <param name = "control">Target control. Can not be null.</param>
        /// <example>
        ///   bool designMode = this.button1.IsInWinDesignMode();
        /// </example>
        /// <remarks>
        ///   The design mode is set only to direct controls in desgined control/form.
        ///   However the child controls in controls/usercontrols does not flag for "my parent is in design mode".
        ///   The solution is traversion upon parents of target control.
        /// 
        ///   Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
        /// </remarks>
        public static bool IsInWinDesignMode(this Control control)
        {
            var ret = false;

            var ctl = control;
            while (!ReferenceEquals(ctl, null))
            {
                var site = ctl.Site;
                if (!ReferenceEquals(site, null))
                {
                    if (site.DesignMode)
                    {
                        ret = true;
                        break;
                    }
                }
                ctl = ctl.Parent;
            }

            return ret;
        }

        /// <summary>
        ///   Returns <c>true</c> if target control is NOT in design mode and none of the target's parent is NOT in design mode.
        ///   Othervise returns <c>false</c>.
        /// </summary>
        /// <param name = "control">Target control. Can not be null.</param>
        /// <example>
        ///   bool runtimeMode = this.button1.IsInWinRuntimenMode();
        /// </example>
        /// <remarks>
        ///   The design mode is set only to direct controls in desgined control/form.
        ///   However the child controls in controls/usercontrols does not flag for "my parent is in design mode".
        ///   The solution is traversion upon parents of target control.
        /// 
        ///   Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
        /// </remarks>
        public static bool IsInWinRuntimeMode(this Control control)
        {
            var ret = true;

            var ctl = control;
            while (!ReferenceEquals(ctl, null))
            {
                var site = ctl.Site;
                if (!ReferenceEquals(site, null))
                {
                    if (site.DesignMode)
                    {
                        ret = false;
                        break;
                    }
                }
                ctl = ctl.Parent;
            }

            return ret;
        }

        /// <summary>
        ///   Invoke action on UI thread of target control.
        ///   If current thread is other than the UI thread of control, the Control.Invoke will be used.
        ///   Othervise the action is invoked on current thread.
        /// </summary>
        /// <param name = "control">Target control. Can not be null.</param>
        /// <param name = "action">Action to invoke. Can not be null.</param>
        /// <example>
        ///   this.button1.RunInUIThread( ()=> this.button1.Text = "Click me!" );
        /// </example>
        /// <remarks>
        ///   Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
        /// </remarks>
        public static void RunInUIThread(this Control control, Action action)
        {
            if (control.InvokeRequired)
                control.Invoke(action);
            else
                action();
        }

        /// <summary>
        ///   Find parent controls of target control which are (inherits or implements) specified type.
        /// </summary>
        /// <typeparam name = "T">Type of searched controls.</typeparam>
        /// <param name = "control">Target control. Can not be null.</param>
        /// <returns>Return enumerable of parent controls.</returns>
        /// <example>
        ///   var parentPanels = this.button1.FindParentsOfType&lt;Panel&gt;();
        /// 
        ///   var firstParentPanel = this.button1.FindParentsOfType&lt;Panel&gt;().FirstOrDefault();
        /// </example>
        /// <remarks>
        ///   Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
        /// </remarks>
        public static IEnumerable<T> FindParentsOfType<T>(this Control control) where T : class
        {
            var ctl = control.Parent;
            while (!ReferenceEquals(ctl, null))
            {
                var typedControl = ctl as T;
                if (!ReferenceEquals(typedControl, null))
                {
                    yield return typedControl;
                }

                ctl = ctl.Parent;
            }
        }

        /// <summary>
        ///   Find child controls of target control which are (inherits or implements) specified type.
        /// 
        ///   Overload for: FindChildsOfType&lt;T&gt;(target, false);
        /// </summary>
        /// <typeparam name = "T">Type of searched controls.</typeparam>
        /// <param name = "control">Target control. Can not be null.</param>
        /// <remarks>
        ///   Depth-first search is used.
        /// 
        ///   Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
        /// </remarks>
        /// <returns>Enumerable object with child controls of specified type.</returns>
        public static IEnumerable<T> FindChildsOfType<T>(this Control control) where T : class
        {
            return FindChildsOfType<T>(control, false);
        }

        /// <summary>
        ///   Find child controls of target control which are (inherits or implements) specified type.
        /// </summary>
        /// <typeparam name = "T">Type of searched controls.</typeparam>
        /// <param name = "control">Target control. Can not be null.</param>
        /// <param name = "searchChildren">If true, the search algorithm will be continue in returned controls. Othervise the returned control will not be searched.</param>
        /// <remarks>
        ///   Depth-first search is used.
        /// 
        ///   Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
        /// </remarks>
        /// <returns>Enumerable object with child controls of specified type.</returns>
        public static IEnumerable<T> FindChildsOfType<T>(this Control control, bool searchChildren) where T : class
        {
            foreach (Control child in control.Controls)
            {
                var typedControl = child as T;
                if (!ReferenceEquals(typedControl, null))
                {
                    yield return typedControl;
                }

                if (child.HasChildren)
                {
                    var subChilds = FindChildsOfType<T>(child);
                    foreach (var subChild in subChilds)
                    {
                        yield return subChild;
                    }
                }
            }
        }

        // ---

        public static void MakeResizable(this Control control)
        {
            var isSizing = false;
            var pointStart = Point.Empty;
            var pointStop = Point.Empty;
            var size = Size.Empty;

            control.MouseDown += delegate(object sender, MouseEventArgs entMouse)
            {
                pointStart = pointStop = entMouse.Location;
                size = control.Parent.ClientSize;
                var X = control.Left + entMouse.X;
                var Y = control.Top + entMouse.Y;
                if (X + 10 >= control.Right && X <= control.Right
                 && Y + 10 >= control.Bottom && Y <= control.Bottom)
                {
                    isSizing = true;
                    control.Capture = true;
                }
            };

            control.MouseMove += delegate(object sender, MouseEventArgs entMouse)
            {
                var X = control.Left + entMouse.X;
                var Y = control.Top + entMouse.Y;

                if (X + 10 >= control.Right && X <= control.Right
                 && Y + 10 >= control.Bottom && Y <= control.Bottom)
                {
                    control.Cursor = Cursors.SizeNWSE;
                }
                else
                {
                    control.Cursor = Cursors.Default;
                }

                if (isSizing)
                {
                    if (pointStop != entMouse.Location)
                    {
                        pointStop = entMouse.Location;
                        control.Width += entMouse.X - pointStart.X;
                        control.Height += entMouse.Y - pointStart.Y;
                    }
                }
            };

            control.MouseLeave += delegate(object sender, EventArgs ent)
            {
            };

            control.MouseUp += delegate(object sender, MouseEventArgs entMouse)
            {
                control.Capture = false;
                isSizing = false;
            };
        }


        public static void MakeDragable(this Control control)
        {
            var isDragging = false;
            var pointDrag = Point.Empty;

            control.MouseDown += delegate(object sender, MouseEventArgs entMouse)
            {
                isDragging = true;
                pointDrag = entMouse.Location;
                control.Capture = true;
            };

            control.MouseMove += delegate(object sender, MouseEventArgs entMouse)
            {
                if (isDragging)
                {
                    if (control.Cursor == Cursors.Default)
                    {
                        control.Left += entMouse.X - pointDrag.X;
                        control.Top += entMouse.Y - pointDrag.Y;
                    }
                }
            };

            control.MouseUp += delegate(object sender, MouseEventArgs entMouse)
            {
                control.Capture = false;
                isDragging = false;
            };
        }


    }
}