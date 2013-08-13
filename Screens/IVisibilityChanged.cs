using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Morph
{
    public interface IVisibilityChanged
    {
        /// <summary>
        /// in WPF, usually we'd have an event that would get fired automatically, hooking on VisibilityChanged.
        /// Assuming that either WP hasn't gotten to here yet, or no silverlight support.
        /// We might not need the effort to implement an attached property to WP's user control, so holding
        /// off for now
        /// </summary>
        void OnVisibilityChanged();
    }
}
