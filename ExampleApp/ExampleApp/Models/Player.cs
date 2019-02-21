﻿using System;
using System.Data.Common;
using System.Diagnostics;
using System.Text;
using ExampleApp.Enums;
using SkiaSharp;

namespace ExampleApp.Models
{
    public class Player
    {
        public ActionType Action{ get; private set; }
        public float X { get; set; }
        public float Y { get; set; }

        public SKPaint Paint => GetPaint();
        public SKRect Object => GetRect();

        private float Width { get; } = 90;
        private float Height { get; } = 90;

        private SKColor Color => SKColors.Red;

        private SKPaint GetPaint()
        {
            return new SKPaint{ Color = Color };
        }

        private SKRect GetRect()
        {
            var rect = new SKRect
            {
                Location = new SKPoint(X,Y),
                Size = new SKSize(Width,Height)
            };
            return rect;
        }

        public bool CanMakeAction(ActionType action,float canvasWidth, float canvasHeight)
        {
            // ReSharper disable once ReplaceWithSingleAssignment.False
            var canMakeAction = false;

            switch(action)
            {
                case ActionType.Down:
                    canMakeAction = ((Y - 10 >= 0));
                    break;
                case ActionType.Up:
                    canMakeAction = (Y +  10.0f <= canvasHeight);
                    break;
                case ActionType.Right:
                    canMakeAction = (X - 10 >= 0);
                    break;
                case ActionType.Left:
                    //todo repair it
                    canMakeAction = (X + 10 < canvasWidth);
                    break;
                default:
                    return false;
            }
            
            return canMakeAction;
        }

        public void MakeAction(ActionType action)
        {
            switch(action)
            {
                case ActionType.Right:
                    X -= 10;
                    break;  
                case ActionType.Left:
                    X += 10;
                    break;
                case ActionType.Down:
                    Y -= 10;
                    break;
                case ActionType.Up:
                    Y += 10;
                    break;
                default:
                    throw new Exception("Brak wskazanej akcji");
            }
        }

        public Player()
        {
            X = 10;
            Y = 10;
        }
    }
}