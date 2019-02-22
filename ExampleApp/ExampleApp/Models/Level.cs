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
        public StateLevel StateLevel { get; set; } 
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
            StateLevel = StateLevel.Started;
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
                    SetupTile(i, j, (TileType)map[i, j]);
                    
                }
            }

            return map;
        }

        private void SetupTile(int i, int j, TileType type)
        {
            Tile tile = new Tile
            {
                Y = i * BlockHeight,
                X = j * BlockWidth,
                Size = new SKSize(BlockWidth, BlockHeight),
                Type = type
            };
            switch (type)
            {
                case TileType.End:
                    EndPoint = tile;
                    break;
                case TileType.Player:
                    PlayerTile = new Player()
                    {
                        X = i * BlockHeight + 10,
                        Y = j * BlockWidth + 10,
                    };
                    break;
                case TileType.None:
                    break;
                default:
                    TileElements.Add(tile);
                    break;
            }
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
            if (StateLevel == StateLevel.Ongoing)
            {
                var tile = TileElements.FirstOrDefault(t => PlayerTile.Rect.IntersectsWith(t.Rect));
                if (tile != null)
                    StateLevel = StateLevel.Looser;
            }

            CheckEndCondition();
        }

        public void CheckEndCondition()
        {
            if (PlayerTile.Rect.IntersectsWithInclusive(EndPoint.Rect))
            {
                StateLevel = StateLevel.Finished;
            }
        }
    }
}