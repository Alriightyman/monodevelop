﻿//
// RunButton.cs
//
// Author:
//       Marius Ungureanu <marius.ungureanu@xamarin.com>
//
// Copyright (c) 2015 Xamarin, Inc (http://www.xamarin.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using AppKit;
using Foundation;
using CoreGraphics;
using MonoDevelop.Components.MainToolbar;
using MonoDevelop.Ide;
using MonoDevelop.Components;

namespace MonoDevelop.MacIntegration.MainToolbar
{
	[Register]
	class RunButton : NSButton
	{
		NSImage stopIcon, continueIcon, buildIcon;

		public RunButton ()
		{
			UpdateIcons ();

			icon = OperationIcon.Run;
			ImagePosition = NSCellImagePosition.ImageOnly;
			BezelStyle = NSBezelStyle.TexturedRounded;
			Enabled = false;
		}

		void UpdateIcons (object sender = null, EventArgs e = null)
		{
			// HACK: NSButton does not support images with NSCustomImageRep used
			//       by Xwt to draw custom/themed images. We have to convert them
			//       to bitmaps, which has to be done after each theme/skin change,
			//       but does not support custom per Image styles (ToBitmap does
			//       not support images with different tags, only global styles are
			//       supported)
			stopIcon = ImageService.GetIcon ("stop").ToBitmap (GtkWorkarounds.GetScaleFactor ()).ToNSImage ();
			continueIcon = ImageService.GetIcon ("continue").ToBitmap (GtkWorkarounds.GetScaleFactor ()).ToNSImage ();
			buildIcon = ImageService.GetIcon ("build").ToBitmap (GtkWorkarounds.GetScaleFactor ()).ToNSImage ();

			// We can use Template images supported by NSButton, thus no reloading
			// on theme/skin change is required.
			stopIcon.Template = continueIcon.Template = buildIcon.Template = true;
		}

		NSImage GetIcon ()
		{
			switch (icon) {
			case OperationIcon.Stop:
				return stopIcon;
			case OperationIcon.Run:
				return continueIcon;
			case OperationIcon.Build:
				return buildIcon;
			}
			throw new InvalidOperationException ();
		}

		public override bool Enabled {
			get {
				return base.Enabled;
			}
			set {
				base.Enabled = value;
				Image = GetIcon ();
			}
		}

		OperationIcon icon;
		public OperationIcon Icon {
			get { return icon; }
			set {
				if (value == icon)
					return;
				icon = value;
				Image = GetIcon ();
			}
		}

		public override CGSize IntrinsicContentSize {
			get {
				return new CGSize (38, 25);
			}
		}
	}
}

