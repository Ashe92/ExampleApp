using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using ExampleApp.Enums;
using ExampleApp.Helpers;
using ExampleApp.Views;
using SkiaSharp;
using Xamarin.Forms.Internals;

namespace ExampleApp.Models
{
    public class Level
    {
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int Height { get; private set; }
        public int Width { get; private set; }
        public int BlockWidth { get; private set; }
        public int BlockHeight { get; private set; }
        public bool CollisionDetected { get; private set; } = false;
        public int Number { get;  }

        public List<Tile> TileElements { get;private set; }
        public Player PlayerTile { get; private set; }
        public Tile EndPoint { get; private set; }

        public Level(float width, float height, int number =0)
        {
            Number = number;
            CalculateWidth(width);
            CalculateHeight(height);
            TileElements = new List<Tile>();
        }

        public void CalculateMap()
        {
            var file =  ReadFile();
            SetUpMap(file);
        }

        public int[,] SetUpMap(string file)
        {
            var lines = file.Split(new[] { Environment.NewLine },StringSplitOptions.RemoveEmptyEntries);
            var map = new int[Constants.HeightElements, Constants.WidthElements];

            for (int i = 0; i < Constants.HeightElements; i++)
            {
                var splitted = lines[i].Split();
                for (int j = 0; j < Constants.WidthElements; j++)
                {
                    map[i, j] = Convert.ToInt32(splitted[j]);
                    if (map[i, j] == 3)
                    {
                        SetupPlayer(i, j, map[i, j]);
                    }
                    else if (map[i, j] != 1)
                    {
                        SetupTile(i, j, map[i, j]);
                    }
                }
            }

            return map;
        }

        private void SetupPlayer(int i, int j, int tileType)
        {
            if(tileType != (int)TileType.Player)
                throw new Exception("That's not player");

            PlayerTile = new Player()
            {
                X = i * BlockHeight+10,
                Y = j * BlockWidth+ 10,
                Type = TileType.Player,
                Size = new SKSize(Constants.SizeOfPlayer, Constants.SizeOfPlayer),
            };
        }

        private void SetupTile(int i, int j, int tileType)
        {
            var tile = new Tile
            {
                Y = i * BlockHeight,
                X = j * BlockWidth,
                Size = new SKSize(BlockWidth, BlockHeight),
                Type = (TileType) tileType
            };
            if (tile.Type == TileType.End)
            {
                EndPoint = tile;
            }
            TileElements.Add(tile);
        }


        private string ReadFile()
        {
            string text = "";
            try
            {
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(MapPage)).Assembly;
                Stream stream = assembly.GetManifestResourceStream($"ExampleApp.Resources.Level_{Number}.txt");

                using (var reader = new StreamReader(stream))
                {
                    text = reader.ReadToEnd();
                }
            }
            catch (FileNotFoundException)
            {

            }
            catch (Exception)
            {

            }
            return text;
        }

        private void CalculateHeight(float canvasHeight)
        {
            var sizeOfBlock = (int)Math.Floor(canvasHeight / Constants.HeightElements);
            Height = (sizeOfBlock * Constants.HeightElements);
            BlockHeight = sizeOfBlock;

            var difference = canvasHeight - Height;
            StartX = (int) Math.Floor(difference / 2);
        }

        private void CalculateWidth(float canvasWidth)
        {
            var sizeOfBlock = (int)Math.Floor(canvasWidth / Constants.WidthElements); ;
            Width = sizeOfBlock * Constants.WidthElements;
            BlockWidth = sizeOfBlock;
            var difference = canvasWidth - Width;
            StartY = (int)Math.Floor(difference / 2);
        }

        public void CheckPlayerTileCollision(ActionType action)
        {
            if (CollisionDetected) return;
            Tile tile = null;
            //switch (action)
            //{
            //    case ActionType.Left:
            //        tile = TileElements.FirstOrDefault(t =>
            //        {
            //            if (XIsInRange(t.Y, t.Y + (int)t.Size.Height))
            //                return YIsInRange(t.X, t.X + (int)t.Size.Width);
            //            return false;
            //        });
            //        break;
            //    case ActionType.Right:
            //        tile = TileElements.FirstOrDefault(t =>
            //        {
            //            if (XIsInRange(t.Y, t.Y + (int)t.Size.Height))
            //                return YIsInRange(t.X, t.X + (int)t.Size.Width);
            //            return false;
            //        });
            //        break;
            //    case ActionType.Up:
            //        tile = TileElements.FirstOrDefault(t =>
            //        {
            //            if (YIsInRange(t.Y, t.Y + (int)t.Size.Height))
            //                return XIsInRange(t.X, t.X + (int)t.Size.Width);
            //            return false;
            //        });
            //        break;
            //    case ActionType.Down:
            //        tile = TileElements.FirstOrDefault(t =>
            //        {
            //            if (YIsInRange(t.Y, t.Y + (int)t.Size.Height))
            //                return XIsInRange(t.X, t.X + (int)t.Size.Width);
            //            return false;
            //        });
            //        break;
            //}
            tile = TileElements.FirstOrDefault(t => PlayerTile.Rect.IntersectsWith(t.Rect));
            CollisionDetected = tile != null;
        }

        public bool XIsInRange(int tilePoint, int tileSize)
        {
            var x = PlayerTile.X;
            if (Enumerable.Range(tilePoint, tileSize).Contains(x))
                return true;
            x = PlayerTile.X + (int)PlayerTile.Size.Height;
            if (Enumerable.Range(tilePoint, tileSize).Contains(x))
                return true;
            return false;
        }

        bool YIsInRange(int tilePoint, int tileSize)
        {
            var x = PlayerTile.Y;
            if (Enumerable.Range(tilePoint, tileSize).Contains(x))
                return true;
            x = PlayerTile.Y + (int)PlayerTile.Size.Width;
            if(Enumerable.Range(tilePoint, tileSize).Contains(x))
                return true;
            return false;
        }
    }
}