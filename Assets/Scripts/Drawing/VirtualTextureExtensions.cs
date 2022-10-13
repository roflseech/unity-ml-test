using System;
using System.Collections.Generic;
using UnityEngine;

public static class VirtualTextureExtensions
{
    public static VirtualTexture Resize(this VirtualTexture virtualTexture, int targetWidth, int targetHeight)
    {
        if (targetWidth < 0) throw new Exception($"Target width {targetWidth} < 0");
        if (targetHeight < 0) throw new Exception($"Target height {targetHeight} < 0");

        var newTexture = new VirtualTexture(targetWidth, targetHeight);

        int wSegment = virtualTexture.Width / targetWidth;
        int hSegment = virtualTexture.Height / targetHeight;

        int deltaX = (virtualTexture.Width - wSegment * targetWidth) / 2;
        int deltaY = (virtualTexture.Height - hSegment * targetHeight) / 2;

        for (int x = 0; x < targetWidth; x++)
        {
            for (int y = 0; y < targetHeight; y++)
            {
                Color accumulated = new Color(0, 0, 0, 0);
                for (int subX = wSegment * x + deltaX; subX < wSegment * (x+1) + deltaX; subX++)
                {
                    for (int subY = hSegment * y + deltaY; subY < hSegment * (y + 1) + deltaY; subY++)
                    {
                        accumulated += virtualTexture.GetPixel(subX, subY);
                    }
                }
                accumulated /= (wSegment * hSegment);
                newTexture.SetPixel(x, y, accumulated);
            }
        }

        return newTexture;
    }
    public static VirtualTexture FilledSubset(this VirtualTexture virtualTexture, Color background, int padding)
    {
        int minX = -1;
        int minY = -1;
        int maxX = -1;
        int maxY = -1;

        bool _started = false;

        for (int x = 0; x < virtualTexture.Width; x++)
        {
            for (int y = 0; y < virtualTexture.Height; y++)
            {
                if (virtualTexture.GetPixel(x, y) != background)
                {
                    if (_started)
                    {
                        if (x < minX) minX = x;
                        if (x > maxX) maxX = x;

                        if (y < minY) minY = y;
                        if (y > maxY) maxY = y;
                    }
                    else
                    {
                        minX = x;
                        minY = y;
                        maxX = x;
                        maxY = y;
                        _started = true;
                    }
                }
            }
        }

        minX = Mathf.Max(minX - padding, 0);
        minY = Mathf.Max(minY - padding, 0);

        maxX = Mathf.Min(maxX + padding, virtualTexture.Width - 1);
        maxY = Mathf.Min(maxY + padding, virtualTexture.Height - 1);

        var newTexture = new VirtualTexture(maxX - minX + 1, maxY - minY + 1);

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                newTexture.SetPixel(x - minX, y - minY, virtualTexture.GetPixel(x, y));
            }
        }

        return newTexture;
    }
    public static VirtualTexture TransparentBlackToBlackWhite(this VirtualTexture virtualTexture)
    {
        var newTexture = new VirtualTexture(virtualTexture.Width, virtualTexture.Height);

        for (int i = 0; i < virtualTexture.Pixels.Length; i++)
        {
            var grayscale = virtualTexture.Pixels[i].a;

            newTexture.Pixels[i] = new Color(grayscale, grayscale, grayscale, 1);
        }

        return newTexture;
    }
}