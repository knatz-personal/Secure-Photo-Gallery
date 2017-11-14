using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace WebPortal.Extensions
{
    public static class BitmapExtensions
    {
        private static void AddWaterMark(this Bitmap image, string message)
        {
            using (Graphics graphics = Graphics.FromImage(image))
            {
                Font fontWatermark = new Font("Verdana", 12, FontStyle.Italic);

                var layoutRectangle = new Rectangle(10, 10, image.Width - 10, image.Height - 10);

                StringFormat stringFormat = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                graphics.DrawString(message, fontWatermark, Brushes.Beige, layoutRectangle, stringFormat);
            }
        }
    }
}