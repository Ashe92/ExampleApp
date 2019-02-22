using System;
using System.Data.Common;
using System.Diagnostics;
using System.Text;
using ExampleApp.Enums;
using ExampleApp.Helpers;
using SkiaSharp;

namespace ExampleApp.Models
{
    public class Player:Tile
    {
        private float Width { get; } = Constants.SizeOfPlayer;
        private float Height { get; } = Constants.SizeOfPlayer;

        public bool CanMakeAction(ActionType action,float canvasWidth, float canvasHeight)
        {
            // ReSharper disable once ReplaceWithSingleAssignment.False
            var canMakeAction = false;

            switch(action)
            {
                case ActionType.Up:
                    canMakeAction = ((Y - 10 >= 0));
                    break;
                case ActionType.Down:
                    canMakeAction = (Y + Height +  10.0f < canvasHeight);
                    break;
                case ActionType.Left:
                    canMakeAction = (X - 10 >= 0);
                    break;
                case ActionType.Right:
                    //todo repair it
                    canMakeAction = (X + Width + 10 < canvasWidth);
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
                case ActionType.Left:
                    X -= 10;
                    break;  
                case ActionType.Right:
                    X += 10;
                    break;
                case ActionType.Up:
                    Y -= 10;
                    break;
                case ActionType.Down:
                    Y += 10;
                    break;
                default:
                    throw new Exception("Brak wskazanej akcji");
            }
        }

        public Player()
        {
            Size =  new SKSize(Constants.SizeOfPlayer, Constants.SizeOfPlayer);
            Type = TileType.Player;
        }

        public void SetUpPlayerLocation(int x, int y)
        {

        }
    }
}