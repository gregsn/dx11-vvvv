using SlimDX;
using SlimDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVVV.Utils.VMath;

namespace VVVV.DX11.Nodes
{
    internal class DX11Viewport
    {
        Viewport Viewport;
        Matrix FromNormalizedViewportToNormalizedProjection;
        Matrix FromNormalizedProjectionToProjection;

        public DX11Viewport(Viewport viewport, Matrix aspectRatio, Matrix crop)
        {
            Viewport = viewport;
            FromNormalizedProjectionToProjection = aspectRatio;
            FromNormalizedViewportToNormalizedProjection = crop;
        }

        public bool MapFromPixels(Point inPixels, out Vector2D inNormalizedProjection, out Vector2D inProjection)
        {
            var inNormalizedViewport = new Vector2(
                -1 + 2 * (inPixels.X - Viewport.X) / (Viewport.Width - 1),
                 1 - 2 * (inPixels.Y - Viewport.Y) / (Viewport.Height - 1));

            if (inNormalizedViewport.X >= -1 && inNormalizedViewport.X <= 1 &&
                inNormalizedViewport.Y >= -1 && inNormalizedViewport.Y <= 1)
            {
                Vector2 _inNormalizedProjection;
                Vector2.TransformCoordinate(ref inNormalizedViewport, ref FromNormalizedViewportToNormalizedProjection, out _inNormalizedProjection);
                inNormalizedProjection = new Vector2D(_inNormalizedProjection.X, _inNormalizedProjection.Y);

                Vector2 _inProjection;
                Vector2.TransformCoordinate(ref _inNormalizedProjection, ref FromNormalizedProjectionToProjection, out _inProjection);
                inProjection = new Vector2D(_inProjection.X, _inProjection.Y);
                return true;
            }
            else
            {
                inNormalizedProjection = Vector2D.Zero;
                inProjection = Vector2D.Zero;
                return false;
            }
        }
    }
}
